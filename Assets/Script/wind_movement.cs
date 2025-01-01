using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wind_movement : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private
    float
        frequencyUpDownValue = 10.0f,
        frequencyLeftRightValue = 30.0f,
        moveUpDownFactor = 1.1f,
        moveLeftRightFactor = 1.1f,
        initialSpeed = 10.0f,
        minVelocity = 5.0f,
        maxVelocity = 20.0f,
        duration = 20.0f;

    private
    Vector3
        velocity;

    private
    Rigidbody
        rb;

    private
    float
        spawnTime;

    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();
        rb.useGravity = false;
        velocity = new Vector3(0.0f, 0.0f, initialSpeed);
        //Debug.Log("wind apperare");
        //Debug.Log(this.gameObject.transform.position);
        spawnTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        movment_control();
        rb.velocity = velocity;
    }

    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("hit" + collision.gameObject.name);
    }
    private void movment_control()
    {
        // è„â∫ÇÃìÆÇ´Çâ¡Ç¶ÇÈ
        float verticalMovement = Mathf.Sin(Time.time * frequencyUpDownValue) * moveUpDownFactor;
        velocity.y += verticalMovement;

        // ç∂âEÇÃìÆÇ´Çâ¡Ç¶ÇÈ
        float horizontalMovement = Mathf.Sin(Time.time * frequencyLeftRightValue) * moveLeftRightFactor;
        velocity.x += horizontalMovement;

        // ç≈í·ë¨ìxí≤êÆ
        if (velocity.magnitude < minVelocity)
        {
            velocity = velocity.normalized * minVelocity;
        }

        //ç≈çÇë¨ìxí≤êÆ
        if (velocity.magnitude > maxVelocity)
        {
            velocity = velocity.normalized * maxVelocity;
        }

        if(Time.time - spawnTime >= duration)
        {
            Destroy(this.gameObject);
            
        }
    }
}
