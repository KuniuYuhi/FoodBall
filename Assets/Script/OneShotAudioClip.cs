using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �Đ�����Ǝ����ŏ�������ʉ�
/// isLoop��ݒ肷��Ǝ����ŏ����Ȃ����[�v�������\
/// 
/// �����p��static�Ȋ֐����K�v
/// </summary>
public class OneShotAudioClip : MonoBehaviour
{
    AudioSource m_audioSource;
    // �I�[�f�B�I�\�[�X��Ԃ�
    public AudioSource GetAudioSource()
    {
        return m_audioSource;
    }
    bool m_isPlay = false;
    // �Đ������擾
    public bool GetIsPlay()
    {
        return m_isPlay;
    }
    bool m_isLoop = false;
    // ���[�v���邩�擾
    public bool GetIsLoop()
    {
        return m_isLoop;
    }

    // ������
    void Awake()
    {
        // �V�[�����؂�ւ���Ă��I�u�W�F�N�g���폜����Ȃ��悤�ɂ���
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// ���ʉ����Đ����� 2D
    /// </summary>
    /// <param name="audioClip">����</param>
    /// <param name="volume">�{�����[��</param>
    /// <param name="pitch">�s�b�`</param>
    /// <param name="isLoop">���[�v���邩�ǂ����i�蓮�ŏ����Ȃ��Ƃ����Ȃ��j</param>
    /// <returns>���g��Ԃ�</returns>
    public OneShotAudioClip PlaySE(AudioClip audioClip,
        float volume = 1.0f, 
        float pitch = 1.0f,
        bool isLoop = false)
    {
        // �����ɃA�^�b�`����Ă���AudioSource���擾
        m_audioSource = GetComponent<AudioSource>();

        // �I�[�f�B�I�N���b�v��ݒ�
        m_audioSource.clip = audioClip;
        m_audioSource.volume = volume;
        m_audioSource.pitch = pitch;
        m_isLoop = isLoop;

        // ���[�v����
        if (isLoop)
        {
            m_audioSource.loop = true;
        }

        // �Đ�
        m_audioSource.Play();

        // �Đ��t���O�𗧂Ă�
        m_isPlay = true;

        return this;
    }

    /// <summary>
    /// ���ʉ��Đ� 3D
    /// </summary>
    public OneShotAudioClip PlaySE3D(AudioClip audioClip,
        Vector3 position, float minDis, float maxDis,
        float volume = 1.0f,
        float pitch = 1.0f,
        bool isLoop = false)
    {
        // �ʏ�Đ�
        PlaySE(audioClip, volume, pitch, isLoop);

        // 3D����
        m_audioSource.spatialBlend = 1.0f;
        transform.position = position;
        m_audioSource.minDistance = minDis;
        m_audioSource.maxDistance = maxDis;

        return this;
    }

    void Update()
    {
        // �Đ��t���O�������Ă��āA�I�[�f�B�I�\�[�X�̍Đ����I�������c
        if(m_isPlay && 
            m_audioSource.isPlaying == false &&
            m_isLoop == false)
        {
            // ���g���폜����
            Destroy(gameObject);
        }
    }
}
