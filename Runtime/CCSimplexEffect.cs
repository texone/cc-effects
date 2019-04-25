using cc.creativecomputing.math.signal;
using UnityEngine;
using System.Collections.Generic;

namespace cc_effects
{
    public class CCSimplexEffect : CCEffect
    {

        public CCEffectModulation phase = new CCEffectModulation();
        public CCEffectModulation amp = new CCEffectModulation();

        public CCSimplexNoise noise = new CCSimplexNoise();

        public Vector3 speed = new Vector3();

        public float windSpeed = 0;

        public Vector3 offset = new Vector3();

        public Dictionary<int, float> lightOffsets = new Dictionary<int, float>();

        private void Update()
        {
            offset += Time.deltaTime * speed;
        }

        public override float Apply(CCEffectData theData)
        {
            if (!lightOffsets.ContainsKey(theData.id)) lightOffsets.Add(theData.id,0f);
            float[] result = noise.values(
                 phase.X(theData) + offset.x + phase.Random(theData) + phase.Id(theData) + phase.IdMod(theData) + phase.Group(theData) + phase.GroupMod(theData),
                phase.Y(theData) + offset.y,
                phase.Z(theData) + offset.z
            );
            lightOffsets[theData.id] += result[1] * windSpeed;
            return (result[0] * 2 - 1) * amp.Modulation(theData) * amount;
        }
    }
}