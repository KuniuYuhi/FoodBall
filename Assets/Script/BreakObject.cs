using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakObject : MonoBehaviour
{
    [SerializeField, Header("破壊エフェクト")]
    GameObject m_BreakEffect;

    [SerializeField, Header("座標補正")]
    Vector3 m_effectOffset;

    [SerializeField, Header("エフェクトの大きさ")]
    float m_effectScale;

    [SerializeField, Header("破壊に必要な食べ物パワー")]
    int m_breakPoint;

    [SerializeField, Header("効果音")]
    AudioClip m_explosion;

    public float m_minRange=200;
    public float m_maxRange = 400;

    public int m_volume = 18;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player") ||
            collision.gameObject.CompareTag("Enemy"))
        {
            Actor actor = collision.gameObject.GetComponent<Actor>();

            if (actor.GetEatFoods() >= m_breakPoint)
            {
                // エフェクト再生
                GameObject effect = Instantiate(m_BreakEffect,
                    transform.position + m_effectOffset,
                    Quaternion.identity);
                effect.transform.localScale = new Vector3(m_effectScale, m_effectScale, m_effectScale);

                Destroy(gameObject);

                GameManager.PlaySE3D(
                    m_explosion,
                    transform.position,
                    m_minRange,
                    m_maxRange,
                    m_volume
                    );
            }
        }
    }

}
