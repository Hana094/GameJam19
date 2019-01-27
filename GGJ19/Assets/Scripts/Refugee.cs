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
        if (other.transform.CompareTag("PlayerInteractor"))
        {
            //GameManager.instance.UpdateScores((int)resource);
            other.GetComponent<PArentSetrer>().parent.RefugeeRef = this;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("PlayerInteractor") && other.GetComponent<PArentSetrer>().parent.RefugeeRef == this)
        {
            //GameManager.instance.UpdateScores((int)resource);
            other.GetComponent<PArentSetrer>().parent.RefugeeRef = null;
        }
    }

}
