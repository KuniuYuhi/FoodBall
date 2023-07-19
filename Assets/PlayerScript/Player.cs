using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{
    //�p�����[�^
    //�v���C���[�̈ړ����x
    [SerializeField,Header("�v���C���[�̈ړ����x")]
    float m_moveSpeed = 0.0f;

    GameObject m_mainCamera;

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

    protected override void GetStartInformation()
    {
        base.GetStartInformation();

        // ���C���J�����̃Q�[���I�u�W�F�N�g���擾����
        m_mainCamera = Camera.main.gameObject;
    }

    private void FixedUpdate()
    {
        Move();
    }

    //�ړ�����
    private void Move()
    {
        Vector3 m_playerMove = Vector3.zero;
        Vector3 m_stickL = Vector3.zero;
        m_stickL.z = Input.GetAxis("Vertical");
        m_stickL.x = Input.GetAxis("Horizontal");

        Vector3 forward = m_mainCamera.transform.forward;
        Vector3 right = m_mainCamera.transform.right;
        forward.y = 0.0f;
        right.y = 0.0f;
        right *= m_stickL.x;
        forward *= m_stickL.z;
        // �ړ����x�ɏ�L�Ōv�Z�����x�N�g�������Z����
        m_playerMove += right + forward;

        //�v���C���[�̑��x��ݒ肷�邱�Ƃňړ��ł���
        m_playerMove = (m_playerMove * m_moveSpeed * Time.deltaTime);
        m_playerMove.y = 0.0f;
        m_rigidbody.AddForce(m_playerMove);
        //Debug.Log(m_playerMove);

    }
}
