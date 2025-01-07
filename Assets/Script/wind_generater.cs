using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wind_generater : MonoBehaviour
{
    [SerializeField]
    float
        rangeX = 5.0f,
        rangeY = 5.0f,
        rangeZ = 5.0f,
        offSetZ = -5.0f;

    [SerializeField]
    float
        generateTime = 1.0f;



    public
    GameObject
        wind;

    private
    float
        currentTime,
        previousTime;



    void Start()
    {
        currentTime = Time.deltaTime;
        previousTime = Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        generate_wind();
    }

    private void generate_wind()
    {
        float posX = UnityEngine.Random.Range(-rangeX, rangeX);
        float posY = UnityEngine.Random.Range(0, rangeY);
        float posZ = UnityEngine.Random.Range(-rangeZ, rangeZ);
        Vector3 position = new Vector3(posX, posY, posZ + offSetZ);

        currentTime += Time.deltaTime;

        if (currentTime - previousTime >= generateTime)
        { 
            Instantiate(wind, position, Quaternion.identity);
            wind.GetComponent<wind_movement>().set_vector(new Vector3(0, 0, posZ));
            //UnityEngine.Debug.Log("generate");
            previousTime = currentTime;
        }
    }

}
