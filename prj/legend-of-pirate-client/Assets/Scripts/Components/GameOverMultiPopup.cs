using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverMultiPopup : MonoBehaviour
{
    [SerializeField] public Text yourPoint;
    [SerializeField] public Text enemyPoint;
    public void Setup(int yourScore, int enemyScore)
    {
        gameObject.SetActive(true);
        yourPoint.text = "Your gems:  " + yourScore.ToString();
        enemyPoint.text = "Enemy gems:"+ enemyScore.ToString(); 
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
