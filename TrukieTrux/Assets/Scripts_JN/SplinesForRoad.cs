using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SplinesForRoad : MonoBehaviour
{

    public float interpolateAmount;

    public List<Anchor> anchors;
    public List<GameObject> markers;
    public List<GameObject> marker_hands;


    public GameObject splineMark;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    
    void Update()
    {

        for (int i = 0; i < anchors.Count; i++)
        {
            markers[i].SetActive(true);
            markers[i].transform.position = anchors[i].position;

            marker_hands[i*2].SetActive(true);
            marker_hands[i*2 + 1].SetActive(true);
            marker_hands[i*2].transform.position = anchors[i].handleA;
            marker_hands[i*2 + 1].transform.position = anchors[i].handleB;

        }


        interpolateAmount = (interpolateAmount + Time.deltaTime) % 1f;

        splineMark.transform.position = CubicLerp(anchors[0].position, anchors[0].handleA, anchors[1].handleB, anchors[1].position, interpolateAmount);
    }

    Vector3 QuadraticLerp(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        Vector3 ab = Vector3.Lerp(a,b,t);
        Vector3 bc = Vector3.Lerp(b,c,t);
        return Vector3.Lerp(ab,bc,interpolateAmount);

    }

    Vector3 CubicLerp(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
    {
        Vector3 ab_bc = QuadraticLerp(a,b,c,t);
        Vector3 bc_cd = QuadraticLerp(b,c,d,t);
        return Vector3.Lerp(ab_bc, bc_cd, interpolateAmount);
    }

    [Serializable]
    public class Anchor
    {
        public Vector3 position;
        public Vector3 handleA;
        public Vector3 handleB;

    }
}

