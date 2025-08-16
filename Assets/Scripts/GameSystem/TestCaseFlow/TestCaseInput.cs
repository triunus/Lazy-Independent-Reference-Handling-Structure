using UnityEngine;

using Foundations.ReferencesHandler;

using GameSystem.UIUXSystem;
using GameSystem.UIUXSystem.UnitInteractionUIUX;

public class SelectActionTypeView : MonoBehaviour
{
    public void SelectAttack()
    {
        var HandlerManager = LazyReferenceHandlerManager.Instance;
        var IUnitInteractionUIUXManager = HandlerManager.GetDynamicHandler<UIUXHandler>().IUnitInteractionUIUXManager;

        IUnitInteractionUIUXManager.RegisterBehaviourData(BehaviourType.Attack);
    }

    public void SelecteHeal()
    {
        var HandlerManager = LazyReferenceHandlerManager.Instance;
        var IUnitInteractionUIUXManager = HandlerManager.GetDynamicHandler<UIUXHandler>().IUnitInteractionUIUXManager;

        IUnitInteractionUIUXManager.RegisterBehaviourData(BehaviourType.Heal);
    }
}