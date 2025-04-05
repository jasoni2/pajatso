using UnityEngine;
using UnityEditor;

/// <summary>
/// Creates two position gizmos in the scene view when you select the GameplayManager
/// so you can adjust the ball launch location and launch angle easier.
/// </summary>
[CustomEditor(typeof(GameplayManager))]
public class LaunchGizmo : Editor
{
    private void OnSceneGUI()
    {
        Handles.color = Color.red;

        GameplayManager instance = (GameplayManager)target;

        Vector3 labelOffset = new (0f, 0.5f, 0.0f);

        Handles.DrawDottedLine(instance.ballStart.position, instance.launchAngleTarget.position, 2.0f);
        Handles.DrawWireArc(instance.ballStart.position, instance.ballStart.forward, -instance.ballStart.right, 360.0f, 0.1f);
        Handles.DrawWireArc(instance.launchAngleTarget.position, instance.launchAngleTarget.forward, -instance.launchAngleTarget.right, 360.0f, 0.1f);

        Handles.Label(instance.ballStart.position - labelOffset, "Ball spawn");
        Handles.Label(instance.launchAngleTarget.position - labelOffset, "Ball launch angle");

        instance.ballStart.position = Handles.PositionHandle(instance.ballStart.position, Quaternion.identity);
        instance.launchAngleTarget.position = Handles.PositionHandle(instance.launchAngleTarget.position, Quaternion.identity);
    }
}
