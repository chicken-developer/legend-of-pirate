using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPopup : MonoBehaviour
{
    [SerializeField] public Text totalPointsText;
    [SerializeField] public Text highestPointsText;
    public void Setup(int totalScore)
    {
        gameObject.SetActive(true);
        totalPointsText.text = "Total eggs broken \n" + totalScore.ToString();
        highestPointsText.text = "Highest eggs broken \n"+ totalScore.ToString(); 
    }
}
