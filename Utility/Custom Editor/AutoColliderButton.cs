using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(MeshRenderer))]
[CanEditMultipleObjects]    // ¿©·¯ °´Ã¼
public class AutoColliderButton : Editor
{    
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.Space(50);
        
        if(GUILayout.Button("Add Box Collider to Selection"))
        {            
            foreach (GameObject obj in Selection.gameObjects)
            {
                MeshRenderer meshRenderer = obj.GetComponent<MeshRenderer>();                       
                Mesh mesh = meshRenderer.GetComponent<MeshFilter>().sharedMesh;

                float xMin = mesh.vertices[0].x;
                float xMax = mesh.vertices[0].x;
                float yMin = mesh.vertices[0].y;
                float yMax = mesh.vertices[0].y;
                float zMin = mesh.vertices[0].z;
                float zMax = mesh.vertices[0].z;

                for (int i = 1; i < mesh.vertices.Length; i++)
                {
                    Vector3 vertex = mesh.vertices[i];
                    xMin = Mathf.Min(xMin, vertex.x);
                    xMax = Mathf.Max(xMax, vertex.x);
                    yMin = Mathf.Min(yMin, vertex.y);
                    yMax = Mathf.Max(yMax, vertex.y);
                    zMin = Mathf.Min(zMin, vertex.z);
                    zMax = Mathf.Max(zMax, vertex.z);
                }


                float tmpX = Mathf.Abs(xMax - xMin) * meshRenderer.transform.localScale.x;
                float tmpY = Mathf.Abs(yMax - yMin) * meshRenderer.transform.localScale.y;
                float tmpZ = Mathf.Abs(zMax - zMin) * meshRenderer.transform.localScale.z;

                meshRenderer.gameObject.AddComponent<BoxCollider>();

                BoxCollider box = meshRenderer.gameObject.GetComponent<BoxCollider>();
                box.size = new Vector3(tmpX, tmpY, tmpZ);
            }
        }         
    }    
}
