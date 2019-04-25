using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class CCLightPosition : MonoBehaviour
{

    public Vector4 frontPosition = new Vector4();
    public Vector4 backPosition = new Vector4();

    public float length;

    // Start is called before the first frame update
    void Start()
    {
        length = GetComponent<MeshFilter>().sharedMesh.bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        frontPosition = transform.parent.position + transform.right * (length / 2);
        backPosition = transform.parent.position - transform.right * (length / 2);
    }

    private void OnDrawGizmos()
    {

        Gizmos.DrawWireSphere(transform.parent.position, 0.2f);
        Gizmos.DrawWireSphere(frontPosition, 0.2f);
        Gizmos.DrawWireSphere(backPosition, 0.2f);
    }
}
