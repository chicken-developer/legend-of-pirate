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

    [SerializeField] GameObject mEggPrefab;
	[SerializeField] GameObject mGameOverPopup;
	[SerializeField] GameObject mShooter;
    [SerializeField] int ROWS = 9;
    [SerializeField] int COLUMNS = 8;
    [SerializeField] float OFFSET_EGG = 0.35f;
    [SerializeField] int NUM_ROWS_SHOW = 3;
	
    [SerializeField] float mTimeToDrop = 10;// 5s
    List<List<Egg>> mGird = new List<List<Egg>>();
    STATE mState;
    Timer mTimer = new Timer();
	
	//game level
	[SerializeField] GameObject mLevelText;
	Timer mLevelTimer = new Timer();
	static int INSCREASE_LEVEL_TIME = 120;	
	static int INITIALIZE_LEVEL = 1;
	static int MAX_LEVEL = 6;
	[SerializeField] int mCurrentLevel = INITIALIZE_LEVEL;
	//game level end

    bool mOdd = false;//hang le~
	bool animateEggDown = false;
	[SerializeField] float eggDownSpeedSeconds = 0.25f;
	float eggDownAnimTime;
	
	int totalScore;
	[SerializeField] GameObject mScoreText;
	
	int INITIAL_FALLEN_EGG = 4;
	List<GameObject> fallenEggPool;

    // Start is called before the first frame update
    void Start()
    {
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
		mScoreText.GetComponent<Text>().text = "Eggs broken: " + totalScore.ToString("N0");
	}

	void PrepareFallenEggPool()
	{
		fallenEggPool = new List<GameObject>();
		for(int i = 0; i < INITIAL_FALLEN_EGG; i++)
		{
			GameObject newEgg = Instantiate(mEggPrefab);
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
                if(mTimer.isDone())
                {
                    SpawnLine();
                    mTimer.SetDuration(mTimeToDrop);
#if UNITY_EDITOR
                    //if(Debug.isDebugBuild)
                    //    PrintGird();
#endif
                }
				if(mLevelTimer.isDone())
				{
					if(mCurrentLevel < MAX_LEVEL)
					{
						mCurrentLevel ++;
						mTimeToDrop --;
						mLevelText.GetComponent<Text>().text = "Level: " + mCurrentLevel.ToString();
						mLevelTimer.SetDuration(INSCREASE_LEVEL_TIME);
					}
				}
                break;
            default:
                break;
        }		
    }

	public void ExitGame()
	{
		AudioManager.Instance().StopAllAudio();
		Application.Quit();
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
                break;
            case STATE.PLAYGAME:
                mTimer.SetDuration(mTimeToDrop);
				mLevelTimer.SetDuration(INSCREASE_LEVEL_TIME);
				AudioManager.Instance().PlayBackgroundMusic();
                break;
			case STATE.GAMEOVER:
				enabled = false;
				mShooter.GetComponent<Shooter>().enabled = false;
				mGameOverPopup.SetActive(true);
				mGameOverPopup.GetComponent<GameOverPopup>().Setup(totalScore);
				break;
            default:
                break;
        }
    }

    void InitPlayField()
    {
        for(int r = 0; r < ROWS; r++)
        {
            List<Egg> eggs = new List<Egg>();
            float offset = (mOdd) ? OFFSET_EGG : 0;

            for (int c = 0; c < COLUMNS; c++)
            {
                GameObject obj = Instantiate(mEggPrefab, transform.position, Quaternion.identity);
                Egg egg = obj.GetComponent<Egg>();
                egg.SetPosition(c, r, offset);
                int color = Random.Range(0, (int)Defines.COLOR.YELLOW+1);
                egg.SetColor(color);
                if (r > NUM_ROWS_SHOW)
                    egg.gameObject.SetActive(false);
                eggs.Add(egg);
            }
            mGird.Add(eggs);
            mOdd = !mOdd;
        }
		mOdd = true;
        SetState(STATE.PLAYGAME);
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
                if (e.gameObject.activeSelf)
                {
					mGird[r][e.GetCol()].UpdatePositionAnim(-Egg.EGG_RATIO * ((eggDownSpeedSeconds - eggDownAnimTime) / eggDownSpeedSeconds), e.GetOffset());
                }
                else
                {
					mGird[r][e.GetCol()].UpdatePositionAnim(-Egg.EGG_RATIO * ((eggDownSpeedSeconds - eggDownAnimTime) / eggDownSpeedSeconds), e.GetOffset());
                }
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
				mGird[r + 1][e.GetCol()].SetPosition(e.GetCol(), r + 1, e.GetOffset());
				mGird[r + 1][e.GetCol()].gameObject.SetActive(e.gameObject.activeSelf);
				mGird[r + 1][e.GetCol()].UpdatePositionAnim(-Egg.EGG_RATIO, e.GetOffset());
            }
        }
        
        float offset = mOdd ? OFFSET_EGG : 0;
        foreach (Egg e in mGird[0])
        {
            int color = Random.Range(0, (int)Defines.COLOR.YELLOW+1);
            e.SetColor(color);      
            e.SetPosition(e.GetCol(), 0, offset);          
            e.gameObject.SetActive(true);
			e.UpdatePositionAnim(-Egg.EGG_RATIO, e.GetOffset());
        }
        mOdd = !mOdd;
		animateEggDown = true;
		eggDownAnimTime = 0.0f;
    }

	// Handle adding egg to grid when hit
	public void AttachEggOnGrid(GameObject eggOnGrid, GameObject refBullet)
	{
		Egg newEgg = GetNewEggPosition(eggOnGrid, refBullet);
		//Debug.Log("OKKK --- "+bulletEgg.GetComponent<Bullet>().GetColor());
		newEgg.SetColor(refBullet.GetComponent<Bullet>().GetColor());
		newEgg.gameObject.SetActive(true);
		List<Egg> crackedEgg = SpreadToAllNearbyEgg(newEgg);
		int crackedCount = crackedEgg.Count;
		int fallenCount = 0;
		if(crackedCount >= 3)
		{
			PopEggs(crackedEgg);
			fallenCount = FallEggs();
			AddTotalScoreByCount(crackedCount, fallenCount);
		}
		
	}
	
	// Handle special case, egg has reached the top
	public void AttachEggOnGridTop(GameObject bulletEgg)
	{
		float minDist = 99999.0f;
		Egg newEgg = null;
		for(int i = 0; i < GetMaxCol(); i++)
		{
			Egg egg = GetEggInGrid(0, i);
			float currentDist = Vector3.Distance(egg.gameObject.transform.position, bulletEgg.transform.position);
			if(currentDist < minDist)
			{
				minDist = currentDist;
				newEgg = egg;
			}
		}
		newEgg.SetColor(bulletEgg.GetComponent<Bullet>().GetColor());
		newEgg.gameObject.SetActive(true);
		List<Egg> crackedEgg = SpreadToAllNearbyEgg(newEgg);
		int crackedCount = crackedEgg.Count;
		int fallenCount = 0;
		if(crackedCount >= 3)
		{
			PopEggs(crackedEgg);
			fallenCount = FallEggs();
			AddTotalScoreByCount(crackedCount, fallenCount);
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

    	// Handle popping eggs
	void PopEggs(List<Egg> eggs)
	{
		foreach(Egg egg in eggs)
		{
			egg.SetColor((int)Defines.COLOR.NONE);
			egg.gameObject.SetActive(false);
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
			SetFallenEggInPool(egg, iFallenEggPos);
			egg.SetColor((int)Defines.COLOR.NONE);
			egg.gameObject.SetActive(false);
			iFallenEggPos++;
		}
		return fallenCount;
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
