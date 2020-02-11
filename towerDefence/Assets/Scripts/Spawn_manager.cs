using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_manager : MonoBehaviour
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

    enum Status {ready,spawning, waiting};
    Status SpawnState = Status.waiting;
    private int basicEnemyToSpawn;//queue de spawn des enemies simples
    private int hardEnemyToSpawn;//queue de spawn des enemies durs
    private int bossLevel;//temoins du nombre de boss déjà passés
    private float waveCountdown;//compteur avant le debut d'une vague


    void Start()
    {
        //On attend le debut de la prochaine vague
        waveCountdown = timeBetweenWaves;
    }



    // Update is called once per frame
    void Update()
    {
        int enemyCount = transform.childCount;

        if (SpawnState == Status.waiting && enemyCount == 0 && basicEnemyToSpawn <= 0 && hardEnemyToSpawn <= 0) // changement de wave d'une vague à l'autre
        {
            setupNewWave();
        }
        else
        {
            //On verifie si l'on peut commencer à spawn
            if(waveCountdown <= 0)
            {
                //on verifie si l'on a quelque chose a spawn
                if(SpawnState == Status.ready)
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


        basicEnemyToSpawn = bossLevel + (int)Mathf.Floor((float) Mathf.Repeat(actualWave, bossWave) * basicEnemyRate);
        
        /*
         * On obtient un nombre qui s'incremente petit à petit d'une vague de boss a l'autre, le jeu de conversion set à 
         * obtenir l'arrondi vers le bas de la multiplication par le taux d'enemy
         */
        hardEnemyToSpawn = bossLevel + (int)Mathf.Floor((float)(actualWave * hardEnemyRate));


        //On remet le timer à 0
        waveCountdown = timeBetweenWaves;
        SpawnState = Status.ready;

        
    }

    IEnumerator spawnAgent()
    {
        SpawnState = Status.spawning;

        //while there is still enemies to spawn
        while (basicEnemyToSpawn > 0 || hardEnemyToSpawn > 0)
        {
            int aleatoire = Random.Range(0, 2);
            //1 out of 3
            if (aleatoire == 0)
            {
                //we try to spawn an hard enemy, if we dont, we spawn a basic one
                if (!spawnHardEnemy())
                    spawnBasicEnemy();
            }
            else//2 out of 3
            {
                //we try to spawn a basic enemy, if we dont, we spawn an hard one
                if (!spawnBasicEnemy())
                    spawnHardEnemy();
            }

            //waiting
            yield return new WaitForSeconds(1/spawnFrequency);

        }
        SpawnState = Status.waiting;

        yield break;
    }

    bool spawnHardEnemy()
    {
        if (hardEnemyToSpawn <= 0)
            return false;

        Instantiate(enemyHard, spawnpoint.transform.position, spawnpoint.transform.rotation, gameObject.transform);

        hardEnemyToSpawn--;
        //Debug.Log("spawn hard enemy");
        return true;
    }

    bool spawnBasicEnemy()
    {
        if (basicEnemyToSpawn <= 0)
            return false;

        Instantiate(enemyBasic, spawnpoint.transform.position, spawnpoint.transform.rotation, gameObject.transform);

        basicEnemyToSpawn--;
        //Debug.Log("spawn basic enemy");
        return true;
    }

    public int aliveEnemiesCount()
    {
        return transform.childCount;
    }

    public List <GameObject> GetAliveEnemies()
    {
        //thanks jessy https://answers.unity.com/questions/38760/how-to-get-an-array-of-all-children-of-any-type-bu.html
        List<GameObject> enemies = new List<GameObject>();
        foreach (Transform child in transform) enemies.Add(child.gameObject);
        return enemies;
    }
}
