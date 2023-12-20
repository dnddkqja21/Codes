using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AutoPlay : MonoBehaviour
{
    public Ledge ledge;
    float waitTime = 0.3f;
    float curTime;
    void Update()
    {
        curTime += Time.deltaTime;
        if (!GameManager.isGameOver)
        {
            Auto();
            //artCoroutine(AutoCo());
        }
    }

    
    private void Auto()
    {
        if(curTime > waitTime)
        {
            switch (ledge.blocks[ledge.nowBlock].type)
            {
                case 0:
                    ledge.Select(0);
                    break;
                case 1:
                    ledge.Select(1);
                    break;
                case 2:
                    ledge.Select(2);
                    break;
                case 3:
                    ledge.Select(3);
                    break;
            }
            curTime = 0;
        }

    }
    

    IEnumerator AutoCo()
    {        
        switch (ledge.blocks[ledge.nowBlock].type)
        {
            case 0:
                ledge.Select(0);                
                break;
            case 1:
                ledge.Select(1);                
                break;
            case 2:
                ledge.Select(2);
                break;
            case 3:
                ledge.Select(3);                
                break;
        }
        yield return new WaitForSeconds(2f);
    }
}
