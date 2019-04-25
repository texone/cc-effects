using System.Collections;
using System.Collections.Generic;
using System;
using cc_effects;
using UnityEngine;
using UnityEditor;

public class CCEffects : MonoBehaviour
{

    public Bounds bounds;

    public GameObject effectorParent;

    public GameObject distancePlane;

    public GameObject distanceSphere;

    public CustomRenderTexture lightTexture;

    public CustomRenderTexture lightPositionTexture;

    public Material ledMaterial;

    public float amount = 35;

    [Range(0,360)]
    public float baseRotation = 180;
    [Range(-30, 30)]
    public float elementAddRotation = 0;
    [Range(0,1)]
    public float baseRotationBlend = 0;
    [Range(0, 1)]
    public float addBaseColor = 0;

    public bool useSphere = true;
    
    public List<GameObject> _myElements = new List<GameObject>();

    public Dictionary<int,float> _myYRotation = new Dictionary<int, float>();

    private void SetUpdateZones(CustomRenderTexture theTexture, List<GameObject> theElements)
    {

        CustomRenderTextureUpdateZone[] updateZones = new CustomRenderTextureUpdateZone[theElements.Count];

        theElements.ForEach(e =>
        {
            CCEffectData myData = e.GetComponent<CCEffectData>();
            updateZones[myData.id] = new CustomRenderTextureUpdateZone
            {
                updateZoneSize = new Vector3(1, 1f / theElements.Count),
                updateZoneCenter = new Vector3(0.5f, 1 - (1f / theElements.Count * myData.id + 0.5f / theElements.Count))
            };
        });

        theTexture.SetUpdateZones(updateZones);
        theTexture.Update();
    }

    private void InitBounds(Transform theEffectedNode)
    {
        if (bounds.center.Equals(new Vector3()))
        {
            bounds = new Bounds(theEffectedNode.localPosition, new Vector3());
        }
        bounds.Encapsulate(theEffectedNode.localPosition);
    }

    private Transform InitEffectNode(Transform t)
    {
        if (t.name.StartsWith("effector"))
        {
            return t;
        }
        var myEffectedNode = new GameObject("effector " + t.name).transform;
        myEffectedNode.SetParent(t.parent, false);
        t.SetParent(myEffectedNode, false);

        myEffectedNode.localEulerAngles = t.localEulerAngles;
        myEffectedNode.localPosition = t.localPosition;
        myEffectedNode.localScale = t.localScale;
        return myEffectedNode;
    }

    private List<Transform> InitEffectNodes(List<Transform> theTransforms)
    {
        var myResult = new List<Transform>();
        theTransforms.ForEach(t => {
            var myEffectedNode = InitEffectNode(t);
            _myYRotation.Add(t.GetInstanceID(), myEffectedNode.localEulerAngles.y);
            InitBounds(myEffectedNode);
            myResult.Add(myEffectedNode.GetChild(0).transform);
        });
        return myResult;
    }

    private void InitEffectData(List<Transform> theEffectNodes)
    {
        var effectDatas = new List<CCEffectData>();

        theEffectNodes.ForEach(t =>
        {
            t.localEulerAngles = new Vector3();
            t.localPosition = new Vector3();
            t.localScale = new Vector3(1, 1, 1);

            var myData = t.gameObject.GetComponent<CCEffectData>();
            myData.xBlend = (t.parent.localPosition.x - bounds.min.x) / bounds.size.x * 2 - 1;
            myData.yBlend = (t.parent.localPosition.y - bounds.min.y) / bounds.size.y * 2 - 1;
            myData.zBlend = (t.parent.localPosition.z - bounds.min.z) / bounds.size.z * 2 - 1;

            effectDatas.Add(myData);

            var renderer = t.gameObject.GetComponent<MeshRenderer>();
            renderer.material.SetFloat("_ID", (myData.id + 0.5f) / 20f);
        });

        int maxID = 0;
        int maxGroup = 0;
        Dictionary<int, int> maxGroupIDs = new Dictionary<int, int>();

        effectDatas.ForEach(e =>
        {
            maxID = Math.Max(maxID, e.id);
            maxGroup = Math.Max(maxGroup, e.group);

            if (!maxGroupIDs.ContainsKey(e.group))
            {
                maxGroupIDs.Add(e.group, e.groupID);
            }
            maxGroupIDs[e.group] = Math.Max(maxGroupIDs[e.group], e.groupID);
        });

        effectDatas.ForEach(e =>
        {
            e.idBlend = e.id / (float)maxID * 2 - 1;
            e.groupBlend = e.group / (float)maxGroup * 2 - 1;
            e.groupIDBlend = e.groupID / (float)maxGroupIDs[e.group] * 2 - 1;
        });
    }

