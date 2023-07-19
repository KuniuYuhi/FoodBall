using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    enum GameState
    {
        enGameMode_Ready,   // �J�E���g�_�E��
        enGameMode_Play,    // �v���C��
        enGameMode_End,     // �I��
        enGameMode_Pause,   // �|�[�Y
    }

    [SerializeField, Header("�Q�[���̏��")]
    GameState m_gameState = GameState.enGameMode_Play;

    // �^�C�}�[
    [SerializeField, Header("�f�t�H���g�������ԁi���j")]
    int m_defLimitMinit;

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

    void Awake()
    {
        // UI�̐���
        SceneManager.LoadScene("UI", LoadSceneMode.Additive);

        // ���Ԃ̏�����
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
