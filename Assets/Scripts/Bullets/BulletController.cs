using UnityEngine;
using CosmicCuration.VFX;
using CosmicCuration.Audio;
using CosmicCuration.Player;

namespace CosmicCuration.Bullets
{

    public class BulletController : IBullet
    {
        private BulletView bulletView;
        private BulletScriptableObject bulletScriptableObject;
        private PlayerService playerService;

        public BulletController(BulletView bulletViewPrefab, BulletScriptableObject bulletScriptableObject)
        {
            bulletView = Object.Instantiate(bulletViewPrefab);
            bulletView.SetController(this);
            this.bulletScriptableObject = bulletScriptableObject;
        }

        public void ConfigureBullet(Transform spawnTransform)
        {
            bulletView.transform.position = spawnTransform.position;
            bulletView.transform.rotation = spawnTransform.rotation;
            bulletView.gameObject.SetActive(true);
        }

        public void UpdateBulletMotion() => bulletView.transform.Translate(Vector2.up * Time.deltaTime * bulletScriptableObject.speed);

        public void OnBulletEnteredTrigger(GameObject collidedGameObject)
        {
            if (collidedGameObject.GetComponent<IDamageable>() != null)
            {
                collidedGameObject.GetComponent<IDamageable>().TakeDamage(bulletScriptableObject.damage);
                GameService.Instance.GetSoundService().PlaySoundEffects(SoundType.BulletHit);
                GameService.Instance.GetVFXService().PlayVFXAtPosition(VFXType.BulletHitExplosion, bulletView.transform.position);

                GameService.Instance.GetPlayerService().ReturnBulletToPool(this);
                bulletView.gameObject.SetActive(false);
            }
        }
    }
}