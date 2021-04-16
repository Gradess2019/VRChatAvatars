using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class BeatParticleCapture : MonoBehaviour
{
    public ParticleSystem particle;
    public string property = "InitialModule.startLifetime.scalar";

    public Vector2 offsetLeft = Vector2.zero;
    [HideInInspector] public Vector2 pivot = Vector2.zero;
    public Vector2 offsetRight = Vector2.zero;

    public AnimationUtility.TangentMode mode;

    public AnimationCurve curve = new AnimationCurve();

    public int amount = 1;
    public float timeBetween = 0.5f;

    private void Start()
    {
        if (particle)
        {
            return;
        }

        particle = GetComponent<ParticleSystem>();
    }

    public void Capture(BeatListener listener, AnimationClip clip)
    {
        particle.Play();

        if (Application.isPlaying)
        {
            curve.AddKey(listener.time + offsetLeft.x, offsetLeft.y);
            curve.AddKey(listener.time, pivot.y);
            curve.AddKey(listener.time + offsetRight.x, offsetRight.y);

            AnimationUtility.SetKeyLeftTangentMode(curve, curve.keys.Length - 3, mode);
            AnimationUtility.SetKeyLeftTangentMode(curve, curve.keys.Length - 2, mode);
            AnimationUtility.SetKeyLeftTangentMode(curve, curve.keys.Length - 1, mode);

            AnimationUtility.SetKeyRightTangentMode(curve, curve.keys.Length - 3, mode);
            AnimationUtility.SetKeyRightTangentMode(curve, curve.keys.Length - 2, mode);
            AnimationUtility.SetKeyRightTangentMode(curve, curve.keys.Length - 1, mode);

            var path = GetGameObjectPath(particle.gameObject);
            clip.SetCurve(path, typeof(ParticleSystem), property, curve);
        }
        else
        {
            curve = new AnimationCurve();
            
            for (int i = 0; i < amount; i++)
            {
                var baseOffset = timeBetween * i;
                curve.AddKey(baseOffset + offsetLeft.x, offsetLeft.y);
                curve.AddKey(baseOffset, pivot.y);
                curve.AddKey(baseOffset + offsetRight.x, offsetRight.y);

                AnimationUtility.SetKeyLeftTangentMode(curve, curve.keys.Length - 3, mode);
                AnimationUtility.SetKeyLeftTangentMode(curve, curve.keys.Length - 2, mode);
                AnimationUtility.SetKeyLeftTangentMode(curve, curve.keys.Length - 1, mode);

                AnimationUtility.SetKeyRightTangentMode(curve, curve.keys.Length - 3, mode);
                AnimationUtility.SetKeyRightTangentMode(curve, curve.keys.Length - 2, mode);
                AnimationUtility.SetKeyRightTangentMode(curve, curve.keys.Length - 1, mode);

                var path = GetGameObjectPath(particle.gameObject);
                clip.SetCurve(path, typeof(ParticleSystem), property, curve);
            }
        }
    }

    private static string GetGameObjectPath(GameObject obj)
    {
        if (!obj)
        {
            return null;
        }

        string path = "/" + obj.name;
        while (obj.transform.parent != null)
        {
            obj = obj.transform.parent.gameObject;
            path = "/" + obj.name + path;
        }

        var regex = new Regex("^/.+?/");
        path = regex.Replace(path, "");
        return path;
    }
}

[CustomEditor(typeof(BeatParticleCapture))]
public class BeatParticleCaptureEditor : Editor
{
    private bool _editPattern;

    public Vector2 offsetLeft = Vector2.zero;
    public Vector2 pivot = Vector2.zero;
    public Vector2 offsetRight = Vector2.zero;


    public AnimationCurve curve = new AnimationCurve(
        new Keyframe(0, 0, 0, 0),
        new Keyframe(0.15f, 1, 0, 0),
        new Keyframe(0.3f, 0, 0, 0)
    );

    public override void OnInspectorGUI()
    {
        // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
        base.OnInspectorGUI();
        EditorGUILayout.Separator();
        EditorGUILayout.LabelField("Editor");
        var particleCapture = (BeatParticleCapture) target;

        offsetLeft = particleCapture.offsetLeft;
        offsetLeft.x += particleCapture.pivot.x;
        offsetRight = particleCapture.offsetRight;
        offsetRight.x += particleCapture.pivot.x;

        pivot = particleCapture.pivot;


        offsetLeft = EditorGUILayout.Vector2Field("Offset left", offsetLeft);
        pivot = EditorGUILayout.Vector2Field("Pivot", pivot);
        offsetRight = EditorGUILayout.Vector2Field("Offset right", offsetRight);

        for (var i = 0; i < curve.keys.Length; i++)
        {
            curve.RemoveKey(i);
        }

        curve.AddKey(new Keyframe(offsetLeft.x, offsetLeft.y));
        curve.AddKey(new Keyframe(pivot.x, pivot.y));
        curve.AddKey(new Keyframe(offsetRight.x, offsetRight.y));

        for (var i = 0; i < curve.keys.Length; i++)
        {
            AnimationUtility.SetKeyLeftTangentMode(curve, i, particleCapture.mode);
            AnimationUtility.SetKeyRightTangentMode(curve, i, particleCapture.mode);
        }

        EditorGUILayout.CurveField(curve);

        particleCapture.pivot = pivot;

        particleCapture.offsetLeft = offsetLeft;
        particleCapture.offsetLeft.x = offsetLeft.x - pivot.x;

        particleCapture.offsetRight = offsetRight;
        particleCapture.offsetRight.x = offsetRight.x - pivot.x;

        if (GUILayout.Button("Find particle system"))
        {
            particleCapture.particle = particleCapture.gameObject.GetComponent<ParticleSystem>();
        }
    }
}