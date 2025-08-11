using System.Collections.Generic;
using UnityEngine;

namespace GameSystem.UnitSystem
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "ScriptableObject/UnitSystem/UnitSpawnDataGroupSO", fileName = "UnitSpawnDataGroupSO")]
    public class UnitSpawnDataGroupSO : ScriptableObject
    {
        public List<UnitSpawnDataGroup> UnitSpawnDataGroups;

        public bool TryGetUnitSpawnDataGroup(int stageID, out UnitSpawnDataGroup unitSpawnDataGroup)
        {
            unitSpawnDataGroup = null;
            if (this.UnitSpawnDataGroups == null) return false;

            foreach(var data in this.UnitSpawnDataGroups)
            {
                if(data.StageID == stageID)
                {
                    unitSpawnDataGroup = data;
                    return true;
                }
            }

            return false;
        }
    }

    [System.Serializable]
    public class UnitSpawnDataGroup
    {
        public int StageID;
        public List<UnitSpawnData> UnitSpawnDatas;
    }

    [System.Serializable]
    public class UnitSpawnData
    {
        public int UnitID;
        public Vector3 SpawnPosition;
    }
}

