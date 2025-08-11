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

        // �ൿ ����.
        public void OperateBehaviour()
        {
            // �ʵ� ������� ��ȿ�� ���� ���� ���� ��� ����.
            // ���� ���� �� ��.
            if (this.myUnitInteractionUIUXData.IsAnimationActivated && !this.IsActionValid()) return;

            this.StopAllCoroutines();
            this.StartCoroutine(this.OperateBehaviour_Coroutine());
        }

        private IEnumerator OperateBehaviour_Coroutine()
        {
            this.myUnitInteractionUIUXData.IsAnimationActivated = true;

            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var UnitManagerDbHandler = HandlerManager.GetDynamicDataHandler<UnitSystem.UnitManager.UnitManagerDBHandler>();

            // ������ ������ ����� �����ϱ� ����, '������ ���ֵ��� ��� Dynamic Handler'�� ���� �� Get.
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

        // �ൿ�� ������ ��� ������ ���߾����� �Ǵ�.
        // �ʵ� ������� ��ȿ�� ���� ������ Ȯ��.
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
            // ���� ���� �� ��.
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

            // ���� ���� �� ��.

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
            // ���� ���� �� ��.
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