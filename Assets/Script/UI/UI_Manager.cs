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

    [Header("------------------")]
    // ミニUI
    [SerializeField, Tooltip("Cat->Duck->Penguin\nAI区別用列挙型の順番")]
    GameObject[] m_enemyUI = new GameObject[3];

    GameManager m_gameManager;
    Camera m_mainCamera;
    GameObject[] m_enemys;
    Player m_player;

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

}
