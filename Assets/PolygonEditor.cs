using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(Polygon))]
public class PolygonEditor : Editor
{
    #region Members
    Polygon[] polygons;
    Polygon selected;

    int selectedVertex = -1;

    bool hotControlFlag = false; // For keeping track of mouse events on Handles
    #endregion

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        selected = (Polygon)target;

        GUI.enabled = true;
        if (selectedVertex < 0)
            GUI.enabled = false;

        // Delete selected vertex button
        if (GUILayout.Button("Delete Vertex"))
        {
            DeleteSelectedVertex();
        }


        // Add new vertex
        GUILayout.BeginHorizontal();
        
        // Clockwise vertex
        if (GUILayout.Button("Add CW Vertex"))
        {
            AddClockwiseVertex();
        }
        if (GUILayout.Button("Add CCW Vertex"))
        {
            AddCounterClockwiseVertex();
        }

        GUILayout.EndHorizontal();    
    }

    void OnSceneGUI()
    {
        if (!selected)
            return;

        // Vertex handles
        VertexHandles();

        // Input events
        //Event e = Event.current;
        //switch(e.type)
        //{
        //    case EventType.mouseDown:
        //    {
        //        break;
        //    }

        //    case EventType.KeyDown:
        //    {
        //        break;
        //    }
        //}
    }

    void VertexHandles()
    {
        Handles.color = Color.white;
        for (int i = 0; i < selected.vertices.Count; i++)
        {
            GUI.SetNextControlName("VertexHandle" + i);
            Vector3 prev = selected.vertices[i];
            Undo.RecordObject(selected, "Move Vertex");
            selected.vertices[i] = Handles.FreeMoveHandle(selected.transform.position + selected.vertices[i], 
                                                          Quaternion.identity, 
                                                          Camera.main.orthographicSize * 0.03f, 
                                                          new Vector3(0.1f, 0.1f), 
                                                          Handles.CubeHandleCap) 
                                                          - selected.transform.position;          

            // Circumvent floating point precision loss
            if (Mathf.Approximately(prev.x, selected.vertices[i].x) && Mathf.Approximately(prev.y, selected.vertices[i].y))
            {
                selected.vertices[i] = prev;
            }

            GetSelectedVertex();
        }
    }

    void GetSelectedVertex()
    {
        string controlName = GUI.GetNameOfFocusedControl();
        if (controlName.StartsWith("VertexHandle"))
        {
            if (GUIUtility.hotControl != 0)
            {
                // Currently dragging/holding a vertex handle
                hotControlFlag = true;
                selectedVertex = Int32.Parse(controlName[controlName.Length - 1].ToString());
            }
            else if (hotControlFlag)
            {
                // Released mouse on vertex handle
                hotControlFlag = false;
                UpdateMesh();
            }
        }
        else
        {
            selectedVertex = -1;
        }
    }

    void UpdateMesh()
    {
        MeshFilter MF = selected.GetComponent<MeshFilter>();
        MeshRenderer MR = selected.GetComponent<MeshRenderer>();

        if (!MF)
        {
            Debug.LogError("No MeshFilter component found on Polygon");
            return;
        }

        if (!MR)
        {
            Debug.LogError("No MeshRenderer component found on Polygon");
            return;
        }

        Mesh mesh = new Mesh();
        mesh.vertices = selected.vertices.ToArray();
        //mesh.triangles

        MF.mesh = mesh;
    }

    void DeleteSelectedVertex()
    {
        Undo.RecordObject(selected, "Delete Vertex");
        selected.vertices.RemoveAt(selectedVertex);
    }

    void AddCounterClockwiseVertex()
    {
        Vector2 midpoint = (selected.vertices[mod(selectedVertex - 1, selected.vertices.Count)] + selected.vertices[selectedVertex]) * 0.5f;

        Undo.RecordObject(selected, "Add Vertex");
        selected.vertices.Insert(selectedVertex, midpoint);
    }

    void AddClockwiseVertex()
    {
        int index = (selectedVertex + 1) % selected.vertices.Count;
        Vector2 midpoint = (selected.vertices[index] + selected.vertices[selectedVertex]) * 0.5f;

        Undo.RecordObject(selected, "Add Vertex");
        selected.vertices.Insert(index, midpoint);

        selectedVertex = index;
    }

    int mod(int x, int m)
    {
        int r = x % m;
        return r < 0 ? r + m : r;
    }
}
