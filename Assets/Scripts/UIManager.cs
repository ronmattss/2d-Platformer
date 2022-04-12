using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject endLevelScreen;
    public TMP_Text scoreText;
    public GameObject mainMenu;
    public GameObject pauseMenu;
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + GameManager.instance.coins.ToString();
    }
}
