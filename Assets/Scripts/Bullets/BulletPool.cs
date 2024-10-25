using System.Collections.Generic;

namespace CosmicCuration.Bullets
{
    public class BulletPool
    {
        private BulletView bulletView;
        private BulletScriptableObject bulletScriptableObject;
        private List<PooledBullet> polledBullets = new List<PooledBullet>();

        public BulletPool(BulletView bulletView, BulletScriptableObject bulletScriptableObject)
        {
            this.bulletView = bulletView;
            this.bulletScriptableObject = bulletScriptableObject;
        }

        public BulletController GetBullet()
        {
            if(polledBullets.Count > 0)
            {
                PooledBullet pooledBullet = polledBullets.Find(b => b.IsUsed == false);

                if(pooledBullet != null)
                {
                    pooledBullet.IsUsed = true;
                    return pooledBullet.Bullet;
                }
            }

            return CreateNewPooledBullet();
        }

        private BulletController CreateNewPooledBullet()
        {
            PooledBullet pooledBullet = new PooledBullet();
            pooledBullet.Bullet = new BulletController(bulletView, bulletScriptableObject); 
            pooledBullet.IsUsed = true;

            polledBullets.Add(pooledBullet);

            return pooledBullet.Bullet;
        }

        public class PooledBullet
        {
            public BulletController Bullet;
            public bool IsUsed;
        }
    }
}