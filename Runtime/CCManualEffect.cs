using UnityEngine;

using System.Collections.Generic;
namespace cc_effects
{
   // [ExecuteAlways]
    public class CCManualEffect : CCEffect
    {
        [Range(-1,1)]
        public List<float> amounts = new List<float>();

        private void Start()
        {
            amounts.Clear();
            for (int i = 0; i < 20;i++)
            {
                amounts.Add(0);
            }
        }

        public override float Apply(CCEffectData theObject)
        {
            
            return amounts[theObject.id] * amount;
        }
    }
}