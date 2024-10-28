using CosmicCuration.Utilities;

namespace CosmicCuration.VFX
{
    public class VFXPool : GenericObjectPool<VFXController>
    {
        private VFXView viewToCreate;

        public VFXController GetVFXController(VFXView prefabToSpawn)
        {
            viewToCreate = prefabToSpawn;
            return GetItem<VFXController>();
        }

        protected override VFXController CreateItem<T>()
        {
            return new VFXController(viewToCreate);
        }
    }
}