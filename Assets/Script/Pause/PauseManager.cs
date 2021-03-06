﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading;

public class PauseManager : MonoBehaviour
{
    private SettingManager theSM;

    public GameObject PausePanel;
   
    void Start()
    {
        //theSM = SettingManager.instance;
        theSM = FindObjectOfType<SettingManager>();

        if (GameManager.instance.GetPlayState() != GameManager.PlayState.Ending)
            UIManager.instance.SetIsPausedFalse();
    }

    void Update()
    {
        /*키입력 받아 일시정지*/
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if ((GameManager.instance.GetPlayState() == GameManager.PlayState.Tutorial || GameManager.instance.GetPlayState() == GameManager.PlayState.Act)
                && !MiniMapManager.instance.IsMiniMapOpen() && !UIManager.instance.IsBookOpened() && !UIManager.instance.GetIsOpenedParchment())
            {
                if (UIManager.instance.GetIsPaused() == false)
                {
                    UIManager.instance.SetIsPausedTrue();
                    OpenPausePanel();
                }
                else if (UIManager.instance.GetIsPaused() == true && SettingManager.instance.GetIsSetting() == false)
                {
                    BacktoGame();
                }
                else if (UIManager.instance.GetIsPaused() == true && SettingManager.instance.GetIsSetting() == true)
                {
                    Debug.Log("환경설정 저장됨");
                    theSM.SaveCurSetting();//환경설정 저장
                    SettingManager.instance.CloseSettingPanel();
                }
            }
            else if (GameManager.instance.GetPlayState() == GameManager.PlayState.Ending)
            {
                if (EndingManager.instance.GetIsPaused() == false)
                {
                    EndingManager.instance.SetIsPausedTrue();
                    OpenPausePanel();
                }
                else if (EndingManager.instance.GetIsPaused() == true && SettingManager.instance.GetIsSetting() == false)
                {
                    BacktoGame();
                }
                else if (EndingManager.instance.GetIsPaused() == true && SettingManager.instance.GetIsSetting() == true)
                {
                    Debug.Log("환경설정 저장됨");
                    theSM.SaveCurSetting();//환경설정 저장
                    SettingManager.instance.CloseSettingPanel();
                }
            }
        }
    }

    public void Setting()
    {
        SettingManager.instance.OpenSettingPanel();
    }

    public void BacktoGame()
    {
        EffectManager.instance.Play("버튼 클릭음");
        if (GameManager.instance.GetPlayState() != GameManager.PlayState.Ending)
        {
            UIManager.instance.SetIsPausedFalse();
        }
        else
        {
            EndingManager.instance.SetIsPausedFalse();
        }
        ClosePausePanel();       
    }

    public void BacktoMain()
    {
        EffectManager.instance.Play("버튼 클릭음");
        ClosePausePanel();

        if (GameManager.instance.GetPlayState() != GameManager.PlayState.Ending)
        {
            UIManager.instance.SetIsPausedFalse();
        }
        else
        {
            EndingManager.instance.SetIsPausedFalse();
        }

        // 세이브
        //GameManager.instance.thread = new Thread(GameManager.instance.SaveGameData);
        //GameManager.instance.thread.IsBackground = true;
        //GameManager.instance.thread.Start();

        StartCoroutine(LoadAsyncTitleScene());
    }

    void OpenPausePanel()
    {
        PausePanel.SetActive(true);
    }

    void ClosePausePanel()
    {
        PausePanel.SetActive(false);
    }

    IEnumerator LoadAsyncTitleScene()
    {
        GameManager.instance.SetPlayState(GameManager.PlayState.Title);

        SceneManager.sceneLoaded += LoadingManager.instance.LoadSceneEnd;
        yield return StartCoroutine(LoadingManager.instance.Fade(true));

        float timer = 0.0f;

        LoadingManager.instance.loadSceneName = "Title_Tmp";
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Title_Tmp");

        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            yield return null;

            timer += Time.unscaledDeltaTime;

            if (asyncLoad.progress >= 0.9f)
            {
                if (timer > 2.0f)//페이크 로딩
                {
                    asyncLoad.allowSceneActivation = true;

                    timer = 0.0f;
                    yield break;
                }
            }
        }

    }
}
