using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroy_object : MonoBehaviour
{
    // Start is called before the first frame update

    private
    float
        startTime,
        duration;

    private
    GameObject
        part;
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(duration <= (Time.time - startTime)  || this.gameObject.transform.position.y <= 0)
        {
            if(part != null)
            {
                GameObject particle = Instantiate(part, this.gameObject.transform.position, Quaternion.identity);
                //particle.transform.localScale = this.gameObject.transform.localScale;
                particle.AddComponent<destroy_object>().set_time(20.0f, null);
                Debug.Log("Instanitiate " + particle);
                Destroy(this.gameObject);

            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void set_time(float time, GameObject obj)
    {
        duration = time;
        part = obj;
    }
}
