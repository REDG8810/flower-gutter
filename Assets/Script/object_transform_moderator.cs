using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class object_transform_moderator : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public
    float
        sizeInitial = 0.1f,
        minSizeTarget = 0.8f,
        maxSizeTarget = 1.0f,
        sizingSpeed = 1.0f;

    [SerializeField]
    public
    bool
        rotateMoving = false,
        rotateEternal = false;

    [SerializeField]
    public
    float
        rotateDuration = 10.0f,
        rotateSpeedInitial = 0.1f,
        rotateSpeedTarget = 1.0f,
        rotateAccelarate = 0.1f;

    [SerializeField]
    private
    Vector3
        currentSize;

    private
    float
        rand,
        elapsedTime = 0.0f;

    private
    Vector3
        objectSizeInitial,
        objectSizeTarget;

    void Start()
    {
        objectSizeInitial = new Vector3(sizeInitial, sizeInitial, sizeInitial);
        rand = UnityEngine.Random.Range(minSizeTarget, maxSizeTarget);
        this.gameObject.transform.localScale = objectSizeInitial;
        objectSizeTarget = new Vector3(rand, rand, rand);
    }

    // Update is called once per frame
    void Update()
    {
        sizing_object();

        if (rotateMoving)
        {
            if (rotateEternal)
            {
                rotating_object();
            }
            else
            {
                rotating_object(rotateDuration);
            }

        }
    }

    private void sizing_object()
    {
        elapsedTime += Time.deltaTime * sizingSpeed;  // 経過時間に基づいてサイズを変化
        float t = Mathf.Clamp01(elapsedTime); // Lerpのための進行度を0から1にクランプ

        this.gameObject.transform.localScale = Vector3.Lerp(objectSizeInitial, objectSizeTarget, t);
        currentSize = this.gameObject.transform.localScale;
    }

    private void rotating_object(float duration)
    {
        float time = Time.deltaTime;
        float rotateSpeed = rotateSpeedInitial + rotateAccelarate * time;  

        if(time <= duration)
        {
            if(rotateSpeed >= rotateSpeedTarget)
            {
                rotateSpeed = rotateSpeedTarget;
            }
            this.gameObject.transform.rotation = Quaternion.Euler(0.0f, rotateSpeed, 0.0f);
        }
    }

    private void rotating_object()
    {
        float time = Time.deltaTime;
        float rotateSpeed = rotateSpeedInitial + rotateAccelarate * time;
      
        if (rotateSpeed >= rotateSpeedTarget)
        {
            rotateSpeed = rotateSpeedTarget;
        }

        this.gameObject.transform.rotation = Quaternion.Euler(0.0f, rotateSpeed, 0.0f);
       
    }

   
}
