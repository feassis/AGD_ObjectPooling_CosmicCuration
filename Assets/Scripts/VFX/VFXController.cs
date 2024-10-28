using UnityEngine;

namespace CosmicCuration.VFX
{

    public class VFXController
    {
        private VFXView vfxView;

        public VFXController(VFXView vfxPrefab)
        {
            vfxView = Object.Instantiate(vfxPrefab);
            vfxView.SetController(this);
        }

        public void Configure(Vector2 spawnPosition)
        {
            vfxView.gameObject.SetActive(true);
            vfxView.ConfigureAndPlay(spawnPosition);
        }

        public void ReturnToPool()
        {
            GameService.Instance.GetVFXService().ReturnVFXToPool(this);
        }
    } 
}