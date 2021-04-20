﻿using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(AnimationUtils))]
public class AnimationUtilsEditor : Editor
{
    public AnimationUtils animationUtils;

    private string _sourcePropertyName;
    private string _targetPropertyName;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        animationUtils = (AnimationUtils) target;

        if (!animationUtils.animator)
        {
            return;
        }

        var clip = animationUtils.animator.runtimeAnimatorController.animationClips.GetValue(0) as AnimationClip;
        if (!clip)
        {
            return;
        }

        InitProperties(clip);
        DuplicateFieldKeys(clip);
    }

    private void InitProperties(AnimationClip clip)
    {
        if (!GUILayout.Button("Get properties"))
        {
            return;
        }
        
        animationUtils.properties.Clear();
        
        var curveBindings = AnimationUtility.GetCurveBindings(clip);
        foreach (var binding in curveBindings)
        {
            animationUtils.properties.Add(binding.propertyName);
        }
    }

    private void DuplicateFieldKeys(AnimationClip clip)
    {

        _sourcePropertyName = EditorGUILayout.TextField("Source property", _sourcePropertyName);
        _targetPropertyName = EditorGUILayout.TextField("Target property", _targetPropertyName);

        if (!GUILayout.Button("Duplicate field"))
        {
            return;
        }

        var curve = GetAnimationCurveByPropertyName(clip, _sourcePropertyName, out var newBinding);
        if (!curve)
        {
            return;
        }
        
        newBinding.propertyName = _targetPropertyName;
        
        AnimationUtility.SetEditorCurve(clip, newBinding, curve);
    }

    private AnimationCurve GetAnimationCurveByPropertyName(AnimationClip clip, string propertyName, out EditorCurveBinding foundBinding)
    {
        var curveBindings = AnimationUtility.GetCurveBindings(clip);

        foreach (var binding in curveBindings)
        {
            if (binding.propertyName.Equals(propertyName))
            {
                foundBinding = binding;
                return AnimationUtility.GetEditorCurve(clip, binding);
            }
        }

        foundBinding = new EditorCurveBinding();
        return null;
    }
}
#endif