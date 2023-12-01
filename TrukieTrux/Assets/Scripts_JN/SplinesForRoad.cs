using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class SplinesForRoad : MonoBehaviour
{

    public float interpolateAmount;

    public List<Anchor> anchors;
    //public List<GameObject> markers;
    //public List<GameObject> marker_hands;

    public GameObject road;

    [Header("Track Design")]
    [Range(1,25)]
    public int difficulty;
    public int length;
    [Range(0.00f,1.00f)]
    public float straightAwayChance = 0.1f;
    public float roadWidth = 7f;

    public GameObject splineMark;

    public GameObject cube;
    public GameObject sphere;

    [Header("NPC Cars")]
    public List<GameObject> npcCars;
    public Vector3 spawnPos;
    public float spawnRate;
    public float zigzagChance;
    float lastSpawn;

    [Header("Scale Gen")]
    public GameObject weighInPrefab;
    public Vector3 spawnDisplacement;
    public float whereOnRoad = 0.5f;
    public float spawnChance = 0.66f;

    [Header("End Gen")]
    public GameObject depotAtEnd;
    public Vector3 depotDisplacement;

    [Header("Obstacles")]
    public GameObject rightLight;
    public GameObject leftLight;
    public GameObject depotSign;

    public float lightSpawnRate = 0.01f;

    int current = 1;

    Vector3[] newVertices;
    Vector2[] newUV;
    int[] newTriangles;

    List<Vector3> dumdumList;

    // Start is called before the first frame update
    void Start()
    {

        if (StaticStats.run != null)
            ExtractRunData();

        anchors.Clear();
        CreateRoad();


        Mesh mesh = new Mesh();
        mesh.vertices = newVertices;
        //mesh.uv = newUV;
        mesh.triangles = newTriangles;

        road.GetComponent<MeshFilter>().mesh = mesh;
        mesh.normals = UpNormals(mesh);

        TerrainCollider mc = road.AddComponent<TerrainCollider>();
        //mc.sharedMesh = mesh;

        
        
    }

    void ExtractRunData()
    {
        Run run = StaticStats.run;

        length = run.length;
        difficulty = run.difficulty;
    }




    
    void FixedUpdate()
    {

        /*
        for (int i = 0; i < anchors.Count; i++)
        {
            markers[i].SetActive(true);
            markers[i].transform.position = anchors[i].position;

            marker_hands[i*2].SetActive(true);
            marker_hands[i*2 + 1].SetActive(true);
            marker_hands[i*2].transform.position = anchors[i].handleA;
            marker_hands[i*2 + 1].transform.position = anchors[i].handleB;

        }
        */
        /*
        interpolateAmount = (interpolateAmount + Time.deltaTime) % 1f;

        splineMark.transform.position = CubicLerp(anchors[current-1].position, anchors[current-1].handleA, anchors[current].handleB, anchors[current].position, interpolateAmount);
        
        if (interpolateAmount > 0.98)
        {
            current++;
            
        }
        //Debug.Log($"current: {current}");
        if (current >= anchors.Count)
            current = 1;
        */

        if (Time.time > lastSpawn + spawnRate + UnityEngine.Random.Range(-1f, 4.5f))
        {
            GameObject car = Instantiate(npcCars[UnityEngine.Random.Range(0, npcCars.Count)], spawnPos, Quaternion.identity);
            car.GetComponent<NPC_Car>().Initiate(this);
            car.GetComponent<NPC_Car>().moveForce += UnityEngine.Random.Range(-35f, 65f);
            car.GetComponent<NPC_Car>().carDist += UnityEngine.Random.Range(-4f, 4f);

            if (UnityEngine.Random.Range(0.00f, 1.00f) <= zigzagChance)
            {
                car.GetComponent<NPC_Car>().isZigZagger = true;
            }

            lastSpawn = Time.time;
        }


    }

    Vector3 QuadraticLerp(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        Vector3 ab = Vector3.Lerp(a,b,t);
        Vector3 bc = Vector3.Lerp(b,c,t);
        return Vector3.Lerp(ab,bc,t);

    }

    Vector3 CubicLerp(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
    {
        Vector3 ab_bc = QuadraticLerp(a,b,c,t);
        Vector3 bc_cd = QuadraticLerp(b,c,d,t);
        return Vector3.Lerp(ab_bc, bc_cd, t);
    }


    void CreateRoad()
    {
        
        int nodes = difficulty * 2 + 1;
        float interDist = length * (nodes*0.5f) / (0.6f + difficulty * difficulty/4f);
        


        for (int i = 0; i < nodes; i++)
        {
            Vector3 anchor_position = new Vector3(0f,0f,0f);
            Vector3 a_position = new Vector3(0f,0f,0f);
            Vector3 b_position = new Vector3(0f,0f,0f);

            float squig1 = UnityEngine.Random.Range(-1.5f * difficulty/2f, 1.5f * difficulty/2f);
            float squig2 = UnityEngine.Random.Range(-1.5f * difficulty/2f - (1.3f* difficulty), 1.5f * difficulty/2f + (1.3f* difficulty));
            
            float r = UnityEngine.Random.Range(0.00f,1.00f);

            float distanceModifier = 0f;

            float chanceMod = (straightAwayChance * difficulty/16f);

            if (r < chanceMod)
            {
                squig2 = UnityEngine.Random.Range(-1.5f * difficulty/4f, 1.5f * difficulty/4f);
                distanceModifier = difficulty*3f + interDist / 2f;

                //Debug.Log($"{i} = true, {r}, {straightAwayChance}");
            }
            else if (r > 0.989f)
            {
                squig2 = UnityEngine.Random.Range(interDist * 2f, interDist * 2f);
                distanceModifier = difficulty*2f + interDist;

                Debug.Log($"CRAZY LINE");
            }




            Anchor new_anc = null;

            if (i == 0)
            {
                a_position = new Vector3(interDist/2.3f,0f,squig1);
                //a_position = new Vector3(interDist/2f,0f,0f);

                new_anc = new Anchor(anchor_position, a_position, b_position);
                //Instantiate(sphere, anchor_position, Quaternion.identity);
            }
            else
            {
                
                anchor_position = new Vector3(anchors[i-1].position.x + interDist + distanceModifier, 0f, anchors[i-1].handleA.z + squig2);
                
                b_position = QuadraticLerp(anchor_position, anchors[i-1].handleA, anchors[i-1].position, 0.3f);

                Vector3 modo = anchor_position - b_position;
                
                modo = Vector3.Normalize(modo);

                a_position = anchor_position + (modo * interDist/2.3f);


                new_anc = new Anchor(anchor_position, a_position, b_position);

                //Instantiate(sphere, anchor_position, Quaternion.identity);
                //Instantiate(cube, a_position, Quaternion.identity);
                //Instantiate(cube, b_position, Quaternion.identity);
            }
            /*{
                //b_position = anchors[i-1].handleA;    
                b_position = new Vector3(anchors[i-1].position.x + interDist/1.2f, 0f, anchors[i-1].handleA.z + squig1 / 5f);  

                Vector3 nextP = new Vector3(anchors[i-1].position.x + interDist + squig2, 0f, anchors[i-1].handleA.z + squig2 - (anchor_position.z - b_position.z)/ interDist);

                anchor_position = nextP;    

                Vector3 nextA = new Vector3(nextP.x + interDist/3f, 0f, nextP.z + squig1 + (anchor_position.z - b_position.z)/ interDist);

                a_position = nextA;

                new_anc = new Anchor(anchor_position, a_position, b_position);

                Instantiate(sphere, anchor_position, Quaternion.identity);
                Instantiate(cube, a_position, Quaternion.identity);
                Instantiate(cube, b_position, Quaternion.identity);


            }
            */


            anchors.Add(new_anc);

            
        }

        anchors.Add(LongStraightAwayAtEnd(anchors[anchors.Count-1], length * (difficulty/4.5f)));

        PlaceVertices();
    }

    Anchor LongStraightAwayAtEnd(Anchor lastAnchor, float leng)
    {
        Vector2 direction_prev_to_next = new Vector2(0f,0f);

        direction_prev_to_next = new Vector2(lastAnchor.handleA.x, lastAnchor.handleA.z) - new Vector2(lastAnchor.position.x, lastAnchor.position.z);

        direction_prev_to_next.Normalize();


        Vector3 newPos = new Vector3(lastAnchor.position.x + direction_prev_to_next.x * leng, 0f, lastAnchor.position.z + direction_prev_to_next.y * leng);
        Vector3 newPosForB = new Vector3(lastAnchor.position.x + direction_prev_to_next.x * leng/2f, 0f, lastAnchor.position.z + direction_prev_to_next.y * leng/2f);
        Vector3 newPosForA = new Vector3(lastAnchor.position.x + direction_prev_to_next.x * leng * 1.1f, 0f, lastAnchor.position.z + direction_prev_to_next.y * leng * 1.1f);
        Anchor newAnc = new Anchor(newPos, newPosForA, newPosForB);

        return newAnc;
    }


    void PlaceVertices()
    {
        dumdumList = new List<Vector3>();


        int vertCounter = 0;
        
        for (int a = 1; a < anchors.Count; a++)
        {
            
            if (a == Mathf.FloorToInt(anchors.Count/2f))
            {
                SpawnWeighIn(anchors[a]);
            }

            else if (a == anchors.Count-1)
            {
                SpawnEnd(anchors[a]);
            }

            if (a == anchors.Count -1)
            {
                for (float p = 0.00f; p < 1.00f; p += 0.01f)
                {
                    //newVertices[vertCounter] = CubicLerp(anchors[current-1].position, anchors[current-1].handleA, anchors[current].handleB, anchors[current].position, interpolateAmount);
                    //dummyVertices[vertCounter] = CubicLerp(anchors[a-1].position, anchors[a-1].handleA, anchors[a].handleB, anchors[a].position, p);
                
                    dumdumList.Add(CubicLerp(anchors[a-1].position, anchors[a-1].handleA, anchors[a].handleB, anchors[a].position, p));

                    //Debug.Log($"vc {vertCounter}");

                    //Vector3 debugPos = new Vector3(newVertices[vertCounter].x,newVertices[vertCounter].y + 10f,newVertices[vertCounter].z);
                    //Instantiate(cube, debugPos, Quaternion.identity);

                    vertCounter++;

                }
            }
            else
            {
                for (float p = 0.00f; p < 1.00f; p += 0.01f) // was incrementing by 0.02
                {
                    //newVertices[vertCounter] = CubicLerp(anchors[current-1].position, anchors[current-1].handleA, anchors[current].handleB, anchors[current].position, interpolateAmount);
                    //dummyVertices[vertCounter] = CubicLerp(anchors[a-1].position, anchors[a-1].handleA, anchors[a].handleB, anchors[a].position, p);
                
                    dumdumList.Add(CubicLerp(anchors[a-1].position, anchors[a-1].handleA, anchors[a].handleB, anchors[a].position, p));

                    //Debug.Log($"vc {vertCounter}");

                    //Vector3 debugPos = new Vector3(newVertices[vertCounter].x,newVertices[vertCounter].y + 10f,newVertices[vertCounter].z);
                    //Instantiate(cube, debugPos, Quaternion.identity);

                    vertCounter++;

                }
            }


        }

        Vector3[] dummyVertices = new Vector3[(dumdumList.Count)];   
        
        for (int i = 0; i < dumdumList.Count; i++)
        {
            dummyVertices[i] = dumdumList[i];
        }


        newVertices = new Vector3[(dummyVertices.Length*2)];    


        for (int v = 0; v < dummyVertices.Length; v++)
        {
            //newVertices[v * 2] = dummyVertices[v];

            if (v == 0)
            {
                
                newVertices[0] = new Vector3(dummyVertices[v].x, 0f, dummyVertices[v].z + roadWidth);
                newVertices[1] = new Vector3(dummyVertices[v].x, 0f, dummyVertices[v].z - roadWidth);

            }
            else 
            {

                Vector2 direction_prev_to_next = new Vector2(0f,0f);

                //Vector2 direction_prev_to_next = dummyVertices[v+1] - dummyVertices[v-1];

                if (v < dummyVertices.Length-1)
                {
                    direction_prev_to_next = new Vector2(dummyVertices[v+1].x, dummyVertices[v+1].z) - new Vector2(dummyVertices[v-1].x, dummyVertices[v-1].z);
                }
                else
                {
                    direction_prev_to_next = new Vector2(dummyVertices[v].x, dummyVertices[v].z) - new Vector2(dummyVertices[v-1].x, dummyVertices[v-1].z);
                }

                direction_prev_to_next.Normalize();

                Vector2 perpDir = Vector2.Perpendicular(direction_prev_to_next);



                newVertices[v * 2] = new Vector3(dummyVertices[v].x + perpDir.x * roadWidth, 0f, dummyVertices[v].z + perpDir.y * roadWidth);
                newVertices[v * 2 + 1] = new Vector3(dummyVertices[v].x - perpDir.x * roadWidth, 0f, dummyVertices[v].z - perpDir.y * roadWidth);


                if (UnityEngine.Random.Range(0.00f, 1.00f) < lightSpawnRate)
                {
                    if (UnityEngine.Random.Range(0, 2) == 0)
                    {
                        SpawnLight(true, newVertices[v * 2]);

                    }
                    else
                    {
                        SpawnLight(false, newVertices[v * 2 + 1]);
                    }

                }
                else if (UnityEngine.Random.Range(0.000f, 1.000f) < lightSpawnRate / 10f)
                {
                    if (UnityEngine.Random.Range(0, 2) == 0)
                    {
                        SpawnObstacle(true, newVertices[v * 2]);

                    }
                    else
                    {
                        SpawnObstacle(false, newVertices[v * 2 + 1]);
                    }
                }
            }


        }

        /*
        for (int tester = 0; tester < newVertices.Length; tester++)
        {
            float yMod = tester * 0.1f;
            Vector3 debugPos = new Vector3(newVertices[tester].x,newVertices[tester].y + yMod,newVertices[tester].z);
            Instantiate(cube, debugPos, Quaternion.identity);

        }

        */

        
        newTriangles = new int[(newVertices.Length)*3]; 

        for (int t = 0; t < newVertices.Length-2; t++)
        {
            if (t % 2 == 0)
            {
                newTriangles[t * 3] = t;    
                newTriangles[t * 3 + 1] = t + 2;    
                newTriangles[t * 3 + 2] = t + 1;    
            }
            else
            {
                newTriangles[t * 3] = t;    
                newTriangles[t * 3 + 1] = t + 1;    
                newTriangles[t * 3 + 2] = t + 2; 
            }


            //Debug.Log($"triangle {t}");
        }
        
        //newTriangles = MakeTriangles(newVertices.Length);


        newUV = new Vector2[newVertices.Length];

        for (int u = 0; u < newVertices.Length; u++)
        {
            newUV[u] = new Vector2(0f,0f);



        }

        
        
        
    }

    public List<Vector3> GetMiddles()
    {
        return dumdumList;
    }

    Vector3[] UpNormals(Mesh mesh)
    {
        
        Vector3[] normals = mesh.normals;

        for (int i = 0; i < normals.Length; i++)    
        {
            normals[i] = Vector3.up;
        }

        return normals;

    }

    void SpawnWeighIn(Anchor nearAnchor)
    {
        Vector3 roadCent = nearAnchor.position;
        
        Vector3 spawnPosition = roadCent + spawnDisplacement;
        if (UnityEngine.Random.Range(0.00f,1.00f) <= spawnChance)
        {
            GameObject newWeighIn = Instantiate(weighInPrefab, spawnPosition, Quaternion.identity);

        }
    }

    void SpawnEnd(Anchor nearAnchor)
    {
        Vector3 roadCent = nearAnchor.position;

        Vector3 spawnPosition = roadCent + depotDisplacement;

        GameObject newEnd = Instantiate(depotAtEnd, spawnPosition, Quaternion.identity);

        
    }

    void SpawnLight(bool isRight, Vector3 position)
    {

        if (isRight)
        {
            GameObject light = Instantiate(rightLight, position, Quaternion.identity);
            light.transform.Rotate(0f, 90f, 0f);
        }
        else
        {
            GameObject light = Instantiate(rightLight, position, Quaternion.identity);
            light.transform.Rotate(0f, -90f, 0f);
        }
        
    }

    void SpawnObstacle(bool isRight, Vector3 position)
    {
        if (isRight)
        {
            GameObject light = Instantiate(depotSign, position, Quaternion.identity);
            //light.transform.Rotate(0f, 90f, 0f);

            light.transform.Translate(0f,0f, 3f);

        }
        else
        {
            GameObject light = Instantiate(depotSign, position, Quaternion.identity);
            //light.transform.Rotate(0f, -90f, 0f);
            light.transform.Translate(0f,0f, -3f);
        }
    }


    public List<Vector3> PositionsForNPC()
    {
        List<Vector3> positionsToDish = new List<Vector3>();

        for (int i = 0; i < anchors.Count; i++)
        {
            if (i == 0)
            {
                positionsToDish.Add(anchors[0].position);
               // positionsToDish.Add(anchors[0].handleA);
            }
            else if (i < anchors.Count-1)
            {
                //positionsToDish.Add(anchors[i].handleB);
                positionsToDish.Add(anchors[i].position);
                //positionsToDish.Add(anchors[i].handleA);
            }
            else
            {
                //positionsToDish.Add(anchors[i].handleB);
                positionsToDish.Add(anchors[i].position);
            }
        }

        return positionsToDish;

    }


    [Serializable]
    public class Anchor
    {
        public Vector3 position;
        public Vector3 handleA;
        public Vector3 handleB;

        public Anchor(Vector3 pos, Vector3 handA, Vector3 handB)
        {
            this.position = pos;
            this.handleA = handA;
            this.handleB = handB;
        }
    }
}

