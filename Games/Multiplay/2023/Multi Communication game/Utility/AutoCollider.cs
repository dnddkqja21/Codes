using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 자동으로 정점 계산하여 콜라이더 씌우는 기능
/// </summary>

public class AutoCollider : MonoBehaviour
{
    MeshRenderer meshRenderer;
    Mesh mesh;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        mesh = meshRenderer.GetComponent<MeshFilter>().mesh;
    }

    void Start()
    {
        float xMin;
        float xMax;

        xMin = mesh.vertices[0].x;
        xMax = mesh.vertices[0].x;

        for (int i = 1; i < mesh.vertices.Length; i++)
        {
            if (mesh.vertices[i].x < xMin)
            {
                xMin = mesh.vertices[i].x;
            }
            if (mesh.vertices[i].x > xMax)
            {
                xMax = mesh.vertices[i].x;
            }
        }

        float yMin;
        float yMax;

        yMin = mesh.vertices[0].y;
        yMax = mesh.vertices[0].y;

        for (int i = 1; i < mesh.vertices.Length; i++)
        {
            if (mesh.vertices[i].y < yMin)
            {
                yMin = mesh.vertices[i].y;
            }
            if (mesh.vertices[i].y > yMax)
            {
                yMax = mesh.vertices[i].y;
            }
        }

        float zMin;
        float zMax;

        zMin = mesh.vertices[0].z;
        zMax = mesh.vertices[0].z;

        for (int i = 1; i < mesh.vertices.Length; i++)
        {
            if (mesh.vertices[i].z < zMin)
            {
                zMin = mesh.vertices[i].z;
            }
            if (mesh.vertices[i].z > zMax)
            {
                zMax = mesh.vertices[i].z;
            }
        }

        float tmpX = Mathf.Abs(xMax - xMin) * transform.localScale.x;
        float tmpY = Mathf.Abs(yMax - yMin) * transform.localScale.y;
        float tmpZ = Mathf.Abs(zMax - zMin) * transform.localScale.z;

        gameObject.AddComponent<BoxCollider>();

        BoxCollider box = GetComponent<BoxCollider>();  
        box.size = new Vector3(tmpX, tmpY, tmpZ);
    }

    
}
