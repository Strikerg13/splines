using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathCreator))]
public class PathEditor : Editor
{

    PathCreator creator;
    Path path;

    void OnSceneGUI()
    {
        Input();
        Draw();
    }

    void Input()
    {
        Event guiEvent = Event.current;

        Vector3 mousepos = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;

        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.shift)
        {
            Undo.RecordObject(creator, "Add Segment");
            path.AddSegment(mousepos);
        }
    }

    void Draw()
    {
        for (int i=0; i< path.NumSegments; i++)
        {
            Vector3[] points = path.GetPointsInSegment(i);

            // Draws the lines for the handles, connecting the control points.
            Handles.color = Color.black;
            Handles.DrawLine(points[1], points[0]);
            Handles.DrawLine(points[2], points[3]);

            // draws the green curve
            Handles.DrawBezier(points[0], points[3], points[1], points[2], Color.green, null, 2);

        }

        // draws the red handles for the control points
        Handles.color = Color.red;
        for (int i = 0; i <path.NumPoints; i++)
        {
            Vector3 newPos = Handles.FreeMoveHandle(path[i], Quaternion.identity, 0.1f, Vector3.zero, Handles.CylinderHandleCap);
            if (path[i] != newPos)
            {
                Undo.RecordObject(creator, "Move point");  // supposed to allow you to undo moving of the points (not working)
                path.MovePoint(i, newPos);
            }
        }
    }

    private void OnEnable()
    {
        creator = (PathCreator)target;
        if (creator.path == null)
        {
            creator.CreatePath();
        }
        path = creator.path;
    }
}
