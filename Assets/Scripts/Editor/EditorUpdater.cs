using UnityEditor;
using UnityEngine;

[ExecuteAlways]
public class EditorUpdater : MonoBehaviour
{
    
    public void Update()
    {
        Debug.Log("kek");
        if (!Application.isPlaying)
        {
            UnityEditor.EditorApplication.QueuePlayerLoopUpdate();
            UnityEditor.SceneView.RepaintAll();
        }
    }

    void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            UnityEditor.EditorApplication.QueuePlayerLoopUpdate();
            UnityEditor.SceneView.RepaintAll();
        }
    }
}
