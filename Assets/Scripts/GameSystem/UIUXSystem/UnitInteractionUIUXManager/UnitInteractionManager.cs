using System.Collections;

using Foundations.ReferencesHandler;

using UnityEngine;

namespace GameSystem.UIUXSystem.UnitInteractionUIUX
{
    public interface IUnitInteractionManager
    {
        public IEnumerator DisplayUnitUIUX_Coroutine();

        public void RegisterUnitData(RegisterData registeredData);
        public void RegisterBehaviourData(BehaviourType behaviourType);
        public void OperateBehaviour();
        public void RemoveData();
    }

    public class UnitInteractionManager : MonoBehaviour, IUnitInteractionManager
    {
        [SerializeField] private UnitInteractionUIUX_Content UnitInteractionUIUX_Content;
        [SerializeField] private UnitInteractionUIUX_Animation UnitInteractionUIUX_Animation;
        [SerializeField] private UnitInteractionController UnitInteractionController;
        
        private UnitInteractionUIUXData myUnitInteractionUIUXData;

        private void Awake()
        {
            this.myUnitInteractionUIUXData = new();

            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var UIUXHandler = HandlerManager.GetDynamicDataHandler<UIUXHandler>();
            UIUXHandler.IUnitInteractionUIUXManager = this;

            this.UnitInteractionUIUX_Content.InitialSetting(this.myUnitInteractionUIUXData);
            this.UnitInteractionUIUX_Animation.InitialSetting(this.myUnitInteractionUIUXData);
            this.UnitInteractionController.InitialSetting(this, this.myUnitInteractionUIUXData);
        }

        // 행동 수행.
        public void OperateBehaviour()
        {
            // 필드 멤버들이 유효한 값을 갖지 않을 경우 블럭함.
            // 동작 중일 때 블럭.
            if (this.myUnitInteractionUIUXData.IsAnimationActivated && !this.IsActionValid()) return;

            this.StopAllCoroutines();
            this.StartCoroutine(this.OperateBehaviour_Coroutine());
        }

        private IEnumerator OperateBehaviour_Coroutine()
        {
            this.myUnitInteractionUIUXData.IsAnimationActivated = true;

            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var UnitManagerDbHandler = HandlerManager.GetDynamicDataHandler<UnitSystem.UnitManager.UnitManagerDBHandler>();

            // 가격자 유닛의 기능을 참조하기 위해, '가격자 유닛들이 담긴 Dynamic Handler'를 참조 및 Get.
            if (!UnitManagerDbHandler.TryGetUnitManagerData(
                this.myUnitInteractionUIUXData.AttackerData.UniqueID,
                this.myUnitInteractionUIUXData.AttackerData.UnitType,
                out var unitManagerData)) yield break;

            switch (this.myUnitInteractionUIUXData.BehaviourType)
            {
                case BehaviourType.Attack:
                    Debug.Log($"Attack");
                    yield return StartCoroutine(unitManagerData.IUnitManager.AttackOperation(this.myUnitInteractionUIUXData.TargetData.UniqueID, this.myUnitInteractionUIUXData.TargetData.UnitType));
                    break;
                case BehaviourType.Heal:
                    Debug.Log($"Heal");
                    yield return StartCoroutine(unitManagerData.IUnitManager.HealOperation(this.myUnitInteractionUIUXData.TargetData.UniqueID, this.myUnitInteractionUIUXData.TargetData.UnitType));
                    break;
                default:
                    break;
            }

            yield return new WaitForSeconds(3f);

            this.myUnitInteractionUIUXData.AttackerData = null;
            this.myUnitInteractionUIUXData.TargetData = null;
            this.myUnitInteractionUIUXData.BehaviourType = BehaviourType.None;
            this.UnitInteractionUIUX_Content.ResettingUIUX();

            this.myUnitInteractionUIUXData.IsAnimationActivated = false;
        }

        // 행동을 수행할 모든 조건을 갖추었는지 판단.
        // 필드 멤버들이 유효한 값을 갖는지 확인.
        private bool IsActionValid()
        {
            if (this.myUnitInteractionUIUXData == null) return false;

            if (this.myUnitInteractionUIUXData.AttackerData == null) return false;
            if (this.myUnitInteractionUIUXData.TargetData == null) return false;
            if (this.myUnitInteractionUIUXData.BehaviourType == BehaviourType.None) return false;

            return true;
        }

        public void RegisterBehaviourData(BehaviourType behaviourType)
        {
            // 동작 중일 때 블럭.
            if (this.myUnitInteractionUIUXData.IsAnimationActivated) return;

            if (this.myUnitInteractionUIUXData.BehaviourType == BehaviourType.None)
            {
                this.myUnitInteractionUIUXData.BehaviourType = behaviourType;
                this.UnitInteractionUIUX_Content.UpdateUIUX();
                return;
            }
        }

        public void RegisterUnitData(RegisterData registeredData)
        {
            Debug.Log($"0");

            // 동작 중일 때 블럭.

            if (this.myUnitInteractionUIUXData.IsAnimationActivated) return;

            Debug.Log($"1");
            if (this.myUnitInteractionUIUXData.AttackerData == null)
            {
                this.myUnitInteractionUIUXData.AttackerData = registeredData;
                this.UnitInteractionUIUX_Content.UpdateUIUX();
                return;
            }

            Debug.Log($"2");
            if (this.myUnitInteractionUIUXData.TargetData == null)
            {
                this.myUnitInteractionUIUXData.TargetData = registeredData;
                this.UnitInteractionUIUX_Content.UpdateUIUX();
                return;
            }
        }

        public void RemoveData()
        {
            // 동작 중일 때 블럭.
            if (this.myUnitInteractionUIUXData.IsAnimationActivated) return;

            if (this.myUnitInteractionUIUXData.BehaviourType != BehaviourType.None)
            {
                this.myUnitInteractionUIUXData.BehaviourType = BehaviourType.None;
                this.UnitInteractionUIUX_Content.UpdateUIUX();
                return;
            }

            if (this.myUnitInteractionUIUXData.TargetData != null)
            {
                this.myUnitInteractionUIUXData.TargetData = null;
                this.UnitInteractionUIUX_Content.UpdateUIUX();
                return;
            }

            if (this.myUnitInteractionUIUXData.AttackerData != null)
            {
                this.myUnitInteractionUIUXData.AttackerData = null;
                this.UnitInteractionUIUX_Content.UpdateUIUX();
                return;
            }
        }

        public IEnumerator DisplayUnitUIUX_Coroutine()
        {
            this.myUnitInteractionUIUXData.IsAnimationActivated = true;

            yield return StartCoroutine(this.UnitInteractionUIUX_Animation.Show_UIUX_Coroutine());

            this.myUnitInteractionUIUXData.IsAnimationActivated = false;
        }
    }

    [System.Serializable]
    public class UnitInteractionUIUXData
    {
        public RegisterData AttackerData;
        public RegisterData TargetData;

        public BehaviourType BehaviourType;

        public bool IsAnimationActivated = false;

        public UnitInteractionUIUXData()
        {
            this.AttackerData = null;
            this.TargetData = null;

            this.BehaviourType = BehaviourType.None;
        }
    }

    [System.Serializable]
    public class RegisterData
    {
        public int UniqueID;
        public int UnitID;
        public UnitSystem.UnitManager.UnitType UnitType;
    }

    [System.Serializable]
    public enum BehaviourType
    {
        None,
        Attack,
        Heal,
    }
}