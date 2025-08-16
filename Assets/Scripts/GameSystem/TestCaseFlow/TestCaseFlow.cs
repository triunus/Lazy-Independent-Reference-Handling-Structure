using System.Collections;
using UnityEngine;

using Foundations.ReferencesHandler;

namespace GameSystem
{
    public class TestCaseFlow : MonoBehaviour
    {
        [SerializeField] private int StageID;
        
        public void OperateTestCaseInitialSettingFlow()
        {
            this.StopAllCoroutines();
            this.StartCoroutine(this.OperateTestCaseInitialSettingFlow_Coroutine());
        }
        private IEnumerator OperateTestCaseInitialSettingFlow_Coroutine()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var UnitSystemHandler = HandlerManager.GetDynamicHandler<UnitSystem.UnitSystemHandler>();
            var UIUXHandler = HandlerManager.GetDynamicHandler<UIUXSystem.UIUXHandler>();

            yield return this.StartCoroutine(UnitSystemHandler.IPlayerSpawner.SpawnPlayerUnit(this.StageID));
            yield return this.StartCoroutine(UnitSystemHandler.IEnemySpawner.SpawnEnemyUnit(this.StageID));

            // UIUX 출력 부분.
            UIUXHandler.IUnitStatUIUXManager.RemoveAllText();
            yield return this.StartCoroutine(UIUXHandler.IUnitInteractionUIUXManager.DisplayUnitUIUX_Coroutine());
        }
    }
}