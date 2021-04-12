using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : Singleton<StateManager>
{
    public enum STATE
    {
        MENU_MAIN = 0,
        MENU_SELECT_PLAY_MODE,
        MENU_SELECT_DRAGON,
        MENU_SELECT_BACKGROUND,
        MENU_OPTIONS,
        MENU_ABOUT,
        MENU_SHOP,
        MENU_SHOP_DRAGON,
        MENU_SHOP_BACKGROUND,
        MENU_WATCH_ADS,
        MENU_ONLINE_PLAY,
        MENU_MATCH_MAKE,
        MENU_PRIVATE_MATCH,
        MENU_JOIN_ROOM,
        MENU_ROOM_STATUS,
        MENU_INGAME,
        MENU_RESULT,
        GAMEPLAY
    }
    private STATE mCurrentState;
    
    public enum MODE
    {
        SINGLE_PLAY = 0,
        ONLINE_PLAY
    }
    private MODE mCurrentMode;

    void Start()
    {
        //limit fps to save battery
		QualitySettings.vSyncCount = 1;
		Application.targetFrameRate = 60;
        
        mCurrentState = STATE.MENU_MAIN;
    }

    public STATE GetState()
    {
        return mCurrentState;
    }

    public void SetState(STATE newState)
    {
        mCurrentState = newState;
    }

    public MODE GetMode()
    {
        return mCurrentMode;
    }

    public void SetMode(MODE newMode)
    {
        mCurrentMode = newMode;
    }
}
