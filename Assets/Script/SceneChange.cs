using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChange : MonoBehaviour
{
    [SerializeField, Header("�؂�ւ���̃V�[����")]
    string m_sceneName;

    void Update()
    {
        if(Input.GetKeyDown("joystick button 0") || Input.GetKeyDown(KeyCode.Space))
        {
            // �V�[���؂�ւ�
            GameManager.SceneChange(m_sceneName);
        }
    }
}
