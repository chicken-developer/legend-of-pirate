using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EggWarehouse: MonoBehaviour
{
    Action<GameObject> currentEvent;
    public GameObject eggWarehousePrefab;
	private List<GameObject> eggColorsInStock;
    private GameObject readyEgg;
    public Shooter targetShooter;
    void Start()
    {
        eggColorsInStock = new List<GameObject>();
        readyEgg = eggColorsInStock[0];
    }
    public void Request(String RequestID)
    {
        switch(RequestID)
        {
            case "reload" :
                currentEvent = (bulletEgg) => {
                    Debug.Log("Reload");
                }; 
                break;
            case "swap" :
                currentEvent = (bulletEgg) => {
                    Debug.Log("Swap");
                }; 
                break ;

        }
        currentEvent(targetShooter.bulletEgg);
    }
}
