using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    enum GameState
    {
        enGameMode_Ready,   // カウントダウン
        enGameMode_Play,    // プレイ中
        enGameMode_End,     // 終了
        enGameMode_Pause,   // ポーズ
    }

    [SerializeField, Header("ゲームの状態")]
    GameState m_gameState = GameState.enGameMode_Play;

    // タイマー
    [SerializeField, Header("デフォルト制限時間（分）")]
    int m_defLimitMinit;

    // 現在のタイマー
    int m_timerMinit;
    public int GetMinit()
    {
        return m_timerMinit;
    }
    float m_timerSecond;
    public float GetSecond()
    {
        return m_timerSecond;
    }

    void Awake()
    {
        // UIの生成
        SceneManager.LoadScene("UI", LoadSceneMode.Additive);

        // 時間の初期化
        m_timerMinit = m_defLimitMinit - 1;
        m_timerSecond = 60.0f;
    }

    void Update()
    {
        switch (m_gameState)
        {
            case GameState.enGameMode_Ready:

                break;
            case GameState.enGameMode_Play:

                // タイマー加算
                m_timerSecond -= Time.deltaTime;
                if (m_timerSecond <= 0.0f)
                {
                    // 1分経過
                    m_timerMinit--;
                    m_timerSecond += 60.0f;

                    // 終了
                    if (m_timerMinit < 0)
                    {
                        m_gameState = GameState.enGameMode_End;
                    }
                }

                break;
            case GameState.enGameMode_End:

                break;
            case GameState.enGameMode_Pause:

                break;
        }

    }

}
