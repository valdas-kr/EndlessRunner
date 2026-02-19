using UnityEngine;

public class Spawner : MonoBehaviour {
    //Visų kliūčių masyvas
    [SerializeField] GameObject[] spawnObjectPrefabs;

    //Pradinis kliūčių atsiradimo laikas ir greitis
    [SerializeField] float startingObstacleSpawnTime;
    [SerializeField] float startingObstacleSpeed;

    //Kintamieji, pagal kuriuos bus apskaičiuojamas žaidimo sunkumas
    [Range(0, 1)] [SerializeField] float obstacleSpawnTimeFactor;
    [Range(0, 1)] [SerializeField] float obstacleSpeedFactor;

    //Kliūčių atsiradimo laikas ir greitis
    private float obstacleSpawnTime;
    private float obstacleSpeed;

    //Laikas iki kitos kliūties
    private float timeUntilObstacleSpawn;

    //Sukuriamas klasės objektas
    private GameManager gm;

    private void Start() {
        gm = GameManager.Instance;
        //Pradedant žaidimą, atstatomos pradinės reikšmės
        gm.onGameOver.AddListener(ResetFactors);
    }
    private void Update() {
        if (gm.isPlaying) {
            CalculateFactors();
            SpawnLoop();
        }
    }

    private void CalculateFactors() {
        //Apskaičiuojamas kliūčių atsiradimo greitis ir kliūčių greitis
        obstacleSpawnTime = startingObstacleSpawnTime / Mathf.Pow(gm.timeScore, obstacleSpawnTimeFactor);
        obstacleSpeed = startingObstacleSpeed * Mathf.Pow(gm.timeScore, obstacleSpeedFactor);
    }

    private void SpawnLoop() {
        //Kaupiamas laikis iki sekančios kliūties
        timeUntilObstacleSpawn += Time.deltaTime;
        if (timeUntilObstacleSpawn >= obstacleSpawnTime) {
            //Jei sukauptas laikas didesnis už kliūties atsiradimo laiką, generuojama nauja kliūtis ir atnaujinamas sukauptas laikas
            Spawn();
            timeUntilObstacleSpawn = 0f;
        }
    }

    private void ResetFactors() {
        //Atstatomi kintamieji į pradines reikšmes
        obstacleSpawnTime = startingObstacleSpawnTime;
        obstacleSpeed = startingObstacleSpeed;
    }

    private void Spawn() {
        if (spawnObjectPrefabs.Length > 0) {
            //Parenkama kliūtis
            GameObject obstacleToSpawn = spawnObjectPrefabs[Random.Range(0, spawnObjectPrefabs.Length)];

            //Kliūtis sugeneruojama
            GameObject spawnedObstacle = Instantiate(obstacleToSpawn, transform.position, Quaternion.identity);

            //Parenkamas greitis
            Rigidbody2D obstacleRB = spawnedObstacle.GetComponent<Rigidbody2D>();
            obstacleRB.linearVelocity = Vector2.left * obstacleSpeed;
        }
    }
}