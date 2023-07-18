using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class FoodGenerator : MonoBehaviour
{
    [SerializeField, Header("生成する食べ物")]
    GameObject[] m_foodObjects;

    [SerializeField, Header("デフォルトで生成する食べ物の数")]
    int m_foodDefNum = 30;
    [SerializeField, Header("この数の食べ物が減ったら再生成")]
    int m_respawnNum = 10;

    [SerializeField, Header("生成する範囲")]
    float m_foodRadius = 20.0f;
    [SerializeField, Header("生成する高さ")]
    float m_rayOriginY = 20.0f;

    void Awake()
    {
        // 食べ物の初期生成
        CreateFood(m_foodDefNum);
    }

    void Update()
    {
        // 食べ物が少なくなったら再生成する
        int foodNum = GameObject.FindGameObjectsWithTag("Food").Length;
        if(foodNum < m_foodDefNum - m_respawnNum)
        {
            CreateFood(m_respawnNum);
        }
    }

    // 食べ物を生成する
    void CreateFood(int num)
    {
        for (int i = 0; i < num; i++)
        {
            // レイの基点を決める
            Vector3 rayOrigin = transform.position +
                (UnityEngine.Random.insideUnitSphere * m_foodRadius);
            rayOrigin.y = m_rayOriginY;

            // 真下にレイを発射して衝突した場所を求める
            RaycastHit hit;
            if (Physics.Raycast(rayOrigin, -Vector3.up, out hit))
            {
                // 衝突点が地面タグだったら生成
                if (hit.collider.gameObject.CompareTag("Ground"))
                {
                    // レイの衝突点に食べ物を生成
                    int foodNum = UnityEngine.Random.Range(0, m_foodObjects.Length);

                    Instantiate(m_foodObjects[foodNum],
                        hit.point + m_foodObjects[foodNum].transform.localPosition,
                        Quaternion.identity);
                }
                else
                {
                    // そうでないならやり直す
                    i--;
                }

            }
            else
            {
                // 地面と衝突しなかったらやり直す
                i--;
            }
        }

    }


}
