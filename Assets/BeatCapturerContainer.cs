using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BeatCapturerContainer : MonoBehaviour
{
    [SerializeField] public List<BeatParticleCapture> capturers = new List<BeatParticleCapture>();

    private void Start()
    {
        GetComponents(capturers);
    }

    public void Capture(BeatListener listener, AnimationClip clip)
    {
        foreach (var beatParticleCapture in capturers)
        {
            beatParticleCapture.Capture(listener, clip);
        }
    }
}

[CustomEditor(typeof(BeatCapturerContainer))]
public class BeatCapturerContainerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var beatCapturerContainer = (BeatCapturerContainer) target;
        
        if (GUILayout.Button("Find capturers"))
        {
            beatCapturerContainer.gameObject.GetComponents(beatCapturerContainer.capturers);
            var particleSystem = beatCapturerContainer.gameObject.GetComponent<ParticleSystem>();
            foreach (var capturer in beatCapturerContainer.capturers)
            {
                capturer.particle = particleSystem;
            }
        }
    }
}
