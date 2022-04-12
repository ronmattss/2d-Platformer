using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject[] playerStartLocations;
    public GameObject[] levels;
    public GameObject background;
    [SerializeField] Sprite[] backgrounds;
    public GameObject[] cameraBounds;
    LevelManager instance;
    int currentLevel = 0; // level 1
    public GameObject currentLoadedLevel;
    void Start()
    {
        instance = this;
        //  currentLoadedLevel = levels[0];

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame(GameObject UI)
    {

        currentLoadedLevel.SetActive(true);
        GameManager.instance.player.SetActive(true);
        UI.SetActive(false);
    }

    public void ResumeGame(GameObject UI)
    {
        Time.timeScale = 1;
        UI.SetActive(false);

    }

    public void PrepareLevel(GameObject UI)
    {
        Time.timeScale = 1;
        if (currentLevel == 2)
            currentLevel = 0;
        else
            currentLevel++;
        Destroy(FindActiveLevel());
        Instantiate(levels[currentLevel]);
        currentLoadedLevel = levels[currentLevel];
        background.GetComponent<SpriteRenderer>().sprite = backgrounds[currentLevel];
        GameManager.instance.ChangeCameraBounds(cameraBounds[currentLevel]);
        GameManager.instance.SetLife(3);
        GameManager.instance.SetCoins(0);
        GameManager.instance.player.transform.position = playerStartLocations[currentLevel].transform.position;
        GameManager.instance.player.SetActive(true);
        UI.gameObject.SetActive(false);
        //Instantiate Level
        // change Background
        // set Player health to max (3)
        //Teleport Player to the starting zone
        // Hide UI
        // Play 
    }
    public void RetryLevel(GameObject UI)
    {
        Time.timeScale = 1;
        Destroy(FindActiveLevel());
        Instantiate(levels[currentLevel]);
        currentLoadedLevel = levels[currentLevel];
        background.GetComponent<SpriteRenderer>().sprite = backgrounds[currentLevel];
        GameManager.instance.ChangeCameraBounds(cameraBounds[currentLevel]);
        GameManager.instance.player.GetComponent<Movement>().isFiring = false;
        GameManager.instance.player.GetComponent<Movement>().isHit = false;
        GameManager.instance.SetLife(3);
        GameManager.instance.SetCoins(0);
        GameManager.instance.player.transform.position = playerStartLocations[currentLevel].transform.position;
        GameManager.instance.player.SetActive(true);
        UI.gameObject.SetActive(false);
        //Instantiate Level
        // change Background
        // set Player health to max (3)
        //Teleport Player to the starting zone
        // Hide UI
        // Play 
    }

    public GameObject FindActiveLevel()
    {
        GameObject level = GameObject.FindGameObjectWithTag("Level");
        return level;
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void displayUI()
    {
        // Timescale set to zero
        // display UI
        // 
    }
}
