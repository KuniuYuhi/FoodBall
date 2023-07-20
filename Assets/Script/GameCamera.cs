using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameCamera : MonoBehaviour
{
    public float RotSpeed = 0.5f;
    public float RotUpLimit = 40.0f;
    public float RotDownLimit = -20.0f;
    public float CameraRange = 3.0f;
    public float CameraY_Up = 1.5f;


    private GameObject m_player;
    Player m_playerC;
    private float m_nowX_Rot = 0.0f;
    float m_defRange;

    void Start()
    {
        // Player�^�O�������I�u�W�F�N�g��T��
        m_player = GameObject.FindGameObjectWithTag("Player");
        m_playerC = m_player.GetComponent<Player>();

        // ����X���̉�]�ʂ�ۑ�
        m_nowX_Rot = transform.localEulerAngles.x;
        m_defRange = CameraRange;
    }


    void Update()
    {
        // �㉺
        float Up_rot = 0.5f;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            Up_rot = -RotSpeed;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            Up_rot = RotSpeed;
        }
        else
        {
            Up_rot = 0.0f;
        }

        // �㉺�p�x����
        m_nowX_Rot += Up_rot;
        if (m_nowX_Rot > RotUpLimit || m_nowX_Rot < RotDownLimit)
        {
            m_nowX_Rot = Mathf.Clamp(m_nowX_Rot, RotDownLimit, RotUpLimit);
            Up_rot = 0.0f;
        }
        transform.RotateAround(m_player.transform.position, this.transform.right, Up_rot);


        // ���E
        float Left_rot;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Left_rot = -RotSpeed;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            Left_rot = RotSpeed;
        }
        else
        {
            Left_rot = 0.0f;
        }
        transform.RotateAround(m_player.transform.position, Vector3.up, Left_rot);


        // ���W�v�Z
        // �J�����̑O�������g���Ĉړ��ʂ��v�Z
        Vector3 cameraMove = transform.forward * -CameraRange;
        // �J���������������グ��
        cameraMove.y += CameraY_Up;
        // ���W�ݒ�
        transform.position = m_player.transform.position + cameraMove;

        // �����𒲐�
        CameraRange = m_defRange + ((m_playerC.GetEatFoods()) / 5.0f);
    }
}