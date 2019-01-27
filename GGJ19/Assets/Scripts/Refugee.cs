using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Refugee : MonoBehaviour
{
    public ResourceCode resource;

    public int[] needs;// 0 food, 1 logs, 3 buildMaterial

    public void Go2Shelter()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            GameManager.instance.UpdateScores((int)resource);
            gameObject.SetActive(false);
        }
    }
}
