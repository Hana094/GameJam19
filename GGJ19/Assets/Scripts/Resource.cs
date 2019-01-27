using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceCode{
    food, wood,brick,refugee
}
public class Resource : MonoBehaviour
{
    public AudioSource audio;
    public ParticleSystem particles;
    public GameObject body;
    bool canInteract = true;
    public ResourceCode resource;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player") && canInteract)
        {
            particles.Play();
            GameManager.instance.UpdateScores((int)resource);
            //gameObject.SetActive(false);
            body.SetActive(false);
            canInteract = false;
            audio.Play();
            Invoke("Deactivate", 2);
        }
    }

    void Deactivate()
    {
        gameObject.SetActive(false);
    }

}
