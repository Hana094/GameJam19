using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public MapGenerator map;
    public Canvas Resources;
    int[] scores = new int[4];
    int[] minimum = new int[4];
    public Image Fader;

    public Text[] textScores; //0 comida,1 logs,3buildmat, 4refugees

    bool GameDone = true;

    void Awake()
    {
        map = GetComponent<MapGenerator>();
        //Set Cursor to not be visible
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        //Check if instance already exists
        if (instance == null)
        {

            //if not, set instance to this
            instance = this;

            //If instance already exists and it's not this:
        }
        else if (instance != this)
        {

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            GameObject.Destroy(gameObject);
        }
        

        SceneManager.sceneLoaded += OnSceneLoaded;

        DontDestroyOnLoad(gameObject);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name + scene.buildIndex);
        if (scene.name.Equals("GameScene"))
        {
            StartGame();
        }
        
        else if (scene.name.Equals("menu"))
        {
           
        }
        
        //
        //print("Lol");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        for (int i = 0; i < scores.Length; i++)
        {
            scores[i] = 0;
        }
        map.BuildMap();
    }

    public void UpdateScores(int resourceId)
    {
        if (!GameDone)
        {
            scores[resourceId]++;
            textScores[resourceId].text= scores[resourceId]+"/"+minimum[resourceId];
        }
    }
}
