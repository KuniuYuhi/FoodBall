using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    [SerializeField,Header("�H�ו���H�ׂ���")]
    int eatFoods = 0;

    [SerializeField, Header("���g�̃T�C�Y")]
    Vector3 size = Vector3.one;



    /// <summary>
    /// �H�ׂ��H�ו��̐���Ԃ�
    /// </summary>
    /// <returns></returns>
    public int GetEatFoods()
    {
        return eatFoods;
    }

    /// <summary>
    /// �T�C�Y��傫������
    /// </summary>
    protected void SizeUp()
    {

    }

    /// <summary>
    /// �T�C�Y������������
    /// </summary>
    protected void SizeDown()
    {

    }



}
