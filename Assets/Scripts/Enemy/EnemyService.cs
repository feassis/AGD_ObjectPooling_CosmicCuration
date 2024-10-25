using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CosmicCuration.Enemy
{

    public class EnemyService
    {
        #region Dependencies
        private EnemyView enemyPrefab;
        private EnemyScriptableObject enemyScriptableObject;
        #endregion

        #region Variables
        private bool isSpawning;
        private float currentSpawnRate;
        private float spawnTimer;
        private EnemyPool enemyPool;
        #endregion

        #region Initialization
        public EnemyService(EnemyView enemyPrefab, EnemyScriptableObject enemyScriptableObject)
        {
            this.enemyPrefab = enemyPrefab;
            this.enemyScriptableObject = enemyScriptableObject;
            InitializeVariables();
        }

        private void InitializeVariables()
        {
            isSpawning = true;
            currentSpawnRate = enemyScriptableObject.initialSpawnRate;
            spawnTimer = currentSpawnRate;
            enemyPool = new EnemyPool(enemyPrefab, enemyScriptableObject);
        } 
        #endregion

        public void Update()
        {
            if (isSpawning)
            {
                spawnTimer -= Time.deltaTime;
                if (spawnTimer <= 0)
                {
                    SpawnEnemy();
                    IncreaseDifficulty();
                    ResetSpawnTimer();
                }
            }
        }

        #region Spawning Enemies
        private void SpawnEnemy()
        {
            // Get a random orientation for the enemy (Up / Down / Left / Right)
            EnemyOrientation randomOrientation = (EnemyOrientation)Random.Range(0, Enum.GetValues(typeof(EnemyOrientation)).Length);

            // Calculate a spawn position outside the game screen according to the orientation and spawn an enemy.
            SpawnEnemyAtPosition(CalculateSpawnPosition(randomOrientation), randomOrientation);
        }

        private void SpawnEnemyAtPosition(Vector2 spawnPosition, EnemyOrientation enemyOrientation)
        {
            EnemyController spawnedEnemy = enemyPool.GetEnemy();
            spawnedEnemy.Configure(spawnPosition, enemyOrientation);
        }

        private Vector2 CalculateSpawnPosition(EnemyOrientation enemyOrientation)
        {
            // Calculate a random spawn position outside the visible screen
            Vector3 spawnPosition = Vector3.zero;
            float halfScreenWidth = Camera.main.aspect * Camera.main.orthographicSize;
            float halfScreenHeight = Camera.main.orthographicSize;

            switch (enemyOrientation)
            {
                case EnemyOrientation.Left:
                    spawnPosition.x = halfScreenWidth + enemyScriptableObject.spawnDistance;
                    spawnPosition.y = Random.Range(-halfScreenHeight, halfScreenHeight);
                    break;

                case EnemyOrientation.Right:
                    spawnPosition.x = -halfScreenWidth - enemyScriptableObject.spawnDistance;
                    spawnPosition.y = Random.Range(-halfScreenHeight, halfScreenHeight);
                    break;

                case EnemyOrientation.Up:
                    spawnPosition.x = Random.Range(-halfScreenWidth, halfScreenWidth);
                    spawnPosition.y = -halfScreenHeight - enemyScriptableObject.spawnDistance;
                    break;

                case EnemyOrientation.Down:
                    spawnPosition.x = Random.Range(-halfScreenWidth, halfScreenWidth);
                    spawnPosition.y = halfScreenHeight + enemyScriptableObject.spawnDistance;
                    break;
            }

            return spawnPosition;
        } 
        #endregion

        private void IncreaseDifficulty()
        {
            if (currentSpawnRate > enemyScriptableObject.minimumSpawnRate)
                currentSpawnRate -= enemyScriptableObject.difficultyDelta;
            else
                currentSpawnRate = enemyScriptableObject.minimumSpawnRate;
        }

        private void ResetSpawnTimer() => spawnTimer = currentSpawnRate;

        public void ReturnEnemyToPool(EnemyController returnedEnemy) => enemyPool.ReturnToEnemyPool(returnedEnemy);

        public void SetEnemySpawning(bool setActive) => isSpawning = setActive;
    }

    public enum EnemyOrientation
    {
        Up,
        Down,
        Left,
        Right
    } 
}