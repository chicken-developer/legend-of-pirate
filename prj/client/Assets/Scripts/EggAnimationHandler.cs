using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggAnimationHandler : MonoBehaviour
{
    [SerializeField] GameObject eggPrefab;
    [SerializeField] GameObject powerEggBlast;
    [SerializeField] GameObject flashEggBlastLeft;
    [SerializeField] GameObject flashEggBlastRight;
    private GameObject powerEggInstance;
    private GameObject flashEggBlastLeftInstance;
    private GameObject flashEggBlastRightInstance;
    private List<GameObject> gameObjectList;
    private List<float> gameObjectTimers;
    private List<float> gameObjectMaxTime;
    [SerializeField] private float MAX_TIME = 0.29f;
    // Start is called before the first frame update
    void Start()
    {
        gameObjectList = new List<GameObject>();
        gameObjectTimers = new List<float>();
        gameObjectMaxTime = new List<float>();
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < gameObjectList.Count; i++)
        {
            gameObjectTimers[i] += Time.deltaTime;
           
            Animator eggAnim = gameObjectList[i].GetComponent<Egg>().GetActiveChildEgg().GetComponent<Animator>();
            if(gameObjectTimers[i] >= MAX_TIME)
            {
                gameObjectTimers.RemoveAt(i);
                GameObject go = gameObjectList[i];
                gameObjectList.RemoveAt(i);
                Destroy(go);
                i--;
            }
        }
    }

    public void AddPowerBlast(Vector3 position)
    {
        powerEggInstance = Instantiate(powerEggBlast);
        powerEggInstance.transform.position = position;
        ParticleSystem ps = powerEggInstance.GetComponent<ParticleSystem>();
        float totalDuration = ps.duration + ps.startLifetime;
        Destroy(powerEggInstance, totalDuration + 1.0f);
    }

    public void AddFlashBlast(Vector3 position)
    {
        flashEggBlastLeftInstance = Instantiate(flashEggBlastLeft);
        flashEggBlastRightInstance = Instantiate(flashEggBlastRight);
        flashEggBlastLeftInstance.transform.position = flashEggBlastRightInstance.transform.position = position;
        ParticleSystem ps1 = flashEggBlastLeftInstance.GetComponent<ParticleSystem>();
        float totalDurationLeft = ps1.duration + ps1.startLifetime;
        Destroy(flashEggBlastLeftInstance, totalDurationLeft + 1.0f);
        ParticleSystem ps2 = flashEggBlastRightInstance.GetComponent<ParticleSystem>();
        float totalDurationRight = ps2.duration + ps2.startLifetime;
        Destroy(flashEggBlastRightInstance, totalDurationRight + 1.0f);
    }

    public void AddBrokenEgg(Egg refEgg)
    {
        GameObject newEgg = Instantiate(eggPrefab);
        newEgg.transform.parent = gameObject.transform;
        newEgg.transform.position = refEgg.gameObject.transform.position;
        newEgg.gameObject.SetActive(true);
        newEgg.GetComponent<Egg>().SetColor(refEgg.GetColor());
        newEgg.gameObject.name = "FallenEgg";
        newEgg.gameObject.tag = "Untagged";
        Animator eggAnim = newEgg.GetComponent<Egg>().GetActiveChildEgg().GetComponent<Animator>();
        eggAnim.SetTrigger("TriggerStart");
        eggAnim.SetInteger("Color", newEgg.GetComponent<Egg>().GetColor());
        gameObjectList.Add(newEgg);
        gameObjectTimers.Add(0);
    }

    public void AddBrokenEggFireHazard(Bullet refEgg, Vector3 customPosition)
    {
        GameObject newEgg = Instantiate(eggPrefab);
        newEgg.transform.parent = gameObject.transform;
        newEgg.transform.position = customPosition;
        newEgg.gameObject.SetActive(true);
        newEgg.GetComponent<Egg>().SetColor(refEgg.GetColor());
        newEgg.gameObject.name = "FallenEgg";
        newEgg.gameObject.tag = "Untagged";

        Animator eggAnim = newEgg.GetComponent<Egg>().GetActiveChildEgg().GetComponent<Animator>();
        if(eggAnim != null)
        {
            eggAnim.SetTrigger("TriggerStart");
            eggAnim.SetInteger("Color", newEgg.GetComponent<Egg>().GetColor());
            gameObjectList.Add(newEgg);
            gameObjectTimers.Add(0);
        }
        else
        {
            newEgg.SetActive(false);
        }
    }
}
