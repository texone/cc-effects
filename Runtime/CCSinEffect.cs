using UnityEngine;
using cc.creativecomputing.math.signal;

namespace cc_effects
{
    public class CCSinEffect : CCEffect
    {

        public CCEffectModulation phase = new CCEffectModulation();
        public CCEffectModulation amp = new CCEffectModulation();

        public CCSinSignal sin = new CCSinSignal();

        public Vector3 speed = new Vector3();

        public bool useSpeed = false;

        public Vector3 offset = new Vector3();

        private void Update()
        {
            if(useSpeed) offset += Time.deltaTime * speed;
        }

        public override float Apply(CCEffectData theData)
        {
            
            float myResult = sin.value(
                phase.X(theData) + offset.x + phase.Random(theData) + phase.Id(theData) + phase.IdMod(theData) + phase.Group(theData) + phase.GroupMod(theData),
                phase.Y(theData) + offset.y,
                phase.Z(theData) + offset.z
            ) * amp.Modulation(theData) * amount;
            return myResult;


        }
    }
}