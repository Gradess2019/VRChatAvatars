using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Internal;
using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Timeline;

public class AnimationClipEditor : EditorWindow
{
    [MenuItem("Window/Animation Clip Editor")]
    public static void ShowWindow()
    {
        GetWindow<AnimationClipEditor>();
    }

    private void OnSelectionChange()
    {
        if(Selection.activeObject is AnimationClip)
        {
            Debug.Log(Selection.activeObject);
        }
    }

    private void OnGUI()
    {
        var timeline = TimelineEditor.inspectedDirector;
        var timelineAsset = TimelineEditor.inspectedAsset;
        var animationTrack = (AnimationTrack) timelineAsset.GetOutputTrack(0);
        var track = animationTrack.outputs.ToArray()[0];
        
        

        var test = false;
        timeline.GetReferenceValue("Position", out test);
        GUILayout.Button(test + "");

    }
}
