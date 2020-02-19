using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject tourelleHolder;
    public UIHandler uIHandler;
    public GameObject goal;
    public GameObject mainMenu;
    public GameObject gameMenu;
    public GameObject pauseMenu;
    public GameObject failMenu;
    public GameObject[] tower_prefabs;
    public  GameObject[] island_prefabs;
    public float money{
        get{
            return _money;
        }
        set{
            _money = value;
            //Debug.Log(_money_old + " -> " + _money);
            updatePrice();
            _money_old = _money;
        }
    }
    private float _money = 100f;
    private float _money_old = 100f;


    public float sellingFactor = 0.5f;
    public float islandPrice = 5f;
    
    private void Awake() {
        if(instance != null){
            Debug.LogError("More than one GameManager in scene");
            return;
        }
        instance = this;
    }
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
        Spawn_manager.instance.reset();
        goal.GetComponent<Goal>().reset();
        foreach (Transform child in tourelleHolder.transform)
            Destroy(child.gameObject);
    }

    void updatePrice(){
        uIHandler.updateUI();
        tourelleHolder.GetComponent<IslandsManager>().updateUI();
    }
}
