﻿using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System.IO;
using System.Threading.Tasks;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    private EventVariable eventVariable;
    public enum GameState { Idle, NewGame_Loaded, PastGame_Loaded };
    private GameState gameState;

    public enum PlayState { Title, Prologue, Tutorial, Act, Ending };
    private PlayState playState;

    public Thread thread;
    public Thread thread2;

    public List<string> playerEventIndexLists;
    public List<string> playerClueNameLists;
    public List<string> playerFirstInfoOfClueLists;

    // 대칭키 비밀번호
    private const string passwordForAES = "gmrrhkqor";
    private DataEncryption dataAES;

    public Texture2D activateMouseCursorTexture;
    private Vector2 pos;            //마우스 위치
    private Ray2D ray;              //마우스 위치에 광선
    private RaycastHit2D hit;       //쏜 광선에 닿은것이 뭔지 확인하기위한 변수

    public bool isEncrypted;        // 암호화 온오프

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void FixedUpdate()
    {
        try
        {
            if (!UIManager.instance.GetIsPaused() && !UIManager.instance.IsBookOpened() && !MiniMapManager.instance.IsMiniMapOpen() && !UIManager.instance.isPaging && !UIManager.instance.isConversationing && !UIManager.instance.isFading && !UIManager.instance.GetIsOpenedParchment())
            {
                pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                ray = new Ray2D(pos, Vector2.zero);
                hit = Physics2D.Raycast(ray.origin, ray.direction);

                if (hit.collider != null)
                {
                    if (hit.collider.tag.Equals("InteractionObject"))
                    {
                        if (DialogManager.instance.CheckInteraction(hit.collider.name))
                            SetCursorActivate();
                        else
                            SetCursorIdle();
                    }
                    else if (hit.collider.tag.Equals("MerteDesk"))
                    {
                        SetCursorActivate();
                    }
                }
                else
                {
                    SetCursorIdle();
                }
            }
        }
        catch
        {

        }
        //// 암호화 ON
        //if (Input.GetKeyDown(KeyCode.F6))
        //{
        //    isEncrypted = true;
        //}

        //// 암호화 OFF
        //if (Input.GetKeyDown(KeyCode.F7))
        //{
        //    isEncrypted = false;
        //}
    }

    // Use this for initialization
    void Start() {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        gameState = GameState.Idle;
        playState = PlayState.Title;
        eventVariable = new EventVariable();
        dataAES = new DataEncryption();

        activateMouseCursorTexture = Resources.Load<Texture2D>("Image/Cursor/Active");

        isEncrypted = true;
    }

    public void SetCursorActivate()
    {
        Cursor.SetCursor(activateMouseCursorTexture, Vector2.zero, CursorMode.Auto);
    }

    public void SetCursorIdle()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }


    public void GetClue(string clueName)
    {
        if (!PlayerManager.instance.CheckClue(clueName))
        {
            //int numOfAct = ItemDatabase.instance.FindClue(clueName);
            //Debug.Log("Act " + (numOfAct) + "의 단서인 " + clueName + "를 얻었습니다.");
            string numOfAct = ItemDatabase.instance.FindClue(clueName);
            //Debug.Log("clueName = " + clueName + " , numOfAct = " + numOfAct);
            //Debug.Log("사건 " + numOfAct + "의 단서인 " + clueName + "를 얻었습니다.");
            Inventory.instance.MakeClueSlot(clueName, numOfAct); // 수첩에 Clue slot 추가

        } else
        {
            //Debug.Log("이미 획득한 단서입니다.");
        }
    }

    public string EncryptData(string text)
    {
        return dataAES.EncryptString(text, passwordForAES);
    }

    public string DecryptData(string text)
    {
        return dataAES.DecryptString(text, passwordForAES);
    }

    public GameState GetGameState()
    {
        return gameState;
    }

    public void SetGameState(GameState gameState)
    {
        this.gameState = gameState;
    }

    public PlayState GetPlayState()
    {
        return playState;
    }

    public void SetPlayState(PlayState playState)
    {
        this.playState = playState;
    }

    // 처음 부터
    public void PlayNewGame()
    {
        // 비동기적으로 초기 데이터를 불러옴
        thread = new Thread(CSVParser.instance.InitDataFromCSV);
        thread.IsBackground = true;
        thread.Start();
        //CSVParser.instance.InitDataFromCSV();
        // 불러온 데이터를 적용시킴
        //DialogManager.instance.SetLists();
        //ItemDatabase.instance.SetLists();
    }

    // 이어 하기
    public void PlaySaveGame()
    {
        try
        {
            // 비동기적으로 저장된 데이터를 불러옴
            thread = new Thread(CSVParser.instance.LoadCSVData);
            thread.IsBackground = true;
            thread.Start();
            //CSVParser.instance.LoadCSVData();
            // 불러온 데이터를 적용시킴
            //DialogManager.instance.SetLists();
            //ItemDatabase.instance.SetLists();
            //LoadPlayerData();
            eventVariable.LoadEventVariables();
        }
        catch
        {
            // 로드 실패시 -> 저장된 게임 데이터가 없을 시
            gameState = GameState.Idle;
            return;
        }
    }

    // PlayerManager.cs 에서 비동기적으로 사용되는 함수
    public void SaveGameData()
    {
        CSVParser.instance.SaveCSVData();
        SavePlayerData();
        eventVariable.SaveEventVariables();
    }

    /* Load된 Player의 정보를 적용 */
    /* 클래스 변수에 넣어진 값들을, 실제 Player 데이터로 적용함 */
    public void LoadPlayerData()
    {
        PlayerInfo tempPlayerInfo = new PlayerInfo();

        tempPlayerInfo.LoadPlayerInfo();
        
        PlayerManager.instance.NumOfAct = tempPlayerInfo.GetNumOfAct();
        PlayerManager.instance.TimeSlot = tempPlayerInfo.GetTimeSlot();

        playerEventIndexLists = tempPlayerInfo.GetEventIndexList();
        playerClueNameLists = tempPlayerInfo.GetPlayerClueNameLists();
        playerFirstInfoOfClueLists = tempPlayerInfo.GetFirstInfoOfClueLists(); // 로드 끝

        for (int i = 0; i < playerEventIndexLists.Count; i++)
        {
            PlayerManager.instance.AddEventCodeToList(playerEventIndexLists[i]);
        }

        for (int i = 0; i < playerClueNameLists.Count; i++)
        {
            GetClue(playerClueNameLists[i]);
            PlayerManager.instance.playerClueLists[i].SetFirstInfoOfClue(playerFirstInfoOfClueLists[i]);
        }
    }

    /* Player의 정보를 Save */
    public void SavePlayerData()
    {
        PlayerInfo tempPlayerInfo = new PlayerInfo(PlayerManager.instance.NumOfAct, PlayerManager.instance.TimeSlot, PlayerManager.instance.GetPlayedEventList(), PlayerManager.instance.playerClueLists);

        tempPlayerInfo.SavePlayerInfo();
    }

    public void SetEventVariable(ref EventVariable eventVariable)
    {
        this.eventVariable = eventVariable;
    }
}
