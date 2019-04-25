using System.Collections.Generic;
using UnityEngine;

namespace cc_effects
{
    public abstract class CCEffect : MonoBehaviour
    {



        [Range(-1, 1)]
        public float amount = 0;

        [HideInInspector]
        public CCEffects effects;
        
        

        public abstract float Apply(CCEffectData theObject);
    }
}