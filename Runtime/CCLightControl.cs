using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

namespace cc_effects
{
    public class CCLightControl : MonoBehaviour
    {
        public GameObject effectControl;

        public Material lightLedMaterial;

        [ValueDropdown("animations")]
        public int animation = 0;

        // The selectable values for the dropdown, with custom names.
        private ValueDropdownList<int> animations = new ValueDropdownList<int>()
    {
        {"texture", 0 },
        {"angle", 1 },
        {"travel", 2 },
        {"fill",   3 },
        {"sphere", 4 },
        {"plane", 5 },
        {"ring", 6 },
        {"stripe", 7 },
        {"wind", 8 },
        {"bird", 9 },
    };


        [Range(0, 1)]
        public float trail = 0;

        //travel anim
        [Range(-1, 2)]
        public float travelPosition = 0;
        [Range(0, 1)]
        public float travelRange = 1;


        //fill anim
        [Range(-1, 2)]
        public float fillPosition = 0;
        [Range(0, 1)]
        public float fillRange = 1;

        //sphere anim
        public GameObject sphereSphere;
        public float sphereRadius = 1;
        public float sphereGradient = 0;
        public bool sphereDebug = true;

        //plane anim
        public GameObject lightPlane;
        public float planeDistance = 1;
        public float planeGradient = 0;

        //ring anim
        public GameObject ringSphere;
        public float ringRadius = 1;
        public float ringGradient = 0;
        public float ringWeight = 0;
        public bool ringDebug = true;
        [Range(-10, 10)]
        public float ringSpeed = 0.1f;
        public float ringOffset = 0;

        //stripe anim
        public GameObject stripePlane;
        public float stripeDistance = 1;
        public float stripeGradient = 0;
        public float stripeWeight = 0;
        [Range(-10, 10)]
        public float stripeSpeed = 0.1f;
        public float stripeOffset = 0;

        //wind anim
        [Range(-10, 10)]
        public float windScale = 1;
        [Range(0, 1)]
        public float windSmoothMin = 0;
        [Range(0, 1)]
        public float windSmoothMax = 1;


        private CCEffects effects;
        private CCSimplexEffect simlexEffect;

        // Start is called before the first frame update
        void Start()
        {
            effects = effectControl.GetComponent<CCEffects>();
            simlexEffect = effectControl.GetComponent<CCSimplexEffect>();
            var lightEffects = GetComponents<CCEffect>();
            Debug.Log(effectControl.GetComponent<CCEffects>());
            Array.ForEach(lightEffects, effect => effect.effects = effects);
        }

        // Update is called once per frame
        void Update()
        {
            ringOffset += Time.deltaTime * ringSpeed;
            stripeOffset += Time.deltaTime * stripeSpeed;
            var effects = effectControl.GetComponent<CCEffects>();
            var lightEffects = GetComponents<CCEffect>();

            float[] myOffsets = new float[effects._myElements.Count];
            Array.ForEach(lightEffects, effect => effect.effects = effects);
            for (int i = 0; i < effects._myElements.Count; i++)
            {
                GameObject element = effects._myElements[i]; 
                
                CCEffectData myData = element.GetComponent<CCEffectData>();

                Array.ForEach(lightEffects, effect => {
                    effect.Apply(myData);
                });
                
                myOffsets[myData.id] = simlexEffect.lightOffsets[myData.id];
            }

            lightLedMaterial.SetInt("_anim", animation);
            lightLedMaterial.SetFloat("_trail", trail);

            CCTravelEffect myTravelEffect = effects.GetComponent<CCTravelEffect>();
            lightLedMaterial.SetFloat("_travelPos", travelPosition);
            lightLedMaterial.SetFloat("_travelRange", travelRange);

            lightLedMaterial.SetFloat("_fillPos", fillPosition);
            lightLedMaterial.SetFloat("_fillRange", fillRange);

            lightLedMaterial.SetVector("_sphereCenter", sphereSphere.transform.localPosition);
            lightLedMaterial.SetFloat("_sphereRadius", sphereRadius);
            lightLedMaterial.SetFloat("_sphereGradient", sphereGradient);

            lightLedMaterial.SetVector("_planeCenter", lightPlane.transform.localPosition);
            lightLedMaterial.SetVector("_planeNormal", lightPlane.transform.up);
            lightLedMaterial.SetFloat("_planeDist", planeDistance);

            lightLedMaterial.SetFloat("_planeGradient", planeGradient);

            lightLedMaterial.SetVector("_ringCenter", ringSphere.transform.localPosition);
            lightLedMaterial.SetFloat("_ringRadius", ringRadius);
            lightLedMaterial.SetFloat("_ringGradient", ringGradient);
            lightLedMaterial.SetFloat("_ringWeight", ringWeight);
            lightLedMaterial.SetFloat("_ringOffset", ringOffset);


            lightLedMaterial.SetVector("_stripeCenter", stripePlane.transform.localPosition);
            lightLedMaterial.SetVector("_stripeNormal", stripePlane.transform.up);
            lightLedMaterial.SetFloat("_stripeDist", stripeDistance);
            lightLedMaterial.SetFloat("_stripeGradient", stripeGradient);
            lightLedMaterial.SetFloat("_stripeWeight", stripeWeight);
            lightLedMaterial.SetFloat("_stripeOffset", stripeOffset);


            lightLedMaterial.SetFloatArray("_WindOffsets", myOffsets);
            lightLedMaterial.SetFloat("_WindScale", windScale);
            lightLedMaterial.SetFloat("_WindSmoothMin", windSmoothMin);
            lightLedMaterial.SetFloat("_WindSmoothMax", windSmoothMax);
        }

        private void OnDrawGizmos()
        {
           // if (sphereDebug) Gizmos.DrawWireSphere(sphereCenter, sphereRadius);
           // if (ringDebug) Gizmos.DrawWireSphere(ringCenter, ringRadius);
        }
    }
}
