using Foundations.ReferencesHandler;

namespace GameSystem.UnitSystem
{
    public class UnitSystemHandler : IDynamicReferenceHandler
    {
        public PlayerSpawner.IPlayerSpawner IPlayerSpawner { get; set; }
        public EnemySpawner.IEnemySpawner IEnemySpawner { get; set; }
    }
}

