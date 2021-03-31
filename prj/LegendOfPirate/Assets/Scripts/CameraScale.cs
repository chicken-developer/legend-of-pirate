using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraScale : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] GameObject gameCanvas;
    private const float referenceWidth = 750;
    private const float referenceHeight = 1334;
    private float referenceRatio;
    private float currentRatio;
    private CanvasScaler scaler;
    // Start is called before the first frame update
    void Start()
    {
        scaler = gameCanvas.GetComponent<CanvasScaler>();
        referenceRatio = referenceWidth / referenceHeight;
        float testRatio = (float)Screen.width / (float)Screen.height;
        if(testRatio != referenceRatio)
        {
            if(testRatio > referenceRatio)
            {
                cam.orthographicSize = 5.0f;
                scaler.matchWidthOrHeight = 1.0f;
            }
            else
            {
                cam.orthographicSize = referenceRatio / testRatio * 5.0f;
                currentRatio = testRatio;
                scaler.matchWidthOrHeight = 0.0f;
            }
            cam.rect = new Rect(0, 0, 1, 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float testRatio = (float)Screen.width / (float)Screen.height;
        if(currentRatio != testRatio)
        {
            if(testRatio > referenceRatio)
            {
                cam.orthographicSize = 5.0f;
                scaler.matchWidthOrHeight = 1.0f;
            }
            else
            {
                cam.orthographicSize = referenceRatio / testRatio * 5.0f;
                currentRatio = testRatio;
                scaler.matchWidthOrHeight = 0.0f;
            }
            cam.rect = new Rect(0, 0, 1, 1);
        }
        
    }
}
