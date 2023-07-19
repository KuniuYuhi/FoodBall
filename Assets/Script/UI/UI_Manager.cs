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

    [Header("------------------")]
    // ミニUI
    [SerializeField, Tooltip("Cat->Duck->Penguin\nAI区別用列挙型の順番")]
    GameObject[] m_enemyUI = new GameObject[3];
    [SerializeField]
    RectTransform m_parentUI;

    GameManager m_gameManager;
    Camera m_mainCamera;
    GameObject[] m_enemys;

    void Awake()
    {
        // ゲームマネージャーを探す
        m_gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        // メインカメラを取得
        m_mainCamera = Camera.main;
        // 敵をまとめて取得
        m_enemys = GameObject.FindGameObjectsWithTag("Enemy");

        // 最初はUIを透明に
        foreach(GameObject uiObj in m_enemyUI)
        {
            uiObj.SetActive(false);
        }

    }

    void Update()
    {
        // 現在の時間を表示
        string timeString = m_gameManager.GetMinit().ToString("00");
        timeString += ":";
        timeString += Mathf.Floor(m_gameManager.GetSecond()).ToString("00");

        m_timeText.text = timeString;

        // ステータス更新
        StatusUpdate();
    }

    void StatusUpdate()
    {
        // なぜか動かないので止める
        return;

        for(int i = 0; i < m_enemyUI.Length; i++)
        {
            // ターゲットの座標を探す
            Vector3 targetPos = Vector3.zero;

            foreach(GameObject target in m_enemys)
            {
                if ((int)target.GetComponent<AI>().GetAICharactor() == i)
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
            bool isFront = Vector3.Dot(m_mainCamera.transform.forward, targetDir) > 0;

            // 距離でも表示切替
            if (targetDir.sqrMagnitude >= 80000.0f)
            {
                isFront = false;
            }

            // カメラ前方ならUI表示、後方なら非表示
            m_enemyUI[i].SetActive(isFront);

            if (isFront == false)
            {
                return;
            }

            // オブジェクトのワールド座標→スクリーン座標変換
            Vector3 targetScreenPos = m_mainCamera.WorldToScreenPoint(targetPos);

            // スクリーン座標変換→UIローカル座標変換
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                m_parentUI,
                targetScreenPos,
                null,
                out Vector2 uiLocalPos
            );

            // RectTransformのローカル座標を更新
            m_enemyUI[i].GetComponent<RectTransform>().localPosition = targetScreenPos;

        }

    }

}
