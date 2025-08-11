
using Foundations.ReferencesHandler;

namespace GameSystem.UnitSystem.UnitManager
{
    public class UnitCombatCalculator : IUtilityReferenceHandler
    {
        // '가격자' 측에서 증폭한 값
        public float CalculateModifiedAttackPower(AttackerAttackData attackerAttackData)
        {
            return attackerAttackData.BaseAttackPower * (1f + attackerAttackData.AttackPowerModifier);
        }

        // '피격자' 측에서 증폭한 값
        public float CalculateFinalAttackPower(DefenderAttackReceiveData defenderAttackReceiveData)
        {
            return defenderAttackReceiveData.IncomingAttackPower * (1f + defenderAttackReceiveData.AttackPowerTakenModifier);
        }

        // '가격자' 측에서 증폭한 값
        public float CalculateModifiedHealPower(AttackerHealData attackerHealData)
        {
            return attackerHealData.BaseHealPower * (1f + attackerHealData.HealPowerModifier);
        }

        // '피격자' 측에서 증폭한 값
        public float CalculateFinalHealPower(DefenderHealReceiveData defenderHealReceiveData)
        {
            return defenderHealReceiveData.IncomingHealPower * (1f + defenderHealReceiveData.HealPowerTakenModifier);
        }
    }

    [System.Serializable]
    public class AttackerAttackData
    {
        public float BaseAttackPower { get; set; }     // A
        public float AttackPowerModifier { get; set; } // CA

        public AttackerAttackData(UnitData unitData)
        {
            this.BaseAttackPower = unitData.BaseAttackPower;
            this.AttackPowerModifier = unitData.AttackPowerModifier;
        }
    }

    [System.Serializable]
    public class AttackerHealData
    {
        public float BaseHealPower { get; set; }       // H
        public float HealPowerModifier { get; set; }   // CH

        public AttackerHealData(UnitData unitData)
        {
            this.BaseHealPower = unitData.BaseHealPower;
            this.HealPowerModifier = unitData.HealPowerModifier;
        }
    }

    public class DefenderAttackReceiveData
    {
        public float IncomingAttackPower { get; set; }       // 공격자가 보내온 값
        public float AttackPowerTakenModifier { get; set; }  // CCA

        public DefenderAttackReceiveData(float calculatedAmount, UnitData unitData)
        {
            this.IncomingAttackPower = calculatedAmount;
            this.AttackPowerTakenModifier = unitData.AttackPowerTakenModifier;
        }
    }

    [System.Serializable]
    public class DefenderHealReceiveData
    {
        public float IncomingHealPower { get; set; }         // 회복자가 보내온 값
        public float HealPowerTakenModifier { get; set; }    // CCH

        public DefenderHealReceiveData(float calculatedAmount, UnitData unitData)
        {
            this.IncomingHealPower = calculatedAmount;
            this.HealPowerTakenModifier = unitData.HealPowerTakenModifier;
        }
    }
}