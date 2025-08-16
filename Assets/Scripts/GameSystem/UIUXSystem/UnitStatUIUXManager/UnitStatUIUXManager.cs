using UnityEngine;
using TMPro;

using Foundations.ReferencesHandler;

namespace GameSystem.UIUXSystem.UnitStatUIUX
{
    public interface IUnitStatUIUXManager
    {
        public void RegisterAttackerData(float basePower, float modifier, float calculatedValue);
        public void RegisterTargetData(float basePower, float modifier, float calculatedValue, float previouseHP, float currentHP);

        public void UpdateUIUX();
        public void RemoveAllText();
    }

    public class UnitStatUIUXManager : MonoBehaviour, IUnitStatUIUXManager
    {
        [SerializeField] private TextMeshProUGUI AttackerPowerText;
        [SerializeField] private TextMeshProUGUI AttackerCalculationText;
        [SerializeField] private TextMeshProUGUI AttackerResultText;

        [SerializeField] private TextMeshProUGUI IncomingPowerText;
        [SerializeField] private TextMeshProUGUI TargetCalculationText;
        [SerializeField] private TextMeshProUGUI TargetResultText;

        [SerializeField] private TextMeshProUGUI PreviousHPText;
        [SerializeField] private TextMeshProUGUI CurrentHPText;

        private UnitStatUIUXData myUnitStatUIUXData = new();

        private void Awake()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var UIUXHandler = HandlerManager.GetDynamicHandler<UIUXHandler>();

            UIUXHandler.IUnitStatUIUXManager = this;
        }

        public void RegisterAttackerData(float basePower, float modifier, float calculatedValue)
        {
            this.myUnitStatUIUXData.AttackerPower = basePower;
            this.myUnitStatUIUXData.AttackerModifier = modifier;
            this.myUnitStatUIUXData.AttackerCalculatedValue = calculatedValue;
        }

        public void RegisterTargetData(float basePower, float modifier, float calculatedValue, float previouseHP, float currentHP)
        {
            this.myUnitStatUIUXData.IncomingPower = basePower;
            this.myUnitStatUIUXData.TargetModifier = modifier;
            this.myUnitStatUIUXData.TargetCalculatedValue = calculatedValue;

            this.myUnitStatUIUXData.PreviousHP = previouseHP;
            this.myUnitStatUIUXData.CurrentHP = currentHP;
        }

        public void RemoveAllText()
        {
            this.AttackerPowerText.text = default;
            this.AttackerCalculationText.text = default;
            this.AttackerResultText.text = default;

            this.IncomingPowerText.text = default;
            this.TargetCalculationText.text = default;
            this.TargetResultText.text = default;

            this.PreviousHPText.text = default;
            this.CurrentHPText.text = default;
        }

        public void UpdateUIUX()
        {
            this.AttackerPowerText.text = this.myUnitStatUIUXData.AttackerPower.ToString();
            this.AttackerCalculationText.text = $"{this.myUnitStatUIUXData.AttackerPower} * {this.myUnitStatUIUXData.AttackerModifier}";
            this.AttackerResultText.text = this.myUnitStatUIUXData.AttackerCalculatedValue.ToString();

            this.IncomingPowerText.text = this.myUnitStatUIUXData.IncomingPower.ToString();
            this.TargetCalculationText.text = $"{this.myUnitStatUIUXData.IncomingPower} * {this.myUnitStatUIUXData.TargetModifier}";
            this.TargetResultText.text = this.myUnitStatUIUXData.TargetCalculatedValue.ToString();

            this.PreviousHPText.text = this.myUnitStatUIUXData.PreviousHP.ToString();
            this.CurrentHPText.text = this.myUnitStatUIUXData.CurrentHP.ToString();
        }
    }

    [System.Serializable]
    public class UnitStatUIUXData
    {
        public float AttackerPower;
        public float AttackerModifier;
        public float AttackerCalculatedValue;

        public float IncomingPower;
        public float TargetModifier;
        public float TargetCalculatedValue;

        public float PreviousHP;
        public float CurrentHP;
    }
}