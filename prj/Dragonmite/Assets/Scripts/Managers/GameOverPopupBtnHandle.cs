using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPopupBtnHandle : MonoBehaviour
{
    private Button replayGameBtn;
    private Button quitMatch;
    void ReplayGameOnClicked()
    {
        Debug.Log("Replay");
       //TODO: Handle replay game: When end, click this button will reset process and play again in single
       //On multi mode, not support
    }
   

    void QuitMatchOnClicked()
    {
        Debug.Log("Quit game");
        //TODO: Quit match handle: When click, back to main menu. And from main menu click single will enter game and play again
    }
    void Awake()
    {
        replayGameBtn = GameObject.FindGameObjectWithTag("replay").GetComponent<Button>();
        quitMatch = GameObject.FindGameObjectWithTag("quitmatch").GetComponent<Button>();
    }
    void Start()
    {
        replayGameBtn.onClick.AddListener(ReplayGameOnClicked);
        quitMatch.onClick.AddListener(QuitMatchOnClicked);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
