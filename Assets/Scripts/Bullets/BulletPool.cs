using System.Collections.Generic;

namespace CosmicCuration.Bullets
{
    public class BulletPool
    {
        private BulletView bulletView;
        private BulletScriptableObject bulletScriptableObject;
        private List<PooledBullet> pooledBullets = new List<PooledBullet>();

        public BulletPool(BulletView bulletView, BulletScriptableObject bulletScriptableObject)
        {
            this.bulletView = bulletView;
            this.bulletScriptableObject = bulletScriptableObject;
        }

        public BulletController GetBullet()
        {
            if(pooledBullets.Count > 0)
            {
                PooledBullet pooledBullet = pooledBullets.Find(b => b.IsUsed == false);

                if(pooledBullet != null)
                {
                    pooledBullet.IsUsed = true;
                    return pooledBullet.Bullet;
                }
            }

            return CreateNewPooledBullet();
        }

        public void ReturnToBulletPool(BulletController returndedBullet)
        {
            PooledBullet pooledBullet = pooledBullets.Find(b => b.Bullet.Equals(returndedBullet));
            pooledBullet.IsUsed = false;
        }

        private BulletController CreateNewPooledBullet()
        {
            PooledBullet pooledBullet = new PooledBullet();
            pooledBullet.Bullet = new BulletController(bulletView, bulletScriptableObject); 
            pooledBullet.IsUsed = true;

            pooledBullets.Add(pooledBullet);

            return pooledBullet.Bullet;
        }

        public class PooledBullet
        {
            public BulletController Bullet;
            public bool IsUsed;
        }
    }
}