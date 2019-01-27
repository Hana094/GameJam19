using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AdvanceManager : MonoBehaviour
{
    public GameObject[] levels;

    public RefugeeColor[] Ref;

    Coroutine refugeesR;
    
    public float time2Spawn=1.5f;

    public Canvas Menu;

    public GameObject firstButton;

    public void Awake()
    {
        Menu.gameObject.SetActive(true);
        foreach (GameObject g in levels)
        {
            g.SetActive(false);
        }
    }

    public void evaluate(bool won)
    {
        if (GameManager.instance.refAtShelter.Count<levels.Length*2 && won)
        {
            GameManager.instance.UpdateLimit();
        }
        else if(!won && GameManager.instance.refAtShelter.Count >0)
        {
            GameManager.instance.refAtShelter.RemoveAt(GameManager.instance.refAtShelter.Count-1);
        }
        
        for (int i = 0; i < GameManager.instance.RefLimit / 2 ; i++)
        {
            levels[i].SetActive(true);
        }


        if (refugeesR!=null)
        {
            StopCoroutine(refugeesR);
        }
        refugeesR = StartCoroutine(RefCoroutine());
        
    }

    IEnumerator RefCoroutine()
    {
        for (int i = 0; i < GameManager.instance.refAtShelter.Count; i++)
        {
            Ref[i].RunAlpha(time2Spawn);
        }
        yield return new WaitForSeconds(time2Spawn);
        Menu.gameObject.SetActive(true);
    }

    public void LoadNextScene()
    {
        Menu.gameObject.SetActive(false);
        GameManager.instance.LoadMapLevel(SceneName.GameScene,true);
    }

    public void EndApp()
    {
        GameManager.instance.EndApp();
    }
}
