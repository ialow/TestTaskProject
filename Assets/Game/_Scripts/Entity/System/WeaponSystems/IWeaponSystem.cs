namespace Game
{
    public interface IWeaponSystem : IEntitySystem
    {
        void StartShoot();
        void StopShoot();
    }
}
