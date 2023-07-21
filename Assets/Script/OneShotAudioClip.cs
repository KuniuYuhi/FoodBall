using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 再生すると自動で消える効果音
/// isLoopを設定すると自動で消えないループ音源も可能
/// 
/// 生成用のstaticな関数が必要
/// </summary>
public class OneShotAudioClip : MonoBehaviour
{
    AudioSource m_audioSource;
    // オーディオソースを返す
    public AudioSource GetAudioSource()
    {
        return m_audioSource;
    }
    bool m_isPlay = false;
    // 再生中か取得
    public bool GetIsPlay()
    {
        return m_isPlay;
    }
    bool m_isLoop = false;
    // ループするか取得
    public bool GetIsLoop()
    {
        return m_isLoop;
    }

    // 初期化
    void Awake()
    {
        // シーンが切り替わってもオブジェクトが削除されないようにする
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// 効果音を再生する 2D
    /// </summary>
    /// <param name="audioClip">音源</param>
    /// <param name="volume">ボリューム</param>
    /// <param name="pitch">ピッチ</param>
    /// <param name="isLoop">ループするかどうか（手動で消さないといけない）</param>
    /// <returns>自身を返す</returns>
    public OneShotAudioClip PlaySE(AudioClip audioClip,
        float volume = 1.0f, 
        float pitch = 1.0f,
        bool isLoop = false)
    {
        // 自分にアタッチされているAudioSourceを取得
        m_audioSource = GetComponent<AudioSource>();

        // オーディオクリップを設定
        m_audioSource.clip = audioClip;
        m_audioSource.volume = volume;
        m_audioSource.pitch = pitch;
        m_isLoop = isLoop;

        // ループ処理
        if (isLoop)
        {
            m_audioSource.loop = true;
        }

        // 再生
        m_audioSource.Play();

        // 再生フラグを立てる
        m_isPlay = true;

        return this;
    }

    /// <summary>
    /// 効果音再生 3D
    /// </summary>
    public OneShotAudioClip PlaySE3D(AudioClip audioClip,
        Vector3 position, float minDis, float maxDis,
        float volume = 1.0f,
        float pitch = 1.0f,
        bool isLoop = false)
    {
        // 通常再生
        PlaySE(audioClip, volume, pitch, isLoop);

        // 3D処理
        m_audioSource.spatialBlend = 1.0f;
        transform.position = position;
        m_audioSource.minDistance = minDis;
        m_audioSource.maxDistance = maxDis;

        return this;
    }

    void Update()
    {
        // 再生フラグが立っていて、オーディオソースの再生が終わったら…
        if(m_isPlay && 
            m_audioSource.isPlaying == false &&
            m_isLoop == false)
        {
            // 自身を削除する
            Destroy(gameObject);
        }
    }
}
