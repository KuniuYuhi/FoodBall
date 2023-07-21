using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    enum Character
    {
        m_en_Sheep,
        m_en_Cat,
        m_en_Duc,
        m_en_Penguin,
    }

    private const int MAX_CHARACTER_NUMBER = 4;
    // Start is called before the first frame update
    //リザルトのスコア
    private int[] m_resultScore = new int[MAX_CHARACTER_NUMBER];

    //挿入する画像
    public Sprite m_sheepSprite;
    public Sprite m_catSprite;
    public Sprite m_dugSprite;
    public Sprite m_penguinSprite;

    GameObject[] m_resultCharacter = new GameObject[MAX_CHARACTER_NUMBER];
    void Start()
    {
        m_resultCharacter[(int)Character.m_en_Sheep].GetComponent<Image>().sprite = m_sheepSprite;
        m_resultCharacter[(int)Character.m_en_Cat].GetComponent<Image>().sprite = m_catSprite;
        m_resultCharacter[(int)Character.m_en_Duc].GetComponent<Image>().sprite = m_dugSprite;
        m_resultCharacter[(int)Character.m_en_Penguin].GetComponent<Image>().sprite = m_penguinSprite;
        for (int i=0;i< MAX_CHARACTER_NUMBER-1; i++)
        {
            for(int j=i+1;i< MAX_CHARACTER_NUMBER;i++)
            {
                int hoge = 0;
                if(m_resultScore[i]<m_resultScore[j])
                {
                    hoge = m_resultScore[i];
                    m_resultScore[i] = m_resultScore[j];
                    m_resultScore[j] = hoge;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
