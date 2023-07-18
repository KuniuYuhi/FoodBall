using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    [SerializeField,Header("�H�ו���H�ׂ���")]
    int eatFoods = 0;

    [SerializeField, Header("���g�̃T�C�Y")]
    Vector3 size = Vector3.one;

    [SerializeField, Header("�g�傷���")]
    const float m_scaleUp = 1.01f;


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
        transform.localScale *= m_scaleUp;
    }

    /// <summary>
    /// �T�C�Y������������
    /// </summary>
    protected void SizeDown()
    {

    }

    protected void OnTriggerEnter(Collider other)
    {
        //�������g�ƏՓ˂�����I�u�W�F�N�g�̃^�O���H�ו���������
        if(other.CompareTag("Food"))
        {
            //�H�ׂ��ʂ����Z����
            eatFoods++;

            //���f����傫������
            SizeUp();

            //�H�ו�������
            Destroy(other.gameObject);
        }
    }

}
