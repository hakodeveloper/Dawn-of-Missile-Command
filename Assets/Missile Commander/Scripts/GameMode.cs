using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(LevelSpawner))]

public class GameMode : MonoBehaviour {
    public GameObject waveAnnouncementUI;
    public GameObject gameOverUI;
    public GameObject[] cities = new GameObject[6];
    public GameObject[] launchers = new GameObject[3];
    public GameObject[] targets = new GameObject[9];
    public Sprite[] citiesDestroyedSprite = new Sprite[3];
    public Sprite launcherHealthy;
    public Sprite launcherHealthyRight;
    public Sprite launcherDestroyed;
    public int incomingMissiles = 10;
    public int score = 0;
    public int level = 1;
    public int difficultyMultiplier = 1;
    public bool[] citiesDestroyed = new bool[6];
    public bool[] launchersDestroyed = new bool[3];

    private GameObject scoreCounter;
    private GameObject incomingCounter;
    private GameObject levelCounter;
    private GameObject waveAnnouncement;
    private GameObject gameOverAnnouncement;

    public bool waveEnded = false;
    public bool isGameOver = false;
    private bool showedGameOver = false;
        
	void Start () {
        scoreCounter = GameObject.Find("Score");
        incomingCounter = GameObject.Find("Incoming");
        levelCounter = GameObject.Find("Level");
        for (int i = 0; i < 6; i++)
            targets[i] = cities[i];
        for (int i = 0; i < 3; i++)
            targets[i+6] = launchers[i];
        ShowAnnouncement();
        Invoke("NewWave", 2f);
    }
	
	void Update () {
        scoreCounter.GetComponent<Text>().text = "Score " + score;
        incomingCounter.GetComponent<Text>().text = "Incoming " + incomingMissiles;
        levelCounter.GetComponent<Text>().text = "Level " + level;
        incomingCounter.GetComponent<Text>().text = "Incoming " + GameObject.Find("Level Script").GetComponent<LevelSpawner>().missilesToLaunch;
        if (citiesDestroyed[0]) cities[0].GetComponent<SpriteRenderer>().sprite = citiesDestroyedSprite[0];
        if (citiesDestroyed[1]) cities[1].GetComponent<SpriteRenderer>().sprite = citiesDestroyedSprite[1];
        if (citiesDestroyed[2]) cities[2].GetComponent<SpriteRenderer>().sprite = citiesDestroyedSprite[2];
        if (citiesDestroyed[3]) cities[3].GetComponent<SpriteRenderer>().sprite = citiesDestroyedSprite[1];
        if (citiesDestroyed[4]) cities[4].GetComponent<SpriteRenderer>().sprite = citiesDestroyedSprite[2];
        if (citiesDestroyed[5]) cities[5].GetComponent<SpriteRenderer>().sprite = citiesDestroyedSprite[0];
        for (int i = 0; i<3; i++) {
            if (launchersDestroyed[i]) {
                launchers[i].GetComponent<SpriteRenderer>().sprite = launcherDestroyed;
                launchers[i].transform.Find("Launcher Cannon").gameObject.SetActive(false);
            } else {
                if (i == 2) {
                    launchers[i].GetComponent<SpriteRenderer>().sprite = launcherHealthyRight;
                } else {
                    launchers[i].GetComponent<SpriteRenderer>().sprite = launcherHealthy;
                }
                launchers[i].transform.Find("Launcher Cannon").gameObject.SetActive(true);
            }
        }
        if(waveEnded) {
            waveEnded = false;
            WaveEnded();
        }
        if (isGameOver) {
            for (int i = 0; i < 3; i++) {
                launchersDestroyed[i] = true;
            }
            if(!showedGameOver)
                ShowGameOver();
        } else {
            foreach (bool cityBool in citiesDestroyed) {
                isGameOver = cityBool;
                if (!cityBool)
                    break;
            }
        }
    }

    public void RestartGame() {
        SceneManager.LoadScene(1);
    }

    public void WaveEnded() {
        Invoke("ShowAnnouncement",0.5f);
        level++;
        foreach (bool ciudad in citiesDestroyed) {
            if (!ciudad) {
                score += Mathf.RoundToInt(200 * (difficultyMultiplier*0.5f));
            }
        }
        for (int i = 0; i < 3; i++) {
            launchersDestroyed[i] = false;
        }
        Invoke("NewWave", 3f);
    }

    public void NewWave() {        
        LevelSpawner spawn = GameObject.Find("Level Script").GetComponent<LevelSpawner>();
        spawn.missilesToLaunch = incomingMissiles;
        spawn.difficultyMultiplier = difficultyMultiplier;
        spawn.StartLevel();
        incomingMissiles += difficultyMultiplier;
    }

    void ShowGameOver() {
        gameOverAnnouncement = Instantiate(gameOverUI);
        gameOverAnnouncement.transform.SetParent(GameObject.Find("Canvas").transform);
        gameOverAnnouncement.transform.localPosition = new Vector3(0, 0, 0);
        gameOverAnnouncement.transform.localScale = new Vector3(1, 1, 1);
        gameOverAnnouncement.GetComponentInChildren<Button>().onClick.AddListener(() => { RestartGame(); });
        showedGameOver = true;
    }

    void ShowAnnouncement() {
        waveAnnouncement = Instantiate(waveAnnouncementUI);
        waveAnnouncement.transform.SetParent(GameObject.Find("Canvas").transform);
        waveAnnouncement.transform.localPosition = new Vector3(0, 0, 0);
        waveAnnouncement.transform.localScale = new Vector3(1,1,1);
    }
}