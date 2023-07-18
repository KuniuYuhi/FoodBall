using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
