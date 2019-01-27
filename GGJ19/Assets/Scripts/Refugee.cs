using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Refugee : MonoBehaviour
{
    public ResourceCode resource;

    public GameObject body;

    public SpriteRenderer sprite;

    public ParticleSystem particles;

    public AudioSource audio;

    public int[] needs;// 0 food, 1 logs, 3 buildMaterial
    bool canInteract = true;

    private void Awake()
    {
        sprite.gameObject.SetActive(false);
    }

    public void Go2Shelter()
    {
        if (GameManager.instance.RefugeeAtShelter(needs))
        {
            particles.Play();
            body.SetActive(false);
            sprite.gameObject.SetActive(false);
            canInteract = false;
            audio.Play();
            Invoke("Deactivate",5);
        }
    }
    

    void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("PlayerInteractor") && canInteract)
        {
            //GameManager.instance.UpdateScores((int)resource);
            other.GetComponent<PArentSetrer>().parent.RefugeeRef = this;
            sprite.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("PlayerInteractor") && other.GetComponent<PArentSetrer>().parent.RefugeeRef == this)
        {
            //GameManager.instance.UpdateScores((int)resource);
            other.GetComponent<PArentSetrer>().parent.RefugeeRef = null;
            sprite.gameObject.SetActive(false);
        }
    }

}
