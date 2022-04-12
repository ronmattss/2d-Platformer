using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;
public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    // These are all references that this class uses
    public GameObject player;
    public bool canBeDamaged = true;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] LayerMask invulLayer;
    public static GameManager instance;
    public int coins = 0;
    public int life = 0;
    public TMP_Text lifeText;
    public TMP_Text coinText;

    public CinemachineVirtualCamera virtualCamera;
    public CinemachineConfiner confiner;
    void Start()
    {
        instance = this;                                                                // Single Reference for this class
        coinText = GameObject.Find("CoinText").GetComponent<TMP_Text>();                // UI component
        lifeText = GameObject.Find("LifeText").GetComponent<TMP_Text>();                // UI component

    }

    // Update is called once per frame
    void Update()
    {
        if (life == 0)
        {
            player.SetActive(false);                                            // if player life sets to zero disable the player and show the screen
            UIManager.instance.endLevelScreen.SetActive(true);
        }
    }
    private void LateUpdate()
    {
        coinText.text = "X " + coins.ToString();                                // To Show the Coins and life on the screen
        lifeText.text = "X " + life.ToString();
    }
    public void ChangeCameraBounds(GameObject collider)
    {
        confiner.m_BoundingShape2D = collider.GetComponent<Collider2D>();       // Change the camera bounds for each camera
    }
    public void PlayerDamage()
    {
        StartCoroutine(PlayerInvul());
        StopCoroutine(PlayerInvul());
    }

    public void SetLife(int _life)          // Sets the life available per level
    {
        life = _life;
    }
    public void SetCoins(int _coins)        //          // Sets the coins when being picked up
    {
        coins = _coins;
    }

    IEnumerator PlayerInvul()                   // this means that make the player invulnerable for 1.5f seconds then back to normal again
    {
        if (life > 0 && canBeDamaged) life--;
      
        canBeDamaged = false;
        yield return new WaitForSecondsRealtime(1.5f);              //stops the code execution for 1.5f secs
        canBeDamaged = true;

        yield return null;                                          // basically means that the function execution is finish 
    }
}
