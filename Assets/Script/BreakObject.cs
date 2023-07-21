using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakObject : MonoBehaviour
{
    [SerializeField, Header("�j��G�t�F�N�g")]
    GameObject m_BreakEffect;

    [SerializeField, Header("���W�␳")]
    Vector3 m_effectOffset;

    [SerializeField, Header("�G�t�F�N�g�̑傫��")]
    float m_effectScale;

    [SerializeField, Header("�j��ɕK�v�ȐH�ו��p���[")]
    int m_breakPoint;

    [SerializeField, Header("���ʉ�")]
    AudioClip m_explosion;

    [SerializeField]
    public int m_volume = 25;

    int m_minRange = 500;
    int m_maxRange = 800;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player") ||
            collision.gameObject.CompareTag("Enemy"))
        {
            Actor actor = collision.gameObject.GetComponent<Actor>();

            if (actor.GetEatFoods() >= m_breakPoint)
            {
                // �G�t�F�N�g�Đ�
                GameObject effect = Instantiate(m_BreakEffect,
                    transform.position + m_effectOffset,
                    Quaternion.identity);
                effect.transform.localScale = new Vector3(m_effectScale, m_effectScale, m_effectScale);

                AudioClip se = Instantiate((AudioClip)Resources.Load("explosion"));

                GameManager.PlaySE3D(
                    se,
                    transform.position,
                    m_minRange,
                    m_maxRange,
                    m_volume
                    );

                Destroy(gameObject);
            }
        }
    }

}