    // Start is called before the first frame update
    private void Start()
    {
        Debug.Log("Effects start");
        _myElements.Clear();

        var childCount = effectorParent.transform.childCount;

        var myTubes = new List<Transform>();

        for (var i = 0; i < childCount;i++)
        {
            if (effectorParent.transform.GetChild(i).name.Equals("LED_Tube_waage")) continue;
            myTubes.Add(effectorParent.transform.GetChild(i));
        }

        var myEffectsTransforms = InitEffectNodes(myTubes);

        InitEffectData(myEffectsTransforms);
        myEffectsTransforms.ForEach(t =>  _myElements.Add(t.gameObject));

        SetUpdateZones(lightTexture, _myElements);
        SetUpdateZones(lightPositionTexture, _myElements);
    }

    // Update is called once per frame
    private void Update()
    {
        
        var effects = GetComponents<CCEffect>();
        Array.ForEach( effects, effect => effect.effects = this);
        var myAngles = new float[20];
        var myFrontPositions = new Vector4[20];
        var myBackPositions = new Vector4[20];
        
        Plane myPlane = new Plane(distancePlane.transform.up, distancePlane.transform.localPosition);
       
        _myElements.ForEach(element =>
        {

        });

        _myElements.ForEach(element =>
        {

            if (useSphere)
            {
                float x = distanceSphere.transform.position.x - element.transform.parent.position.x;
                float y = distanceSphere.transform.position.z - element.transform.parent.position.z;
                float angle = Mathf.Atan2(-y, x) * Mathf.Rad2Deg;
                // Debug.Log(angle);
                element.transform.parent.localEulerAngles = new Vector3(0, angle, 0);
            }
            else
            {
                var myPlaneDist = myPlane.GetDistanceToPoint(element.transform.parent.localPosition);
                var myBaseRotation = myPlaneDist * elementAddRotation + baseRotation;
                element.transform.parent.localEulerAngles = new Vector3(0, myBaseRotation, 0);
            }

            float myAngle = 0;
            CCEffectData myData = element.GetComponent<CCEffectData>();

            Array.ForEach(effects, effect => {
                float myEffectAngle = effect.Apply(myData);
                if (float.IsNaN(myEffectAngle))
                {
                    myEffectAngle = 0;
                }
                myAngle += myEffectAngle;
            });
            myAngles[myData.id] = myAngle;

            var localAngles = element.transform.localEulerAngles;
            myAngle *= amount;
            myData.angle = myAngle * Mathf.Deg2Rad;
            element.transform.localEulerAngles = new Vector3(localAngles.x, localAngles.y, myAngle);

            var renderer = element.GetComponent<MeshRenderer>();
            renderer.material.SetFloat("_addBaseColor", addBaseColor);

            CCLightPosition myLightPosition = element.GetComponent<CCLightPosition>();
            myFrontPositions[myData.id] = myLightPosition.frontPosition;
            myBackPositions[myData.id] = myLightPosition.backPosition;
        });

        lightPositionTexture.material.SetVectorArray("_FrontPositions", myFrontPositions);
        lightPositionTexture.material.SetVectorArray("_BackPositions", myBackPositions);

        lightTexture.material.SetFloatArray("_Angles", myAngles);
    }
    
    void OnDrawGizmosSelected()
    {
        // Draw a yellow cube at the transform position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(effectorParent.transform.position + bounds.center, bounds.size);
    }

    
}

