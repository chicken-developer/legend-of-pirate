using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPopup : MonoBehaviour
{
    [SerializeField] public Text totalPointsText;
    [SerializeField] public Text highestPointsText;
    public void Setup(int totalScore, int highScore)
    {
        gameObject.SetActive(true);
        totalPointsText.text = "Total gems broken \n" + totalScore.ToString();
        highestPointsText.text = "Highest gems broken \n"+ highScore.ToString(); 
    }

    public void Replay()
	{
        gameObject.SetActive(false);        
        Playfield.Instance().ResetPlayfield();
	}

	public void ExitGame()
	{
		AudioManager.Instance().StopAllAudio();
		Application.Quit();
	}
}
