using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationDisplay : MonoBehaviour
{
    public static InformationDisplay instance;

    private Text textcmp;

    private void Awake() {
        if(instance != null){
            Debug.LogError("More than one InformationDisplay in scene");
            return;
        }
        instance = this;

        /*

        This string should be in the start function,
        but "UpdateContent()" is called by the 
        spawnmanager earlier than the start call. and
        UpdateContent() need textcmp to not be null

        */
        textcmp = gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void UpdateContent()
    {
        int enemycount = Spawn_manager.instance.aliveEnemiesCount();
        float money = GameManager.instance.money;
        int wavecount = Spawn_manager.instance.actualWave;
        int bosswaveCount = Spawn_manager.instance.actualWave % Spawn_manager.instance.bossWave;
        float wavecountdown = Spawn_manager.instance.waveCountdown;

        textcmp.text = "" +
         money + "$ in cash \n";
        textcmp.text += "wave " + wavecount + " (" + bosswaveCount + " until boss wave)\n";
        textcmp.text += enemycount + " enemie(s) remaining\n";
        textcmp.text += Mathf.Round(wavecountdown) + " second(s) before next wave";
    }

    public void startCountDown(Spawn_manager self){
        StartCoroutine("countdown",self);
    }


    IEnumerator countdown(Spawn_manager self){
        Debug.Log("CountDown started");
        while(self.waveCountdown >= -1){
            UpdateContent();
            yield return new WaitForSeconds(1f);
        }
    }

}
