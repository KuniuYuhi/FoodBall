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
    const float m_scaleUp = 1.05f;
    //�L���b�V��
    protected Rigidbody m_rigidbody;
    protected GameObject m_gameCameraObj;
    //�W�����v�\���ǂ���
    protected bool m_isJumpFlag = true;
    virtual protected void GetStartInformation(){}

    //�H�ו����l���������ǂ����BAI�p
    bool m_getFoodFlag = false;

    public void SetGetFoodFlag(bool flag)
    {
        m_getFoodFlag = flag;
    }

    public bool GetGetFoodFlag()
    {
        return m_getFoodFlag;
    }

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
        if(transform.localScale.x<transform.localScale.x*2.0f)
        {
            transform.localScale *= m_scaleUp;
        }
        
    }

    /// <summary>
    /// �T�C�Y������������
    /// </summary>
    protected void SizeDown()
    {
        transform.localScale *= m_scaleUp;
    }

    protected void isFalling()
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
            m_rigidbody.mass = (1.0f + (eatFoods * 0.05f));
            m_rigidbody.mass = Mathf.Min(m_rigidbody.mass, 2.5f);
            //���f����傫������
            SizeUp();

            //�H�ו�������
            Destroy(other.gameObject);

            //�H�ו���H�ׂ��̂ł����ɐV�����^�[�Q�b�g�����߂�
            SetTarget();

            //�H�ו���H�ׂ��̂�true�ɂ���
            SetGetFoodFlag(true);
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player"))
        {
            Vector3 Backlash = collision.gameObject.transform.position - transform.position;
            Backlash.y += 50.0f;

            collision.gameObject.GetComponent<Rigidbody>().AddForce(Backlash, ForceMode.Impulse);
            m_rigidbody.AddForce(-Backlash, ForceMode.Impulse);
        }
    }
    virtual protected void SetTarget(){}

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            m_isJumpFlag = true;
            return;
        }

        if (collision.gameObject.CompareTag("JumpGround"))
        {
            m_isJumpFlag = true;
            return;
        }        
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            m_isJumpFlag = false;
            return;
        }

        if (collision.gameObject.CompareTag("JumpGround"))
        {
            m_isJumpFlag = false;
            return;
        }
    }

}
