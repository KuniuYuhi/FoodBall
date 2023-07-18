using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class FoodGenerator : MonoBehaviour
{
    [SerializeField, Header("��������H�ו�")]
    GameObject[] m_foodObjects;

    [SerializeField, Header("�f�t�H���g�Ő�������H�ו��̐�")]
    int m_foodDefNum = 30;
    [SerializeField, Header("���̐��̐H�ו�����������Đ���")]
    int m_respawnNum = 10;

    [SerializeField, Header("��������͈�")]
    float m_foodRadius = 20.0f;
    [SerializeField, Header("�������鍂��")]
    float m_rayOriginY = 20.0f;

    void Awake()
    {
        // �H�ו��̏�������
        CreateFood(m_foodDefNum);
    }

    void Update()
    {
        // �H�ו������Ȃ��Ȃ�����Đ�������
        int foodNum = GameObject.FindGameObjectsWithTag("Food").Length;
        if(foodNum < m_foodDefNum - m_respawnNum)
        {
            CreateFood(m_respawnNum);
        }
    }

    // �H�ו��𐶐�����
    void CreateFood(int num)
    {
        for (int i = 0; i < num; i++)
        {
            // ���C�̊�_�����߂�
            Vector3 rayOrigin = transform.position +
                (UnityEngine.Random.insideUnitSphere * m_foodRadius);
            rayOrigin.y = m_rayOriginY;

            // �^���Ƀ��C�𔭎˂��ďՓ˂����ꏊ�����߂�
            RaycastHit hit;
            if (Physics.Raycast(rayOrigin, -Vector3.up, out hit))
            {
                // �Փ˓_���n�ʃ^�O�������琶��
                if (hit.collider.gameObject.CompareTag("Ground"))
                {
                    // ���C�̏Փ˓_�ɐH�ו��𐶐�
                    int foodNum = UnityEngine.Random.Range(0, m_foodObjects.Length);

                    Instantiate(m_foodObjects[foodNum],
                        hit.point + m_foodObjects[foodNum].transform.localPosition,
                        Quaternion.identity);
                }
                else
                {
                    // �����łȂ��Ȃ��蒼��
                    i--;
                }

            }
            else
            {
                // �n�ʂƏՓ˂��Ȃ��������蒼��
                i--;
            }
        }

    }


}
