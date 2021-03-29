using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    // Start is called before the first frame update
    private Button singleModeBtn;
    private Button multiModeBtn;
    [SerializeField] GameObject multiPlayModePopup;
    private Button quitBtn;

    void SingleBtnOnClicked()
    {
        Debug.Log("Enter single mode");
        SceneManager.LoadScene("DragonMite");
    }

    void MultiBtnOnClicked()
    {
        Debug.Log("Enter multi mode");
        multiPlayModePopup.SetActive(true);
    }

    void QuitBtnOnClicked()
    {
        Debug.Log("Quit game");
        Application.Quit();
    }
    void Awake()
    {
        singleModeBtn = GameObject.FindGameObjectWithTag("singleModeBtn").GetComponent<Button>();
        multiModeBtn = GameObject.FindGameObjectWithTag("multiModeBtn").GetComponent<Button>();
        quitBtn = GameObject.FindGameObjectWithTag("quitBtn").GetComponent<Button>();
    }
    void Start()
    {
        singleModeBtn.onClick.AddListener(SingleBtnOnClicked);
        multiModeBtn.onClick.AddListener(MultiBtnOnClicked);
        quitBtn.onClick.AddListener(QuitBtnOnClicked);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
