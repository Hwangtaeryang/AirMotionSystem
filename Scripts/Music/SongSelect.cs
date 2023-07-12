using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongSelect : MonoBehaviour {

    // 클릭한 노래 재생
	public void SelectME()
	{
        // ListPrefabs 으로 받은 노래의 제목 : 부모이름 보내줘야함
	    SoundManager.Instance.SelectSongByName(transform.parent.name);
		//Debug.Log(transform.parent.name);
	}

}
