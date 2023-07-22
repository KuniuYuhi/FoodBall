using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Ready : MonoBehaviour
{
    [SerializeField]
    AudioClip m_count, m_start;

    public void SE_Count()
    {
        GameManager.PlaySE(m_count);
    }

    public void SE_Start()
    {
        GameManager.PlaySE(m_start);
    }

}
