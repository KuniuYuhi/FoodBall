using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField, Header("�l���|�C���g")]
    int m_point = 0;
    public int GetPoint()
    {
        return m_point;
    }

}
