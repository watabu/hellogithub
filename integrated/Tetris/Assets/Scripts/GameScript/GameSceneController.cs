﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//Buttonクラスのスクリプトの参照用
using UnityEngine.SceneManagement;

//ゲームの進行を管理するクラス
//ゲームの開始時や終了時にUIを出させたり
//ミノのコントローラに新しいミノを登録させる
//
//3/21 １プレイヤー分のオブジェクトをまとめて管理するクラスと分離

//4/ 1  SendOjama関数を実装
//     プレイヤーにオジャマミノを送れるように
//4/3   PauseUIを追加　ポーズ画面を表示するUI

public class GameSceneController : MonoBehaviour {
    //各クラスの参照
    [Header("1P Object References")]
    public PlayerControllManager playerControll1P;
    [Header("2P Object References")]
    public PlayerControllManager playerControll2P;
    [Space(1.0f)]
    public WinLoseDrawer result;

    //UIの参照
    [Header("UI Prefab")]
    public GameObject readyCanvasPrefab;
    public GameObject endDialogPrefab;
    public GameObject pauseUI;

    //シーンの状態を表す変数
    private enum GameState
    {
        Null,
        Ready,
        Playing,
        End,
    }
    [SerializeField]
    private GameState state=GameState.Null;

    // Use this for initialization
    void Start () {
        GenerateStartGUI();
        if (PlInput.PlayingNum <= 0) PlInput.PlayingNum = 2;
        if (PlInput.PlayingNum == 1)
        {
           // playerControll2P.gameBoard.enabled = false;
           // playerControll2P.minoRegister.enabled = false;
            GameObject gameboard2P = GameObject.Find("2PGameBoard");
            gameboard2P.SetActive(false);
            GameObject gameboard1P = GameObject.Find("1PGameBoard");
            Vector3 vector = gameboard1P.transform.position;
            vector.x = -3;
            gameboard1P.transform.position = vector;
        }
    }
	
	// Update is called once per frame
	void Update () {
	}

    void GenerateStartGUI()
    {
        //ゲーム開始時のUIをPrefabから生成
        GameObject readyCanvas = UsefulFunctions.CloneObject(readyCanvasPrefab);

        //UIの中のボタンが押されたときゲームを開始させるように関数を登録
        //どうやらゲーム中にSubmittをするとこのボタンを押してしまうらしい
        GameObject button = readyCanvas.transform.Find("Button").gameObject;
        button.GetComponent<Button>().onClick.AddListener(StartGame);
        Button startButton = GameObject.Find("/Canvas(Clone)/Button").GetComponent<Button>();
        startButton.Select();
        //button.SetActive(false);
    }
    void GenerateEndGUI()
    {
        GameObject endCanvas = UsefulFunctions.CloneObject(endDialogPrefab);

        //UIの中のボタンが押されたときゲームを開始させるように関数を登録
        GameObject button = endCanvas.transform.Find("Restart").gameObject;
        button.GetComponent<Button>().onClick.AddListener(()=> { SceneManager.LoadScene("Game"); });
        /*button.GetComponent<Button>().onClick.AddListener(GenerateStartGUI);
        button.GetComponent<Button>().onClick.AddListener(() => { Destroy(endCanvas); });*/
        GameObject end = endCanvas.transform.Find("End").gameObject;
        end.GetComponent<Button>().onClick.AddListener(() => { SceneManager.LoadScene("Title"); });
        Button restartButton = GameObject.Find("/GameOverUI(Clone)/Restart").GetComponent<Button>();
        restartButton.Select();
    }

    //ゲームを開始させる関数
    public void StartGame()
    {
        Debug.Log(PlInput.PlayingNum);
        state = GameState.Ready;
        if (PlInput.PlayingNum == 1)
        {
            playerControll1P.StartGame();
        }
        else if (PlInput.PlayingNum == 2)
        {
            playerControll1P.StartGame();
            playerControll2P.StartGame();
        }
    }

    //ゲームを終了させる関数
    //minoFilledBoardID : -1 引き分け 0 1Pのボードが埋まった 1 2Pのボードが埋まった
    public void EndGame(int minoFilledBoardID)
    {
        state = GameState.End;
        switch (minoFilledBoardID)
        {
            case 0:
                result.ShowResult(ResultState.Win2P);
                break;
            case 1:
                result.ShowResult(ResultState.Win1P);
                break;
        }
        //ゲーム終了時のUIをPrefabから生成
        GenerateEndGUI();
        playerControll1P.EndGame();
        playerControll2P.EndGame();
    }
    public void Pause(bool pause)
    {
        PauseUI pauseScript = pauseUI.GetComponent<PauseUI>();
        if (pause)
        {
            if (!pauseUI.active)
            {
                pauseUI.SetActive(true);
            }
            pauseScript.FadeIn();
        }
        else
        {
            pauseScript.FadeOut();
        }
    }
    //プレイヤーにオジャマミノを送る関数
    //playerNum 送られるプレイヤーの番号 holeX 穴をあける座標
    public void SendOjama(int playerNum,int height)
    {
        switch (playerNum)
        {
            case 0:
                playerControll1P.SetOjama(height);
                break;
            case 1:
                playerControll2P.SetOjama(height);
                break;
        }
    }

}
