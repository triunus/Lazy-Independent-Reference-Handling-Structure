using UnityEngine;

using Foundations.ReferencesHandler;

namespace GameSystem.UnitSystem.UnitManager
{
    public class UnitInteractionController : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer UnitSpriteRenderer;

        [SerializeField] private Color BaseColor;
        [SerializeField] private Color PointerIn;
        [SerializeField] private Color ClickDown;

        private UnitManagerData myUnitManagerData;

        public void InitialSetting(UnitManagerData unitManagerData)
        {
            this.myUnitManagerData = unitManagerData;
        }

        private void OnMouseEnter()
        {
            this.UnitSpriteRenderer.color = this.PointerIn;
        }

        private void OnMouseExit()
        {
            this.UnitSpriteRenderer.color = this.BaseColor;
        }

        private void OnMouseDown()
        {
            this.UnitSpriteRenderer.color = this.ClickDown;
        }

        private void OnMouseUp()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var UIUXHandler = HandlerManager.GetDynamicHandler<UIUXSystem.UIUXHandler>();

            // Unit 등록 작업.
            UIUXSystem.UnitInteractionUIUX.RegisterData registerData = new();
            registerData.UniqueID = this.myUnitManagerData.UniqueID;
            registerData.UnitID = this.myUnitManagerData.UnitData.UnitID;
            registerData.UnitType = this.myUnitManagerData.UnitData.UnitType;
            UIUXHandler.IUnitInteractionUIUXManager.RegisterUnitData(registerData);

            this.UnitSpriteRenderer.color = this.BaseColor;
        }
    }
}