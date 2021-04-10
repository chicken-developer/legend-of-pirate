using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum STATE
{
	IDLE = 0,
	SHOOTING,
	SWAP,
	RELOAD
};
	
public class Shooter : MonoBehaviour
{
	private STATE currentState;
	
	
	public bool IsState(STATE state)
	{
		return currentState == state;
	}
	
	public STATE GetState(STATE state)
	{
		return currentState;
	}
	
	public void SetState(STATE state)
	{
		currentState = state;
		OnStateChanged(state);
	}
	
	public void OnStateChanged(STATE newState)
	{
		switch(newState)
		{
			case STATE.IDLE:
			{
				
			}
			break;
			case STATE.SHOOTING:
			{
				
			} 
			break;
		}
	}
	
	public Camera mainCamera;
	public GameObject bulletEgg;
	public GameObject eggPrefab;
	public EggWarehouse eggWarehouseObj;
	[SerializeField] float MAX_DEGREE_ROTATION;
	[SerializeField] float BULLET_SPEED;
	[SerializeField] List<GameObject> mColors = new List<GameObject>();
	[SerializeField] GameObject mAimPattern;
    [SerializeField] GameObject mEggHolder;
	public int mColor { get; private set; }
	bool mIsMouseDown = false;
    bool mIsSwapping = false;
	
	//crossbow
	public Sprite mCrossbowReady, mCrossbowShoot;
	GameObject mCrossbowObject;
	//crossbow end

	//aim pattern
	public GameObject mAimObject, mAimPositionUp, mAimPositionDown;
	float AIM_MOVE_SPEED = 0.3f;
	//aim pattern end

	private Vector3 currentShootingDir;
	[HideInInspector] public bool randomEgg;
    // Start is called before the first frame update
    void Start()
    {
        SetState(STATE.IDLE);
		//randomEgg = true;//don't need this when active component
		eggWarehouseObj.targetShooter = this;
		//bulletEgg.SetActive(false);
		//BulletColliderBridge cb = bulletEgg.AddComponent<BulletColliderBridge>();
		//cb.Initialize(this);

		mCrossbowObject = mAimPattern.transform.GetChild(1).gameObject;
    }

	
    public void ResetShooter()
    {
		SetState(STATE.IDLE);
		mAimPattern.transform.eulerAngles = new Vector3(0, 0, 0);        
		eggWarehouseObj.ResetEgg();
		bulletEgg.SetActive(false);
		gameObject.SetActive(false);
    }
	
	public void SetColor(int color)
    {
        mColor = color;
        foreach(GameObject obj in mColors)
        {
            obj.SetActive(false);
        }
		if(color >= 0)
			mColors[color].SetActive(true);
		mCrossbowObject.GetComponent<SpriteRenderer>().sprite = mCrossbowReady;
    }

    public int GetColor()
    {
        return mColor;
    }

    // Update is called once per frame
    void Update()
    {
        if(Playfield.Instance().IsHazardState(Playfield.HAZARD_STATUS.FROZEN))
        {
            return;
        }
		float step =  AIM_MOVE_SPEED * Time.deltaTime;
        mAimObject.transform.position = Vector3.MoveTowards(mAimObject.transform.position, mAimPositionUp.transform.position, step);
		if (mAimObject.transform.position == mAimPositionUp.transform.position)
			mAimObject.transform.position = mAimPositionDown.transform.position;
		
		if(randomEgg)
		{
			if(Playfield.Instance().GetDragonState() != Playfield.PLAYER_DRAGON_STATE.SPECIAL_FLASH)
				Playfield.Instance().ReloadEgg();
			randomEgg = false;
		}
		if(IsState(STATE.SHOOTING))
		{
			UpdateBulletEggPosition();
			return;
		}

		if (Input.touches.Length > 0)
		{
			Touch touch = Input.touches[0];

			if (touch.phase == TouchPhase.Began)
			{
				HandleTouchDown(touch.position);
			}
			else if(touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended)
            {
				HandleTouchUp(touch.position);
            }
			else if(touch.phase == TouchPhase.Moved)
            {
				HandleTouchMove(touch.position);
            }
		}
        else
        {
			if(Input.GetMouseButtonDown(0))
            {
				mIsMouseDown = true;
				HandleTouchDown(Input.mousePosition);
			}
			else if(Input.GetMouseButtonUp(0))
            {
				mIsMouseDown = false;
				HandleTouchUp(Input.mousePosition);
            }
			else if(mIsMouseDown)
            {
				HandleTouchMove(Input.mousePosition);
            }
			else if(Input.GetMouseButtonUp(1))
			{
				HandleTouchUpRight(Input.mousePosition); //Special case for swap egg
			}
        }
    }
	
