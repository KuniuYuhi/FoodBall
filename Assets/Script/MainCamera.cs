using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [SerializeField, Header("カメラの座標")]
    public float m_cameraUp=0.0f;
    public float m_cameraForward = 0.0f;


    private GameObject m_player;
    // Start is called before the first frame update
    void Start()
    {
        //Playerタグが付いたオブジェクトを探す
        m_player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cameraPosition = m_player.transform.position;
        cameraPosition.z += m_cameraForward;
        cameraPosition.y = m_cameraUp;
        //座標の計算
        transform.position = cameraPosition;
    }
}
