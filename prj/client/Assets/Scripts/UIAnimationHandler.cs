using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAnimationHandler : MonoBehaviour
{
    [SerializeField] GameObject textCombo;
    float textComboTimeTarget;
    float textComboTimer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(textComboTimeTarget > 0.0f)
        {
            textComboTimer += Time.deltaTime;
            if(textComboTimer >= textComboTimeTarget)
            {
                textCombo.SetActive(false);
                textComboTimeTarget = 0.0f;
            }
        }
    }

    public void ShowComboTextTimed(int currentCombo, float time = 2.0f)
    {
        textComboTimeTarget = time;
        textComboTimer = 0.0f;
        textCombo.GetComponent<Text>().text = "x"+currentCombo.ToString();
        textCombo.SetActive(true);
    }
}
