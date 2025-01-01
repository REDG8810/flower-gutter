using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rain_controller : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public
    GameObject
        characterA,
        characterB;


    [SerializeField]
    private
    float
        height = 10.0f,
        sphereRadius = 5.0f,
        spawnSizeMin = 0.03f,
        spawnSizeMax = 0.1f,
        spawnRadius = 2.0f,
        spawnInterval = 0.5f,
        cycleTime = 8.0f,
        sizingTime = 1.0f,
        initialSizeFactor = 0.1f;

    [SerializeField]
    private
    int
        characterPerWave = 10,
        characterVarius = 2;

    private 
    float 
        angleStep;


    void Start()
    {
        // 一度に生成するオブジェクトの角度間隔を計算
        angleStep = 360f / characterPerWave;
        // 生成を開始
        StartCoroutine(spawn_character());

        this.transform.localPosition = new Vector3(0, height, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator spawn_character()
    {
        while (true)
        {
            
            for (int i = 0; i < characterPerWave; i++)
            {
                float angle = (i * angleStep * Mathf.Deg2Rad) + Random.Range(-1.0f * (angleStep/2), angleStep/2) * Mathf.Deg2Rad; 
                float rand = Random.Range(1, 100);
                GameObject spawnedObject;

                Vector3 spawnPosition = new Vector3(
                    Mathf.Cos(angle) * spawnRadius,
                    this.gameObject.transform.position.y,
                    Mathf.Sin(angle) * spawnRadius
                );

                // オブジェクトを生成
                if(rand%characterVarius == 0)
                {
                    spawnedObject = Instantiate(characterA, spawnPosition, Random.rotation);
                }
                else
                {
                    spawnedObject = Instantiate(characterB, spawnPosition, Random.rotation);
                }


                float size = Random.Range(spawnSizeMin, spawnSizeMax);
                //spawnedObject.transform.localScale = new Vector3(size, size, size);

                Vector3 initialSize = new Vector3(size * initialSizeFactor, size * initialSizeFactor, size * initialSizeFactor);
                Vector3 targetSize = new Vector3(size, size, size);
                spawnedObject.transform.localScale = initialSize;
                


                Vector3 initialPos = new Vector3(spawnPosition.x, height, spawnPosition.z);
                Vector3 direction = (new Vector3(spawnPosition.x, 0, spawnPosition.z).normalized) * sphereRadius;
                Vector3 targetPos = new Vector3(direction.x, -1.0f * height, direction.z);
                Debug.Log("raincontroller cycleTime" + cycleTime);

                spawnedObject.AddComponent<rain_movement>().set_movement(cycleTime, initialPos, targetPos, sizingTime, initialSize, targetSize);

                Debug.Log("initialSize : " + initialSize + ", targetSize : " + targetSize + ", sizingTime : " + sizingTime);
                //spawnedObject.AddComponent<rain_movement>().set_transform(sizingTime, initialSize,  targetSize);
            }

            // 指定した間隔待機
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
