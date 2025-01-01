using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class splint_dissppearer : MonoBehaviour
{
    // Start is called before the first frame update
    private
    float
        gravity = 0.005f;

    private
    GameObject
        ripple;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(new Vector3(0.0f, -1.0f * gravity, 0.0f) * rb.mass, ForceMode.Force);
        if(this.gameObject.transform.position.y <= 0)
        {
            //Vector3 rippleAngle = new Vector3(-90.0f, 0.0f, 0.0f);
            //Instantiate(ripple, this.gameObject.transform.position, Quaternion.Euler(rippleAngle));
            Destroy(this.gameObject);
        }
    }
}
