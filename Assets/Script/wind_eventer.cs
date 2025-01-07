using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class wind_eventer : MonoBehaviour
{
    [SerializeField]
    private
    int
        windNumPerGenerate = 20;


    [SerializeField]
    private
    float
        generateRange = 15.0f,
        generateInterval = 0.5f,
        eventDelay = 15.0f,
        eventInterval = 30.0f,
        eventIntervalDistribution = 10.0f,
        eventDuration = 5.0f,
        generateDistributionX = 5.0f,
        generateDistributionY = 3.0f,
        generateDistributionZ = 5.0f,
        offsetY = 5.0f;

    [SerializeField]
    private
    GameObject
        wind;

    public
    bool 
        isEvent = false;


    private
    float
        startTime,
        elapsedTime;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(wind_event());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator wind_event()
    {
        while (true)
        {
            int i = 0;
            float angle = UnityEngine.Random.Range(0, 360);
            isEvent = true;
            yield return new WaitForSeconds(eventDelay);
            startTime = Time.time;
            while (elapsedTime - startTime <= eventDuration)
            {
                
                elapsedTime = Time.time;
                for (i = 0; i < windNumPerGenerate; i++)
                {
                    float offsetX = UnityEngine.Random.Range(-generateDistributionX, generateDistributionX);
                    float offsetZ = UnityEngine.Random.Range(-generateDistributionZ, generateDistributionZ);

                    float posX = generateRange * Mathf.Cos(angle * Mathf.Deg2Rad) + offsetX;
                    float posY = offsetY + UnityEngine.Random.Range(-generateDistributionY, generateDistributionY);
                    float posZ = generateRange * Mathf.Sin(angle * Mathf.Deg2Rad) + offsetZ;

                    GameObject windObject = Instantiate(wind, new Vector3(posX, posY, posZ), Quaternion.identity);
                    windObject.GetComponent<wind_movement>().set_vector(new Vector3(-1.0f * posX + offsetX, 0.0f, -1.0f * posZ + offsetZ).normalized);
                }

                yield return new WaitForSeconds(generateInterval);
            }
            isEvent = false;
            yield return new WaitForSeconds(eventInterval - UnityEngine.Random.Range(-1.0f * eventIntervalDistribution, eventIntervalDistribution));
        }
        
    }
}
