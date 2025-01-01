using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ripple_movement : MonoBehaviour
{
    [SerializeField]
    float
        transparent_delay = 0.01f,
        sizeInitial = 0.1f,
        minSizeTarget = 0.8f,
        maxSizeTarget = 1.0f,
        sizingSpeed = 1.0f,
        thickFactor = 0.01f;

   MeshRenderer mesh;

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
        mesh = GetComponent<MeshRenderer>();
        StartCoroutine(transparent());
        objectSizeInitial = new Vector3(sizeInitial, sizeInitial, sizeInitial * thickFactor);
        rand = UnityEngine.Random.Range(minSizeTarget, maxSizeTarget);
        this.gameObject.transform.localScale = objectSizeInitial;
        objectSizeTarget = new Vector3(rand, rand , rand * thickFactor);
    }
    void Update()
    {
        sizing_object();

    }
    private void sizing_object()
    {
        elapsedTime += Time.deltaTime * sizingSpeed;  // 経過時間に基づいてサイズを変化
        float t = Mathf.Clamp01(elapsedTime); // Lerpのための進行度を0から1にクランプ

        this.gameObject.transform.localScale = Vector3.Lerp(objectSizeInitial, objectSizeTarget, t);
        currentSize = this.gameObject.transform.localScale;
    }

    private IEnumerator transparent()
    {
        for (int i = 0; i < 255; i++)
        {
            mesh.material.color = mesh.material.color - new Color32(0, 0, 0, 1);
            yield return new WaitForSeconds(transparent_delay);
        }

        Destroy(this.gameObject);
    }
}
