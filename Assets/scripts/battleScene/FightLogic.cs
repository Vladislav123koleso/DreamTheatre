using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightLogic : MonoBehaviour
{
    public GameManager gm;
    public TurnManager turnManager;
    
    void Start()
    {
        turnManager = gameObject.GetComponent<TurnManager>();
        
    }

    void Update()
    {
        
    }

    
}
