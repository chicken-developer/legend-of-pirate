using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAnimationHandler : MonoBehaviour
{
    [SerializeField] GameObject textCombo;
    [SerializeField] GameObject glowEffect;
    [SerializeField] Color[] textColorArray;
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

    public void ShowComboTextTimed(int currentCombo, Color? textColor, Color? glowEffectColor, bool usePreset = true, float time = 2.0f)
    {
        textComboTimeTarget = time;
        textComboTimer = 0.0f;
        textCombo.GetComponent<Text>().text = "x"+currentCombo.ToString();
        if(textColor != null)
            textCombo.GetComponent<Text>().color = textColor.Value;
        else if(usePreset)
            textCombo.GetComponent<Text>().color = textColorArray[System.Math.Min(currentCombo, textColorArray.Length - 1)];
        else
            textCombo.GetComponent<Text>().color = new Color(0.67f, 1.0f, 0.92f, 1.0f);

        if(glowEffectColor != null)
            glowEffect.GetComponent<ParticleSystem>().startColor = glowEffectColor.Value;
        else if(usePreset)
            glowEffect.GetComponent<ParticleSystem>().startColor = textColorArray[System.Math.Min(currentCombo, textColorArray.Length - 1)];
        else
            glowEffect.GetComponent<ParticleSystem>().startColor = new Color(0.0f, 1.0f, 1.0f, 1.0f);
        textCombo.SetActive(true);
    }
}
