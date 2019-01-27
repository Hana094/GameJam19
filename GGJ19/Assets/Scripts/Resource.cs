using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceCode{
    food, wood,brick,refugee
}
public class Resource : MonoBehaviour
{
    public ResourceCode resource;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            GameManager.instance.UpdateScores((int)resource);
            gameObject.SetActive(false);
        }
    }
}
