using System.Collections;
using System;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [Serializable]
    public class Round
    {
        public Transform enemy;
        public int count;
        public float spawnRate = 1;
    }

    private readonly ArrayList rounds = new ArrayList();
    [HideInInspector] public static int nextRound = 0;
    public Transform enemyTransform;
    public Transform[] spawnPoints;

    public float timeBetweenRounds = 5;
    private float searchCountDown = 1;
    private float roundCountDown;
    private float enemyHealth = 50;

    private SpawnState state = SpawnState.COUNTING;

    public static Action roundSwitch;

    private void Start()
    {
        if (spawnPoints.Length == 0)
            Debug.LogError("No spawn points referenced");

        roundCountDown = timeBetweenRounds;
        rounds.Add(CreateRound());
        roundSwitch?.Invoke();
    }

    private void Update()
    {
        if (state == SpawnState.WAITING)
        {
            if (!EnemyAliveCheck())
                NewRound();
            else 
                return;
        }

        if (roundCountDown <= 0)
        {
            if (state != SpawnState.SPAWNING)
                StartCoroutine(SpawnRound((Round)rounds[nextRound]));
        }
        else
            roundCountDown -= Time.deltaTime;
    }

    private void NewRound()
    {
        state = SpawnState.COUNTING;
        roundCountDown = timeBetweenRounds;

        nextRound++;
        rounds.Add(CreateRound());
        roundSwitch?.Invoke();
    }

    private Round CreateRound()
    {
        Round _round = new Round
        {
            count = nextRound switch
            {
                0 => 6,
                1 => 8,
                2 => 13,
                3 => 18,
                4 => 24,
                5 => 27,
                6 => 28,
                7 => 28,
                8 => 29,
                9 => 33,
                10 => 34,
                _ => Mathf.RoundToInt(0.0842f * Mathf.Pow(nextRound, 2) + 0.1954f * nextRound + 22.05f),
            },
            enemy = enemyTransform
        };

        if (nextRound >= 9)
            enemyHealth *= 1.1f;
        else
            enemyHealth += 100;

        return _round;
    }

    private bool EnemyAliveCheck()
    {
        searchCountDown -= Time.deltaTime;

        if (searchCountDown <= 0)
        {
            searchCountDown = 1;

            if (GameObject.FindGameObjectWithTag("Enemy") == null)
                return false;
        }

        return true;
    }

    private IEnumerator SpawnRound(Round _round)
    {
        state = SpawnState.SPAWNING;

        for (int i = 0; i < _round.count; i++)
        {
            SpawnEnemy(_round.enemy);

            yield return new WaitForSeconds(1 / _round.spawnRate);
        }

        state = SpawnState.WAITING;

        yield break;
    }

    public void SpawnEnemy(Transform _enemy)
    {
        Transform _sp = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length - 1)];
        EnemyHealth _eh = _enemy.GetComponent<EnemyHealth>();
        _eh.SetMaxHealth(enemyHealth);

        Instantiate(_enemy, _sp);
    }

    public enum SpawnState
    {
        SPAWNING,
        WAITING,
        COUNTING
    };
}