	void HandleTouchDown(Vector2 touch)
    {
		if(Playfield.Instance().GetDragonState() == Playfield.PLAYER_DRAGON_STATE.RELOAD_MOVE_TO_SHOOTER 
		|| Playfield.Instance().GetDragonState() == Playfield.PLAYER_DRAGON_STATE.RELOAD_MOVE_TO_WAREHOUSE)
			return;
        Vector3 position = mainCamera.ScreenToWorldPoint(touch);
        Vector2 test = new Vector2(position.x, position.y);
        if(mEggHolder.GetComponent<Collider2D>().OverlapPoint(test))
        {
            mIsSwapping = true;
			Playfield.Instance().SwapEgg();
        }
    }
	void HandleTouchUp(Vector2 touch)
    {
		if(Playfield.Instance().GetDragonState() == Playfield.PLAYER_DRAGON_STATE.RELOAD_MOVE_TO_SHOOTER 
		|| Playfield.Instance().GetDragonState() == Playfield.PLAYER_DRAGON_STATE.RELOAD_MOVE_TO_WAREHOUSE)
			return;
        if(!mIsSwapping)
        {
            Vector3 position = mainCamera.ScreenToWorldPoint(touch);
            position = new Vector3(position.x, position.y, 0.0f); // reset Z = 0
            if(position.y <= -6.5f)
                return;
            currentShootingDir = CalculateDirection(position).normalized;
            bulletEgg.SetActive(true);
            bulletEgg.GetComponent<Bullet>().SetColor(mColor);
            SetColor((int)Defines.COLOR.NONE);
            SetState(STATE.SHOOTING);
			mCrossbowObject.GetComponent<SpriteRenderer>().sprite = mCrossbowShoot;
            AudioManager.Instance().PlaySFXShot();
        }
        else
        {
            mIsSwapping = false;
        }
    }
	void HandleTouchUpRight(Vector2 touch)
	{
		if(Playfield.Instance().GetDragonState() == Playfield.PLAYER_DRAGON_STATE.RELOAD_MOVE_TO_SHOOTER 
		|| Playfield.Instance().GetDragonState() == Playfield.PLAYER_DRAGON_STATE.RELOAD_MOVE_TO_WAREHOUSE)
			return;
		Playfield.Instance().SwapEgg();
	}
	void HandleTouchMove(Vector2 touch)
    {
		Vector3 position = mainCamera.ScreenToWorldPoint(touch);

		position = new Vector3(position.x, position.y, 0.0f); // reset Z = 0
		currentShootingDir = CalculateDirection(position).normalized;
	}
	
	// Handle starting direction
	Vector3 CalculateDirection(Vector3 screenPosition)
	{
		Vector3 direction = screenPosition - transform.position;
		float AngleRotation = Vector3.Angle(direction, Vector3.up);
		if (direction.x > 0)
			AngleRotation *= -1;
		mAimPattern.transform.eulerAngles = new Vector3(0, 0, Mathf.Clamp(AngleRotation, -MAX_DEGREE_ROTATION / 2.0f, MAX_DEGREE_ROTATION / 2.0f));
		//Debug.Log(AngleRotation);
		if(Mathf.Abs(AngleRotation) <= MAX_DEGREE_ROTATION / 2.0f)
		{
			return direction;
		}
		else
		{
			if(AngleRotation < 0.0f) // extreme left
			{
				return Quaternion.AngleAxis(MAX_DEGREE_ROTATION / 2.0f, Vector3.back) * Vector3.up;
			}
			else
			{
				return Quaternion.AngleAxis(MAX_DEGREE_ROTATION / 2.0f, Vector3.forward) * Vector3.up;
			}
		}
	}
	
	// Handle moving egg
	void UpdateBulletEggPosition()
	{
		Vector3 newPosition = bulletEgg.transform.position + currentShootingDir * 9999.0f;
		float step = Time.deltaTime * BULLET_SPEED;
		bulletEgg.transform.position = Vector3.MoveTowards(bulletEgg.transform.position, newPosition, step);
		
		if(bulletEgg.transform.position.x <= 0.0f) // Hit left wall
		{
			bulletEgg.transform.position = new Vector3
			(0.0f, bulletEgg.transform.position.y, bulletEgg.transform.position.z); // weird hanging outside bound bug
			currentShootingDir = Vector3.Reflect(currentShootingDir, Vector3.right);
		}
		else if(bulletEgg.transform.position.x >= 5.0f) // Hit right wall
		{
			bulletEgg.transform.position = new Vector3
			(5.0f, bulletEgg.transform.position.y, bulletEgg.transform.position.z); // weird hanging outside bound bug
			currentShootingDir = Vector3.Reflect(currentShootingDir, Vector3.left);
		}

		// special case, egg has gone to the top
		if(bulletEgg.transform.position.y > 0f)
		{
			SetState(STATE.IDLE);
			randomEgg = true;
			bulletEgg.SetActive(false);
			Playfield.Instance().AttachEggOnGrid(null, bulletEgg);
			bulletEgg.transform.position = gameObject.transform.position; // reset position
		}
	}
}
