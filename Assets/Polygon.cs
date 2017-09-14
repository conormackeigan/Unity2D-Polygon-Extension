using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Polygon : MonoBehaviour
{
    public List<Vector3> vertices; 

    void Reset()
    {
        vertices = new List<Vector3>();

        // Default square shape
        AddVertex(-1f, -1f);
        AddVertex(1f, -1f);
        AddVertex(1f, 1f);
        AddVertex(-1f, 1f); 
    }

    void OnDrawGizmos()
    {
        if (!PolygonWindow.showOutlines)
            return;

        // Draw outline
        for (int i = 0; i < vertices.Count - 1; i++)
            Gizmos.DrawLine(transform.position + vertices[i], transform.position + vertices[i + 1]);
        Gizmos.DrawLine(transform.position + vertices[vertices.Count - 1], transform.position + vertices[0]);
    }

    void DrawOutline()
    {
        // Convert vertices to world coordinates
        Vector3[] verts = vertices.ToArray();
        for (int i = 0; i < verts.Length; i++)
        {
            vertices[i] += transform.position;
        }

        // Draw outline
        Handles.DrawPolyLine(verts);
        Handles.DrawLine(verts[verts.Length - 1], verts[0]);
    }

    public void AddVertex(Vector3 vertex)
    {
        vertices.Add(vertex);
    }

    public void AddVertex(float x, float y)
    {
        vertices.Add(new Vector3(x, y));
    }

    public void SetPolygonCollider()
    {
        RemoveEdgeColliders();
    }

    public void SetEdgeColliders()
    {
        RemovePolygonColliders();

        // Place edge colliders

    }

    public void SetNoCollider()
    {
        // Remove any colliders
        RemovePolygonColliders();
        RemoveEdgeColliders();
    }

    void RemovePolygonColliders()
    {
        PolygonCollider2D[] cPolys = GetComponents<PolygonCollider2D>();
        for (int i = 0; i < cPolys.Length; i++)
        {
            DestroyImmediate(cPolys[i]);
        }
    }

    void RemoveEdgeColliders()
    {
        EdgeCollider2D[] cEdges = GetComponents<EdgeCollider2D>();
        for (int i = 0; i < cEdges.Length; i++)
        {
            DestroyImmediate(cEdges[i]);
        }
    }
}
