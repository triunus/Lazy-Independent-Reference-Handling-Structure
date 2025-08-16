
using Foundations.ReferencesHandler;

using UnityEngine;

namespace GameSystem.UIUXSystem.UnitInteractionUIUX
{
    public interface IUnitInteractionContentUIUX
    {
        public void InitialSetting(UnitInteractionUIUXData unitInteractionUIUXData);
        public void UpdateUIUX();
        public void ResettingUIUX();
    }

    public class UnitInteractionUIUX_Content : MonoBehaviour, IUnitInteractionContentUIUX
    {
        private UnitInteractionUIUXData myUnitInteractionUIUXData;

        private UnitSystem.PlayerSpawner.PlayerUnitDBHandler PlayerUnitDBHandler;
        private UnitSystem.EnemySpawner.EnemyUnitDBHandler EnemyUnitDBHandler;

        [SerializeField] private Sprite ImageSprite;

        [SerializeField] private UnityEngine.UI.Image AttackerImage;
        [SerializeField] private UnityEngine.UI.Image TargetImage;
        [SerializeField] private TMPro.TextMeshProUGUI BehaviourText;

        private void Awake()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            this.PlayerUnitDBHandler = HandlerManager.GetStaticHandler<UnitSystem.PlayerSpawner.PlayerUnitDBHandler>();
            this.EnemyUnitDBHandler = HandlerManager.GetStaticHandler<UnitSystem.EnemySpawner.EnemyUnitDBHandler>();
        }

        public void InitialSetting(UnitInteractionUIUXData unitInteractionUIUXData)
        {
            this.myUnitInteractionUIUXData = unitInteractionUIUXData;
        }

        public void ResettingUIUX()
        {
            this.AttackerImage.sprite = null;
            this.AttackerImage.color = Color.white;
            this.TargetImage.sprite = null;
            this.TargetImage.color = Color.white;
            this.BehaviourText.text = default;
        }

        public void UpdateUIUX()
        {
            this.ResettingUIUX();

            if (this.myUnitInteractionUIUXData.BehaviourType != BehaviourType.None)
            {
                this.BehaviourText.text = this.myUnitInteractionUIUXData.BehaviourType.ToString();
            }

            // Attacker가 등록되어 있을 경우.
            if (this.myUnitInteractionUIUXData.AttackerData != null)
            {
                // 아군일 경우, 아군 이미지 가져옴.
                if(this.myUnitInteractionUIUXData.AttackerData.UnitType == UnitSystem.UnitManager.UnitType.PlayerUnit)
                {
                    if (this.PlayerUnitDBHandler.TryGetUnitResourceData(this.myUnitInteractionUIUXData.AttackerData.UnitID, out var unitResourceData))
                    {
                        this.AttackerImage.sprite = this.ImageSprite;
                        this.AttackerImage.color = unitResourceData.UnitColor;
                    }
                }
                // 적일 경우, 적 이미지 가져옴.
                else
                {
                    if (this.EnemyUnitDBHandler.TryGetUnitResourceData(this.myUnitInteractionUIUXData.AttackerData.UnitID, out var unitResourceData))
                    {
                        this.AttackerImage.sprite = this.ImageSprite;
                        this.AttackerImage.color = unitResourceData.UnitColor;
                    }
                }
            }

            // Target가 등록되어 있을 경우.
            if (this.myUnitInteractionUIUXData.TargetData != null)
            {
                // 아군일 경우, 아군 이미지 가져옴.
                if (this.myUnitInteractionUIUXData.TargetData.UnitType == UnitSystem.UnitManager.UnitType.PlayerUnit)
                {
                    if (this.PlayerUnitDBHandler.TryGetUnitResourceData(this.myUnitInteractionUIUXData.TargetData.UnitID, out var unitResourceData))
                    {
                        this.TargetImage.sprite = this.ImageSprite;
                        this.TargetImage.color = unitResourceData.UnitColor;
                    }
                }
                // 적일 경우, 적 이미지 가져옴.
                else
                {
                    if (this.EnemyUnitDBHandler.TryGetUnitResourceData(this.myUnitInteractionUIUXData.TargetData.UnitID, out var unitResourceData))
                    {
                        this.TargetImage.sprite = this.ImageSprite;
                        this.TargetImage.color = unitResourceData.UnitColor;
                    }
                }
            }
        }
    }
}