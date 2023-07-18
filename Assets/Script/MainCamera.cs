using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [SerializeField, Header("�J�����̍��W")]
    public float m_cameraUp=0.0f;
    public float m_cameraForward = 0.0f;


    private GameObject m_player;
    // Start is called before the first frame update
    void Start()
    {
        //Player�^�O���t�����I�u�W�F�N�g��T��
        m_player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cameraPosition = m_player.transform.position;
        cameraPosition.z += m_cameraForward;
        cameraPosition.y = m_cameraUp;
        //���W�̌v�Z
        transform.position = cameraPosition;
    }
}
