using UnityEngine;
using CosmicCuration.VFX;
using CosmicCuration.Audio;

namespace CosmicCuration.Bullets
{
    public class BulletController : IBullet
    {
        private BulletView bulletView;
        private BulletScriptableObject bulletScriptableObject;
        private GameObject owner;

        public BulletController(BulletView bulletViewPrefab, BulletScriptableObject bulletScriptableObject)
        {
            bulletView = Object.Instantiate(bulletViewPrefab);
            bulletView.SetController(this);
            this.bulletScriptableObject = bulletScriptableObject;
        }

        public void ConfigureBullet(Transform spawnTransform, GameObject owner)
        {
            this.owner = owner;
            bulletView.gameObject.SetActive(true);
            bulletView.transform.position = spawnTransform.position;
            bulletView.transform.rotation = spawnTransform.rotation;
        }

        public void UpdateBulletMotion() => bulletView.transform.Translate(Vector2.up * Time.deltaTime * bulletScriptableObject.speed);

        public void OnBulletEnteredTrigger(GameObject collidedGameObject)
        {
            if (collidedGameObject.GetComponent<IDamageable>() != null)
            {
                if (collidedGameObject.gameObject.Equals(owner))
                {
                    return;
                }
                
                collidedGameObject.GetComponent<IDamageable>().TakeDamage(bulletScriptableObject.damage);
                GameService.Instance.GetSoundService().PlaySoundEffects(SoundType.BulletHit);
                GameService.Instance.GetVFXService().PlayVFXAtPosition(VFXType.BulletHitExplosion, bulletView.transform.position);
                bulletView.gameObject.SetActive(false);
                GameService.Instance.GetPlayerService().ReturnBulletToPool(this);
            }
        }
    }
}