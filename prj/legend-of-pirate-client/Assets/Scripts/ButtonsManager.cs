using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsManager : MonoBehaviour
{
    public GameObject menuMain, menuPlayMode, menuSelectDragon, menuSelectBackground, menuOptions, menuAbout, menuShop, menuPlayOnline, menuMatchMake, menuPrivateMatch, menuJoinRoom, menuRoomStatus;

    public GameObject mAboutText, mPlayerNameInputText, mMatchMakeInfoText, mCodeInputText;

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
        menuAbout.SetActive(false);
        menuShop.SetActive(false);
        menuPlayOnline.SetActive(false);
        menuMatchMake.SetActive(false);
        menuPrivateMatch.SetActive(false);
        menuJoinRoom.SetActive(false);
        menuRoomStatus.SetActive(false);

        popupError.SetActive(false);

        aboutTextPosY = -500f;
    }

    void Update() 
    {
        //update about
        if(StateManager.Instance().GetState() == StateManager.STATE.MENU_ABOUT)
        {
            aboutTextPosY ++;
            if(aboutTextPosY >= 900f)
                aboutTextPosY = -900f;
            mAboutText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, aboutTextPosY);
        }
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
                menuPrivateMatch.SetActive(false);
                menuRoomStatus.SetActive(true);
                StateManager.Instance().SetState(StateManager.STATE.MENU_ROOM_STATUS);
                //TODO: create token to send to 2nd player
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
                if(mPlayerNameInputText.GetComponent<Text>().text == "")
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
                menuPrivateMatch.SetActive(false);
                menuJoinRoom.SetActive(true);
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
                menuAbout.SetActive(true);
                aboutTextPosY = -500f;
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
                if(mPlayerNameInputText.GetComponent<Text>().text == "")
                {
                    popupError.SetActive(true);
                    mErrorInfoText.GetComponent<Text>().text = "Please enter your name";
                    return;
                }
                menuPlayOnline.SetActive(false);
                menuPrivateMatch.SetActive(true);
                StateManager.Instance().SetState(StateManager.STATE.MENU_PRIVATE_MATCH);
            break;
            case StateManager.STATE.MENU_MATCH_MAKE:
            break;
            case StateManager.STATE.MENU_PRIVATE_MATCH:
            break;
            case StateManager.STATE.MENU_JOIN_ROOM:
                if(mCodeInputText.GetComponent<Text>().text == "")
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
                menuJoinRoom.SetActive(false);
                menuRoomStatus.SetActive(true);
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
                menuAbout.SetActive(false);
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
                menuMatchMake.SetActive(false);
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
                menuPrivateMatch.SetActive(false);
                menuPlayOnline.SetActive(true);
                StateManager.Instance().SetState(StateManager.STATE.MENU_ONLINE_PLAY);
            break;
            case StateManager.STATE.MENU_JOIN_ROOM:
                if(popupError.activeSelf)
                {
                    popupError.SetActive(false);
                    return;
                }
                menuJoinRoom.SetActive(false);
                menuPrivateMatch.SetActive(true);
                StateManager.Instance().SetState(StateManager.STATE.MENU_PRIVATE_MATCH);
            break;
            case StateManager.STATE.MENU_ROOM_STATUS:
                if(popupError.activeSelf)
                {
                    popupError.SetActive(false);
                    return;
                }
                menuRoomStatus.SetActive(false);
                menuPrivateMatch.SetActive(true);
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
