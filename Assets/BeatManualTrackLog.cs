using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BeatManualTrackLog : MonoBehaviour, IPointerClickHandler
{
    private float time = 0;
    private float previousTime = 0;
    private float totalDelta = 0;
    
    private int count = 0;
    

    private void Update()
    {
        time += Time.deltaTime;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var delta = time - previousTime;
        totalDelta += delta;
        count++;
        Debug.Log("Time: " + delta + " Average: " + totalDelta / count);
        previousTime = time;
    }
}
