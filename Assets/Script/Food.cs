using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField, Header("獲得ポイント")]
    int m_point = 0;
    public int GetPoint()
    {
        return m_point;
    }

}
