using UnityEngine;
using cc.creativecomputing.math.signal;

namespace cc_effects
{
    public abstract class CCSignalEffect : CCEffect
    {

        public CCEffectModulation phase = new CCEffectModulation();
        public CCEffectModulation amp = new CCEffectModulation();

        public CCSignal signal;

        public Vector3 speed = new Vector3();

        public bool useSpeed = false;

        public Vector3 offset = new Vector3();

        public abstract CCSignal CreateSignal();

        private void Start()
        {
            signal = CreateSignal();
        }

        private void Update()
        {
            if(useSpeed) offset += Time.deltaTime * speed;
        }

        public override float Apply(CCEffectData theData)
        {
            
            float myResult = signal.value(
                phase.X(theData) + offset.x + phase.Random(theData) + phase.Id(theData) + phase.IdMod(theData) + phase.Group(theData) + phase.GroupMod(theData),
                phase.Y(theData) + offset.y,
                phase.Z(theData) + offset.z
            ) * amp.Modulation(theData) * amount;
            return myResult;


        }
    }
}