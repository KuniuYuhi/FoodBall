using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{
    //�p�����[�^
    //�v���C���[�̈ړ����x
    float m_speed = 0.0f;

    //�L���b�V��
    Rigidbody m_rigidbody;
    GameObject m_gameCameraObj;

    //�v���C���[�̈ړ����x�ݒ�
    [SerializeField]
    void SetMoveSpeed(float speed)
    {
        m_speed = speed;
    }

    //�v���C���[�̈ړ����x�̎擾
    [SerializeField]
    float GetMoveSpeed()
    {
        return m_speed;
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
        Player
    }

    // Update is called once per frame
    void Update()
    {
       if()
    }
}
