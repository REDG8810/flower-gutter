using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rain_movement : MonoBehaviour
{
    // Start is called before the first frame update
    private
    float
        sphereHeight,
        spreadSpeed,
        customGravity,
        sphereSize,
        cycleTime,
        sizingTime;

    private
    Vector3
        initialPos,
        targetPos;

    private
    Vector3
        spreadDirection;

    private
    float
        startTime,
        elapsedTime = 0.0f;

    private
    Vector3
        objectSizeInitial,
        objectSizeTarget;
    void Start()
    {
        startTime = Time.time;
        //this.gameObject.transform.localScale = objectSizeInitial;
        if(cycleTime <= 0)
        {
            cycleTime = 8.0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*
        this.transform.position += spreadSpeed * Time.deltaTime * spreadDirection;

        float dis = new Vector2(this.transform.position.x, this.transform.position.z).magnitude;
        float nomalDis = dis / sphereSize;
        float posY = slerp(sphereHeight, -10, nomalDis);


        this.transform.position = new Vector3(this.transform.position.x, posY, this.transform.position.z);
        */

        float normT = (Time.time - startTime) / cycleTime;

        /*
        if (cycleTime <= 0)
        {
            Debug.LogError("cycleTime must be greater than 0.");
            //return;
        }

        Debug.Log($"initialPos: {initialPos}, targetPos: {targetPos}, normT: {normT}");
        */

        this.gameObject.transform.position = Vector3.Slerp(initialPos, targetPos, normT);

        sizing_object();

        if (transform.position.y <= 0)
        {
            Destroy(gameObject);
        }

        
    }

    private float slerp(float a, float b, float t)
    {
        float angle = Mathf.PI * t; // ”¼‰~‚ÌŠp“x
        float slerpT = (1f - Mathf.Cos(angle)) / 2f; // ‰~ŒÊó‚É•âŠÔ
        return Mathf.Lerp(a, b, slerpT);
    }

    public void set_movement(float timeCy, Vector3 initialPositon, Vector3 targetPosition, float timeSi, Vector3 initialSize, Vector3 targetSize)
    {
        cycleTime = timeCy;
        initialPos = initialPositon;
        targetPos = targetPosition;
        sizingTime = timeSi;
        objectSizeInitial = initialSize;
        objectSizeTarget = targetSize;
    }

    /*
    public void set_transform(float time, Vector3 initialSize, Vector3 targetSize)
    {
        sizingTime = time;
        objectSizeInitial = initialSize;
        objectSizeTarget = targetSize;
    }
    */

    private void sizing_object()
    {
        elapsedTime = Time.time - startTime;   

        this.gameObject.transform.localScale = Vector3.Lerp(objectSizeInitial, objectSizeTarget, elapsedTime / sizingTime);
    }


    

}
