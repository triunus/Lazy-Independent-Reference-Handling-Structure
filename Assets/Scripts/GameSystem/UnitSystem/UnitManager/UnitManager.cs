using System.Collections;

using UnityEngine;

using Foundations.ReferencesHandler;

namespace GameSystem.UnitSystem.UnitManager
{
    public interface IUnitManager
    {
        public IEnumerator SpawnOperation();
        public IEnumerator AttackOperation(int targetUnitID, UnitType targetUnitType);
        public IEnumerator HittedOperation(float damage);
        public IEnumerator HealOperation(int targetUnitID, UnitType targetUnitType);
        public IEnumerator HealedOperation(float healAmount);
    }

    public class UnitManager : MonoBehaviour, IUnitManager
    {
        [SerializeField] private UnitInteractionController UnitInteractionController;

        private UnitManagerData myUnitManagerData;
        [SerializeField] private UnitData myUnitData;

        public IEnumerator SpawnOperation()
        {
            this.InitialSetting();

            yield return 0.3f;
        }
        private void InitialSetting()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var UnitManagerDbHandler = HandlerManager.GetDynamicHandler<UnitManagerDBHandler>();

            this.myUnitManagerData = new(this.gameObject.GetInstanceID());
            // 이후, 유닛 규격화가 완료되면, [SerializeField] 명시하고 있는 유닛 DB 부분을 Static Handler에서 가져오는 것으로 위임합니다.
            this.myUnitManagerData.UnitData = this.myUnitData;
            this.myUnitManagerData.IUnitManager = this;

            UnitManagerDbHandler.AddUnitManagerData(this.myUnitManagerData.UniqueID, this.myUnitData.UnitType, this.myUnitManagerData);

            this.UnitInteractionController.InitialSetting(this.myUnitManagerData);
        }

        // 공격 코루틴 흐름 수행.
        // 지정된 적 탐색 -> 데미지 계산 -> 탐색된 적에게 Hitted 전달.
        public IEnumerator AttackOperation(int targetUniquID, UnitType targetUnitType)
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var UnitManagerDbHandler = HandlerManager.GetDynamicHandler<UnitManagerDBHandler>();
            var UnitCombatCalculator = HandlerManager.GetUtilityHandler<UnitCombatCalculator>();
            var IUnitStatUIUXManager = HandlerManager.GetDynamicHandler<UIUXSystem.UIUXHandler>().IUnitStatUIUXManager;

            // targetID 와 대응되는 unitManagerData를 찾을 수 없으면 리턴.
            if (!UnitManagerDbHandler.TryGetUnitManagerData(targetUniquID, targetUnitType, out var unitManagerData)) yield break;

            // 가격 측 Damage량 계산
            float calculatedDamage = UnitCombatCalculator.CalculateModifiedAttackPower(new AttackerAttackData(this.myUnitData));

            IUnitStatUIUXManager.RegisterAttackerData(this.myUnitData.BaseAttackPower, this.myUnitData.AttackPowerModifier, calculatedDamage);

            // 피격측 Hitted 흐름 호출.
            yield return this.StartCoroutine(unitManagerData.IUnitManager.HittedOperation(calculatedDamage));
        }
        // Heal 코루틴 흐름 수행.
        // 지정된 적 탐색 -> 회복량 계산 -> 탐색된 적에게 Healed 전달.
        public IEnumerator HealOperation(int targetUniquID, UnitType targetUnitType)
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var UnitManagerDbHandler = HandlerManager.GetDynamicHandler<UnitManagerDBHandler>();
            var UnitCombatCalculator = HandlerManager.GetUtilityHandler<UnitCombatCalculator>();
            var IUnitStatUIUXManager = HandlerManager.GetDynamicHandler<UIUXSystem.UIUXHandler>().IUnitStatUIUXManager;

            // targetID 와 대응되는 unitManagerData를 찾을 수 없으면 리턴.
            if (!UnitManagerDbHandler.TryGetUnitManagerData(targetUniquID, targetUnitType, out var unitManagerData)) yield break;

            // 가격 측 Heal량 계산
            float calculatedHealAmount = UnitCombatCalculator.CalculateModifiedHealPower(new AttackerHealData(this.myUnitData));

            IUnitStatUIUXManager.RegisterAttackerData(this.myUnitData.BaseHealPower, this.myUnitData.HealPowerModifier, calculatedHealAmount);

            // 피격측 Healed 흐름 호출.
            yield return this.StartCoroutine(unitManagerData.IUnitManager.HealedOperation(calculatedHealAmount));
        }

        public IEnumerator HittedOperation(float damage)
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var UnitCombatCalculator = HandlerManager.GetUtilityHandler<UnitCombatCalculator>();
            var IUnitStatUIUXManager = HandlerManager.GetDynamicHandler<UIUXSystem.UIUXHandler>().IUnitStatUIUXManager;

            // 피격 측 Damage량 계산
            float calculatedDamage = UnitCombatCalculator.CalculateFinalAttackPower(new DefenderAttackReceiveData(damage, this.myUnitData));

            IUnitStatUIUXManager.RegisterTargetData(damage, this.myUnitData.AttackPowerTakenModifier, calculatedDamage, this.myUnitData.HP, this.myUnitData.HP - calculatedDamage);
            IUnitStatUIUXManager.UpdateUIUX();

            this.myUnitData.HP -= (int)calculatedDamage;

            yield return null;
        }

        public IEnumerator HealedOperation(float healAmount)
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var UnitCombatCalculator = HandlerManager.GetUtilityHandler<UnitCombatCalculator>();
            var IUnitStatUIUXManager = HandlerManager.GetDynamicHandler<UIUXSystem.UIUXHandler>().IUnitStatUIUXManager;

            // 피격 측 Heal량 계산
            float calculatedHealAmount = UnitCombatCalculator.CalculateFinalHealPower(new DefenderHealReceiveData(healAmount, this.myUnitData));

            IUnitStatUIUXManager.RegisterTargetData(healAmount, this.myUnitData.HealPowerTakenModifier, calculatedHealAmount, this.myUnitData.HP, this.myUnitData.HP + calculatedHealAmount);
            IUnitStatUIUXManager.UpdateUIUX();

            this.myUnitData.HP += (int)calculatedHealAmount;

            yield return null;
        }
    }
}