using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Spawn_manager spawnManager;
    public GameObject tourelleHolder;
    public GameObject goal;
    public GameObject mainMenu;
    public GameObject gameMenu;
    public GameObject pauseMenu;
    public GameObject failMenu;

    void Start()
    {
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onClickPlayOnMainMenu()
    {
        Debug.Log("play");
        Time.timeScale = 1;
        mainMenu.SetActive(false);
        gameMenu.SetActive(true);
    }

    public void onClickPauseOnGameMenu()
    {
        Time.timeScale = 0;
        gameMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void onClickResumeOnPauseMenu()
    {
        Time.timeScale = 1;
        gameMenu.SetActive(true);
        pauseMenu.SetActive(false);
    }

    public void onClickExitOnPauseMenu()
    {
        reset();
        //Time.timeScale = 1;
        mainMenu.SetActive(true);
        pauseMenu.SetActive(false);
    }

    public void onShieldFallen() {
        Time.timeScale = 0;
        reset();
    }

    void reset()
    {
        spawnManager.reset();
        goal.GetComponent<Goal>().reset();
        foreach (Transform child in tourelleHolder.transform)
            Destroy(child.gameObject);
    }
}
