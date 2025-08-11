
using UnityEngine;

namespace GameSystem.UIUXSystem.UnitInteractionUIUX
{
    public class UnitInteractionController : MonoBehaviour
    {
        private IUnitInteractionManager IUnitInteractionUIUXManager;
        private UnitInteractionUIUXData myUnitInteractionUIUXData;

        public void InitialSetting(IUnitInteractionManager unitInteractionUIUXManager, UnitInteractionUIUXData UnitInteractionUIUXData)
        {
            this.IUnitInteractionUIUXManager = unitInteractionUIUXManager;
            this.myUnitInteractionUIUXData = UnitInteractionUIUXData;
        }

        private void Update()
        {
            // 마우스 우클릭 눌렀을 때 (한번만)
            if (Input.GetMouseButtonDown(1) && !this.myUnitInteractionUIUXData.IsAnimationActivated)
            {
                this.IUnitInteractionUIUXManager.RemoveData();
            }
        }
    }
}