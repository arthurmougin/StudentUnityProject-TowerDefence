using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawn_manager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject enemyHard; 
    public GameObject enemyBasic;
    public GameObject spawnpoint;

    public int actualWave = 0;//à masquer, compteur de wave

    public int bossWave = 5;//nombre de vague à a partir de laquelle intervient un boss
    public float basicEnemyRate = 2f; //emplificateur des énemies basiques d'une vague à l'autre
    public float hardEnemyRate = 0.2f;//emplificateur des énemies durs d'une vague à l'autre
    public float spawnFrequency = 0.1f;//frequence de spawn des enemies
    public float timeBetweenWaves = 5f;//Delai de debut de vague

    private bool spawnDone;//temoins de spawn
    private int basicEnemyToSpawn;//queue de spawn des enemies simples
    private int hardEnemyToSpawn;//queue de spawn des enemies durs
    private int bossLevel;//temoins du nombre de boss déjà passés
    private float waveCountdown;//compteur avant le debut d'une vague
    private float basicEnemySpawnCountDown = 0;//compteur avant le spawn d'un enemie simple
    private float hardEnemySpawnCountDown = 0;//compteur avant le spawn d'un enemie dur
    [System.NonSerialized]
    public List<GameObject> aliveEnemies;

    void Start()
    {
        //On attend le debut de la prochaine vague
        waveCountdown = timeBetweenWaves;
        aliveEnemies = new List<GameObject>();
    }



    // Update is called once per frame
    void Update()
    {

        if(aliveEnemies.Count <= 0 && basicEnemyToSpawn <= 0 && hardEnemyToSpawn <= 0) // changement de wave d'une vague à l'autre
        {
            setupNewWave();
        }
        else
        {
            //On verifie si l'on peut commencer à spawn
            if(waveCountdown <= 0)
            {
                //on verifie si l'on a quelque chose a spawn
                if(spawnDone == false)
                {
                    StartCoroutine(spawnAgent());
                }
            }
            else waveCountdown -= Time.deltaTime;
        }
    }

    private void setupNewWave()
    {

        actualWave++;
        Debug.Log("starting wave " + actualWave.ToString());

        //nombre de bosswave atteints
        bossLevel = actualWave / bossWave;

        /*
         * indice d'increment du taux de spawn des unités simples
         * 
         * Il augmente d'une vague à l'autre jusqu'à une vague de bosse ou il recommence à 0 + le numéro de bosse
         * 
         */
        basicEnemyToSpawn = bossLevel + (int)Mathf.Floor((float) Mathf.Repeat(actualWave, bossLevel) * basicEnemyRate);

        /*
         * On obtient un nombre qui s'incremente petit à petit d'une vague de boss a l'autre, le jeu de conversion set à 
         * obtenir l'arrondi vers le bas de la multiplication par le taux d'enemy
         */
        hardEnemyToSpawn = bossLevel + (int)Mathf.Floor((float)(actualWave * hardEnemyRate));


        //On remet le timer à 0
        waveCountdown = timeBetweenWaves;
        spawnDone = false;
    }

    IEnumerator spawnAgent()
    {
        spawnDone = true;

        //while there is still enemies to spawn
        while(basicEnemyToSpawn > 0 && hardEnemyToSpawn < 0)
        {
            int aleatoire = Random.Range(0, 2);
            //1 out of 3
            if(aleatoire == 0)
            {
                //we try to spawn an hard enemy, if we dont, we spawn a basic one
                if (!spawnHardEnemy())
                    spawnBasicEnemy();
            }
            else
            {
                //we try to spawn a basic enemy, if we dont, we spawn an hard one
                if (!spawnBasicEnemy())
                    spawnHardEnemy();
            }

            //waiting


        }


        yield break;
    }

    bool spawnHardEnemy()
    {
        if (hardEnemyToSpawn <= 0)
            return false;

        hardEnemyToSpawn--;
        Debug.Log("spawn hard enemy");
        return true;
    }

    bool spawnBasicEnemy()
    {
        if (basicEnemyToSpawn <= 0)
            return false;

        basicEnemyToSpawn--;
        Debug.Log("spawn basic enemy");
        return true;
    }
}
