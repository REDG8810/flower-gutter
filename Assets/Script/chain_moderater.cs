using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chain_moderater : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public
    string
        targetTag = "destroyer";


    [SerializeField]
    public
    float
        scatteringFactor = 1.0f,
        gravityFactor = 0.2f,
        minMaginitude = 0.1f,
        indexDelay = 0.05f,
        splinterLifeTime = 3.0f,
        chainLifeTime = 6.0f;

    [SerializeField]
    public
    int
        splinterIndexMin = 5,
        splinterIndexMid = 10,
        splinterIndexMax = 15;


    [SerializeField]
    private
    GameObject
        sprinterObjectSmall,
        sprinterObjectMidium,
        sprinterObjectBig,
        particle;

    private
    GameObject
        splinterObject;

    public
    int
        myIndex;

    public
    Vector3
        hitPosition;

    public
    chain_flower
        parent;

    public
    bool
        collapse = false;

    private
    Rigidbody
        rb;
    void Start()
    {
        myIndex = get_self_index();

        rb = this.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void set_parent(chain_flower parentObject)
    {
        parent = parentObject;
    }
    private int get_self_index()
    {
        int index = parent.chainList.IndexOf(gameObject);
        return index;
    }

    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("hit chain!!");
        // contacts配列から最初の接触点を取得
        if (collision.gameObject.CompareTag(targetTag)) {

            if (collision.contacts.Length > 0)
            {
                hitPosition = collision.contacts[0].point; // 最初の接触点の座標を取得
                //Debug.Log("Collision at position: " + hitPosition);

                collapse = true;
            }

        }
    }

    public void collapse_object(int collapseIndex, GameObject obj)
    {
        int indexDistance = Mathf.Abs(collapseIndex - myIndex);

        Vector3 vectorNormalized = get_vector(hitPosition, this.transform.position);
        float vectorDistance = get_distance(hitPosition, this.transform.position);

        //衝突したオブジェクトとのインデックスの差で破片の細かさを変更
        
        //if (collapseIndex <=  myIndex)
        //{
        //    splinterObject = Instantiate(sprinterObjectSmall, this.transform.position, this.transform.rotation);
        //    splinterObject.transform.SetParent(parent.transform);
        //    splinterObject.transform.localScale = this.transform.localScale;
        //}
        if (indexDistance <= splinterIndexMin)
        {
            splinterObject = Instantiate(sprinterObjectSmall, this.transform.position, this.transform.rotation);
            splinterObject.transform.SetParent(parent.transform);
            splinterObject.transform.localScale = this.transform.localScale;
        }
        else if (indexDistance <= splinterIndexMid)
        {
            splinterObject = Instantiate(sprinterObjectMidium, this.transform.position, this.transform.rotation);
            splinterObject.transform.SetParent(parent.transform);
            splinterObject.transform.localScale = this.transform.localScale;
        }
        else if (indexDistance <= splinterIndexMax)
        {
            splinterObject = Instantiate(sprinterObjectBig, this.transform.position, this.transform.rotation);
            splinterObject.transform.SetParent(parent.transform);
            splinterObject.transform.localScale = this.transform.localScale;
        }
        else
        {
            Rigidbody objRb = obj.GetComponent<Rigidbody>();
            objRb.isKinematic = false;

            this.gameObject.transform.SetParent(null);
            this.gameObject.AddComponent<destroy_object>().set_time(chainLifeTime, particle);

            objRb.AddForce(Physics.gravity * gravityFactor * objRb.mass);
            float skalaForce = scatteringFactor * (1.0f / vectorDistance);
            if (skalaForce < minMaginitude)
            {
                skalaForce = minMaginitude;
            }
            index_delay(indexDistance);
            objRb.AddForce(vectorNormalized * skalaForce * (-1.0f), ForceMode.Acceleration);

        }
        

        //全ての子オブジェクトにRigidbodyを適用
        if (indexDistance <= splinterIndexMax)
        {
            foreach (Transform child in splinterObject.transform)
            {
                //child.transform.localScale = this.transform.localScale;
                child.gameObject.AddComponent<Rigidbody>();
                child.gameObject.AddComponent<destroy_object>().set_time(splinterLifeTime, particle);
                Rigidbody childRb = child.gameObject.GetComponent<Rigidbody>();
                childRb.useGravity = false;
                childRb.isKinematic = false;
            }
            Destroy(this.gameObject);
            index_delay(indexDistance);
            scattering_splinter(splinterObject);
        }
    }

    private IEnumerator GradualScatter(Rigidbody rb, Vector3 forceDirection, float initialForce, float maxForce, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float currentForce = Mathf.Lerp(initialForce, maxForce, elapsed / duration);
            rb.AddForce(forceDirection * currentForce, ForceMode.Acceleration);
            elapsed += Time.deltaTime;
            yield return null; // 次のフレームまで待つ
        }
    }

    private void scattering_splinter(GameObject splinterTarget)
    {
        
        List<Transform> splinterList = new List<Transform>();


        foreach (Transform splint in splinterTarget.transform)
        {
            splinterList.Add(splint);
        }

        foreach (Transform splinter in splinterList)
        {
            splinter.SetParent(null);
            Rigidbody splinterRb = splinter.GetComponent<Rigidbody>();
            Vector3 splinterPosition = splinter.transform.position;
            
            Vector3 vectorNormalized = get_vector(hitPosition, splinterPosition);
            float vectorDistance = get_distance(hitPosition, splinterPosition);
            float skalaForce = scatteringFactor * (1.0f / vectorDistance);

            // 最小保証力が適用されるように調整
            if (skalaForce < minMaginitude)
            {
                skalaForce = minMaginitude;
            }

            
            //StartCoroutine(GradualScatter(splinterRb, vectorNormalized, 0, skalaForce, 5.0f)); 
            splinterRb.AddForce(vectorNormalized * skalaForce, ForceMode.VelocityChange);
            splinterRb.AddForce(Physics.gravity * gravityFactor * splinterRb.mass);
        }


    }



    private Vector3 get_vector(Vector3 target, Vector3 self)
    {
        return (target - self).normalized;
    }

    private float get_distance(Vector3 target, Vector3 self)
    {
        return (target - self).magnitude;
    }

    private IEnumerator index_delay(int index)
    {
        yield return new WaitForSeconds(index * indexDelay);
    }
}
