using System.Collections.Generic;

using Foundations.ReferencesHandler;

namespace GameSystem.UnitSystem.UnitManager
{
    [System.Serializable]
    public class UnitManagerDBHandler : IDynamicReferenceHandler
    {
        private Dictionary<int, UnitManagerData> PlayerUnitManagerDatas;
        private Dictionary<int, UnitManagerData> EnemyUnitManagerDatas;

        public UnitManagerDBHandler()
        {
            this.PlayerUnitManagerDatas = new();
            this.EnemyUnitManagerDatas = new();
        }

        public void AddUnitManagerData(int uniqueID, UnitType unitType_ , UnitManagerData unitManagerData)
        {
            if (this.PlayerUnitManagerDatas == null)
                this.PlayerUnitManagerDatas = new();

            if (this.EnemyUnitManagerDatas == null)
                this.EnemyUnitManagerDatas = new();

            if (unitType_ == UnitType.PlayerUnit)
            {
                this.PlayerUnitManagerDatas.Add(uniqueID, unitManagerData);
            }
            else
            {
                this.EnemyUnitManagerDatas.Add(uniqueID, unitManagerData);
            }
        }

        public bool TryGetUnitManagerData(int uniqueID, UnitType UnitType_, out UnitManagerData unitManagerData)
        {
            unitManagerData = null;
            if (this.PlayerUnitManagerDatas == null && this.EnemyUnitManagerDatas == null) return false;

            if(UnitType_ == UnitType.PlayerUnit)
            {
                return this.PlayerUnitManagerDatas.TryGetValue(uniqueID, out unitManagerData);
            }
            else
            {
                return this.EnemyUnitManagerDatas.TryGetValue(uniqueID, out unitManagerData);
            }
        }

        public bool TryGetPlayerUnitManagerData(int uniqueID, out UnitManagerData unitManagerData)
        {
            unitManagerData = null;
            if (this.PlayerUnitManagerDatas == null) return false;

            return this.PlayerUnitManagerDatas.TryGetValue(uniqueID, out unitManagerData);
        }
        // 멤버 정의 체크 +  덮어쓰기.
        public void AddPlayerUnitManagerData_Override(int uniqueID, UnitManagerData unitManagerData)
        {
            if (this.PlayerUnitManagerDatas == null)
                this.PlayerUnitManagerDatas = new();

            this.PlayerUnitManagerDatas.Add(uniqueID, unitManagerData);
        }

        public bool TryGetEnemyUnitManagerData(int uniqueID, out UnitManagerData unitManagerData)
        {
            unitManagerData = null;
            if (this.EnemyUnitManagerDatas == null) return false;

            return this.EnemyUnitManagerDatas.TryGetValue(uniqueID, out unitManagerData);
        }
        // 멤버 정의 체크 +  덮어쓰기.
        public void AddEnemyUnitManagerData(int uniqueID, UnitManagerData unitManagerData)
        {
            if (this.EnemyUnitManagerDatas == null)
                this.EnemyUnitManagerDatas = new();

            this.EnemyUnitManagerDatas.Add(uniqueID, unitManagerData);
        }
    }

    [System.Serializable]
    public class UnitManagerData
    {
        public int UniqueID;

        public UnitData UnitData;
        public IUnitManager IUnitManager { get; set; }

        public UnitManagerData(int uniqueID)
        {
            UniqueID = uniqueID;
        }
    }

    [System.Serializable]
    public class UnitData
    {
        public int UnitID;
        public UnitType UnitType;

        public int HP;

        public int BaseAttackPower;
        public float AttackPowerModifier;
        public float AttackPowerTakenModifier;

        public int BaseHealPower;
        public float HealPowerModifier;
        public float HealPowerTakenModifier;
    }

    [System.Serializable]
    public enum UnitType
    {
        PlayerUnit,
        EnemyUnit
    }
}