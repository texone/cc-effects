using UnityEngine;

namespace cc_effects
{
    public class CCDirectEffect : CCEffect
    {

        public CCEffectModulation modulation = new CCEffectModulation();


        public override float Apply(CCEffectData theData)
        {
            
            return modulation.Modulation(theData) * amount;
        }
    }
}