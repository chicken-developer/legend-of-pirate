using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraScale : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] GameObject[] gameCanvas;
    private const float referenceWidth = 750;
    private const float referenceHeight = 1334;
    private float referenceRatio;
    private float currentRatio;
    // Start is called before the first frame update
    void Start()
    {
        referenceRatio = referenceWidth / referenceHeight;
        float testRatio = (float)Screen.width / (float)Screen.height;
        if (testRatio != referenceRatio)
        {
            for (int i = 0; i < gameCanvas.Length; i++)
            {
                CanvasScaler scaler = gameCanvas[i].GetComponent<CanvasScaler>();
                if (testRatio > referenceRatio)
                {
                    cam.orthographicSize = 5.0f;
                    scaler.matchWidthOrHeight = 1.0f;
                    currentRatio = testRatio;
                }
                else
                {
                    cam.orthographicSize = referenceRatio / testRatio * 5.0f;
                    scaler.matchWidthOrHeight = 0.0f;
                    currentRatio = testRatio;
                }
                cam.rect = new Rect(0, 0, 1, 1);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        float testRatio = (float)Screen.width / (float)Screen.height;
        if (currentRatio != testRatio)
        {
            for (int i = 0; i < gameCanvas.Length; i++)
            {
                CanvasScaler scaler = gameCanvas[i].GetComponent<CanvasScaler>();
                if (testRatio > referenceRatio)
                {
                    cam.orthographicSize = 5.0f;
                    scaler.matchWidthOrHeight = 1.0f;
                    currentRatio = testRatio;
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
}