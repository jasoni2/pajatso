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

        GameplayManager instance = (GameplayManager)target;
        var labelOffset = new Vector3(0f, 0.5f, 0.0f);

        // Render lines and handles showing the launch juttu
        Handles.color = Color.red;
        Handles.DrawDottedLine(instance.ballStart.position, instance.launchAngleTarget.position, 2.0f);
        Handles.DrawWireArc(instance.ballStart.position, instance.ballStart.forward, -instance.ballStart.right, 360.0f, 0.1f);
        Handles.DrawWireArc(instance.launchAngleTarget.position, instance.launchAngleTarget.forward, -instance.launchAngleTarget.right, 360.0f, 0.1f);

        Handles.Label(instance.ballStart.position - labelOffset, "Ball spawn");
        Handles.Label(instance.launchAngleTarget.position - labelOffset, "Ball launch angle");

        instance.ballStart.position = Handles.PositionHandle(instance.ballStart.position, Quaternion.identity);
        instance.launchAngleTarget.position = Handles.PositionHandle(instance.launchAngleTarget.position, Quaternion.identity);

        // Render lines and handles for the reset plane
        var resetPlaneHorizontalSpan = 50.0f;
        var resetPlaneHorizontalDepth = 10.0f;

        Handles.color = Color.yellow;
        Handles.DrawLines(new Vector3[]
            {
                // TR -> TL
            new Vector3(-resetPlaneHorizontalSpan, instance.ballResetPlane.position.y, 0.0f),
            new Vector3(resetPlaneHorizontalSpan, instance.ballResetPlane.position.y, 0.0f),
                // TL -> BL
            new Vector3(resetPlaneHorizontalSpan, instance.ballResetPlane.position.y, 0.0f),
            new Vector3(resetPlaneHorizontalSpan, instance.ballResetPlane.position.y - resetPlaneHorizontalDepth, 0.0f),
                // BL -> TR
            new Vector3(resetPlaneHorizontalSpan, instance.ballResetPlane.position.y - resetPlaneHorizontalDepth, 0.0f),
            new Vector3(-resetPlaneHorizontalSpan, instance.ballResetPlane.position.y, 0.0f),
                // TR -> BR
            new Vector3(-resetPlaneHorizontalSpan, instance.ballResetPlane.position.y, 0.0f),
            new Vector3(-resetPlaneHorizontalSpan, instance.ballResetPlane.position.y - resetPlaneHorizontalDepth, 0.0f),
                // BR -> BL
            new Vector3(resetPlaneHorizontalSpan, instance.ballResetPlane.position.y - resetPlaneHorizontalDepth, 0.0f),
            new Vector3(-resetPlaneHorizontalSpan, instance.ballResetPlane.position.y - resetPlaneHorizontalDepth, 0.0f)
            });

        Handles.Label(instance.ballResetPlane.position + new Vector3(0.0f, 0.5f, 0.0f), "Ball reset plane");
        instance.ballResetPlane.position = Handles.PositionHandle(instance.ballResetPlane.position, Quaternion.identity);
    }
}
