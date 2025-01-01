using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flower_generater : MonoBehaviour
{
    // Start is called before the first frame update
    

    [SerializeField]
    int
        rangeX = 5,
        rangeZ = 5,
        posYp = 0;

    [SerializeField]
    float
        generateTime = 5.0f;



    public
    GameObject 
        chain_flower;

    private
    float
        currentTime,
        previousTime;
        


    void Start()
    {
        currentTime = Time.deltaTime;
        previousTime = Time.deltaTime;
        Instantiate(chain_flower, new Vector3(0,0,0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        generate_flower();
    }

    private void generate_flower()
    {
        float posX = UnityEngine.Random.Range(-rangeX, rangeX);
        float posZ = UnityEngine.Random.Range(-rangeZ, rangeZ);
        Vector3 position = new Vector3(posX, posYp, posZ);

        currentTime += Time.deltaTime;

        if (currentTime-previousTime >= generateTime)
        {
            //çΩâ‘ê∂ê¨
            Instantiate(chain_flower, position, Quaternion.identity);
            //UnityEngine.Debug.Log("generate");
            previousTime = currentTime;
        }
    }
}
