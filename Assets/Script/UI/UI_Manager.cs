using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Manager : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI m_timeText;
    [SerializeField]
    TextMeshProUGUI m_rankNumText, m_rankText;
    [SerializeField]
    TextMeshProUGUI m_scoreText;

    [SerializeField]
    Color m_1stColor;
    [SerializeField]
    Color m_2ndColor;
    [SerializeField]
    Color m_3rdColor;
    [SerializeField]
    Color m_4thColor;

    [SerializeField]
    Color m_1stOutLineColor;
    [SerializeField]
    Color m_2ndOutLineColor;
    [SerializeField]
    Color m_3rdOutLineColor;
    [SerializeField]
    Color m_4thOutLineColor;

    [Header("------------------")]
    // ミニUI
    [SerializeField, Tooltip("Cat->Duck->Penguin\nAI区別用列挙型の順番")]
    GameObject[] m_enemyUI = new GameObject[3];

    GameManager m_gameManager;
    Camera m_mainCamera;
    GameObject[] m_enemys;
    Player m_player;

    //enum EnRankColor: uint
    //{
    //    en1st = 0xEABF00FF,
    //    en2nd = 0xC9C9C9FF,
    //    en3rd = 0xC96A18FF,
    //    en4th = 0x0000FFFF
    //}

    void Awake()
    {
        // ゲームマネージャーを探す
        m_gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        // メインカメラを取得
        m_mainCamera = Camera.main;
        // プレイヤーを取得
        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        // 敵をまとめて取得
        m_enemys = GameObject.FindGameObjectsWithTag("Enemy");

        // 最初はUIを透明に
        foreach (GameObject uiObj in m_enemyUI)
        {
            uiObj.SetActive(false);
        }

    }

    void Update()
    {
        // 現在の時間を表示
        int Minit = m_gameManager.GetMinit();
        float second = m_gameManager.GetSecond();
        if (m_gameManager.GetSecond() >= 60.0f)
        {
            Minit++;
            second = 0.0f;
        }

        string timeString = Minit.ToString("00");
        timeString += ":";
        timeString += Mathf.Floor(second).ToString("00");

        m_timeText.text = timeString;

        // スコア更新
        m_scoreText.text = "" + m_player.GetEatFoods() + " pt";
        
        // ステータス更新
        StatusUpdate();

        //プレイヤーの順位更新
        PlayerRank();
    }

    void StatusUpdate()
    {
        for(int i = 0; i < m_enemyUI.Length; i++)
        {
            // ターゲットの座標を探す
            Vector3 targetPos = Vector3.zero;
            AI targetAI = null;

            foreach(GameObject target in m_enemys)
            {
                targetAI = target.GetComponent<AI>();

                if ((int)targetAI.GetAICharactor() == i)
                {
                    // 1番目の子供の座標
                    targetPos = target.transform.GetChild(1).transform.position;
                    break;
                }
            }

            // 少し持ち上げる
            targetPos.y += 14.0f;

            // カメラからターゲットへのベクトル
            Vector3 targetDir = targetPos - m_mainCamera.transform.position;

            // 内積を使ってカメラ前方かどうかを判定
            if(Vector3.Dot(m_mainCamera.transform.forward, targetDir) < 0)
            {
                m_enemyUI[i].SetActive(false);
            }
            else
            {
                m_enemyUI[i].SetActive(true);

                // 距離でも表示切替
                if (targetDir.sqrMagnitude >= 80000.0f)
                {
                    m_enemyUI[i].SetActive(false);
                }
            }

            // 座標更新
            m_enemyUI[i].transform.position
                = RectTransformUtility.WorldToScreenPoint(Camera.main, targetPos);

            // 表示更新
            m_enemyUI[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
                "" + targetAI.GetEatFoods() + "pt";

        }

    }

    void PlayerRank()
    {
        //プレイヤーの食べ物を食べた数
        int playerEatFoods = m_player.GetEatFoods();
        int PlayerRank = 4;

        for(int i=0;i<m_enemys.Length;i++)
        {
            if(m_enemys[i].GetComponent<AI>().GetEatFoods()< playerEatFoods)
            {
                PlayerRank--;
            }

        }

        switch(PlayerRank)
        {
            case 1:
                m_rankNumText.text = "1";
                m_rankNumText.color = m_1stColor;
                m_rankNumText.outlineColor = m_1stOutLineColor;

                m_rankText.text = "st";
                m_rankText.color = m_1stColor;
                m_rankText.outlineColor = m_1stOutLineColor;
                break;
            case 2:
                m_rankNumText.text = "2";
                m_rankNumText.color = m_2ndColor;
                m_rankNumText.outlineColor = m_2ndOutLineColor;

                m_rankText.text = "nd";
                m_rankText.color = m_2ndColor;
                m_rankText.outlineColor = m_2ndOutLineColor;
                break;
            case 3:
                m_rankNumText.text = "3";
                m_rankNumText.color = m_3rdColor;
                m_rankNumText.outlineColor = m_3rdOutLineColor;

                m_rankText.text = "rd";
                m_rankText.color = m_3rdColor;
                m_rankText.outlineColor = m_3rdOutLineColor;
                break;
            case 4:
                m_rankNumText.text = "4";
                m_rankNumText.color = m_4thColor;
                m_rankNumText.outlineColor = m_4thOutLineColor;

                m_rankText.text = "th";
                m_rankText.color = m_4thColor;
                m_rankText.outlineColor = m_4thOutLineColor;
                break;
            default:
                break;
        }

        

    }

}
