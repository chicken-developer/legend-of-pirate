using System;
using UnityEngine;

public class EggWarehouse: MonoBehaviour
{
    private int currentEgg;
    private int mColor;

    [HideInInspector] public Shooter targetShooter;
    [SerializeField] private GameObject[] Colors;

    void Start()
    {
        RandomEgg();
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
        currentEgg = UnityEngine.Random.Range(0, (int)Defines.COLOR.YELLOW + 1);
        Debug.Log("currentEgg: " + currentEgg);
        SetColor(currentEgg);
    }
    public void Request(String RequestID)
    {
        switch (RequestID)
        {
            case "reload":
                targetShooter.SetColor(currentEgg);
                RandomEgg();
                break;
            case "swap":
                int shooterCurrentEgg = targetShooter.mColor;
                targetShooter.SetColor(currentEgg);
                SetColor(shooterCurrentEgg);
                break;

        }
    }
}
