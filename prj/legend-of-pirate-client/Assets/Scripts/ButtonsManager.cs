using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonsManager : MonoBehaviour
{
    public GameObject menuMain, menuPlayMode, menuSelectDragon, menuSelectBackground, menuOptions, menuPlayOnline;

    public Text  mPlayerNameInputText, mCodeInputText;

    public GameObject popupError, mErrorInfoText;

    private GameObject mPopupInfoText;//menu show info popup

    
    
    float aboutTextPosY;

    // Start is called before the first frame update
    void Start()
    {
        menuMain.SetActive(true);
        menuPlayMode.SetActive(false);
        menuSelectDragon.SetActive(false);
        menuSelectBackground.SetActive(false);
        menuOptions.SetActive(false);
        menuPlayOnline.SetActive(false);

        popupError.SetActive(false);

        aboutTextPosY = -500f;
    }

    void Update() 
    {
        
    }

    //Press the button on top of screen
    public void OnTopButtonPressed()
    {
        switch(StateManager.Instance().GetState())
        {
            case StateManager.STATE.MENU_MAIN:
                //play
                menuMain.SetActive(false);
                menuPlayMode.SetActive(true);
                StateManager.Instance().SetState(StateManager.STATE.MENU_SELECT_PLAY_MODE);
            break;
            case StateManager.STATE.MENU_SELECT_PLAY_MODE:
                //single play
                menuSelectDragon.SetActive(true);
                menuPlayMode.SetActive(false);
                StateManager.Instance().SetState(StateManager.STATE.MENU_SELECT_DRAGON);
                StateManager.Instance().SetMode(StateManager.MODE.SINGLE_PLAY);
            break;
            case StateManager.STATE.MENU_SELECT_DRAGON:
                //left
            break;
            case StateManager.STATE.MENU_SELECT_BACKGROUND:
                //left
            break;
            case StateManager.STATE.MENU_OPTIONS:
            break;
            case StateManager.STATE.MENU_ABOUT:
            break;
            case StateManager.STATE.MENU_SHOP:
            break;
            case StateManager.STATE.MENU_SHOP_DRAGON:
            break;
            case StateManager.STATE.MENU_SHOP_BACKGROUND:
            break;
            case StateManager.STATE.MENU_WATCH_ADS:
            break;
            case StateManager.STATE.MENU_ONLINE_PLAY:
            break;
            case StateManager.STATE.MENU_MATCH_MAKE:
            break;
            case StateManager.STATE.MENU_PRIVATE_MATCH:
                if(!QueuePlayerSusscess())
                {
                    popupError.SetActive(true);
                    mErrorInfoText.GetComponent<Text>().text = "Fail to create private match";
                    return;
                }
            break;
            case StateManager.STATE.MENU_JOIN_ROOM:
            break;
            case StateManager.STATE.MENU_ROOM_STATUS:
            break;
            case StateManager.STATE.MENU_INGAME:
            break;
            case StateManager.STATE.MENU_RESULT:
            break;
            case StateManager.STATE.GAMEPLAY:
            break;
        }
    }

     //Press the button on middle of screen
    public void OnMidButtonPressed()
    {
        switch(StateManager.Instance().GetState())
        {
            case StateManager.STATE.MENU_MAIN:
                //option
                menuMain.SetActive(false);
                menuOptions.SetActive(true);                
                StateManager.Instance().SetState(StateManager.STATE.MENU_OPTIONS);
            break;
            case StateManager.STATE.MENU_SELECT_PLAY_MODE:
                //online play                
                menuSelectDragon.SetActive(true);
                menuPlayMode.SetActive(false);                
                StateManager.Instance().SetState(StateManager.STATE.MENU_SELECT_DRAGON);
                StateManager.Instance().SetMode(StateManager.MODE.ONLINE_PLAY);
            break;
            case StateManager.STATE.MENU_SELECT_DRAGON:
                
            break;
            case StateManager.STATE.MENU_SELECT_BACKGROUND:
                
            break;
            case StateManager.STATE.MENU_OPTIONS:
            break;
            case StateManager.STATE.MENU_ABOUT:
            break;
            case StateManager.STATE.MENU_SHOP:
            break;
            case StateManager.STATE.MENU_SHOP_DRAGON:
            break;
            case StateManager.STATE.MENU_SHOP_BACKGROUND:
            break;
            case StateManager.STATE.MENU_WATCH_ADS:
            break;
            case StateManager.STATE.MENU_ONLINE_PLAY:
                if(mPlayerNameInputText.text == "")
                {
                    popupError.SetActive(true);
                    mErrorInfoText.GetComponent<Text>().text = "Please enter your name";
                    return;
                }
                //temp
                popupError.SetActive(true);
                mErrorInfoText.GetComponent<Text>().text = "Implementing...";
                
                //menuPlayOnline.SetActive(false);
                //menuMatchMake.SetActive(true);
                //StateManager.Instance().SetState(StateManager.STATE.MENU_MATCH_MAKE);
            break;
            case StateManager.STATE.MENU_MATCH_MAKE:
            break;
            case StateManager.STATE.MENU_PRIVATE_MATCH:                
                StateManager.Instance().SetState(StateManager.STATE.MENU_JOIN_ROOM);
            break;
            case StateManager.STATE.MENU_JOIN_ROOM:
            break;
            case StateManager.STATE.MENU_ROOM_STATUS:
            break;
            case StateManager.STATE.MENU_INGAME:
            break;
            case StateManager.STATE.MENU_RESULT:
            break;
            case StateManager.STATE.GAMEPLAY:
            break;
        }
    }

    //Press the button on the bottom of screen
    public void OnBottomButtonPressed()
    {
        switch(StateManager.Instance().GetState())
        {
            case StateManager.STATE.MENU_MAIN:
                //play
            break;
            case StateManager.STATE.MENU_SELECT_PLAY_MODE:                
            break;
            case StateManager.STATE.MENU_SELECT_DRAGON:
                //select
                menuSelectDragon.SetActive(false);
                menuSelectBackground.SetActive(true);
                StateManager.Instance().SetState(StateManager.STATE.MENU_SELECT_BACKGROUND);
            break;
            case StateManager.STATE.MENU_SELECT_BACKGROUND:
                if(StateManager.Instance().GetMode() == StateManager.MODE.SINGLE_PLAY)
                {
                    PlayerData.SetData("Single", "0");
                    SceneManager.LoadScene("Single");
                }
                else if(StateManager.Instance().GetMode() == StateManager.MODE.ONLINE_PLAY)
                {
                    menuSelectBackground.SetActive(false);
                    menuPlayOnline.SetActive(true);
                    StateManager.Instance().SetState(StateManager.STATE.MENU_ONLINE_PLAY);
                }
            break;
            case StateManager.STATE.MENU_OPTIONS:
                menuOptions.SetActive(false);
                StateManager.Instance().SetState(StateManager.STATE.MENU_ABOUT);
            break;
            case StateManager.STATE.MENU_ABOUT:
            break;
            case StateManager.STATE.MENU_SHOP:
            break;
            case StateManager.STATE.MENU_SHOP_DRAGON:
            break;
            case StateManager.STATE.MENU_SHOP_BACKGROUND:            
            break;
            case StateManager.STATE.MENU_WATCH_ADS:
            break;
            case StateManager.STATE.MENU_ONLINE_PLAY:
                if(mPlayerNameInputText.text == "")
                {
                    popupError.SetActive(true);
                    mErrorInfoText.GetComponent<Text>().text = "Please enter your name";
                    return;
                }
                menuPlayOnline.SetActive(false);
                //StateManager.Instance().SetState(StateManager.STATE.MENU_PRIVATE_MATCH);
                PlayerData.SetData(mPlayerNameInputText.text, mCodeInputText.text);
                StateManager.Instance().SetState(StateManager.STATE.GAMEPLAY);
                SceneManager.LoadScene("Multi");
                break;
            case StateManager.STATE.MENU_MATCH_MAKE:
            break;
            case StateManager.STATE.MENU_PRIVATE_MATCH:
            break;
            case StateManager.STATE.MENU_JOIN_ROOM:
                if(mCodeInputText.text == "")
                {
                    popupError.SetActive(true);
                    mErrorInfoText.GetComponent<Text>().text = "Please enter room code";
                    return;
                }
                if(!ConnectRoomSuccess())
                {
                    popupError.SetActive(true);
                    mErrorInfoText.GetComponent<Text>().text = "Join failed\nPlease try again";
                    return;
                }
                StateManager.Instance().SetState(StateManager.STATE.MENU_ROOM_STATUS);
            break;
            case StateManager.STATE.MENU_ROOM_STATUS:
            break;
            case StateManager.STATE.MENU_INGAME:
            break;
            case StateManager.STATE.MENU_RESULT:
            break;
            case StateManager.STATE.GAMEPLAY:
            break;
        }
    }

    //Press X button in screen
    public void OnXButtonPressed()
    {
        switch(StateManager.Instance().GetState())
        {
            case StateManager.STATE.MENU_MAIN:
                //no x
            break;
            case StateManager.STATE.MENU_SELECT_PLAY_MODE:
                menuPlayMode.SetActive(false);
                menuMain.SetActive(true);
                StateManager.Instance().SetState(StateManager.STATE.MENU_MAIN);
            break;
            case StateManager.STATE.MENU_SELECT_DRAGON:
                menuSelectDragon.SetActive(false);
                menuPlayMode.SetActive(true);
                StateManager.Instance().SetState(StateManager.STATE.MENU_SELECT_PLAY_MODE);
            break;
            case StateManager.STATE.MENU_SELECT_BACKGROUND:
                menuSelectBackground.SetActive(false);
                menuSelectDragon.SetActive(true);
                StateManager.Instance().SetState(StateManager.STATE.MENU_SELECT_DRAGON);
            break;
            case StateManager.STATE.MENU_OPTIONS:
                menuMain.SetActive(true);
                menuOptions.SetActive(false);
                StateManager.Instance().SetState(StateManager.STATE.MENU_MAIN);
            break;
            case StateManager.STATE.MENU_ABOUT:
                menuOptions.SetActive(true);
                StateManager.Instance().SetState(StateManager.STATE.MENU_OPTIONS);
            break;
            case StateManager.STATE.MENU_SHOP:
            break;
            case StateManager.STATE.MENU_SHOP_DRAGON:
            break;
            case StateManager.STATE.MENU_SHOP_BACKGROUND:
            break;
            case StateManager.STATE.MENU_WATCH_ADS:
            break;
            case StateManager.STATE.MENU_ONLINE_PLAY:
                if(popupError.activeSelf)
                {
                    popupError.SetActive(false);
                    return;
                }
                menuSelectBackground.SetActive(true);
                menuPlayOnline.SetActive(false);
                StateManager.Instance().SetState(StateManager.STATE.MENU_SELECT_BACKGROUND);                
            break;
            case StateManager.STATE.MENU_MATCH_MAKE:
                menuPlayOnline.SetActive(true);
                StateManager.Instance().SetState(StateManager.STATE.MENU_ONLINE_PLAY);
                //TODO: cancel search online player
            break;
            case StateManager.STATE.MENU_PRIVATE_MATCH:
                if(popupError.activeSelf)
                {
                    popupError.SetActive(false);
                    return;
                }
                menuPlayOnline.SetActive(true);
                StateManager.Instance().SetState(StateManager.STATE.MENU_ONLINE_PLAY);
            break;
            case StateManager.STATE.MENU_JOIN_ROOM:
                if(popupError.activeSelf)
                {
                    popupError.SetActive(false);
                    return;
                }
                StateManager.Instance().SetState(StateManager.STATE.MENU_PRIVATE_MATCH);
            break;
            case StateManager.STATE.MENU_ROOM_STATUS:
                if(popupError.activeSelf)
                {
                    popupError.SetActive(false);
                    return;
                }
                StateManager.Instance().SetState(StateManager.STATE.MENU_PRIVATE_MATCH);
            break;
            case StateManager.STATE.MENU_INGAME:
            break;
            case StateManager.STATE.MENU_RESULT:
            break;
            case StateManager.STATE.GAMEPLAY:
            break;
        }
    }

    private bool QueuePlayerSusscess()
    {
        return true;
    }
    private bool ConnectRoomSuccess()
    {
        return true;
    }
}
