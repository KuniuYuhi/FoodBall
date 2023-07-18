using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    [SerializeField,Header("食べ物を食べた数")]
    int eatFoods = 0;

    [SerializeField, Header("自身のサイズ")]
    Vector3 size = Vector3.one;



    /// <summary>
    /// 食べた食べ物の数を返す
    /// </summary>
    /// <returns></returns>
    public int GetEatFoods()
    {
        return eatFoods;
    }

    /// <summary>
    /// サイズを大きくする
    /// </summary>
    protected void SizeUp()
    {

    }

    /// <summary>
    /// サイズを小さくする
    /// </summary>
    protected void SizeDown()
    {

    }



}
