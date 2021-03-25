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
	public GameObject eggWarehouse;
	private EggWarehouse eggWarehouseObj;
	[SerializeField] float MAX_DEGREE_ROTATION;
	[SerializeField] float BULLET_SPEED;
	[SerializeField] List<GameObject> mColors = new List<GameObject>();
	[SerializeField] GameObject mAimPattern;
	int mColor;
	bool mIsMouseDown = false;
	
	private Vector3 currentShootingDir;
	public bool randomEgg;
    // Start is called before the first frame update
    void Start()
    {
        SetState(STATE.IDLE);
		randomEgg = true;
		eggWarehouseObj = eggWarehouse.GetComponent<EggWarehouse>();
		//bulletEgg.SetActive(false);
		//BulletColliderBridge cb = bulletEgg.AddComponent<BulletColliderBridge>();
		//cb.Initialize(this);
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
    }

    // Update is called once per frame
    void Update()
    {
		if(randomEgg)
		{
			int color = Random.Range(0, (int)Defines.COLOR.YELLOW+1);
			mColor = color;
			SetColor(color);
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

    }
	void HandleTouchUp(Vector2 touch)
    {
		Vector3 position = mainCamera.ScreenToWorldPoint(touch);

        position = new Vector3(position.x, position.y, 0.0f); // reset Z = 0
        currentShootingDir = CalculateDirection(position).normalized;
		bulletEgg.SetActive(true);
		bulletEgg.GetComponent<Bullet>().SetColor(mColor);
		SetColor((int)Defines.COLOR.NONE);
        SetState(STATE.SHOOTING);
		AudioManager.Instance().PlayShotSFX();
    }
	void HandleTouchUpRight(Vector2 touch)
	{
		eggWarehouseObj.Request("swap");
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
		Vector3 up = Vector3.up;
		float AngleRotation = Vector3.Angle(direction, Vector3.up);
		if (direction.x > 0)
			AngleRotation *= -1;
		mAimPattern.transform.eulerAngles = new Vector3(0, 0, AngleRotation);
		//Debug.Log(AngleRotation);
		if(Mathf.Abs(AngleRotation) <= MAX_DEGREE_ROTATION / 2.0f)
		{
			return direction;
		}
		else
		{
			if(AngleRotation < 0.0f) // extreme left
			{
				return Quaternion.AngleAxis(MAX_DEGREE_ROTATION / 2.0f, Vector3.forward) * Vector3.up;
			}
			else
			{
				return Quaternion.AngleAxis(MAX_DEGREE_ROTATION / 2.0f, Vector3.back) * Vector3.up;
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
			Playfield.Instance().AttachEggOnGridTop(bulletEgg);
			bulletEgg.transform.position = new Vector3(2.5f, -7.48f, 0.0f); // reset position
		}
	}
}
