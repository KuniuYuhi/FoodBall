using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;               // Imageを扱う時に必要
using UnityEngine.SceneManagement;  // Sceneを扱う時に必要
using TMPro;

public class FadeScene : MonoBehaviour
{
    // パラメータ
    bool m_fadeStart = false;   // trueなら一連の処理を開始

    bool m_fadeMode = false;    // falseなら暗くなる trueなら明るくなる
    float m_alpha = 0.0f;       // 画像の不透明度
    float m_fadeWait = 0.0f;    // フェードの待ち時間

    [SerializeField]
    float FadeSpeed = 1.0f;     // フェードの速度（大きいほど速い）

    // 遷移先のシーン名
    string m_sceneName;
    // 背景画像
    Image m_image;

    // 装飾
    bool m_isAcc = false;
    List<Image> m_accImages = new List<Image>();
    List<TextMeshProUGUI> m_accTMpro = new List<TextMeshProUGUI>();

    /// <summary>
    /// フェード開始
    /// </summary>
    /// <param name="sceneName">遷移先のシーン名（空白なら現在シーン）</param>
    /// <param name="initSprite">遷移に使う画像</param>
    public void FadeStart(string sceneName, Sprite initSprite = null)
    {
        // 自身はシーンをまたいでも削除されないようにする
        DontDestroyOnLoad(gameObject);

        // シーン名が空白なら現在シーンに置き換え
        if (sceneName == "")
        {
            sceneName = SceneManager.GetActiveScene().name;
        }

        // フェード開始の準備をする
        m_fadeStart = true;
        m_sceneName = sceneName;

        // 自分の子オブジェクトにアタッチされているImageを取得する
        m_image = transform.GetChild(0).GetComponent<Image>();

        // 画像が指定されているなら切り替える
        if (initSprite != null)
        {
            m_image.sprite = initSprite;
        }

        // アクセサリーのコンポーネントをまとめて取得
        if (m_image.transform.childCount > 0)
        {
            m_isAcc = true;

            // 子オブジェクトを順に取得する
            for (int i = 0; i < m_image.transform.childCount; i++)
            {
                // 取得した子オブジェクトを処理する
                Image image = m_image.transform.GetChild(i).GetComponent<Image>();
                if (image != null)
                {
                    m_accImages.Add(image);
                }
                else
                {
                    // 画像じゃないならテキスト
                    TextMeshProUGUI tmpro = m_image.transform.GetChild(i).GetComponent<TextMeshProUGUI>();
                    m_accTMpro.Add(tmpro);
                }
            }

        }

    }

    void Update()
    {
        // フェードが開始していないなら中断
        if (m_fadeStart == false)
        {
            return;
        }

        // フェード処理
        if (m_fadeMode == false)
        {
            // 画面を暗くする
            m_alpha += FadeSpeed * Time.unscaledDeltaTime;

            // 完全に暗くなったのでシーンを変更する
            if (m_alpha >= 1.0f)
            {

                // 遷移を待つ
                if (m_fadeWait < 1.0f)
                {
                    m_fadeWait += Time.deltaTime;
                }
                else
                {
                    // シーン切り替え
                    SceneManager.LoadScene(m_sceneName);

                    // 明るくするモードに変更
                    m_fadeMode = true;

                }

            }
        }
        else
        {
            // 画面を明るくする
            m_alpha -= FadeSpeed * Time.deltaTime;

            // 完全に明るくなったので自身を削除する
            if (m_alpha <= 0.0f)
            {
                Destroy(gameObject);
            }
        }

        // 最後に不透明度を設定する
        m_image.material.SetFloat("_Alpha", m_alpha);

        // アクセサリーの不透明度を設定する
        if (m_isAcc)
        {
            Color nowColor;
            foreach (Image image in m_accImages)
            {
                nowColor = image.color;
                nowColor.a = m_alpha;
                image.color = nowColor;
            }
            foreach (TextMeshProUGUI textMeshProUGUI in m_accTMpro)
            {
                nowColor = textMeshProUGUI.color;
                nowColor.a = m_alpha;
                textMeshProUGUI.color = nowColor;
            }
        }

    }
}