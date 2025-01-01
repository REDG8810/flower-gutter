using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class point_topology : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh.SetIndices(meshFilter.mesh.GetIndices(0), MeshTopology.Points, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
