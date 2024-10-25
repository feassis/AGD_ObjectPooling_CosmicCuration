using System;
using System.Collections.Generic;

namespace CosmicCuration.Enemy
{
    public class EnemyPool
    {
        private EnemyView enemyView;
        private EnemyScriptableObject enemySO;
        private List<PooledEnemy> pooledEnemies = new List<PooledEnemy>();


        public EnemyPool(EnemyView enemyView, EnemyScriptableObject enemySO)
        {
            this.enemyView = enemyView;
            this.enemySO = enemySO;
        }

        public EnemyController GetEnemy()
        {
            if(pooledEnemies.Count >0)
            {
                PooledEnemy pooledEnemy = pooledEnemies.Find(e => e.IsUsed == false);
                
                if(pooledEnemy != null)
                {
                    pooledEnemy.IsUsed = true;
                    return pooledEnemy.Enemy;
                }
            }

            return CreateNewPooledEnemy();
        }

        public void ReturnToEnemyPool(EnemyController returnedEnemy)
        {
            PooledEnemy pooledEnemy = pooledEnemies.Find(e => e.Enemy.Equals(returnedEnemy));
            pooledEnemy.IsUsed = false;
        }

        private EnemyController CreateNewPooledEnemy()
        {
            PooledEnemy enemy = new PooledEnemy();
            enemy.Enemy = new EnemyController(enemyView, enemySO.enemyData);
            enemy.IsUsed = true;

            pooledEnemies.Add(enemy);

            return enemy.Enemy;
        }

        public class PooledEnemy
        {
            public EnemyController Enemy;
            public bool IsUsed;
        }
    }
}