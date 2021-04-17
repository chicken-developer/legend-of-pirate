using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Playfield : Singleton<Playfield>
{
    public enum STATE
    {
        INIT = 0,

        PLAYGAME,

        GAMEOVER
    }

    [Header("Gamefield Settings")]
    [SerializeField] GameObject mEggPrefab;
	[SerializeField] GameObject mGameOverPopup;
	[SerializeField] GameObject mShooter;
    [SerializeField] GameObject mEggAnimation;
    [SerializeField] int ROWS = 9;
    [SerializeField] int COLUMNS = 8;
    [SerializeField] float OFFSET_EGG = 0.33f;
    [SerializeField] int NUM_ROWS_SHOW = 3;
	[SerializeField] int WEIGHTED_RANDOM_CHANCE = 70;

	static int DEFAULT_DROP_TIME = 10;
    [SerializeField] float mTimeToDrop = DEFAULT_DROP_TIME;// 5s
    List<List<Egg>> mGird = new List<List<Egg>>();
    STATE mState;
    MTimer mTimer = new MTimer();
	
	//game level
	[SerializeField] GameObject mLevelText;
	MTimer mLevelTimer = new MTimer();
	static int INSCREASE_LEVEL_TIME = 45;	
	static int INITIALIZE_LEVEL = 1;
	static int MAX_LEVEL = 12;
	[SerializeField] int mCurrentLevel = INITIALIZE_LEVEL;
	//game level end

	//dragon move
	public enum PLAYER_DRAGON_STATE
    {
		WAITING_MOVE_UP = 0,

		WAITING_MOVE_DOWN,

		RELOAD_MOVE_TO_SHOOTER,

		RELOAD_MOVE_TO_WAREHOUSE,

		RELOAD_MOVE_TO_DOWN,

		SWAP_MOVE_TO_SHOOTER,

		SWAP_MOVE_TO_DOWN,

		SPECIAL_FLASH
    }
	PLAYER_DRAGON_STATE mPlayerDragonState;	

	static float M_PLAYER_DRAGON_FLY_SPEED = 0.5f;//1
	static float M_PLAYER_DRAGON_RELOAD_SPEED = 10;//10
	static float M_PLAYER_DRAGON_SWAP_SPEED = 10;//10
	float mPlayerDragonMoveSpeed;
    bool forceSpawnLine;
	
    [Header("Dragon holder Settings")]
	[SerializeField] GameObject mPlayerDragon;
	[SerializeField] GameObject mPlayerDragonPositions;
	GameObject mPlayerDragonTarget;
	GameObject mPlayerDragonPosUp;
	GameObject mPlayerDragonPosDown;
	GameObject mPlayerDragonPosShooter;
	GameObject mPlayerDragonPosWarehouse;
	//dragon move end

    [Header("Egg field Settings")]
    bool mOdd = false;//hang le~
	bool animateEggDown = false;
	[SerializeField] float eggDownSpeedSeconds = 0.25f;
	float eggDownAnimTime;

    int INITIAL_FALLEN_EGG = 4;
	List<GameObject> fallenEggPool;
	
    [Header("Game score")]
	int totalScore;
	[SerializeField] GameObject mScoreText;
	
    [Header("Power egg Settings")]
    [SerializeField] int FLASH_EGG_COMBO_REQUIRED;
    int currentCombo;

    [Header("Hazard Settings")]
    [SerializeField] int MAX_HAZARD_ON_SCREEN = 3;
    [SerializeField] int HAZARD_CHANCE = 4;
    int currentHazardCount;

    public enum HAZARD_STATUS
    {
        NONE = 0,
        FROZEN = 1,
        FIREWALL = 2
    }
    HAZARD_STATUS mActiveHazard;
    // Frozen hazard
    [SerializeField] float frozenHazardMaxTime = 3.0f;
    private float frozenHazardTime;
    [SerializeField] GameObject frozenHazardObject;
    // Fire hazard
    [SerializeField] float firewallHazardMaxTime = 6.0f;
    private float firewallHazardTime;
    [SerializeField] GameObject firewallHazardObject;
    [SerializeField] GameObject firewallHazardLinePos;
    public enum HAZARD_LINE_DIRECTION
    {
        LEFT = 0,
        RIGHT
    }
    HAZARD_LINE_DIRECTION mFirewallHazardLineDir;
    GameObject mFirewallHazardLine;
    GameObject mFirewallLineLeftPos;
    GameObject mFirewallLineRightPos;
    [SerializeField] float firewallHazardMoveSpeed = 5.0f;

    [Header("UI Animation")]
    [SerializeField] GameObject uiAnimationObject;
    // Start is called before the first frame update
    void Start()
    {
		//limit fps to save battery
		QualitySettings.vSyncCount = 1;
		Application.targetFrameRate = 60;

		eggDownAnimTime = 0.0f;
		totalScore = 0;
        SetState(STATE.INIT);
		PrepareFallenEggPool();
		AudioManager.Instance().InitAudio();
        InitPlayField();
    }

	public void AddTotalScoreByCount(int brokenEggs, int fallenEggs)
	{
		int score = 10 * brokenEggs;
		if(fallenEggs >= 1)
			score *= fallenEggs;
		totalScore += brokenEggs + fallenEggs;
		mScoreText.GetComponent<Text>().text = "Gems broken: " + totalScore.ToString("N0");
	}

	void PrepareFallenEggPool()
	{
		fallenEggPool = new List<GameObject>();
		for(int i = 0; i < INITIAL_FALLEN_EGG; i++)
		{
			GameObject newEgg = Instantiate(mEggPrefab);
			newEgg.transform.parent = gameObject.transform;
			newEgg.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
			newEgg.GetComponent<Egg>().SetColor((int)Defines.COLOR.NONE);
			newEgg.gameObject.SetActive(false);
			newEgg.gameObject.name = "FallenEgg";
			newEgg.gameObject.tag = "Untagged";
			fallenEggPool.Add(newEgg);
		}
	}

	void SetFallenEggInPool(Egg refEgg, int position)
	{
		if(position >= fallenEggPool.Count)
			ExpandFallenEggPool(refEgg);
		else
		{
			fallenEggPool[position].transform.position = refEgg.gameObject.transform.position;
			fallenEggPool[position].GetComponent<Egg>().SetColor(refEgg.GetColor());
			fallenEggPool[position].gameObject.SetActive(true);
		}
	}

	void ExpandFallenEggPool(Egg refEgg)
	{
		GameObject newEgg = Instantiate(mEggPrefab);
		newEgg.transform.parent = gameObject.transform;
		newEgg.transform.position = refEgg.gameObject.transform.position;
		newEgg.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
		newEgg.gameObject.SetActive(true);
		newEgg.GetComponent<Egg>().SetColor(refEgg.GetColor());
		newEgg.gameObject.name = "FallenEgg";
		newEgg.gameObject.tag = "Untagged";
		fallenEggPool.Add(newEgg);
	}
	
    // Update is called once per frame
    void Update()
    {
        mTimer.Update(Time.deltaTime);
		mLevelTimer.Update(Time.deltaTime);
		if(animateEggDown)
		{
			eggDownAnimTime += Time.deltaTime;
			UpdateEggGridDownAnim();
		}
        switch(mState)
        {
            case STATE.INIT:
                break;
            case STATE.PLAYGAME:
                UpdateHazardStatus();
                if(mTimer.isDone() || forceSpawnLine)
                {
                    SpawnLine();
                    mTimer.SetDuration(mTimeToDrop);
                    forceSpawnLine = false;
#if UNITY_EDITOR
                    //if(Debug.isDebugBuild)
                    //    PrintGird();
#endif
                }
                if(currentCombo >= FLASH_EGG_COMBO_REQUIRED)
                {
                    uiAnimationObject.GetComponent<UIAnimationHandler>().ShowComboTextTimed(currentCombo, null, null);
                    currentCombo = 0;
					mShooter.GetComponent<Shooter>().eggWarehouseObj.Request("prepairflash");
					mPlayerDragonState = PLAYER_DRAGON_STATE.SPECIAL_FLASH;
                }
				if(mLevelTimer.isDone())
				{
					if(mCurrentLevel < MAX_LEVEL)
					{
						mCurrentLevel ++;
                        if(mTimeToDrop > 3)
						    mTimeToDrop --;
                        else
                            mTimeToDrop *= 0.75f;
						mLevelText.GetComponent<Text>().text = "Level: " + mCurrentLevel.ToString();
						mLevelTimer.SetDuration(INSCREASE_LEVEL_TIME);
					}
				}
				//update dragon move
				switch (mPlayerDragonState)
				{
					case PLAYER_DRAGON_STATE.WAITING_MOVE_UP:
						mPlayerDragonMoveSpeed = M_PLAYER_DRAGON_FLY_SPEED;
						mPlayerDragonTarget = mPlayerDragonPosUp;
						if (mPlayerDragon.transform.position == mPlayerDragonPosUp.transform.position)
							mPlayerDragonState = PLAYER_DRAGON_STATE.WAITING_MOVE_DOWN;
					break;
					case PLAYER_DRAGON_STATE.WAITING_MOVE_DOWN:
						mPlayerDragonMoveSpeed = M_PLAYER_DRAGON_FLY_SPEED;
						mPlayerDragonTarget = mPlayerDragonPosDown;
						if (mPlayerDragon.transform.position == mPlayerDragonPosDown.transform.position)
							mPlayerDragonState = PLAYER_DRAGON_STATE.WAITING_MOVE_UP;
						break;
					case PLAYER_DRAGON_STATE.RELOAD_MOVE_TO_SHOOTER:
						mPlayerDragonMoveSpeed = M_PLAYER_DRAGON_RELOAD_SPEED;
						mPlayerDragonTarget = mPlayerDragonPosShooter;
						if (mPlayerDragon.transform.position == mPlayerDragonPosShooter.transform.position)
						{
							if (mShooter.activeSelf)
							{							
								mShooter.GetComponent<Shooter>().eggWarehouseObj.Request("reload");
								mShooter.GetComponent<Shooter>().eggWarehouseObj.gameObject.SetActive(false);
								mPlayerDragonState = PLAYER_DRAGON_STATE.RELOAD_MOVE_TO_WAREHOUSE;
							}
							else
							{
								mShooter.SetActive(true);
							}
						}
						break;
					case PLAYER_DRAGON_STATE.RELOAD_MOVE_TO_WAREHOUSE:
						mPlayerDragonMoveSpeed = M_PLAYER_DRAGON_RELOAD_SPEED;
						mPlayerDragonTarget = mPlayerDragonPosWarehouse;
						mShooter.GetComponent<Shooter>().eggWarehouseObj.gameObject.SetActive(false);
						if (mPlayerDragon.transform.position == mPlayerDragonPosWarehouse.transform.position)
						{
							mPlayerDragonState = PLAYER_DRAGON_STATE.RELOAD_MOVE_TO_DOWN;
							mShooter.GetComponent<Shooter>().eggWarehouseObj.gameObject.SetActive(true);
						}
						break;
					case PLAYER_DRAGON_STATE.RELOAD_MOVE_TO_DOWN:
						mPlayerDragonMoveSpeed = M_PLAYER_DRAGON_RELOAD_SPEED;
						mPlayerDragonTarget = mPlayerDragonPosDown;
						if (mPlayerDragon.transform.position == mPlayerDragonPosDown.transform.position)
							mPlayerDragonState = PLAYER_DRAGON_STATE.WAITING_MOVE_UP;
						break;
					case PLAYER_DRAGON_STATE.SWAP_MOVE_TO_SHOOTER:
						mPlayerDragonMoveSpeed = M_PLAYER_DRAGON_SWAP_SPEED;
						mPlayerDragonTarget = mPlayerDragonPosShooter;
						if (mPlayerDragon.transform.position == mPlayerDragonPosShooter.transform.position)
						{
							mShooter.GetComponent<Shooter>().eggWarehouseObj.Request("swap");
							mPlayerDragonState = PLAYER_DRAGON_STATE.SWAP_MOVE_TO_DOWN;
						}
						break;
					case PLAYER_DRAGON_STATE.SWAP_MOVE_TO_DOWN:
						mPlayerDragonMoveSpeed = M_PLAYER_DRAGON_SWAP_SPEED;
						mPlayerDragonTarget = mPlayerDragonPosDown;
						if (mPlayerDragon.transform.position == mPlayerDragonPosDown.transform.position)
							mPlayerDragonState = PLAYER_DRAGON_STATE.WAITING_MOVE_UP;
						break;
					case PLAYER_DRAGON_STATE.SPECIAL_FLASH:
						mPlayerDragonMoveSpeed = M_PLAYER_DRAGON_RELOAD_SPEED;
						mPlayerDragonTarget = mPlayerDragonPosShooter;
						if (mPlayerDragon.transform.position == mPlayerDragonPosShooter.transform.position)
						{
							if (mShooter.activeSelf)
							{	
								mShooter.GetComponent<Shooter>().eggWarehouseObj.Request("specialflash");
								mShooter.GetComponent<Shooter>().eggWarehouseObj.gameObject.SetActive(false);
								mPlayerDragonState = PLAYER_DRAGON_STATE.RELOAD_MOVE_TO_WAREHOUSE;
							}
							else
							{
								mShooter.SetActive(true);
							}
						}
						break;
				}
				float step =  mPlayerDragonMoveSpeed * Time.deltaTime;
                if(!IsHazardState(HAZARD_STATUS.FROZEN))
                {
                    mPlayerDragon.transform.position = Vector3.MoveTowards(mPlayerDragon.transform.position, mPlayerDragonTarget.transform.position, step);
                    mShooter.GetComponent<Shooter>().eggWarehouseObj.transform.position = new Vector3(mPlayerDragon.transform.position.x - 0.2f, mPlayerDragon.transform.position.y - 0.9f, 0);
                }			
				//end dragon move

                break;				
            default:
                break;
        }		
    }

	public void ReloadEgg()
    {
		mPlayerDragonState = PLAYER_DRAGON_STATE.RELOAD_MOVE_TO_SHOOTER;
	}

	public void SwapEgg()
    {
		mPlayerDragonState = PLAYER_DRAGON_STATE.SWAP_MOVE_TO_SHOOTER;
    }

	public PLAYER_DRAGON_STATE GetDragonState()
	{
		return mPlayerDragonState;
	}

    public bool isOdd()
    {
        return mOdd;
    }

    public void SetState(STATE st)
    {
        mState = st;
        switch(mState)
        {
            case STATE.INIT:
				mShooter.GetComponent<Shooter>().enabled = true;
				enabled = true;
                forceSpawnLine = false;
				totalScore = 0;
				eggDownAnimTime = 0;
                currentCombo = 0;
				mCurrentLevel = INITIALIZE_LEVEL;
				mTimeToDrop = DEFAULT_DROP_TIME;
				mPlayerDragonState = PLAYER_DRAGON_STATE.RELOAD_MOVE_TO_SHOOTER;
				mPlayerDragonPosUp = mPlayerDragonPositions.transform.GetChild(0).gameObject;
				mPlayerDragonPosDown = mPlayerDragonPositions.transform.GetChild(1).gameObject;
				mPlayerDragonPosShooter = mPlayerDragonPositions.transform.GetChild(2).gameObject;
				mPlayerDragonPosWarehouse = mPlayerDragonPositions.transform.GetChild(3).gameObject;
				mPlayerDragonTarget = mPlayerDragonPosShooter;
                currentHazardCount = 0;
                mActiveHazard = 0;
                frozenHazardObject.SetActive(false);
                firewallHazardObject.SetActive(false);
                SetHazardState(HAZARD_STATUS.NONE);
                mFirewallHazardLine = firewallHazardObject.transform.GetChild(2).gameObject;
                mFirewallLineLeftPos = firewallHazardLinePos.transform.GetChild(0).gameObject;
                mFirewallLineRightPos = firewallHazardLinePos.transform.GetChild(1).gameObject;
				break;
            case STATE.PLAYGAME:
                mTimer.SetDuration(mTimeToDrop);
				mLevelTimer.SetDuration(INSCREASE_LEVEL_TIME);
				AudioManager.Instance().PlayMusicGamePlayBackground();
                break;
			case STATE.GAMEOVER:
				enabled = false;
				mShooter.GetComponent<Shooter>().enabled = false;
                SaveHighScore();
				mGameOverPopup.SetActive(true);
				mGameOverPopup.GetComponent<GameOverPopup>().Setup(totalScore, LoadHighScore());
				AudioManager.Instance().PlayMusicGameOverBackground();
				break;
            default:
                break;
        }
    }
    public void SaveHighScore()
    {
        if(totalScore > LoadHighScore())
        {
            PlayerPrefs.SetInt("highscore", totalScore);
            PlayerPrefs.Save();
        }
    }
    public int LoadHighScore()
    {
        return PlayerPrefs.GetInt("highscore", 0);
    }

	void ClearPlayField()
    {
		foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
	}

    void InitPlayField()
    {	
		mOdd = false;
        for(int r = 0; r < ROWS; r++)
        {
            List<Egg> eggs = new List<Egg>();
            float offset = (mOdd) ? OFFSET_EGG : 0;

            for (int c = 0; c < COLUMNS; c++)
            {
                GameObject obj = Instantiate(mEggPrefab, transform.position, Quaternion.identity);
				obj.transform.parent = gameObject.transform;
                Egg egg = obj.GetComponent<Egg>();
                egg.SetPosition(c, r, offset);
                int color = Random.Range(0, Defines.EggCount);
                egg.SetColor(color);
                egg.gameObject.SetActive(false);
                egg.SetHazard(false);
                eggs.Add(egg);
            }
            mGird.Add(eggs);
            mOdd = !mOdd;
        }
		mOdd = true;
		for(int i = 0; i < NUM_ROWS_SHOW; ++i)
        {
			SpawnLine();
        }
        SetState(STATE.PLAYGAME);
    }

	public void ResetPlayfield()
	{
		SetState(STATE.INIT);
		mShooter.GetComponent<Shooter>().ResetShooter();
		AudioManager.Instance().InitAudio();
		ResetLevel();
		
		ClearPlayField();
		for(int i = 0; i < NUM_ROWS_SHOW; ++i)
        {
			SpawnLine();
        }
        SetState(STATE.PLAYGAME);
	}

	void ResetLevel()
	{
		mScoreText.GetComponent<Text>().text = "Eggs broken: " + totalScore.ToString("N0");
		mLevelText.GetComponent<Text>().text = "Level: " + mCurrentLevel.ToString();
		mLevelTimer.SetDuration(INSCREASE_LEVEL_TIME);		
		mTimer.SetDuration(mTimeToDrop);
	}

	public Egg GetEggInGrid(int row, int column)
	{
		int newCol = (column < 0) ? 0 : ((column > COLUMNS-1) ? COLUMNS-1 : column);
        int newRow = (row < 0) ? 0 : ((row > ROWS-1) ? ROWS-1 : row);
		return mGird[newRow][newCol];
	}

    public int GetMaxRow()
    {
        return ROWS;
    }

    public int GetMaxCol()
    {
        return COLUMNS;
    }
	
	void UpdateEggGridDownAnim()
	{
		for(int r = 0; r < ROWS; r++)
        {
            foreach(Egg e in mGird[r])
            {
                mGird[r][e.GetCol()].UpdatePositionAnim(-Egg.EGG_RATIO * ((eggDownSpeedSeconds - eggDownAnimTime) / eggDownSpeedSeconds), e.GetOffset());
            }
        }
		if(eggDownAnimTime >= eggDownSpeedSeconds)
		{
			animateEggDown = false;
		}
	}
	
    void SpawnLine() 
    {
        for(int r = ROWS - 2; r >= 0; r--)
        {
            foreach(Egg e in mGird[r])
            {
				mGird[r + 1][e.GetCol()].SetColor(e.GetColor());
                mGird[r + 1][e.GetCol()].SetHazard(e.IsHazard());
				mGird[r + 1][e.GetCol()].SetPosition(e.GetCol(), r + 1, e.GetOffset());
				mGird[r + 1][e.GetCol()].gameObject.SetActive(e.gameObject.activeSelf);
				mGird[r + 1][e.GetCol()].UpdatePositionAnim(-Egg.EGG_RATIO, e.GetOffset());
            }
        }
        
        float offset = mOdd ? OFFSET_EGG : 0;
        for(int i = 0; i < COLUMNS; ++i)
        {
			Egg e = mGird[0][i];
			int color;

			int left = i == 0 ? 0 : mOdd ? i : i - 1;
			int right = i == (COLUMNS - 1) ? (COLUMNS - 1) : mOdd ? i + 1 : i;
			bool haveLeftEgg = mGird[1][left].isActiveAndEnabled;
			bool haveRightEgg = mGird[1][right].isActiveAndEnabled;

			if (Random.Range(0,100) < WEIGHTED_RANDOM_CHANCE && (haveLeftEgg || haveRightEgg)) // 80%
            {
				if(haveLeftEgg && haveRightEgg)
                {
					if (Random.Range(0, 2) == 1) // 50%
					{
						color = mGird[1][left].GetColor();
					}
					else
					{
						color = mGird[1][right].GetColor();
					}
				}
                else if(haveLeftEgg)
                {
					color = mGird[1][left].GetColor();
				}
                else
                {
					color = mGird[1][right].GetColor();
				}
            }
            else
            {
				color = Random.Range(0, Defines.EggCount);
			}
            e.SetColor(color);      
            e.SetPosition(e.GetCol(), 0, offset);          
            e.gameObject.SetActive(true);
			e.UpdatePositionAnim(-Egg.EGG_RATIO, e.GetOffset());
            e.SetHazard(false);
            if(Random.Range(0,100) < HAZARD_CHANCE && currentHazardCount < MAX_HAZARD_ON_SCREEN)
            {  
                switch(color)
                {
                    case 2:
                    case 4:
                        e.SetHazard(true);
                        currentHazardCount++;
                    break;
                }
            }
        }
        mOdd = !mOdd;
		animateEggDown = true;
		eggDownAnimTime = 0.0f;
    }

	// Handle adding egg to grid when hit, if eggOnGrid is null, it is hit at the top
	public void AttachEggOnGrid(GameObject eggOnGrid, GameObject refBullet)
	{
        float minDist = 99999.0f;
		Egg newEgg = null;
        if(eggOnGrid == null) // Hit on top case
        {
            for(int i = 0; i < GetMaxCol(); i++)
            {
                Egg egg = GetEggInGrid(0, i);
                float currentDist = Vector3.Distance(egg.gameObject.transform.position, refBullet.transform.position);
                if(currentDist < minDist)
                {
                    minDist = currentDist;
                    newEgg = egg;
                }
            }
        }
        else // normal case
        {
            newEgg = GetNewEggPosition(eggOnGrid, refBullet);
        }
        int fallenCount = 0;
        int crackedCount = 0;
        switch(refBullet.GetComponent<Bullet>().GetColor())
        {
            case (int)Defines.COLOR.SPECIAL_FLASH:
                
                if(eggOnGrid != null)
                {
                    crackedCount = PopEggsFlashEgg(eggOnGrid.GetComponent<Egg>());
                    mEggAnimation.GetComponent<EggAnimationHandler>().AddFlashBlast(eggOnGrid.transform.position);
                }
                else
                {
                    crackedCount = PopEggsFlashEgg(newEgg);
                    mEggAnimation.GetComponent<EggAnimationHandler>().AddFlashBlast(newEgg.transform.position);
                }
                fallenCount = FallEggs();
                //Debug.Log(crackedCount + " and " + fallenCount);
                AddTotalScoreByCount(crackedCount, fallenCount);
                AudioManager.Instance().PlaySFXFlashEgg();
                break;
            case (int)Defines.COLOR.SPECIAL_DARKGREY:
                if(eggOnGrid != null)
                {
                    crackedCount = PopEggsPowerEgg(eggOnGrid.GetComponent<Egg>().GetColor());
                    mEggAnimation.GetComponent<EggAnimationHandler>().AddPowerBlast(eggOnGrid.transform.position);
					AudioManager.Instance().PlaySFXPowerEgg();
                }
                else
                {
                    Egg nearbyEgg = GetNearbyEgg(newEgg);
                    if(nearbyEgg != null)
                    {
                        crackedCount = PopEggsPowerEgg(nearbyEgg.GetColor());
						mEggAnimation.GetComponent<EggAnimationHandler>().AddPowerBlast(nearbyEgg.transform.position);
						AudioManager.Instance().PlaySFXPowerEgg();
                    }
                }
                fallenCount = FallEggs();
                //Debug.Log(crackedCount + " and " + fallenCount);
                AddTotalScoreByCount(crackedCount, fallenCount);                
                break;
			case (int)Defines.COLOR.SPECIAL_RAINBOW:
                //Debug.Log("OKKK --- "+bulletEgg.GetComponent<Bullet>().GetColor());
                if(eggOnGrid != null)
                {
                    newEgg.SetColor(eggOnGrid.GetComponent<Egg>().GetColor());
                }
                else
                {
                    int currentEgg = UnityEngine.Random.Range(0, Defines.EggCount);
                    newEgg.SetColor(currentEgg);
                }
                newEgg.gameObject.SetActive(true);
                List<Egg> crackedEggRainbow = SpreadToAllNearbyEgg(newEgg);
                crackedCount = crackedEggRainbow.Count;
                fallenCount = 0;
                if(crackedCount >= 3)
                {
                    PopEggs(crackedEggRainbow, true);
                    fallenCount = FallEggs();
                    AddTotalScoreByCount(crackedCount, fallenCount);                    
                    currentCombo++;
                    uiAnimationObject.GetComponent<UIAnimationHandler>().ShowComboTextTimed(currentCombo, null, null);
                }
                else
                {                    
                    if(currentCombo > 0)
                        uiAnimationObject.GetComponent<UIAnimationHandler>().ShowComboTextTimed(0, null, null);
                    currentCombo = 0;
                }
				AudioManager.Instance().PlaySFXRainbowEgg();
                break;
            default:
                //Debug.Log("OKKK --- "+bulletEgg.GetComponent<Bullet>().GetColor());
                newEgg.SetColor(refBullet.GetComponent<Bullet>().GetColor());
                newEgg.gameObject.SetActive(true);
                List<Egg> crackedEgg = SpreadToAllNearbyEgg(newEgg);
                crackedCount = crackedEgg.Count;
                fallenCount = 0;
                if(crackedCount >= 3)
                {
                    PopEggs(crackedEgg);
                    fallenCount = FallEggs();
                    AddTotalScoreByCount(crackedCount, fallenCount);
                    AudioManager.Instance().PlaySFXEggBreak();
                    currentCombo++;
                    uiAnimationObject.GetComponent<UIAnimationHandler>().ShowComboTextTimed(currentCombo, null, null);
                }
                else
                {
                    AudioManager.Instance().PlaySFXEggHit();
                    if(currentCombo > 0)
                        uiAnimationObject.GetComponent<UIAnimationHandler>().ShowComboTextTimed(0, null, null);
                    currentCombo = 0;
                }
                break;
        }
		
	}

	// -180 < AngleOfVector < 180, which is angle from Vector3.up or Upwards direction
	// There are 6 possible new spawn of flat-top hex grids, the rules are here
	// Top-Left: -60 <= AngleOfVector < 0 ; Top-Right: 0 <= AngleOfVector < 60
	// Left: -120 <= AngleOfVector < -60 ; Right: 60 <= AngleOfVector < 120
	// Bottom-Left: -180 <= AngleOfVector < -120 ; Bottom-Right: 120 <= AngleOfVector < 180
	// Down here is shortened function of that
    int GetKeyFromAngle(float AngleOfVector)
	{
		if(AngleOfVector >= 0)
		{
			return (int)Mathf.Ceil(AngleOfVector / 60);
		}
		else
		{
			return 6 - (int)Mathf.Floor(-AngleOfVector / 60);
		}
	}
	
	float GetAngleCenterByKey(int key)
	{
		switch(key)
		{
			case 1:
				return 30.0f;
			case 2:
				return 90.0f;
			case 3:
				return 150.0f;
			case 4:
				return -150.0f;
			case 5:
				return -90.0f;
			case 6:
				return -30.0f;
		}
		return 999f;
	}
	
	int GetPositionFromKey(Egg eggData, int key, out int newRow, out int newCol)
	{
		int offsetRow = (eggData.GetRow() % 2 == (isOdd() ? 1 : 0)) ? 1 : 0;
		newCol = -1;
		newRow = -1;
		//Debug.Log(key + " and " + offsetRow);
		switch(key)
		{
			case 1:
				newCol = eggData.GetCol() + offsetRow;
				newRow = eggData.GetRow() - 1;
			break;
			case 2:
				newCol = eggData.GetCol() + 1;
				newRow = eggData.GetRow();
			break;
			case 3:
				newCol = eggData.GetCol() + offsetRow;
				newRow = eggData.GetRow() + 1;
			break;
			case 4:
				newCol = eggData.GetCol() - 1 + offsetRow;
				newRow = eggData.GetRow() + 1;
			break;
			case 5:
				newCol = eggData.GetCol() - 1;
				newRow = eggData.GetRow();
			break;
			case 6:
				newCol = eggData.GetCol() - 1 + offsetRow;
				newRow = eggData.GetRow() - 1;
			break;
		}
		return key;
	}
	
	// Find closest egg, which takes bullet egg and eggOnGrid to process;
	Egg GetNewEggPosition(GameObject eggOnGrid, GameObject refBullet)
	{
		Vector3 direction = refBullet.transform.position - eggOnGrid.transform.position; // this is a vector from hit egg to bullet
		Egg eggData = eggOnGrid.GetComponent<Egg>();
		float AngleOfVector = Vector3.Angle(Vector3.up, direction);
		if(direction.x < 0)
			AngleOfVector = -AngleOfVector;
		int offsetRow = (eggData.GetRow() % 2 == 1) ? 0 : 1;
		int newCol = -1;
		int newRow = -1;
		//Debug.Log(AngleOfVector);
		int positionKey = GetPositionFromKey(eggData, GetKeyFromAngle(AngleOfVector), out newRow, out newCol);
		//Debug.Log(eggData.GetRow() +":"+ eggData.GetCol()+ " ;; New egg: "+newRow+":"+newCol);
		Egg firstCandidate = GetEggInGrid(newRow, newCol);
		//Debug.Log(firstCandidate.GetStringFromColor());
        if(firstCandidate.GetStringFromColor() != "None") // We have to fetch second candidate, as this one is there
        {
            //Debug.Log("Failed 1");
            int newKey = -1;
            float minAngleDiff = 181f;
            for(int key = 1; key <= 6; key++)
            {
                if(key != positionKey)
                {
                    //Debug.Log("Key "+key+" : "+Mathf.Abs(GetAngleCenterByKey(key) - AngleOfVector));
                    if(Mathf.Abs(GetAngleCenterByKey(key) - AngleOfVector) < minAngleDiff)
                    {
                        newKey = key;
                        minAngleDiff = Mathf.Abs(GetAngleCenterByKey(key) - AngleOfVector);
                    }
                }
            }
            GetPositionFromKey(eggData, newKey, out newRow, out newCol);
            //Debug.Log("C2: "+eggData.GetRow() +":"+ eggData.GetCol()+ " ;; New egg: "+newRow+":"+newCol);
            return GetEggInGrid(newRow, newCol);
        }
		return firstCandidate;
	}
	
	// Handle egg pop and fall, once hit grid, we are finding all nearby eggs with same color, and trigger pop and fall if >= 3 are the same
	public List<Egg> SpreadToAllNearbyEgg(Egg egg, bool isOnlyCheckSame = true)
	{
		Stack<Egg> uncheckedEgg = new Stack<Egg>(); // contains all potential eggs that has not been further check
		SortedSet<Egg> checkedEgg = new SortedSet<Egg>(); // contains all eggs that are checked
		List<Egg> crackedEgg = new List<Egg>(); // list of eggs that can be cracked (same color with hit)
		uncheckedEgg.Push(egg);
		crackedEgg.Add(egg);
		while(uncheckedEgg.Count > 0)
		{
			Egg currentEgg = uncheckedEgg.Pop();
			int curRow, curCol;
			for(int i = 1; i <= 6; i++) // Check 6 eggs next to it
			{
				GetPositionFromKey(currentEgg, i, out curRow, out curCol);
				if(curRow < 0 || curRow > ROWS || curCol < 0 || curCol > COLUMNS)
					continue;
				Egg eggKey = GetEggInGrid(curRow, curCol);
				if(eggKey != null)
				{
					if(!checkedEgg.Contains(eggKey))
					{
						if(eggKey.GetStringFromColor() != "None")
						{
							if(isOnlyCheckSame && eggKey.GetColor() != currentEgg.GetColor())
								continue;
							if(!uncheckedEgg.Contains(eggKey))
								uncheckedEgg.Push(eggKey);
							if(!crackedEgg.Contains(eggKey))
								crackedEgg.Add(eggKey);
						}
					}
				}
			}
			checkedEgg.Add(currentEgg);
		}
		return crackedEgg;
	}

    int PopEggsPowerEgg(int eggColor)
    {
        int crackedCount = 0;
        for(int r = 0; r < ROWS; r++)
            for(int c = 0; c < COLUMNS; c++)
            {
                if(mGird[r][c].GetColor() == eggColor && mGird[r][c].gameObject.activeSelf)
                {
                    //Debug.Log("UHM: "+r+":"+c);
                    if(mGird[r][c].IsHazard())
                    {
                        mGird[r][c].SetHazard(false);
                        currentHazardCount--;
                    }
                    mEggAnimation.GetComponent<EggAnimationHandler>().AddBrokenEgg(mGird[r][c]);
                    mGird[r][c].SetColor((int)Defines.COLOR.NONE);
                    mGird[r][c].gameObject.SetActive(false);
                    crackedCount++;
                }
            }
        forceSpawnLine = true;
        for(int c = 0; c < COLUMNS; c++)
        {
            if(mGird[0][c].GetColor() != (int)Defines.COLOR.NONE && mGird[0][c].gameObject.activeSelf)
            {
                forceSpawnLine = false;
                break;
            }
        }
        return crackedCount;
    }

    int PopEggsFlashEgg(Egg egg)
    {
        int r = egg.GetRow();
        int crackedCount = 0;
        for(int c = 0; c < COLUMNS; c++)
        {
            if(mGird[r][c].gameObject.activeSelf)
            {
                //Debug.Log("UHM: "+r+":"+c);
                if(mGird[r][c].IsHazard())
                {
                    mGird[r][c].SetHazard(false);
                    currentHazardCount--;
                }
                mEggAnimation.GetComponent<EggAnimationHandler>().AddBrokenEgg(mGird[r][c]);
                mGird[r][c].SetColor((int)Defines.COLOR.NONE);
                mGird[r][c].gameObject.SetActive(false);
                crackedCount++;
            }
        }
        forceSpawnLine = true;
        for(int c = 0; c < COLUMNS; c++)
        {
            if(mGird[0][c].GetColor() != (int)Defines.COLOR.NONE && mGird[0][c].gameObject.activeSelf)
            {
                forceSpawnLine = false;
                break;
            }
        }
        return crackedCount;
    }

    Egg GetNearbyEgg(Egg currentEggPosition)
    {
        int row, col;
        for(int i = 1; i <= 6; i++)
        {
            GetPositionFromKey(currentEggPosition, i, out row, out col);
            if(row >= 0 && row < ROWS && col >= 0 && col < COLUMNS)
            {
                if(mGird[row][col].gameObject.activeSelf)
                {
                    return mGird[row][col];
                }
            }
        }
        return null;
    }

    	// Handle popping eggs
	void PopEggs(List<Egg> eggs, bool ignoreHazard = false)
	{
		foreach(Egg egg in eggs)
		{
            if(egg.IsHazard())
            {
                if(!ignoreHazard)
                    ActivateHazard(egg);
                egg.SetHazard(false);
                currentHazardCount--;
            }
            mEggAnimation.GetComponent<EggAnimationHandler>().AddBrokenEgg(egg);
			egg.SetColor((int)Defines.COLOR.NONE);
			egg.gameObject.SetActive(false);
		}
        forceSpawnLine = true;
        for(int c = 0; c < COLUMNS; c++)
        {
            if(mGird[0][c].GetColor() != (int)Defines.COLOR.NONE && mGird[0][c].gameObject.activeSelf)
            {
                forceSpawnLine = false;
                break;
            }
        }
	}

	// Handle fall eggs
	int FallEggs()
	{
		// Step 1, we need to put all eggs that is attached to the first row into the list
		SortedSet<Egg> listConnectedEggs = new SortedSet<Egg>();
		List<Egg> fallEggs = new List<Egg>();
		for(int i = 0; i < COLUMNS; i++)
		{
			Egg topEgg = GetEggInGrid(0, i);
			if(!topEgg.gameObject.activeSelf) // there is no egg here
				continue;
			List<Egg> eggs = SpreadToAllNearbyEgg(GetEggInGrid(0, i), false);
			foreach(Egg eg in eggs)
			{
				listConnectedEggs.Add(eg);
			}
		}
		for(int row = 0; row < ROWS; row++)
            for(int col = 0; col < COLUMNS; col++)
            {
                Egg eggTest = GetEggInGrid(row, col);
                if(eggTest.gameObject.activeSelf && !listConnectedEggs.Contains(eggTest))
                {
                    fallEggs.Add(eggTest);
                }
            }
		int fallenCount = fallEggs.Count;
		int iFallenEggPos = 0;
		int currentEggMax = fallenEggPool.Count;
		foreach(Egg egg in fallEggs)
		{
            if(egg.IsHazard())
            {
                egg.SetHazard(false);
                currentHazardCount--;
            }
			SetFallenEggInPool(egg, iFallenEggPos);
			egg.SetColor((int)Defines.COLOR.NONE);
			egg.gameObject.SetActive(false);
			iFallenEggPos++;
		}
		return fallenCount;
	}

    public HAZARD_STATUS GetHazardState()
    {
        return mActiveHazard;
    }

    public bool IsHazardState(HAZARD_STATUS value)
    {
        return mActiveHazard == value;
    }

    public void SetHazardState(HAZARD_STATUS newVal)
    {
        mActiveHazard = newVal;
    }

    void ActivateHazard(Egg egg)
    {
        if(GetHazardState() != HAZARD_STATUS.NONE)
        {
            return;
        }
        switch(egg.GetColor())
        {
            case (int)Defines.COLOR.LIGHT_BLUE: // Frozen
                frozenHazardTime = 0.0f;
                frozenHazardObject.SetActive(true);
                SetHazardState(HAZARD_STATUS.FROZEN);
                AudioManager.Instance().PlaySFXFrozenHazard();
                break;
            case (int)Defines.COLOR.RED: // Fire
                firewallHazardTime = 0.0f;
                firewallHazardObject.SetActive(true);
                SetHazardState(HAZARD_STATUS.FIREWALL);
            break;
        }
    }

    void UpdateHazardStatus()
    {
        switch(mActiveHazard)
        {
            case HAZARD_STATUS.FROZEN: // Frozen
                frozenHazardTime += Time.deltaTime;
                if(frozenHazardTime >= frozenHazardMaxTime)
                {
                    frozenHazardObject.SetActive(false);
                    SetHazardState(HAZARD_STATUS.NONE);
                }
                break;
            case HAZARD_STATUS.FIREWALL: // Fire
                firewallHazardTime += Time.deltaTime;
                if(firewallHazardTime >= firewallHazardMaxTime)
                {
                    firewallHazardObject.SetActive(false);
                    SetHazardState(HAZARD_STATUS.NONE);
                }
                if(mFirewallHazardLineDir == HAZARD_LINE_DIRECTION.LEFT)
                {
                    mFirewallHazardLine.transform.position = Vector3.MoveTowards(mFirewallHazardLine.transform.position, mFirewallLineLeftPos.transform.position, firewallHazardMoveSpeed * Time.deltaTime);
                }
                else if(mFirewallHazardLineDir == HAZARD_LINE_DIRECTION.RIGHT)
                {
                    mFirewallHazardLine.transform.position = Vector3.MoveTowards(mFirewallHazardLine.transform.position, mFirewallLineRightPos.transform.position, firewallHazardMoveSpeed * Time.deltaTime);
                }
                if(mFirewallHazardLine.transform.position == mFirewallLineLeftPos.transform.position)
                    mFirewallHazardLineDir = HAZARD_LINE_DIRECTION.RIGHT;
                else if(mFirewallHazardLine.transform.position == mFirewallLineRightPos.transform.position)
                    mFirewallHazardLineDir = HAZARD_LINE_DIRECTION.LEFT;
                break;
            break;
        }
    }
	
#if UNITY_EDITOR
    void PrintGird()
    {
        int r = 0;
        foreach(List<Egg> eggs in mGird)
        {
            string str = "row " + r.ToString() + " | ";
            foreach(Egg egg in eggs)
            {
                str += egg.GetStringFromColor() + " | ";
            }
            Debug.Log(str);
            r++;
        }
    }
#endif
}
