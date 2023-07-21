using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// 【効果音再生関連】
    /// </summary>

    // 効果音再生関数 どこからでも呼べる
    static public OneShotAudioClip PlaySE(AudioClip clip, float volume = 1.0f,
        float pitch = 1.0f, bool isLoop = false)
    {
        // 未設定なら鳴らさない
        if (clip == null)
        {
            return null;
        }

        GameObject oneShotObj = Instantiate((GameObject)Resources.Load("OneShotSE"));

        // 効果音再生
        OneShotAudioClip audio = oneShotObj.GetComponent<OneShotAudioClip>().
            PlaySE(clip, volume, pitch, isLoop);

        return audio;
    }
    // 3D版
    static public OneShotAudioClip PlaySE3D(AudioClip clip, Vector3 position,
        float minRange, float maxRange,
        float volume = 1.0f,
        float pitch = 1.0f, bool isLoop = false)
    {
        // 未設定なら鳴らさない
        if (clip == null)
        {
            return null;
        }

        GameObject oneShotObj = Instantiate((GameObject)Resources.Load("OneShotSE"));

        // 効果音再生
        OneShotAudioClip audio = oneShotObj.GetComponent<OneShotAudioClip>().PlaySE3D(clip, position,
            minRange, maxRange, volume, pitch, isLoop);

        return audio;
    }

    /// <summary>
    /// シーン切り替え
    /// </summary>
    /// <param name="sceneName">遷移先のシーン名</param>
    /// <param name="initSprite">遷移に使う画像</param>
    /// <param name="bgm">使用BGM nullならBGM切り替えない</param>
    static public GameObject SceneChange(string sceneName, Sprite initSprite = null)
    {
        Debug.Log("シーン切り替え");
        // 生成
        GameObject fadeObj = Instantiate((GameObject)Resources.Load("FadeCanvas"));
        fadeObj.GetComponent<FadeScene>().FadeStart(sceneName, initSprite);

        return fadeObj;
    }

    public enum GameState
    {
        enGameMode_Ready,   // カウントダウン
        enGameMode_Play,    // プレイ中
        enGameMode_End,     // 終了
        enGameMode_Pause,   // ポーズ
    }

    [SerializeField, Header("ゲームの状態")]
    GameState m_gameState = GameState.enGameMode_Play;
    public GameState GetGameMode()
    {
        return m_gameState;
    }

    // タイマー
    [SerializeField, Header("デフォルト制限時間（分）")]
    int m_defLimitMinit;

    [SerializeField, Header("Ready用キャンバス")]
    GameObject ReadyCanvas;
    [SerializeField, Header("End用キャンバス")]
    GameObject EndCanvas;
    [SerializeField, Header("Pause用キャンバス")]
    GameObject PauseCanvas;

    [SerializeField]
    AudioSource m_bgm;

    public int m_volume = 8;

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

    async void Awake()
    {
        // UIの生成
        SceneManager.LoadScene("UI", LoadSceneMode.Additive);

        // 時間の初期化
        m_timerMinit = m_defLimitMinit - 1;
        m_timerSecond = 60.0f;

        ReadyCanvas.SetActive(true);
        EndCanvas.SetActive(false);
        PauseCanvas.SetActive(false);

        //待ってから開始
        await UniTask.Delay(6400);
        ReadyCanvas.SetActive(false);
        m_gameState = GameState.enGameMode_Play;

        //BGM再生
        m_bgm.Play();
    }

    void Update()
    {
        switch (m_gameState)
        {
            case GameState.enGameMode_Ready:
                // 何もしない

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
                        m_timerMinit = 0;
                        m_timerSecond = 0.0f;
                        GameEnd();
                    }
                }

                // ポーズ開始
                if (Input.GetKeyDown(KeyCode.P))
                {
                    m_gameState = GameState.enGameMode_Pause;
                    Time.timeScale = 0.0f;
                    PauseCanvas.SetActive(true);
                }

                break;
            case GameState.enGameMode_End:
                if (m_gameEnd == false)
                {
                    GameEnd();
                }

                break;

            case GameState.enGameMode_Pause:

                // ポーズ解除
                if (Input.GetKeyDown(KeyCode.P))
                {
                    m_gameState = GameState.enGameMode_Play;
                    Time.timeScale = 1.0f;
                    PauseCanvas.SetActive(false);
                }

                break;
        }

    }

    bool m_gameEnd = false;
    async void GameEnd()
    {
        //BGM停止
        m_bgm.Stop();

        m_gameState = GameState.enGameMode_End;
        EndCanvas.SetActive(true);
        m_gameEnd = true;
        await UniTask.Delay(3000);

        // データの生成
        GameData gameData = GameObject.FindGameObjectWithTag("GameData").GetComponent<GameData>();
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        GameData.ActorData[] actorDatas = new GameData.ActorData[4];

        // 取得
        // まずはプレイヤー
        actorDatas[0].actorCharacter = GameData.ActorCharacter.enActorCharacter_Sheep;
        actorDatas[0].Score = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>()
            .GetEatFoods();
        // そして敵
        for(int i = 0; i < enemys.Length; i++)
        {
            AI ai = enemys[i].GetComponent<AI>();
            // 美しくない書き方で判別
            switch (ai.GetAICharactor())
            {
                case AI.EnAICharacter.enCharacter_Cat:
                    actorDatas[i + 1].actorCharacter = GameData.ActorCharacter.enActorCharacter_Cat;

                    break;
                case AI.EnAICharacter.enCharacter_Duck:
                    actorDatas[i + 1].actorCharacter = GameData.ActorCharacter.enActorCharacter_Duck;

                    break;
                case AI.EnAICharacter.enCharacter_Penguin:
                    actorDatas[i + 1].actorCharacter = GameData.ActorCharacter.enActorCharacter_Penguin;

                    break;
            }

            actorDatas[i + 1].Score = ai.GetEatFoods();
        }

        // 保存
        gameData.SetActorData(actorDatas);

        // シーン切り替え処理
        SceneChange("Result");
    }

}
