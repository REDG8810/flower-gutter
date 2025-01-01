using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drop_movement : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private
    float
        gravity = 4.9f,
        boundForce = 4.9f,
        boundFactor = 0.5f,
        boundSizeFactor = 0.8f,
        splashIntervalMin = 0.5f,
        splashIntervalMax = 1.0f;


    [SerializeField]
    private
    int
        dropTime = 3;

    public
    Vector3 splashPos;

    public
    Vector3
        initialSize;

    public
    int
        dropIndex;

    public
    int
        flowerVarius = 3;

    [SerializeField]
    private
    GameObject
        drop,
        ripple,
        chainFlowerA,
        chainFlowerB,
        chainFlowerC,
        splashObject;
    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = false;
        float randInterval = Random.Range(splashIntervalMin, splashIntervalMax);
        if (dropIndex == 0)
        {
            StartCoroutine(start_splash(randInterval));
        }
        else
        {
            rb.AddForce(new Vector3(0.0f, boundForce * Mathf.Pow(boundFactor, dropIndex), 0.0f) * rb.mass, ForceMode.Impulse);
        }

    }

    private IEnumerator start_splash(float interval)
    {
        yield return new WaitForSeconds(interval);
        generate_ripple();
        splashPos = this.gameObject.transform.position;
        drop_bound();
        Destroy(this.gameObject);
    }

    private void drop_bound()
    {
        if(dropIndex < dropTime)
        {
            GameObject nextDrop = Instantiate(drop, this.gameObject.transform.position, this.gameObject.transform.rotation);
            nextDrop.transform.localScale = this.gameObject.transform.localScale * Mathf.Pow(boundSizeFactor, dropIndex);
            nextDrop.GetComponent<drop_movement>().dropIndex = dropIndex + 1;
            nextDrop.GetComponent<drop_movement>().splashPos = splashPos;
        }
        else
        {
            GameObject newDrop = Instantiate(drop, this.gameObject.transform.position, this.gameObject.transform.rotation);
            newDrop.transform.localScale = initialSize;
            newDrop.GetComponent<drop_movement>().dropIndex = 0;
        }
    }

    private void generate_ripple()
    {
        Instantiate(ripple, this.gameObject.transform.position, this.gameObject.transform.rotation);
    }
    // Update is called once per frame
    void FixedUpdate()
    { 
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(new Vector3(0.0f, -1.0f * gravity, 0.0f) * rb.mass, ForceMode.Force);
        if(this.transform.position.y <= 0)
        {
            GameObject spOb = Instantiate(splashObject, this.gameObject.transform.position, Quaternion.identity);
            spOb.AddComponent<destroy_object>().set_time(5.0f, null);
            generate_ripple();
            //int rand = Random.Range(0, 100);
            if (flowerVarius == 1)
            {
                Instantiate(chainFlowerA, this.gameObject.transform.position, Quaternion.identity);
            }
            else if(flowerVarius == 2)
            {
                Instantiate(chainFlowerB, this.gameObject.transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(chainFlowerC, this.gameObject.transform.position, Quaternion.identity);
            }


            
            Destroy(this.gameObject);
        }

        if(dropIndex != 0)
        {
            if(this.gameObject.transform.position.y < splashPos.y)
            {
                drop_bound();
                generate_ripple();
                Destroy(this.gameObject);
            } 
        }
    }
}
