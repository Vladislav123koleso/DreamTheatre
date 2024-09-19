using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pers1DataLogic : MonoBehaviour
{
    public basePers persData;

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            persData.hp = 100;
            persData.mp = 80;
            persData.speed = 2;
            persData.strength = 8;
        }
    }
}
