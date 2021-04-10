using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour, IComparable
{
    [SerializeField] List<GameObject> mColors = new List<GameObject>();
    int mColor;
    int mCol;
    int mRow;
    float mOffset;
    public static float EGG_RATIO = 0.66f;
    [SerializeField] GameObject hazardFX;
    bool isHazard;

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

    public void SetPosition(int c, int r, float offset)
    {
        float x = c * EGG_RATIO + offset;
        float y = r * EGG_RATIO;
        mOffset = offset;
        mCol = c;
        mRow = r;
        transform.position = new Vector3(x, -y, 0);
    }
	
	public void UpdatePositionAnim(float rowOffsetAnim, float offset)
	{
		float x = mCol * EGG_RATIO + offset;
        float y = mRow * EGG_RATIO + rowOffsetAnim;
        mOffset = offset;
        transform.position = new Vector3(x, -y, 0);
	}

    public int GetCol()
    {
        return mCol;
    }
    
    public int GetRow()
    {
        return mRow;
    }

    public float GetOffset()
    {
        return mOffset;
    }

    public int GetColor()
    {
        return mColor;
    }
    public string GetStringFromColor()
    {
        if (!gameObject.activeSelf)
            return "None";
        string str = "";
        switch (mColor)
        {
            case 0:
                str = "Purple";
                break;
            case 1:
                str = "Green";
                break;
			case 2:
                str = "LightBlue";
                break;
            case 3:
                str = "Orange";
                break;
            case 4:
                str = "Red";
                break;
            case 5:
                str = "Yellow";
                break;
            default:
                break;

        }
        return str;
    }

    public GameObject GetActiveChildEgg()
    {
        if(mColor >= 0)
            return mColors[mColor];
        return null;
    }

    public bool IsHazard()
    {
        return isHazard;
    }

    public void SetHazard(bool newVal)
    {
        // other has not implemented
        if(newVal == false)
        {
            isHazard = newVal;
            hazardFX.SetActive(newVal);
        }
        else
        {
            switch(mColor)
            {
                case 2:
                case 4:
                isHazard = newVal;
                hazardFX.SetActive(newVal);
                break;
            }
        }
    }

    public int CompareTo(object obj) {
        if (obj == null) return 1;

        Egg otherEgg = obj as Egg;
        if (otherEgg != null)
		{
			if(mRow > otherEgg.mRow)
				return 1;
			else if(mRow < otherEgg.mRow)
				return -1;
			else
			{
				if(mCol > otherEgg.mCol)
					return 1;
				else if(mCol < otherEgg.mCol)
					return -1;
				else
					return 0;
			}
		}
        else
           throw new ArgumentException("Object is not a Egg");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "LimitBottom" && gameObject.name == "FallenEgg")
        {
            gameObject.SetActive(false);
        }
        else if(collision.tag == "Rope" && gameObject.name != "FallenEgg")
        {
            Debug.Log("GameOver");
            Playfield.Instance().SetState(Playfield.STATE.GAMEOVER);
        }
    }
}
