using System;
using System.Collections;
using UnityEngine;

namespace Enemy
{
    public class EnemySpawn : MonoBehaviour
    {
        [Serializable]
        public class Round
        {
            public Transform enemy;
            public int roundCount;
            public float spawnRate = 1;
        }

        private readonly ArrayList _rounds = new();
        public static int NextRound;
        [SerializeField] private Transform enemyTransform;
        [SerializeField] private Transform[] spawnPoints;

        [SerializeField] private float timeBetweenRounds = 5;
        private float _searchCountDown = 1;
        private float _roundCountDown;
        private float _enemyHealth = EnemyHealth.StartingHealth;

        private SpawnState _state = SpawnState.Counting;

        public static Action RoundSwitch;

        private void Start()
        {
            if (spawnPoints.Length == 0)
                Debug.LogError("No spawn points referenced");

            _roundCountDown = timeBetweenRounds;
            _rounds.Add(CreateRound());
            RoundSwitch?.Invoke();
        }

        private void Update()
        {
            if (_state == SpawnState.Waiting)
            {
                if (!EnemyAliveCheck())
                    NewRound();
                else 
                    return;
            }

            if (_roundCountDown <= 0)
            {
                if (_state != SpawnState.Spawning)
                    StartCoroutine(SpawnRound((Round)_rounds[NextRound]));
            }
            else
                _roundCountDown -= Time.deltaTime;
        }

        private void NewRound()
        {
            _state = SpawnState.Counting;
            _roundCountDown = timeBetweenRounds;

            NextRound++;
            _rounds.Add(CreateRound());
            RoundSwitch?.Invoke();
        }

        private Round CreateRound()
        {
            Round round = new Round
            {
                roundCount = NextRound switch
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
                    _ => Mathf.RoundToInt(0.0842f * Mathf.Pow(NextRound, 2) + 0.1954f * NextRound + 22.05f)
                },
                enemy = enemyTransform
            };

            switch (NextRound)
            {
                case 0:
                    return round;
                case < 9:
                    _enemyHealth += 100;
                    break;
                default:
                    _enemyHealth *= 1.1f;
                    break;
            }

            return round;
        }

        private bool EnemyAliveCheck()
        {
            _searchCountDown -= Time.deltaTime;

            if (!(_searchCountDown <= 0)) return true;
        
            _searchCountDown = 1;

            return GameObject.FindGameObjectWithTag("Enemy") != null;
        }

        private IEnumerator SpawnRound(Round round)
        {
            _state = SpawnState.Spawning;

            for (int i = 0; i < round.roundCount; i++)
            {
                SpawnEnemy(round.enemy);

                yield return new WaitForSeconds(1 / round.spawnRate);
            }

            _state = SpawnState.Waiting;
        }

        private void SpawnEnemy(Transform enemy)
        {
            Transform sp = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length - 1)];
            try
            {
                enemy.GetComponent<EnemyHealth>().SetMaxHealth(_enemyHealth);
            }
            catch (NullReferenceException)
            {
                
            }
            enemy.transform.GetChild(1).GetComponent<Canvas>().worldCamera = Camera.main;

            Instantiate(enemy, sp);
        }

        private enum SpawnState
        {
            Spawning,
            Waiting,
            Counting
        }
    }
}
