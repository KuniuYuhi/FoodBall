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
    //�L���b�V��
    protected Rigidbody m_rigidbody;
    protected GameObject m_gameCameraObj;


    virtual protected void GetStartInformation(){}

    private void Start()
    {
        //�K�v�ȏ����擾
        m_rigidbody = GetComponent<Rigidbody>();
        m_gameCameraObj = Camera.main.gameObject;
        GetStartInformation();
    }


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
            Food m_food=other.GetComponent<Food>();
            //�H�ׂ��ʂ����Z����
            eatFoods+= m_food.GetPoint();

            //���f����傫������
            SizeUp();

            //�H�ו�������
            Destroy(other.gameObject);
        }
    }

}
