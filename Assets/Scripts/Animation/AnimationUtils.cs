using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AnimationUtils : MonoBehaviour
{
    public Animator animator;
    public Material material;
    
    [HideInInspector]
    public List<string> properties;
    private void Update()
    {
        if (!animator)
        {
            return;
        }

        animator = GetComponent<Animator>();
    }
}
