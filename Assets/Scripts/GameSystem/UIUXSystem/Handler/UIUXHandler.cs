
using Foundations.ReferencesHandler;

namespace GameSystem.UIUXSystem
{
    [System.Serializable]
    public class UIUXHandler : IDynamicReferenceHandler
    {
        public UnitInteractionUIUX.IUnitInteractionManager IUnitInteractionUIUXManager { get; set; }

        public UnitStatUIUX.IUnitStatUIUXManager IUnitStatUIUXManager { get; set; }
    }
}