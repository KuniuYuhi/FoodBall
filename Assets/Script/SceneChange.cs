using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChange : MonoBehaviour
{
    [SerializeField, Header("切り替え先のシーン名")]
    string m_sceneName;

    void Update()
    {
        if(Input.GetKeyDown("joystick button 0") || Input.GetKeyDown(KeyCode.Space))
        {
            // シーン切り替え
            GameManager.SceneChange(m_sceneName);
        }
    }
}
