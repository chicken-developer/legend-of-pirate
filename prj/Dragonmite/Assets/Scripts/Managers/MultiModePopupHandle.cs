using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MultiModePopupHandle : MonoBehaviour
{
    // Start is called before the first frame update
    private InputField addressInF;
    private InputField roomIDInF;
    private Button startGameBtn;
    private Button backGameBtn;

    private string address = "";
    private string roomID = "";

    void StartMultiGame(string add, string id)
    {
        Debug.Log("Start multi game");
        SceneManager.LoadScene("MultiMode");
    }

    void StartMultiBtnOnClicked()
    {
        address = addressInF.GetComponentInChildren<Text>().text;
        roomID = roomIDInF.GetComponentInChildren<Text>().text;
        Debug.Log("Join to multi player");
        if(address != "" && roomID != "")
            StartMultiGame(address, roomID);
    }

    void BackMainMenuOnClicked()
    {
        Debug.Log("Back to main menu");
        //TODO: Hide popup and back to main menu
    }

    void Awake()
    {
        addressInF = GameObject.FindGameObjectWithTag("inputFieldAddress").GetComponent<InputField>();
        roomIDInF = GameObject.FindGameObjectWithTag("inputFieldRoomID").GetComponent<InputField>();
        startGameBtn = GameObject.FindGameObjectWithTag("startMultiBtn").GetComponent<Button>();
        backGameBtn = GameObject.FindGameObjectWithTag("backMainMenuBtn").GetComponent<Button>();
    }
    void Start()
    {
        startGameBtn.onClick.AddListener(StartMultiBtnOnClicked);
        backGameBtn.onClick.AddListener(BackMainMenuOnClicked);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
