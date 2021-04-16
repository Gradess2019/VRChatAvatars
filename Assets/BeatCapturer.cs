using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class OnBeatCaptured : UnityEvent<BeatListener, AnimationClip>
{
}

public class BeatCapturer : MonoBehaviour
{
    public BeatListener listener;
    public AnimationClip clip;

    public OnBeatCaptured functions;

    public void CaptureBeat()
    {
        if (functions == null || clip == null)
        {
            return;
        }

        functions.Invoke(listener, clip);
    }
}

[CustomEditor(typeof(BeatCapturer))]
public class BeatCapturerEditor : Editor
{
    public int amount = 0;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var beatCapturer = (BeatCapturer) target;

        if (GUILayout.Button("Capture"))
        {
            beatCapturer.CaptureBeat();
        }
    }
}