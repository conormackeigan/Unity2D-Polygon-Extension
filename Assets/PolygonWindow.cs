using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PolygonWindow : EditorWindow
{
    bool colliderFoldout = true;
    public static bool showOutlines = true;

    [MenuItem("Window/Polygon Editor Control Panel")]
    public static void ShowWindow()
    {
        GetWindow<PolygonWindow>();
    }

    [MenuItem("GameObject/2D Object/Polygon")]
    public static void CreatePolygon()
    {
        new GameObject("Polygon", typeof(Polygon), typeof(MeshFilter), typeof(MeshRenderer));
    }

    void OnGUI()
    {
        // Collider type options
        colliderFoldout = EditorGUILayout.Foldout(colliderFoldout, "Collider Type", true);
        if (colliderFoldout)
        {
            if (GUILayout.Button("Polygon"))
            {
                Polygon[] polygons = FindObjectsOfType<Polygon>();
                foreach (Polygon polygon in polygons)
                {
                    polygon.SetPolygonCollider();
                }
            }
            if (GUILayout.Button("Edges"))
            {
                Polygon[] polygons = FindObjectsOfType<Polygon>();
                foreach (Polygon polygon in polygons)
                {
                    polygon.SetEdgeColliders();
                }
            }
            if (GUILayout.Button("None"))
            {
                Polygon[] polygons = FindObjectsOfType<Polygon>();
                foreach (Polygon polygon in polygons)
                {
                    polygon.SetNoCollider();
                }
            }
        }

        // Show/hide polygon outlines
        showOutlines = GUILayout.Toggle(showOutlines, "Display Outlines");

        // Export??
    }

}
