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

    public bool GameDone = true;

    public int[] baseResources = { 3, 3, 3 };

    List<RefugeeRef> refAtShelter = new List<RefugeeRef>();

    int refugeeLimit=5;

    public List<Player> players = new List<Player>();

    //Timer Stuff

    public float timeLeft;

    public Text timer;

    Coroutine gameTimer;

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
            players.Clear();
            foreach (Player p in FindObjectsOfType<Player>())
            {
                p.canMove = false;
                players.Add(p);
            }
            StartGame();
        }
        
        else if (scene.name.Equals("menu"))
        {
           
        }
        
        //
        //print("Lol");

    }

    IEnumerator MatchTimer()
    {
        Fader.CrossFadeAlpha(0, 0.1f, true);

        GameDone = false;

        foreach (Player p in players)
        {
            p.canMove = true;
        }

        float timeRemaining = timeLeft;
        while (timeRemaining > 0)
        {

            timeRemaining -= Time.deltaTime;
            string minSec = (int)timeRemaining + "";
            timer.text = minSec;
            
            yield return null;

        }
        GameDone = true;

        yield return new WaitForSeconds(0.75f);

        EndGame();

    }

    void EndGame()
    {


        if (gameTimer != null)
        {
            StopCoroutine(gameTimer);
        }
        //audioSrc.Stop();


        

        //LoadMapLevel(SceneName.scoreScreen, false);


    }

    public bool RefugeeAtShelter( int[] refugeeNeeds)
    {
        bool res = false;

        if (refAtShelter.Count<refugeeLimit && !GameDone)
        {
            print("lol");
            res = true;
            refAtShelter.Add(new RefugeeRef(refugeeNeeds));
        }

        return res;
    }

    public void StartGame()
    {
        for (int i = 0; i < scores.Length; i++)
        {
            scores[i] = 0;
        }
        // needs
        map.BuildMap();
        map.SetPlayersPos();

        gameTimer = StartCoroutine(MatchTimer());
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

[System.Serializable]
public struct RefugeeRef
{
    public int[] needs;

    public RefugeeRef(int [] _needs)
    {
        needs = new int[3];
        for (int i = 0; i < 3; i++)
        {
            needs[i] = _needs[i];
        }
    }
}
