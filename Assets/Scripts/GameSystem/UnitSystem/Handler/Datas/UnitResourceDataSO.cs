using System.Collections.Generic;
using UnityEngine;

namespace GameSystem.UnitSystem
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "ScriptableObject/UnitSystem/UnitResourceDataSO", fileName = "UnitResourceDataSO")]
    public class UnitResourceDataSO : ScriptableObject
    {
        public List<UnitResourceData> UnitResourceDatas;

        public bool TryGetUnitResourceData(int unitID, out UnitResourceData unitResourceData)
        {
            unitResourceData = null;
            if (this.UnitResourceDatas == null) return false;

            foreach (var data in this.UnitResourceDatas)
            {
                if (data.UnitID == unitID)
                {
                    unitResourceData = data;
                    return true;
                }
            }

            return false;
        }
    }


    [System.Serializable]
    public class UnitResourceData
    {
        public int UnitID;
        public GameObject UnitGameObject;
        public Color UnitColor;
//        public Sprite UnitImage;
    }
}

