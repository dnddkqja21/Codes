using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingEf : MonoBehaviour
{
    public GameObject ef1;

    public GameObject ef2;

    public GameObject ef3;

    public static CraftingEf instance = null;


    ActionController player;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    void Start()
    {
        player = FindObjectOfType<ActionController>();
    }    
    

    public void EffectOn()
    {
        StartCoroutine(Effect());
    }

    IEnumerator Effect()
    {
        ef1.SetActive(true);

        yield return new WaitForSeconds(0.2f);

        ef1.SetActive(false);
        ef2.SetActive(true);

        yield return new WaitForSeconds(0.2f);

        ef2.SetActive(false);
        ef3.SetActive(true);
        player.PlayClips(24);

        yield return new WaitForSeconds(0.2f);

        ef3.SetActive(false);
        ef2.SetActive(true);

        yield return new WaitForSeconds(0.2f);

        ef1.SetActive(true);
        ef2.SetActive(false);

        yield return new WaitForSeconds(0.2f);

        ef1.SetActive(false);
        ef2.SetActive(true);

        yield return new WaitForSeconds(0.2f);

        ef2.SetActive(false);
        ef3.SetActive(true);
        player.PlayClips(24);

        yield return new WaitForSeconds(0.2f);

        ef3.SetActive(false);
        ef2.SetActive(true);

        yield return new WaitForSeconds(0.2f);

        ef2.SetActive(false);        
    }
}
