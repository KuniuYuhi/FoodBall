using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    float m_defLimitMinit;
    [SerializeField, Header("�f�t�H���g�������ԁi�b�j")]
    float m_defLimitSecond;

    // ���݂̃^�C�}�[
    float m_timerMinit, m_timerSecond;

}
