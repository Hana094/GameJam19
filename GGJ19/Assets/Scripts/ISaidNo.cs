using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;

public class ISaidNo : MonoBehaviour
{

    private GameObject selectedObj;

    void Start()
    { selectedObj = EventSystem.current.currentSelectedGameObject; }

    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(selectedObj);
        }
        else if (EventSystem.current.currentSelectedGameObject != selectedObj)
        {
            selectedObj = EventSystem.current.currentSelectedGameObject;
        }
    }


    public void ChangeSelectedObject(GameObject g)
    {
        EventSystem.current.SetSelectedGameObject(g);
        selectedObj = g;
    }
}
