using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{
    //�p�����[�^
    //�v���C���[�̈ړ����x
    [SerializeField,Header("�v���C���[�̈ړ����x")]
    float m_moveSpeed = 0.0f;

    //�L���b�V��
    Rigidbody m_rigidbody;
    GameObject m_gameCameraObj;

    //�v���C���[�̈ړ����x�ݒ�
    public void SetMoveSpeed(float speed)
    {
        m_moveSpeed = speed;
    }

    //�v���C���[�̈ړ����x�̎擾
    public float GetMoveSpeed()
    {
        return m_moveSpeed;
    }
    // Start is called before the first frame update
    void Start()
    {
        //�K�v�ȏ����擾
        m_rigidbody = GetComponent<Rigidbody>();
        m_gameCameraObj = Camera.main.gameObject;
    }

    private void FixedUpdate()
    {
        Vector3 m_playerMove = Vector3.zero;
        Vector3 m_stickL = Vector3.zero;
        m_stickL.z = Input.GetAxis("Vertical");
        m_stickL.x = Input.GetAxis("Horizontal");

        Vector3 m_forward = m_gameCameraObj.transform.forward;
        Vector3 m_right = m_gameCameraObj.transform.right;
        m_forward.y = 0.0f;
        m_right.y = 0.0f;

        m_right *= m_stickL.x;
        m_forward *= m_stickL.z;

        //�ړ����x�ɏ�L�Ōv�Z�����x�N�g�������Z����
        m_playerMove += m_right + m_forward;

        //�v���C���[�̑��x��ݒ肷�邱�Ƃňړ��ł���
        m_playerMove = (m_playerMove * m_moveSpeed * Time.deltaTime);
        m_playerMove.y = m_rigidbody.velocity.y;
        m_rigidbody.velocity = m_playerMove;
    }
}
