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
        Debug.Log("Enter multi mode");
       //TODO: Replay btn handle
    }
   

    void QuitMatchOnClicked()
    {
        Debug.Log("Quit game");
        //TODO: Quit match handle
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
