using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    [SerializeField,Header("�H�ו���H�ׂ���")]
    int eatFoods = 0;

    //Vector3 size

    int GetEatFoods()
    {
        return eatFoods;
    }
   
}
