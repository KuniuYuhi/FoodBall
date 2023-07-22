using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChange : MonoBehaviour
{
    [SerializeField, Header("�؂�ւ���̃V�[����")]
    string m_sceneName;

    [SerializeField]
    AudioClip m_enterSE;

    void Update()
    {
        if(Input.GetKeyDown("joystick button 0") || Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.PlaySE(m_enterSE);

            // �V�[���؂�ւ�
            GameManager.SceneChange(m_sceneName);
        }
    }
}
