using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    [SerializeField,Header("食べ物を食べた数")]
    int eatFoods = 0;

    //Vector3 size

    int GetEatFoods()
    {
        return eatFoods;
    }
   
}
