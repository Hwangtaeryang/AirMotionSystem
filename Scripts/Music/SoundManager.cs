using System.Collections;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.IO;
using TMPro;

[RequireComponent(typeof(AudioSource))] // AudioSource Component 필수로 넣기
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    DirectoryInfo MusicFolder;
    DirectoryInfo LyricsFolder;
    WWW myClip; // URL의 컨텐츠를 받아오기위한 작은 유틸리티 모듈

    string myMusicPath; string myMusicPath2; string myMusicPath3;
    string myLyricsPath; string myLyricsPath2; string myLyricsPath3;

    private GameObject[] listings;  // 노래 전체 리스트
    private GameObject[] lyrics_listings;   // 가사 전체 리스트

    private int index = 0;  // 초기설정을 위한 index

    private Component[] songList; // 제목, 시간 저장 변수
    private Component[] lyricsList; // 가사 리스트

    private bool IsRandomloop = false;

    // 노래 목록 생성할 프리펩, 생성위치
    public GameObject listingPrefab;
    public Transform listingsParent;

    // 노래 가사 생성할 프리펩, 생성위치
    public GameObject lyricsPrefab;
    public Transform lyricsParent;

    [Header("Buttons in MainPage")]
    public GameObject Play_Button;
    public GameObject Pause_Button;
    public GameObject Next_Button;
    public GameObject Previous_Button;
    public GameObject Loop_Button;
    public GameObject Unloop_Button;
    public GameObject Random_Button;
    public GameObject NonRandom_Button;
    public Slider progressBar;//, Volume_Slider;
    private float prev_vol_value;

    //플레이 버튼 터치할수있는 큐브
    public GameObject playCube;
    public GameObject pauseCube;
    public GameObject loopCube;
    public GameObject unLoopCube;
    public GameObject randomCube;
    public GameObject unRandomCube;

    [Header("Audio Clips")]
    public AudioClip[] audioClips;
    AudioSource audioSource;
    public int currentTrack = 0;
    public Text songName;
    public Text timeText;
    int fullLength, playTime, sec, min;

    [Header("Buttons in MusicPage")]
    public GameObject page_Play_Button;
    public GameObject page_Pause_Button;
    public GameObject page_Next_Button;
    public GameObject page_Previous_Button;
    public GameObject page_Loop_Button;
    public GameObject page_Unloop_Button;
    public GameObject page_Random_Button;
    public GameObject page_NonRandom_Button;

    //음악페이지 플레이 버튼 터치 큐브
    public GameObject page_playCube;
    public GameObject page_pauseCube;
    public GameObject page_loopCube;
    public GameObject page_unLoopCube;
    public GameObject page_randomCube;
    public GameObject page_unRandomCube;
    public TextMeshProUGUI page_songText;
    public Text page_timeText;
    public Slider page_progressBar;//, page_Volume_Slider;
    public bool isPaused = false;

    // 각각의 앨범내 리스트, 가사 초기화를 위한 프리펩부모
    public GameObject listingsParentPos;
    public GameObject LyricsParentPos;
    bool path1 = false, path2 = false, path3 = false;

    string s_nameCompare;    //제목 비교 이름 변수

    void Awake()
    {
        Instance = this;    // SoundManager의 static 변수로 하나만 사용하려고 선언
        audioSource = GetComponent<AudioSource>();  // Component 초기화
#if UNITY_ANDROID
          myPath = "/storage/sdcard0/Music";
#endif
#if UNITY_STANDALONE
        //myPath = "C:/Users/HUNNY/Music/ogg";
        //myPath = "C:/Users/user/Music/mp3";
        //myMusicPath = "C:/Users/user/Music/wav";
        //yMusicPath = "D:/Music/wav";
#endif
#if Unity_Editor
          myPath = "Builds/Music";
#endif

    }


    public void Start()
    {
        s_nameCompare = page_songText.text;  //처음 비교해줄 제목 초기값 넣어줌

        // 버튼 세팅
        Pause_Button.SetActive(false);
        Play_Button.SetActive(true);

        playCube.SetActive(true);
        pauseCube.SetActive(false);

        Next_Button.SetActive(true);
        Previous_Button.SetActive(true);

        Loop_Button.SetActive(true);
        Unloop_Button.SetActive(false);

        loopCube.SetActive(true);
        unLoopCube.SetActive(false);

        Random_Button.SetActive(true);
        NonRandom_Button.SetActive(false);

        randomCube.SetActive(true);
        unRandomCube.SetActive(false);

        // 페이지 버튼 세팅
        page_Pause_Button.SetActive(false);
        page_Play_Button.SetActive(true);

        page_playCube.SetActive(true);
        page_pauseCube.SetActive(false);

        page_Next_Button.SetActive(true);
        page_Previous_Button.SetActive(true);

        page_Loop_Button.SetActive(true);
        page_Unloop_Button.SetActive(false);

        page_loopCube.SetActive(true);
        page_unLoopCube.SetActive(false);

        page_Random_Button.SetActive(true);
        page_NonRandom_Button.SetActive(false);

        page_randomCube.SetActive(true);
        page_unRandomCube.SetActive(false);

    }

    // 노래 목록 세팅
    private IEnumerator UpdateMusicLibrary()
    {
        int length = MusicFolder.GetFiles().Length;

        audioClips = new AudioClip[length];
        listings = new GameObject[length];


        // 노래 Prefab 초기화
        if (listingsParentPos.transform.childCount != 0)
        {
            for (int i = 0; i < listingsParentPos.transform.childCount; i++)
            {
                Destroy(listingsParentPos.transform.GetChild(i).gameObject);
            }
        }

        // 음악폴더안에 파일을 가져와서 각각에 대입
        for (int i = 0; i < MusicFolder.GetFiles().Length; i++)
        {
            // wav 파일만 가져오기
            if (!(MusicFolder.GetFiles()[i].FullName.Contains("wav")))
                continue;

#if UNITY_STANDALONE
#pragma warning disable CS0618 // 형식 또는 멤버는 사용되지 않습니다.
            myClip = new WWW("file:///" + MusicFolder.GetFiles()[i].FullName);
#pragma warning restore CS0618 // 형식 또는 멤버는 사용되지 않습니다.
#endif
#if Unity_Android
             myClip = cashWWWInstance.GetCachedWWW("file:///" + MusicFolder.GetFiles()[i].FullName);
#endif

            GameObject obj = Instantiate(listingPrefab, listingsParent);    // 프리펩 생성

            obj.name = MusicFolder.GetFiles()[i].Name;  // 파일이름 확장자포함 가져오기
            obj.GetComponent<RectTransform>().localScale = Vector3.one; // 스케일 (1,1,1)
            listings[i] = obj;  // 노래 리스트 저장 변수

            audioClips[i] = myClip.GetAudioClip();  // 클립넣기
            audioClips[i].name = MusicFolder.GetFiles()[i].Name;    // 클립 이름 넣기

            // 로드될때까지 기다리기
            while (audioClips[i].loadState != AudioDataLoadState.Loaded)
            {
                yield return null;
            }

            songList = obj.GetComponentsInChildren<TextMeshProUGUI>();  // 음악 리스트 제목 넣기

            // 확장자 뒤에서부터 잘라서 TextGUI에 넣기
            if (MusicFolder.GetFiles()[i].FullName.Contains(".wav") /*|| MusicFolder.GetFiles()[i].FullName.Contains(".mp3") || MusicFolder.GetFiles()[i].FullName.Contains(".ogg")*/)
            {
                string trimString = MusicFolder.GetFiles()[i].Name; // 파일 이름 저장변수
                string[] trimStrings = trimString.Split(Convert.ToChar("."));   // 확장자 기준으로 문자열 앞뒤 저장 ex) Notepad.text > trimStrings[0] 에 Notepad 저장, trimStrings[1] 에 text 저장
                string inputString = trimString.Substring(0, trimString.IndexOf(trimStrings[trimStrings.Length - 1]) - 1);  // 문자열 자르기

                songList[0].gameObject.GetComponent<TextMeshProUGUI>().SetText(inputString);    //  앨범리스트의 스크롤뷰 제목 저장
                songList[1].gameObject.GetComponent<TextMeshProUGUI>().SetText((((int)audioClips[i].length / 60) % 60) + ":" + ((int)audioClips[i].length % 60).ToString("D2"));    //  앨범리스트의 스크롤뷰 시간 저장
            }

        }

        // 확장자 뒤에서부터 잘라서 TextGUI에 넣기
        string[] _trimStrings = audioClips[index].name.Split(Convert.ToChar("."));
        audioSource.clip = audioClips[index];
        songName.text = audioClips[index].name.Substring(0, audioClips[index].name.IndexOf(_trimStrings[_trimStrings.Length - 1]) - 1);
        page_songText.text = audioClips[index].name.Substring(0, audioClips[index].name.IndexOf(_trimStrings[_trimStrings.Length - 1]) - 1);
    }

    /// <summary>
    /// 스크롤뷰에 있는 노래 클릭시 노래제목과 현재 넣어놓은 클립들중 이름을 비교하여 같으면 선택하는 함수
    /// </summary>
    /// <param name="s"></param>
    public void SelectSongByName(string s)
    {
        string setting; // 현재 클립내에있는 노래중 s 변수(리스트에서 클릭한 노래제목)와 같은지 확인하는 변수
        for (int i = 0; i < audioClips.Length; i++)
        {
            setting = audioClips[i].name;

            if (s.Equals(setting))
            {
                playMine(i);
                break;
            }
        }
    }

    /// <summary>
    /// 기사집 프리팹 생성 함수
    /// </summary>
    /// <param name="_path"></param>
    void CreateLyricsPrefab(string _path)
    {
        int L_length = LyricsFolder.GetFiles().Length;  //길이

        // 가사 프리펩 초기화
        if (LyricsParentPos.transform.childCount != 0)
        {
            //자식이 있을 경우 지움
            Destroy(LyricsParentPos.transform.GetChild(0).gameObject);
        }

        // 음악폴더내에 가사 세팅
        for (int i = 0; i < LyricsFolder.GetFiles().Length; i++)
        {
            //Debug.Log(page_songText.text+"LyricsFolder.GetFiles()[i].Name : " + LyricsFolder.GetFiles()[i].Name);
            //현재 제목과 기사집에 있는 제목이 같을 경우
            if (page_songText.text + ".txt" == LyricsFolder.GetFiles()[i].Name)
            {
                GameObject obj = Instantiate(lyricsPrefab, lyricsParent);   //오브젝트생성

                obj.name = LyricsFolder.GetFiles()[i].Name; //이름 넣기

                obj.GetComponent<RectTransform>().localScale = Vector3.one; //크기 설정

                //Debug.Log("obj.name : " + obj.name);

                // 각각의 텍스트 파일 내용 저장할 변수
                // 읽어올 text file 의 경로를 지정 합니다.
                // C:\Users\user\Music\lyrics\
                string path = @"" + _path + "/" + obj.name;

                // text file 의 전체 text 를 읽어 옵니다.
                string textValue = System.IO.File.ReadAllText(path);

                // 읽어온 내용을 TextMeshProUGUI에 넣고 리스트뷰에 출력.
                //Debug.Log("::: "+textValue);
                lyricsList = obj.GetComponentsInChildren<TextMeshProUGUI>();
                lyricsList[0].gameObject.GetComponent<TextMeshProUGUI>().SetText(textValue);
            }
        }
    }

    void Update()
    {
        // 음악 리모컨
        if (BottomPanelMove.remoteMusic)
        {
            if (ContentRaycast.raycastName == "ClickView4")
            {
                myMusicPath = "D:/Music/wav";
                myLyricsPath = "D:/Music/wav/lyrics";
                MusicFolder = new DirectoryInfo(myMusicPath);
                LyricsFolder = new DirectoryInfo(myLyricsPath);
                StartCoroutine(UpdateMusicLibrary());
            }
            else if (ContentRaycast.raycastName == "ClickView5")
            {
                myMusicPath2 = "D:/Music/wav2";
                myLyricsPath2 = "D:/Music/wav2/lyrics";
                MusicFolder = new DirectoryInfo(myMusicPath2);
                LyricsFolder = new DirectoryInfo(myLyricsPath2);
                StartCoroutine(UpdateMusicLibrary());
            }
            else if (ContentRaycast.raycastName == "ClickView6")
            {
                myMusicPath3 = "D:/Music/wav3";
                myLyricsPath3 = "D:/Music/wav3/lyrics";
                MusicFolder = new DirectoryInfo(myMusicPath3);
                LyricsFolder = new DirectoryInfo(myLyricsPath3);
                StartCoroutine(UpdateMusicLibrary());
            }
            BottomPanelMove.remoteMusic = false;
        }

        // 음악 페이지 전환 선택
        if (ContentObjCtrl.music1)
        {
            myMusicPath = "D:/Music/wav";   //앨범 주소
            myLyricsPath = "D:/Music/wav/lyrics";   //가서 주소

            MusicFolder = new DirectoryInfo(myMusicPath);   //음악경로
            LyricsFolder = new DirectoryInfo(myLyricsPath); //가사경호
            s_nameCompare = "ㅋㅋ";    //제목 초기값
            StartCoroutine(UpdateMusicLibrary());   //앨범노래 리스트 넣는거
            ContentObjCtrl.music1 = false;  //음악페이지 이동 끝났다는 여부

            //페이지 전환 시 노래 주소 변경 여부
            path1 = true;
            path2 = false;
            path3 = false;
        }
        else if (ContentObjCtrl.music2)
        {
            myMusicPath2 = "D:/Music/wav2";
            myLyricsPath2 = "D:/Music/wav2/lyrics";

            MusicFolder = new DirectoryInfo(myMusicPath2);
            LyricsFolder = new DirectoryInfo(myLyricsPath2);

            s_nameCompare = "ㅋㅋ";
            StartCoroutine(UpdateMusicLibrary());
            ContentObjCtrl.music2 = false;
            path1 = false;
            path2 = true;
            path3 = false;
        }
        else if (ContentObjCtrl.music3)
        {
            myMusicPath3 = "D:/Music/wav3";
            myLyricsPath3 = "D:/Music/wav3/lyrics";

            MusicFolder = new DirectoryInfo(myMusicPath3);
            LyricsFolder = new DirectoryInfo(myLyricsPath3);
            s_nameCompare = "ㅋㅋ";
            StartCoroutine(UpdateMusicLibrary());
            ContentObjCtrl.music3 = false;
            path1 = false;
            path2 = false;
            path3 = true;
        }

        //지금 플레이가 되고 있는지 검사 조건
        if (audioSource.isPlaying)
        {
            //Debug.Log(nametest+":::"+page_songText.text);
            if (s_nameCompare != page_songText.text)
            {
                if (path1)
                {
                    CreateLyricsPrefab(myLyricsPath);
                    s_nameCompare = page_songText.text;
                }
                else if (path2)
                {
                    CreateLyricsPrefab(myLyricsPath2);
                    s_nameCompare = page_songText.text;
                }
                else if (path3)
                {
                    CreateLyricsPrefab(myLyricsPath3);
                    s_nameCompare = page_songText.text;
                }
            }

            showTitle();
            //setVol();
            showPlayTime();

            progressBar.maxValue = audioClips[currentTrack].length;
            progressBar.value = audioSource.time;

            page_progressBar.maxValue = audioClips[currentTrack].length;
            page_progressBar.value = audioSource.time;
        }

        // 터치 버튼 큐브상태로 조작
        if (TouchSystem.playState)
        {
            playMusicBtn();
            TouchSystem.playState = false;
        }
        else if (TouchSystem.pauseState)
        {
            pause();
            TouchSystem.pauseState = false;
        }
        else if (TouchSystem.prevState)
        {
            previous();
            TouchSystem.prevState = false;
        }
        else if (TouchSystem.nextState)
        {
            next();
            TouchSystem.nextState = false;
        }
        else if (TouchSystem.randomState)
        {
            RandomloopMusic();
            TouchSystem.randomState = false;
        }
        else if (TouchSystem.unRandomState)
        {
            nonRandomloopMusic();
            TouchSystem.unRandomState = false;
        }
        else if (TouchSystem.loopState)
        {
            loopMusic();
            TouchSystem.loopState = false;
        }
        else if (TouchSystem.unLoopState)
        {
            unloopMusic();
            TouchSystem.unLoopState = false;
        }
    }


    public void playMusicBtn()
    {
        play();
    }

    public void play()
    {

        Play_Button.SetActive(false);
        Pause_Button.SetActive(true);

        //플레이버튼 큐브 비활성화
        playCube.SetActive(false);  
        page_playCube.SetActive(false);

        //플레이버튼 터치 
        if (TouchSystem.playUnClick)
        {
            //일시정지버튼 큐브 활성화
            pauseCube.SetActive(true);
            page_pauseCube.SetActive(true);
            TouchSystem.pauseUnClick = false;
        }

        page_Play_Button.SetActive(false);
        page_Pause_Button.SetActive(true);

        if (isPaused == true)
        {
            audioSource.UnPause();
            isPaused = false;
        }
        if (audioSource.isPlaying)
            return;

        currentTrack--;
        if (currentTrack < 0)
            currentTrack = audioClips.Length - 1;

        StartCoroutine("waitformusic");
    }

    public void pause()
    {
        Play_Button.SetActive(true);
        Pause_Button.SetActive(false);

        if (TouchSystem.pauseUnClick)
        {
            playCube.SetActive(true);
            page_playCube.SetActive(true);
            TouchSystem.playUnClick = false;
        }
        pauseCube.SetActive(false);
        page_pauseCube.SetActive(false);

        page_Play_Button.SetActive(true);
        page_Pause_Button.SetActive(false);

        audioSource.Pause();
        isPaused = true;
        StopCoroutine("waitformusic");
    }


    IEnumerator waitformusic()
    {

        // 노래가 돌고있는지 체크
        while (audioSource.isPlaying)
        {
            string[] _trimStrings = audioSource.clip.name.Split(Convert.ToChar("."));
            songName.text = audioSource.clip.name.Substring(0, audioSource.clip.name.IndexOf(_trimStrings[_trimStrings.Length - 1]) - 1);
            page_songText.text = audioSource.clip.name.Substring(0, audioSource.clip.name.IndexOf(_trimStrings[_trimStrings.Length - 1]) - 1);
            playTime = (int)audioSource.time;
            yield return null;
        }

        next();
    }

    public void playMine(int value)
    {
        audioSource.Stop();
        audioSource.clip = audioClips[value];
        audioSource.Play();

    }

    public void next()
    {
        audioSource.Stop();

        if (IsRandomloop)
        {
            RandomMusic();
        }
        else
        {
            currentTrack++;
            if (currentTrack > audioClips.Length - 1)   // 현재 앨범트랙의 최소/최대범위 제한
                currentTrack = 0;
            audioSource.clip = audioClips[currentTrack];    // 현재 트랙
            audioSource.time = 0;   // 시간 초기화
            audioSource.Play();

            StartCoroutine("waitformusic");
        }
    }

    public void previous()
    {
        audioSource.Stop();

        if (IsRandomloop)
        {
            RandomMusic();
        }
        else
        {
            currentTrack--;
            if (currentTrack < 0)
                currentTrack = audioClips.Length - 1;
            audioSource.clip = audioClips[currentTrack];
            audioSource.time = 0;
            audioSource.Play();

            StartCoroutine("waitformusic");
        }
    }

    public void RandomMusic()
    {
        int m_Rand;
        audioSource.Stop();
        // 랜덤 설정
        while ((m_Rand = UnityEngine.Random.Range(0, audioClips.Length - 1)) == currentTrack)
        {
            continue;
        }

        currentTrack = m_Rand;

        audioSource.clip = audioClips[currentTrack];
        audioSource.time = 0;
        audioSource.Play();

        StartCoroutine("waitformusic");
    }

    // 제목
    void showTitle()
    {
        // 확장자 제거
        string[] _trimStrings = audioSource.clip.name.Split(Convert.ToChar("."));

        songName.text = audioSource.clip.name.Substring(0, audioSource.clip.name.IndexOf(_trimStrings[_trimStrings.Length - 1]) - 1);
        page_songText.text = audioSource.clip.name.Substring(0, audioSource.clip.name.IndexOf(_trimStrings[_trimStrings.Length - 1]) - 1);
        fullLength = (int)audioSource.clip.length;
    }

    // 시간계산 출력
    void showPlayTime()
    {
        sec = playTime % 60;
        min = (playTime / 60) % 60;

        timeText.text = min + ":" + sec.ToString("D2") + "/" + ((fullLength / 60) % 60) + ":" + (fullLength % 60).ToString("D2");
        page_timeText.text = min + ":" + sec.ToString("D2") + "/" + ((fullLength / 60) % 60) + ":" + (fullLength % 60).ToString("D2");
    }

    // 볼륨세팅
    //void setVol()
    //{
    //    //page_Volume_Slider.value = Volume_Slider.value;
    //    //audioSource.volume = page_Volume_Slider.value;

    //    //Debug.Log("prev_vol_value : " + prev_vol_value);

    //    //if (audioSource.volume == 0)
    //    //{
    //    //    // 음소거 상태
    //    //    unmute();
    //    //}
    //    //else
    //    //{
    //    //    // 일반 상태
    //    //    mute();
    //    //}
    //}

    /// <summary>
    /// 재생시간조절 이벤트 함수
    /// </summary>
    public void SetProgressBarDown()
    {
        // 1. 정지
        pause();

        // 2. audioSource.time 조절
        audioSource.time = progressBar.value;
        playTime = (int)audioSource.time;
    }

    public void SetProgressBarUp()
    {
        // SliderBar를 뗄 때
        // 마우스 포인트 떼면 1초뒤 시작
        Invoke("play", 0.8f);
    }

    // 반복
    public void loopMusic()
    {
        Loop_Button.SetActive(false);
        Unloop_Button.SetActive(true);

        loopCube.SetActive(false);
        page_loopCube.SetActive(false);
        if (TouchSystem.loopUnClick)
        {
            unLoopCube.SetActive(true);
            page_unLoopCube.SetActive(true);
            TouchSystem.unLoopUnClick = false;
        }

        page_Loop_Button.SetActive(false);
        page_Unloop_Button.SetActive(true);

        // 반복끌거임
        audioSource.loop = false;
    }

    public void unloopMusic()
    {
        Loop_Button.SetActive(true);
        Unloop_Button.SetActive(false);

        if (TouchSystem.unLoopUnClick)
        {
            loopCube.SetActive(true);
            page_loopCube.SetActive(true);
            TouchSystem.loopUnClick = false;
        }

        unLoopCube.SetActive(false);
        page_unLoopCube.SetActive(false);

        page_Loop_Button.SetActive(true);
        page_Unloop_Button.SetActive(false);

        // 반복듣기할거임
        audioSource.loop = true;
    }

    public void RandomloopMusic()
    {
        // 버튼 설정
        Random_Button.SetActive(false);
        page_Random_Button.SetActive(false);

        randomCube.SetActive(false);
        page_randomCube.SetActive(false);
        if (TouchSystem.randomUnClick)
        {
            unRandomCube.SetActive(true);
            page_unRandomCube.SetActive(true);
            TouchSystem.unRandomUnClik = false;
        }

        NonRandom_Button.SetActive(true);
        page_NonRandom_Button.SetActive(true);

        // 랜덤상태 기억
        IsRandomloop = true;
    }

    public void nonRandomloopMusic()
    {
        // 버튼 설정
        Random_Button.SetActive(true);
        page_Random_Button.SetActive(true);

        if (TouchSystem.unRandomUnClik)
        {
            randomCube.SetActive(true);
            page_randomCube.SetActive(true);
            TouchSystem.randomUnClick = false;
        }

        unRandomCube.SetActive(false);
        page_unRandomCube.SetActive(false);

        NonRandom_Button.SetActive(false);
        page_NonRandom_Button.SetActive(false);

        // 랜덤상태 기억
        IsRandomloop = false;
    }

}