using UnityEngine;

namespace JANOARG.Chartmaker.Behaviors.Chartmaker.PickHandler
{
    public class PlayerViewLanePickHandler : PlayerViewPickHandler
    {
        public ChartmakerLanePlayer Instance;

        public override bool Pick()
        {
            if (TimelinePanel.main.CurrentMode == TimelineMode.Lanes)
            {
                InspectorPanel.main.SetObject(Instance.CurrentLane.Original);

                return true;
            }

            return false;
        }
    }
}