﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    static public BGMManager instance;

    public AudioClip[] clips; // BGM 리스트
    private AudioSource source;

    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);

    private float BGMVolume; //BGM 볼륨

    private int BGMnowplaying; //현재 재생중인 BGM

    /*씬 이름들*/
    public const string titlescene = "Title_Tmp";
    public const string gamescene = "BW_K";


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
    }

    void Start()
    {       
        source = GetComponent<AudioSource>();

        BGMVolume = 1f; //초기 볼륨


        AutoSelectBGM();//BGM 셀렉
        PlayBGM(BGMnowplaying);//BGM 재생

    }

    /*BGM 자동선택 - 씬과 플레이어 위치 판별*/
    public void AutoSelectBGM()
    {
        int preBGMnowPlaying = BGMnowplaying;
        int nextBGMnowPlaying = preBGMnowPlaying;

        string curscene = SceneManager.GetActiveScene().name;

        if (curscene == titlescene)//타이틀 씬 (메인)
        {
            nextBGMnowPlaying = 0;
        }
        else if (curscene == gamescene)// 인게임 씬
        {
            //플레이어 위치 받아와서 분류
            string curpos = PlayerManager.instance.GetCurrentPosition();
            string curhighpos = PlayerManager.instance.GetHigherCurrentPosition();
            nextBGMnowPlaying = GetAreaBGM(curpos, curhighpos);
        }
        //이후 프롤로그 + 엔딩 추가


        /*BGM 변경 시*/
        if (preBGMnowPlaying != nextBGMnowPlaying)
        {
            BGMnowplaying = nextBGMnowPlaying;
            FadeOutBGM();
            //StopBGM();
            PlayBGM(BGMnowplaying);
            FadeInBGM();
        }           
    }

    /*플레이어의 위치에 따른 BGM변경*/
    private int GetAreaBGM(string curpos_, string curhighpos_)
    {
        int area_num;
        switch (curhighpos_)
        {           
            case "Forest":
            case "Mansion":
            case "Chapter":
                area_num = 2;
                break;
            case "Downtown":
            case "Market":
                area_num = 3;
                break;
            case "Viallage":
            case "Harbor":
                area_num = 4;
                break;
            case "Slum":
                area_num = 5;
                break;
            default:
                area_num = 0;
                break;
        }

        if (curpos_ == "Mansion_President_Mansion_Outhouse")
            area_num = 4;
        else if (curpos_ == "Harbor_Cruise")
            area_num = 6;
        else if (curpos_ == "Harbor_Prison")
            area_num = 7;


        return area_num;
    }

    /*BGM 재생*/
    public void PlayBGM(int _playMusicTrack)
    {
        source.volume = BGMVolume;
        source.clip = clips[_playMusicTrack];
        source.Play();
    }

    /*볼륨 조정*/
    public void SetBGMVolume(float _volume)
    {
        BGMVolume = _volume;
        source.volume = BGMVolume;
    }

    /*음악 일시 정지*/
    public void PauseBGM()
    {
        source.Pause();
    }

    /*음악 일시 정지 해제*/
    public void UnpauseBGM()
    {
        source.UnPause();
    }

    /*음악 재생 중단*/
    public void StopBGM()
    {
        source.Stop();
    }

    /*음악 페이드 아웃*/
    public void FadeOutBGM()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutBGMCoroutine());
    }

    IEnumerator FadeOutBGMCoroutine()
    {
        for (float i = BGMVolume; i >= 0f; i -= 0.01f)
        {
            source.volume = i;
            yield return waitTime;
        }
    }

    /*음악 페이드 인*/
    public void FadeInBGM()
    {
        StopAllCoroutines();
        StartCoroutine(FadeInBGMCoroutine());
    }
    IEnumerator FadeInBGMCoroutine()
    {
        for (float i = 0f; i <= BGMVolume; i += 0.01f)
        {
            source.volume = i;
            yield return waitTime;
        }
    }
}