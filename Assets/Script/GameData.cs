using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���U���g���I�������蓮�ŏ����Ă�������
public class GameData : MonoBehaviour
{
    // �L�����N�^�[�̎��
    public enum ActorCharacter
    {
        enActorCharacter_Sheep,
        enActorCharacter_Penguin,
        enActorCharacter_Duck,
        enActorCharacter_Cat,
    }

    // �v���C���ʂ̍\����
    [System.Serializable]
    public struct ActorData
    {
        public ActorCharacter actorCharacter;
        public int Score;
    }

    ActorData[] m_actorData;
    // �v���C���ʂ�ۑ�
    public void SetActorData(ActorData[] actorData)
    {
        // �T�C�Y��ݒ�
        m_actorData = new ActorData[actorData.Length];
        m_actorData = actorData;
    }
    // �v���C���ʂ�Ԃ�
    public ActorData[] GetActorData()
    {
        return m_actorData;
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
