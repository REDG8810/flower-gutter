using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drop_generater : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private
    float
        dropSize = 0.01f,
        generateHeight = 1.0f,
        generateRangeX = 5.0f,
        generateRangeZ = 5.0f,
        generateInterval = 1.0f;

    [SerializeField]
    private
    GameObject
        dropObjectA,
        dropObjectB,
        dropObjectC;

    private
    int
        varius = 3;
    void Start()
    {
        StartCoroutine(generate_rain());    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator generate_rain()
    {
        GameObject drop;
        while (true)
        {
            float posX = Random.Range(-1.0f * generateRangeX, generateRangeX);
            float posZ = Random.Range(-1.0f * generateRangeZ, generateRangeZ);
            Vector3 generatePosition = new Vector3(posX, generateHeight, posZ);
            Vector3 dropSizeInitial = new Vector3(dropSize, dropSize, dropSize);
            Vector3 dropAngle = new Vector3(-90.0f, 0.0f, 0.0f);

            int rand = Random.Range(1, varius + 1);
            if(rand == 1)
            { 
                drop = Instantiate(dropObjectA, generatePosition, Quaternion.Euler(dropAngle));
                drop.GetComponent<drop_movement>().flowerVarius = 1;
            }
            else if(rand == 2)
            {
                drop = Instantiate(dropObjectB, generatePosition, Quaternion.Euler(dropAngle));
                drop.GetComponent<drop_movement>().flowerVarius = 2;
            }
            else
            {
                drop = Instantiate(dropObjectC, generatePosition, Quaternion.Euler(dropAngle));
                drop.GetComponent<drop_movement>().flowerVarius = 3;
            }




            //GameObject drop = Instantiate(dropObject, generatePosition, Quaternion.Euler(dropAngle));
            drop.transform.localScale = dropSizeInitial;
            drop.GetComponent<drop_movement>().dropIndex = 0;
            drop.GetComponent<drop_movement>().initialSize = dropSizeInitial;

            yield return new WaitForSeconds(generateInterval);
        }
        
    }
}
