using UnityEngine;

namespace cc_effects
{
    public class CCTravelEffect : CCEffect
    {
        [Range (-2,3)]
        public float travelPosition = 0;
        [Range(0, 1)]
        public float travelRange = 0;

        public CCEffectModulation modulation = new CCEffectModulation();

        public override float Apply(CCEffectData theData)
        {
            float d = (modulation.Modulation(theData) - travelPosition) / travelRange;
            d = Mathf.Clamp(d, -1, 1) * amount;
            return d;
        }
    }
}