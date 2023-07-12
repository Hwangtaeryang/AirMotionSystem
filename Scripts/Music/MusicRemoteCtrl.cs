using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.IO;



[RequireComponent(typeof(AudioSource))]
public class MusicRemoteCtrl : MonoBehaviour
{
    public static MusicRemoteCtrl Instance;
    DirectoryInfo MusicFolder;
    WWW myClip;
    string myPath; string myPath2; string myPath3;

    private GameObject[] listings;
    private int index = 0;
    private Component[] songList;
    private bool IsRandomloop = false;
    //public GameObject listingPrefab;
    //public Transform listingsParent;
    public int songNumber = 0;


    [Header("Buttons in MainPage")]
    public GameObject Play_Button;
    public GameObject Pause_Button;
    public GameObject Next_Button;
    public GameObject Previous_Button;
    //public GameObject Mute_Button;
    //public GameObject Unmute_Button;
    public GameObject Loop_Button;
    public GameObject Unloop_Button;
    public GameObject Random_Button;
    public GameObject NonRandom_Button;
    public Slider progressBar;//, Volume_Slider;
    private float prev_vol_value;

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
    public bool isPaused = false;



    


    void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
#if UNITY_ANDROID
          myPath = "/storage/sdcard0/Music";
#endif
#if UNITY_STANDALONE
        //myPath = "C:/Users/HUNNY/Music/ogg";
        //myPath = "C:/Users/user/Music/mp3";
        //myPath = "C:/Users/user/Music/wav";
        //myPath = "E:/Music/wav";
        //myPath2 = "E:/Music/wav2";
        //myPath3 = "E:/Music/wav3";
#endif
#if Unity_Editor
          myPath = "Builds/Music";
#endif
        // 폴더 정보 들고오기
        //if (ContentRaycast.raycastName == "ClickView4")
        //{
        //    MusicFolder = new DirectoryInfo(myPath);
        //}
        //else if (ContentRaycast.raycastName == "ClickView5")
        //{
        //    MusicFolder = new DirectoryInfo(myPath2);
        //}
        //else if (ContentRaycast.raycastName == "ClickView6")
        //{
        //    MusicFolder = new DirectoryInfo(myPath3);
        //}
        //else
        //{
        //    MusicFolder = new DirectoryInfo(myPath);
        //}

    }


    public void Start()
    {
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
        // 노래 목록 가져오기
        //StartCoroutine(UpdateMusicLibrary());

    }


    void MusicChoice()
    {

    }

    // 노래 목록 세팅
    private IEnumerator UpdateMusicLibrary()
    {
        int length = MusicFolder.GetFiles().Length;

        audioClips = new AudioClip[length];
        listings = new GameObject[length];

        // 음악폴더안에 파일을 가져와서 각각에 대입
        for (int i = 0; i < MusicFolder.GetFiles().Length; i++)
        {

            //if (!(MusicFolder.GetFiles()[i].FullName.Contains("wav") || MusicFolder.GetFiles()[i].FullName.Contains("ogg")))
            //    continue;
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
            songNumber++;
            //GameObject obj = Instantiate(listingPrefab, listingsParent);

            //obj.transform.SetParent(listingsParent);
            //   obj.transform.parent = listingsParent;

            //obj.name = MusicFolder.GetFiles()[i].Name;

            //obj.GetComponent<RectTransform>().localScale = Vector3.one;

            //listings[i] = obj;

            //Debug.Log("listings : " + listings[i]);

            audioClips[i] = myClip.GetAudioClip();
            audioClips[i].name = MusicFolder.GetFiles()[i].Name;

            while (audioClips[i].loadState != AudioDataLoadState.Loaded)
            {
                yield return null;
            }

            //Debug.Log("songList : " + songList.Length);

            // 확장자 뒤에서부터 잘라서 TextGUI에 넣기
            if (MusicFolder.GetFiles()[i].FullName.Contains(".wav") /*|| MusicFolder.GetFiles()[i].FullName.Contains(".mp3") || MusicFolder.GetFiles()[i].FullName.Contains(".ogg")*/)
            {
                string trimString = MusicFolder.GetFiles()[i].Name;
                string[] trimStrings = trimString.Split(Convert.ToChar("."));
                string inputString = trimString.Substring(0, trimString.IndexOf(trimStrings[trimStrings.Length - 1]) - 1);

                //Debug.Log("trimString : " + trimString.Substring(0, trimString.IndexOf(trimStrings[trimStrings.Length - 1]) - 1));
                
            }
            
        }
        
        // 확장자 뒤에서부터 잘라서 TextGUI에 넣기
        string[] _trimStrings = audioClips[index].name.Split(Convert.ToChar("."));
        //audioClips[index].name.Substring(0, audioClips[index].name.IndexOf(_trimStrings[_trimStrings.Length - 1]) - 1);
        audioSource.clip = audioClips[index];
        songName.text = audioClips[index].name.Substring(0, audioClips[index].name.IndexOf(_trimStrings[_trimStrings.Length - 1]) - 1);
    }

    public void SelectSongByName(string s)
    {
        string setting;
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


    void Update()
    {
        if(BottomPanelMove.remoteMusic)
        {
            if (ContentRaycast.raycastName == "ClickView4")
            {
                myPath = "E:/Music/wav";
                MusicFolder = new DirectoryInfo(myPath);
                StartCoroutine(UpdateMusicLibrary());
            }
            else if (ContentRaycast.raycastName == "ClickView5")
            {
                myPath2 = "E:/Music/wav2";
                MusicFolder = new DirectoryInfo(myPath2);
                StartCoroutine(UpdateMusicLibrary());
            }
            else if (ContentRaycast.raycastName == "ClickView6")
            {
                myPath3 = "E:/Music/wav3";
                MusicFolder = new DirectoryInfo(myPath3);
                StartCoroutine(UpdateMusicLibrary());
            }
            BottomPanelMove.remoteMusic = false;
        }




        //지금 플레이가 되고 있는지 검사 조건
        if (audioSource.isPlaying)
        {
            showTitle();
            showPlayTime();
            //setProgressBar();
            progressBar.maxValue = audioClips[currentTrack].length;
            progressBar.value = audioSource.time;
            
        }

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

        playCube.SetActive(false);

        if (TouchSystem.playUnClick)
        {
            pauseCube.SetActive(true);
            TouchSystem.pauseUnClick = false;
        }
        

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
        //  audioSource.Play();
        StartCoroutine("waitformusic");
    }

    public void pause()
    {

        Play_Button.SetActive(true);
        Pause_Button.SetActive(false);

        //Debug.Log(TouchSystem.playUnClick + ":::" + TouchSystem.pauseUnClick);
        if (TouchSystem.pauseUnClick)
        {
            playCube.SetActive(true);
            TouchSystem.playUnClick = false;
        }
        pauseCube.SetActive(false);
        

        audioSource.Pause();
        isPaused = true;
        StopCoroutine("waitformusic");
    }


    IEnumerator waitformusic()
    {

        // 노래가 돌고있는지 체크
        while (audioSource.isPlaying)
        {
            //Debug.Log("계속체크되나?");
            string[] _trimStrings = audioSource.clip.name.Split(Convert.ToChar("."));
            //audioSource.clip.name.Substring(0, audioSource.clip.name.IndexOf(_trimStrings[_trimStrings.Length - 1]) - 1);
            songName.text = audioSource.clip.name.Substring(0, audioSource.clip.name.IndexOf(_trimStrings[_trimStrings.Length - 1]) - 1);
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
            if (currentTrack > audioClips.Length - 1)
                currentTrack = 0;
            audioSource.clip = audioClips[currentTrack];
            audioSource.time = 0;
            audioSource.Play();

            StartCoroutine("waitformusic");
        }

        //StartCoroutine("waitformusic");
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

        //StartCoroutine("waitformusic");
    }



    public void RandomMusic()
    {
        //Debug.Log("뭐냐구?!?");
        int m_Rand;
        audioSource.Stop();
        // 랜덤 설정
        //currentTrack++;
        while ((m_Rand = UnityEngine.Random.Range(0, audioClips.Length - 1)) == currentTrack)
        {
            //Debug.Log("Rand : " + UnityEngine.Random.Range(0, audioClips.Length - 1) + "currentTrack : " + currentTrack);   
            continue;
        }

        currentTrack = m_Rand;

        //if (currentTrack > audioClips.Length - 1)
        //    currentTrack = 0;

        audioSource.clip = audioClips[currentTrack];

        audioSource.Play();

        StartCoroutine("waitformusic");
    }
    
    // 제목
    void showTitle()
    {
        // 확장자 제거
        string[] _trimStrings = audioSource.clip.name.Split(Convert.ToChar("."));

        songName.text = audioSource.clip.name.Substring(0, audioSource.clip.name.IndexOf(_trimStrings[_trimStrings.Length - 1]) - 1);
        fullLength = (int)audioSource.clip.length;
    }

    // 시간계산 출력
    void showPlayTime()
    {
        sec = playTime % 60;
        min = (playTime / 60) % 60;

        timeText.text = min + ":" + sec.ToString("D2") + "/" + ((fullLength / 60) % 60) + ":" + (fullLength % 60).ToString("D2");
    }
    

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

    public void SetHandleDown()
    {
        // 현재 슬라이더에 마우스 좌클릭 누르고 있을 때
        if (Input.GetMouseButton(0))
        {
            SetProgressBarDown();
        }

        // 현재 슬라이더에 마우스 좌클릭 땠을 때
        if (Input.GetMouseButtonUp(0))
        {
            SetProgressBarUp();
        }
    }
    

    // 반복
    public void loopMusic()
    {
        Loop_Button.SetActive(false);
        Unloop_Button.SetActive(true);

        loopCube.SetActive(false);
        if (TouchSystem.loopUnClick)
        {
            unLoopCube.SetActive(true);
            TouchSystem.unLoopUnClick = false;
        }
        

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
            TouchSystem.loopUnClick = false;
        }

        unLoopCube.SetActive(false);
        

        // 반복듣기할거임
        audioSource.loop = true;
    }

    public void RandomloopMusic()
    {
        // 버튼 설정
        Random_Button.SetActive(false);

        randomCube.SetActive(false);
        if (TouchSystem.randomUnClick)
        {
            unRandomCube.SetActive(true);
            TouchSystem.unRandomUnClik = false;
        }

        NonRandom_Button.SetActive(true);

        // 랜덤상태 기억
        IsRandomloop = true;
    }

    public void nonRandomloopMusic()
    {
        // 버튼 설정
        Random_Button.SetActive(true);

        if (TouchSystem.unRandomUnClik)
        {
            randomCube.SetActive(true);
            TouchSystem.randomUnClick = false;
        }

        unRandomCube.SetActive(false);

        NonRandom_Button.SetActive(false);

        // 랜덤상태 기억
        IsRandomloop = false;
    }

    public void HandleDown()
    {
        Debug.Log("MouseDown");
    }

    public void HandleUp()
    {
        Debug.Log("MouseUp");
    }
}
