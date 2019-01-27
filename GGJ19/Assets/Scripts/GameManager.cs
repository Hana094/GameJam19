using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public enum SceneName
{
    splashScreen,MenuScene,GameScene
}
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

    public List<RefugeeRef> refAtShelter = new List<RefugeeRef>();

    int refugeeLimit=2;

    public int RefLimit
    {
        get { return refugeeLimit; }
    }

    public List<Player> players = new List<Player>();

    //Timer Stuff

    public float timeLeft;

    public Text timer;

    Coroutine gameTimer;

    bool gameWon=false;

    public AudioClip[] clips;

    public AudioSource audio;

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
        audio.Stop();
        Debug.Log("OnSceneLoaded: " + scene.name + scene.buildIndex);
        if (scene.name.Equals("GameScene"))
        {
            audio.clip = clips[1];
            Resources.gameObject.SetActive(true);
            players.Clear();
            foreach (Player p in FindObjectsOfType<Player>())
            {
                p.canMove = false;
                players.Add(p);
            }
            StartGame();
        }
        
        else if (scene.name.Equals("splashScreen"))
        {
            LoadMapLevel(SceneName.MenuScene,true);
        }

        else if (scene.name.Equals("MenuScene"))
        {
            //cambiar
            audio.clip = clips[0];
            if (gameTimer != null)
            {
                StopCoroutine(gameTimer);
            }
            gameTimer = StartCoroutine(ResultRoutine());
        }

        //
        //print("Lol");

    }

    IEnumerator MatchTimer()
    {
        Fader.CrossFadeAlpha(0, 0.1f, true);

        SetNeeds();

        audio.Play();
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

    IEnumerator ResultRoutine()
    {
        Fader.CrossFadeAlpha(0, 0.1f, true);

        AdvanceManager aux = FindObjectOfType<AdvanceManager>();

        aux.evaluate(gameWon);

        //audio.Play();

        yield return null;

    }

    void EndGame()
    {

        Resources.gameObject.SetActive(false);
        if (gameTimer != null)
        {
            StopCoroutine(gameTimer);
        }
        //audioSrc.Stop();

        gameWon = true;
        for (int i = 0; i < 3; i++)
        {
            if (gameWon && minimum[i]>scores[i])
            {
                gameWon = false;
            }
        }
        Fader.CrossFadeAlpha(1, 0, true);

        map.DeleteMap();

        LoadMapLevel(SceneName.MenuScene, false);


    }

    public void UpdateLimit()
    {
        refugeeLimit = Mathf.Clamp(refugeeLimit+2,2,6);
    }

    public void LoadMapLevel(SceneName nextScene, bool delayLoad = true)
    {
        Resources.gameObject.SetActive(false);
        if (gameTimer!=null)
        {
            StopCoroutine(gameTimer);
        }
        gameTimer = StartCoroutine(AsyncLoadMap(nextScene, delayLoad));

    }

    public void EndApp()
    {
        Application.Quit();
    }
    IEnumerator AsyncLoadMap(SceneName sceneIndex, bool time2Load)
    {
        

        yield return new WaitForSeconds(0.75f);

        float time2Wait = (time2Load) ? 1.5f : 0;

        Fader.CrossFadeAlpha(1, time2Wait, true);
        while (time2Wait > 0)
        {
            time2Wait -= Time.deltaTime;
            yield return null;
        }

        

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync((int)(sceneIndex), LoadSceneMode.Single);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

        //pr

        public bool RefugeeAtShelter( int[] refugeeNeeds)
    {
        bool res = false;

        if (refAtShelter.Count<refugeeLimit && !GameDone)
        {
            
            res = true;
            refAtShelter.Add(new RefugeeRef(refugeeNeeds));
            SetNeeds();
        }

        return res;
    }


    public void StartGame()
    {
        scores = new int[4];
        // needs
        map.BuildMap();
        map.SetPlayersPos();

        gameTimer = StartCoroutine(MatchTimer());
    }

    public void SetNeeds()
    {
        

        for (int i = 0; i < 3; i++)
        {
            minimum[i] = baseResources[i];
        }

        foreach (RefugeeRef r in refAtShelter )
        {
            for (int i = 0; i < 3; i++)
            {
                minimum[i] += r.needs[i];
            }
        }
        for (int i = 0; i < 3; i++)
        {
            textScores[i].text = scores[i] + "/" + minimum[i];
        }
        scores[3] = refAtShelter.Count;
        textScores[3].text = scores[3] + "/" + refugeeLimit;

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
