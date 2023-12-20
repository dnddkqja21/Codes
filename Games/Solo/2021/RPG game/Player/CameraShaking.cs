using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaking : MonoBehaviour
{
    public float shakingStr;

    public Vector3 offSet = Vector3.zero;

    public Quaternion originRotate;

    void Start()
    {
        //originRotate = transform.rotation;
    }

    
    void Update()
    {
        
    }

    public void ShakeCam()
    {
        StartCoroutine(Shake());
    }

    public void ResetCam()
    {
        StopAllCoroutines();
        StartCoroutine(Reset());
    }

    IEnumerator Shake()
    {
        Vector3 originAngle = transform.eulerAngles;

        while(true)
        {
            float rotX = Random.Range(-offSet.x, offSet.x);
            float rotY = Random.Range(-offSet.y, offSet.y);
            float rotZ = Random.Range(-offSet.z, offSet.z);

            Vector3 randomRot = originAngle + new Vector3(rotX, rotY, rotZ);

            Quaternion rot = Quaternion.Euler(randomRot);

            while(Quaternion.Angle(transform.rotation, rot) > 0.1f)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, shakingStr * Time.deltaTime);
                yield return null;
            }
            yield return null;
        }
    }

    IEnumerator Reset()
    {
        while(Quaternion.Angle(transform.rotation, originRotate) > 0f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, originRotate, shakingStr * Time.deltaTime);
            yield return null;
        }
    }
}
