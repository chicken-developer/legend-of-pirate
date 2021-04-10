using System.Collections;
using System.Collections.Generic;
using UnityEngine;

	
public class EggWarehouse: MonoBehaviour
{
    private int currentEgg;
    private int mColor;

    [HideInInspector] public Shooter targetShooter;
    [SerializeField] private GameObject[] Colors;
    [SerializeField] private int POWER_EGG_CHANCE;

    void Start()
    {
        RandomEgg();
    }

    public void ResetEgg()
    {
        currentEgg = UnityEngine.Random.Range(0, Defines.EggCount);
        SetColor(currentEgg);      
    }

    private void SetColor(int color)
    {
        currentEgg = color;
        foreach (GameObject obj in Colors)
        {
            obj.SetActive(false);
        }
        if (color >= 0)
            Colors[color].SetActive(true);
    }

    private void RandomEgg()
    {
        if(UnityEngine.Random.Range(1, 101) <= POWER_EGG_CHANCE)
        {
            if(UnityEngine.Random.Range(1, 3) == 1)
                currentEgg = (int)Defines.COLOR.SPECIAL_DARKGREY;
            else
                currentEgg = (int)Defines.COLOR.SPECIAL_RAINBOW;

            //Debug.Log("currentEgg: " + currentEgg);
            SetColor(currentEgg);
        }
        else
        {
            currentEgg = UnityEngine.Random.Range(0, Defines.EggCount);
            //Debug.Log("currentEgg: " + currentEgg);
            SetColor(currentEgg);
        }
    }
    public void Request(string RequestID)
    {
        switch (RequestID)
        {
            case "reload":
                if(targetShooter.GetColor() <= (int)Defines.COLOR.YELLOW)
                    targetShooter.SetColor(currentEgg);
                RandomEgg();
                break;
            case "swap":
                int shooterCurrentEgg = targetShooter.mColor;
                targetShooter.SetColor(currentEgg);
                SetColor(shooterCurrentEgg);
                break;
            case "prepairflash":
                SetColor((int)Defines.COLOR.SPECIAL_FLASH);
                break;
            case "specialflash":
                targetShooter.SetColor((int)Defines.COLOR.SPECIAL_FLASH);
                RandomEgg();
                break;
        }
    }
}