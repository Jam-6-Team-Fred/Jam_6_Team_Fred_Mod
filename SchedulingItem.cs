using NewHorizons;
using NewHorizons.Components.Props;
using NewHorizons.External.Modules.Props;
using NewHorizons.Utility;
using OWML.ModHelper;

namespace Jam6
{
    public class SchedulingItem : NHItem
    {
        public ModBehaviour mod;

        public void OnValidate()
        {
            _type = Jam6.SchedulingItemType;
            DisplayName = "SchedulingItemDisplayName";
            Droppable = true;
        }

        public override void Awake()
        {
            OnValidate();
            base.Awake();
            mod = Jam6.Instance;
            mod.ModHelper.Console.WriteLine("A scheduling item is created", OWML.Common.MessageType.Success);
        }
    }
}
