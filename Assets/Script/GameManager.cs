using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// �y���ʉ��Đ��֘A�z
    /// </summary>

    // ���ʉ��Đ��֐� �ǂ�����ł��Ăׂ�
    static public OneShotAudioClip PlaySE(AudioClip clip, float volume = 1.0f,
        float pitch = 1.0f, bool isLoop = false)
    {
        // ���ݒ�Ȃ�炳�Ȃ�
        if (clip == null)
        {
            return null;
        }

        GameObject oneShotObj = Instantiate((GameObject)Resources.Load("OneShotSE"));

        // ���ʉ��Đ�
        OneShotAudioClip audio = oneShotObj.GetComponent<OneShotAudioClip>().
            PlaySE(clip, volume, pitch, isLoop);

        return audio;
    }
    // 3D��
    static public OneShotAudioClip PlaySE3D(AudioClip clip, Vector3 position,
        float minRange, float maxRange,
        float volume = 1.0f,
        float pitch = 1.0f, bool isLoop = false)
    {
        // ���ݒ�Ȃ�炳�Ȃ�
        if (clip == null)
        {
            return null;
        }

        GameObject oneShotObj = Instantiate((GameObject)Resources.Load("OneShotSE"));

        // ���ʉ��Đ�
        OneShotAudioClip audio = oneShotObj.GetComponent<OneShotAudioClip>().PlaySE3D(clip, position,
            minRange, maxRange, volume, pitch, isLoop);

        return audio;
    }

    /// <summary>
    /// �V�[���؂�ւ�
    /// </summary>
    /// <param name="sceneName">�J�ڐ�̃V�[����</param>
    /// <param name="initSprite">�J�ڂɎg���摜</param>
    /// <param name="bgm">�g�pBGM null�Ȃ�BGM�؂�ւ��Ȃ�</param>
    static public GameObject SceneChange(string sceneName, Sprite initSprite = null)
    {
        Debug.Log("�V�[���؂�ւ�");
        // ����
        GameObject fadeObj = Instantiate((GameObject)Resources.Load("FadeCanvas"));
        fadeObj.GetComponent<FadeScene>().FadeStart(sceneName, initSprite);

        return fadeObj;
    }

    public enum GameState
    {
        enGameMode_Ready,   // �J�E���g�_�E��
        enGameMode_Play,    // �v���C��
        enGameMode_End,     // �I��
        enGameMode_Pause,   // �|�[�Y
    }

    [SerializeField, Header("�Q�[���̏��")]
    GameState m_gameState = GameState.enGameMode_Play;
    public GameState GetGameMode()
    {
        return m_gameState;
    }

    // �^�C�}�[
    [SerializeField, Header("�f�t�H���g�������ԁi���j")]
    int m_defLimitMinit;

    [SerializeField, Header("Ready�p�L�����o�X")]
    GameObject ReadyCanvas;
    [SerializeField, Header("End�p�L�����o�X")]
    GameObject EndCanvas;
    [SerializeField, Header("Pause�p�L�����o�X")]
    GameObject PauseCanvas;

    [SerializeField]
    AudioSource m_bgm;

    public int m_volume = 8;

    // ���݂̃^�C�}�[
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
        // UI�̐���
        SceneManager.LoadScene("UI", LoadSceneMode.Additive);

        // ���Ԃ̏�����
        m_timerMinit = m_defLimitMinit - 1;
        m_timerSecond = 60.0f;

        ReadyCanvas.SetActive(true);
        EndCanvas.SetActive(false);
        PauseCanvas.SetActive(false);

        //�҂��Ă���J�n
        await UniTask.Delay(6400);
        ReadyCanvas.SetActive(false);
        m_gameState = GameState.enGameMode_Play;

        //BGM�Đ�
        m_bgm.Play();
    }

    void Update()
    {
        switch (m_gameState)
        {
            case GameState.enGameMode_Ready:
                // �������Ȃ�

                break;
            case GameState.enGameMode_Play:

                // �^�C�}�[���Z
                m_timerSecond -= Time.deltaTime;
                if (m_timerSecond <= 0.0f)
                {
                    // 1���o��
                    m_timerMinit--;
                    m_timerSecond += 60.0f;

                    // �I��
                    if (m_timerMinit < 0)
                    {
                        m_timerMinit = 0;
                        m_timerSecond = 0.0f;
                        GameEnd();
                    }
                }

                // �|�[�Y�J�n
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

                // �|�[�Y����
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
        //BGM��~
        m_bgm.Stop();

        m_gameState = GameState.enGameMode_End;
        EndCanvas.SetActive(true);
        m_gameEnd = true;
        await UniTask.Delay(3000);

        // �f�[�^�̐���
        GameData gameData = GameObject.FindGameObjectWithTag("GameData").GetComponent<GameData>();
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        GameData.ActorData[] actorDatas = new GameData.ActorData[4];

        // �擾
        // �܂��̓v���C���[
        actorDatas[0].actorCharacter = GameData.ActorCharacter.enActorCharacter_Sheep;
        actorDatas[0].Score = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>()
            .GetEatFoods();
        // �����ēG
        for(int i = 0; i < enemys.Length; i++)
        {
            AI ai = enemys[i].GetComponent<AI>();
            // �������Ȃ��������Ŕ���
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

        // �ۑ�
        gameData.SetActorData(actorDatas);

        // �V�[���؂�ւ�����
        SceneChange("Result");
    }

}
