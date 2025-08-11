using UnityEngine;

using Foundations.ReferencesHandler;

namespace GameSystem.UnitSystem.EnemySpawner
{
    public class EnemyUnitDBHandler : IStaticReferenceHandler
    {
        private string EnemyUnitDataSOPath = "ScriptableObject/EnemySystem/EnemyUnitResourceDataSO";

        public bool TryLoadScriptableObject(out UnitResourceDataSO unitResourceDataSO)
        {
            unitResourceDataSO = Resources.Load<UnitResourceDataSO>(this.EnemyUnitDataSOPath);

            if (unitResourceDataSO == null) return false;
            return true;
        }

        public bool TryGetUnitResourceData(int unitID, out UnitResourceData unitResourceData)
        {
            if (!this.TryLoadScriptableObject(out var unitResourceDataSO))
            {
                Debug.LogError("SO가 경로에 없거나, 정상적이지 않음.");
                unitResourceData = null;
                return false;
            }

            return unitResourceDataSO.TryGetUnitResourceData(unitID, out unitResourceData);
        }
    }
}
