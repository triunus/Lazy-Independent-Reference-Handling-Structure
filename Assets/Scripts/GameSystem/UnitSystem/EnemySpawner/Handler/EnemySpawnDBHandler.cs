using UnityEngine;

using Foundations.ReferencesHandler;

namespace GameSystem.UnitSystem.EnemySpawner
{
    public class EnemySpawnDBHandler : IStaticReferenceHandler
    {
        private string EnemyUnitSpawnDataGroupSOPath = "ScriptableObject/EnemySystem/EnemyUnitSpawnDataGroupSO";

        public bool TryLoadScriptableObject(out UnitSpawnDataGroupSO unitSpawnDataGroupSO)
        {
            unitSpawnDataGroupSO = Resources.Load<UnitSpawnDataGroupSO>(this.EnemyUnitSpawnDataGroupSOPath);

            if (unitSpawnDataGroupSO == null) return false;
            return true;
        }

        public bool TryGetUnitSpawnDataGroup(int stageID, out UnitSpawnDataGroup unitSpawnDataGroup)
        {
            if (!this.TryLoadScriptableObject(out var unitSpawnDataGroupSO))
            {
                Debug.LogError("SO가 경로에 없거나, 정상적이지 않음.");
                unitSpawnDataGroup = null;
                return false;
            }

            return unitSpawnDataGroupSO.TryGetUnitSpawnDataGroup(stageID, out unitSpawnDataGroup);
        }
    }
}
