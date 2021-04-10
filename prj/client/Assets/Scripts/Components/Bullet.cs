using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	[SerializeField] List<GameObject> mColors = new List<GameObject>();
	[SerializeField] GameObject mPlayfield;
	private Playfield playfieldObj;
    [SerializeField] GameObject mShooter;
	private Shooter shooterObj;
    [SerializeField] GameObject mEggAnimation;
    int mColor;
	
    // Start is called before the first frame update
    void Start()
    {
        playfieldObj = mPlayfield.GetComponent<Playfield>();
        shooterObj = mShooter.GetComponent<Shooter>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public int GetColor()
	{
		return mColor;
	}
	
	public void SetColor(int color)
    {
        mColor = color;
        foreach(GameObject obj in mColors)
        {
            obj.SetActive(false);
        }
        
        mColors[color].SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Egg" && gameObject.activeSelf)
        {
            shooterObj.SetState(STATE.IDLE);
            shooterObj.randomEgg = true;
            playfieldObj.AttachEggOnGrid(collision.gameObject, this.gameObject);
            transform.position = new Vector3(2.5f, -7.48f, 0.0f); // reset position
            gameObject.SetActive(false);
        }
        else if(collision.tag == "HazardFire" && gameObject.activeSelf)
        {
            shooterObj.SetState(STATE.IDLE);
            shooterObj.randomEgg = true;
            mEggAnimation.GetComponent<EggAnimationHandler>().AddBrokenEggFireHazard(this, gameObject.transform.position);
            transform.position = new Vector3(2.5f, -7.48f, 0.0f); // reset position
            gameObject.SetActive(false);
        }
    }
}
