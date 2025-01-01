using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class chain_flower : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    float
        disappeareHeight = 15.0f,
        flowerGravityFactor = 4.9f,
        flowerScatteringFactor = 0.01f,
        minMagnitude = 0.001f,
        rotationFactor = 0.1f,
        sizingSpeed = 0.5f,
        destroySizeFactor = 0.1f,
        grawInterval = 2.0f,
        grawSpeed = 3.0f,
        rotateSpeed = 1.0f,
        chainDis = 1.0f,
        chainDisInitial = 1.0f,
        chainSize = 1.0f,
        chainDegreeDispersal = 50.0f,
        veinNum = 3.0f,
        veinCycleInterval = 3.0f,
        veinCycleSize = 1.0f,
        veinCycleSpeed = 2.0f,
        veinUpwardSpeed = 1.0f,
        petalGravityFactor = 0.0001f,
        petalScatteringFactor = 0.01f,
        petalRotatingFactor = 1.0f,
        petalSizeFactor = 1.0f,
        petalIntervalMin = 0.5f,
        petalIntervalMax = 1.0f,
        stamenGravityFactor = 0.0001f,
        stamenScatteringFactor = 0.01f,
        stamenRotatingFactor = 1.0f,
        stamenSizeFactor = 1.0f,
        stamenIntervalMin = 1.0f,
        stamenIntervalMax = 2.0f;

   


    public
    GameObject
        chainPref,
        veinBranch,
        flowerPetalA,
        flowerPetalB,
        flowerStamen;

    public
    List<GameObject>
       chainList = new List<GameObject>();

    public
    List<GameObject>
        veinList = new List<GameObject>();

    public
    Vector3
        currentPos,
        initialPos,
        elapsePos;

    private
    float
        initialRotate;

    public
    int
        chainNum = 0;

    public
    bool
        collapse = false,
        initial = true;

    private
    float
        time;

    private
    bool
        branchPause = false,
        flowerPause = false;

    private
    Vector3
        currentSize,
        initialSize;
        

    private 
    float
        sizingTime = 0.0f,
        veinTime = 0.0f,
        branchTime = 0.0f;


    void Start()
    {
        //InvertMesh();
        initialPos = this.transform.position;
        elapsePos = new Vector3(this.transform.position.x, disappeareHeight, this.transform.position.z);
        generate_vein_branch();
        movement_interval();
    }

    // Update is called once per frame
    void Update()
    {
        currentPos = this.transform.position;
        //Debug.Log("collapse inidicate" + collapse);
        if (!flowerPause)
        {
            if (initial)
            {
                initialSize = this.gameObject.transform.localScale;
                initial = false;
            }

            if (!collapse)
            {
                moving_chain_flower();
                generate_chain();
                collapse_chain_flower();
            }
            else
            {
                vanish_chain_flower();
            }

            if (!branchPause)
            {
                moving_vein_cycle();
            }
            
            disappear_chain_flower();
            veinTime += Time.deltaTime;
        }

        if(this.gameObject.transform.position.y <= -1)
        {
            Destroy(this.gameObject);
        }
    }

    private void moving_chain_flower()
    {
        time += Time.deltaTime * grawSpeed;

        this.transform.position = Vector3.Lerp(initialPos, elapsePos, time);
    }

    private void generate_chain()
    {
        if(currentPos.y-chainDisInitial >= (float)chainNum * chainDis)
        {
            //���𐶐����Ďq�I�u�W�F�N�g�ɐݒ�
            GameObject chainObj = Instantiate(chainPref, initialPos, Quaternion.identity);
            chainObj.transform.localScale = new Vector3(chainSize, chainSize, chainSize);
            chainObj.transform.SetParent(this.transform);
            chainList.Add(chainObj);
            chainNum += 1;
            //UnityEngine.Debug.Log("cleate chain");

            //�q�I�u�W�F�N�g�Ƀ��X�g����^����
            chainObj.GetComponent<chain_moderater>().set_parent(this);

            //������]
            if(chainNum == 1)
            {
                chainList[0].transform.rotation = Quaternion.Euler(0, UnityEngine.Random.Range(0.0f, 180.0f), 0);
                initialRotate = chainList[0].transform.rotation.y;
            }
            else
            {
                /*
                float chainRotatePrevious = chainList[chainNum - 2].transform.rotation.y;
                chainList[chainNum-1].transform.rotation = Quaternion.Euler(0, chainRotatePrevious + UnityEngine.Random.Range(-chainDegreeDispersal, chainDegreeDispersal) + 90.0f, 0);
                */
                if(chainNum % 2 == 0)
                {
                    chainList[chainNum - 1].transform.rotation = Quaternion.Euler(0, initialRotate + UnityEngine.Random.Range(-chainDegreeDispersal, chainDegreeDispersal) + 90.0f, 0);
                }
                else
                {
                    chainList[chainNum - 1].transform.rotation = Quaternion.Euler(0, initialRotate + UnityEngine.Random.Range(-chainDegreeDispersal, chainDegreeDispersal), 0);
                }
            }
        }
    }

    private void collapse_chain_flower()
    {
        //�q�I�u�W�F�N�g�̏Փˏ����Q��
        foreach (GameObject chainObj in chainList)
        {
            bool chainObjCollapse = chainObj.GetComponent<chain_moderater>().collapse;
            int chainObjIndex = chainObj.GetComponent<chain_moderater>().myIndex;

            if (chainObjCollapse)
            {
                //UnityEngine.Debug.Log("collapse");
                collapse = true;
                foreach (GameObject chainObjTemp in chainList)
                {
                    //�Փ˒n�_��S�Ă̍��ɓ`����
                    chainObjTemp.GetComponent<chain_moderater>().hitPosition = chainObj.GetComponent<chain_moderater>().hitPosition;
    

                    //���̕��󏈗��̊J�n
                    chainObjTemp.GetComponent<chain_moderater>().collapse_object(chainObjIndex, chainObjTemp);
                    
                }

                fall_chain_flower(chainObj.GetComponent<chain_moderater>().hitPosition);
                chainList.Clear();

                //Debug.Log("conduct collapse");
                //Destroy(this.gameObject);
                break;
            }
        }

 
    }

    private void disappear_chain_flower()
    {
        if (this.transform.position.y >= disappeareHeight)
        {
            Debug.Log("start Collapse");
            foreach (GameObject chainObjTemp in chainList)
            {
                //�Փ˒n�_��S�Ă̍��ɓ`����
                chainObjTemp.GetComponent<chain_moderater>().hitPosition = this.transform.position;
                

                //�e�I�u�W�F�N�g�̃C���f�b�N�X��ۑ�
                int chainObjIndex = chainObjTemp.GetComponent<chain_moderater>().myIndex;

                //���̕��󏈗��̊J�n
                chainObjTemp.GetComponent<chain_moderater>().collapse_object(chainObjIndex, chainObjTemp);
            }

            chainList.Clear();


            //Destroy(this.gameObject);
        }
    }

    private void fall_chain_flower(Vector3 hitPosition)
    {
        this.gameObject.AddComponent<Rigidbody>();
        Rigidbody rb = this.gameObject.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = false;

        Vector3 vectorNormalized = (hitPosition - this.transform.position).normalized;
        float vectorDistance = (hitPosition - this.transform.position).magnitude;
        float skalaForce = flowerScatteringFactor * (1.0f / vectorDistance);

        // �ŏ��ۏؗ͂��K�p�����悤�ɒ���
        if (skalaForce < minMagnitude)
        {
            skalaForce = minMagnitude;
        }

        // �����_���ȕ����ɉ�]�͂�������
        Vector3 randomTorque = new Vector3(
            Random.Range(-1f, 1f),  // X������
            Random.Range(-1f, 1f),  // Y������
            Random.Range(-1f, 1f)   // Z������
        ).normalized * skalaForce * rotationFactor;

        rb.AddTorque(randomTorque, ForceMode.VelocityChange);
        rb.AddForce(vectorNormalized * skalaForce, ForceMode.VelocityChange);
        rb.AddForce(Physics.gravity * flowerGravityFactor);

        //StartCoroutine(petal_scatter());
        //StartCoroutine(stamen_scatter());

    }

    private IEnumerator petal_scatter()
    {
        GameObject petal;
        while (true)
        {
            float randomNum = Random.Range(-1f, 1f);
            if (0 <= randomNum)
            {
                petal = Instantiate(flowerPetalA, this.gameObject.transform.position, this.gameObject.transform.rotation);
                petal.transform.SetParent(this.gameObject.transform);
                petal.transform.localScale = this.gameObject.transform.localScale * petalSizeFactor;

            }
            else
            {
                petal = Instantiate(flowerPetalB, this.gameObject.transform.position, this.gameObject.transform.rotation);
                petal.transform.SetParent(this.gameObject.transform);
                petal.transform.localScale = this.gameObject.transform.localScale * petalSizeFactor;
            }

            petal.transform.SetParent(null);
            petal.AddComponent<Rigidbody>();
            Rigidbody rb = petal.GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.isKinematic = false;

            // �����_���ȕ����ɉ�]�͂�������
            Vector3 randomTorque = new Vector3(
                Random.Range(-1f, 1f),  // X������
                Random.Range(-1f, 1f),  // Y������
                Random.Range(-1f, 1f)   // Z������
            ).normalized * petalRotatingFactor;

            rb.AddTorque(randomTorque, ForceMode.VelocityChange);
            rb.AddForce(Physics.gravity * petalGravityFactor);

            float randInterval = Random.Range(petalIntervalMin, petalIntervalMax);
            yield return new WaitForSeconds(randInterval);

        }
    }

    private IEnumerator stamen_scatter()
    {
        GameObject stamen;
        while (true)
        {
            stamen = Instantiate(flowerStamen, this.gameObject.transform.position, this.gameObject.transform.rotation);
            stamen.transform.SetParent(this.gameObject.transform);
            stamen.transform.localScale = this.gameObject.transform.localScale * stamenSizeFactor;

            stamen.transform.SetParent(null);
            stamen.AddComponent<Rigidbody>();
            Rigidbody rb = stamen.GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.isKinematic = false;

            // �����_���ȕ����ɉ�]�͂�������
            Vector3 randomTorque = new Vector3(
                Random.Range(-1f, 1f),  // X������
                Random.Range(-1f, 1f),  // Y������
                Random.Range(-1f, 1f)   // Z������
            ).normalized * stamenRotatingFactor;

            rb.AddTorque(randomTorque, ForceMode.VelocityChange);
            rb.AddForce(Physics.gravity * stamenGravityFactor);

            float randInterval = Random.Range(stamenIntervalMin, stamenIntervalMax);
            yield return new WaitForSeconds(randInterval);

        }
    }

    private void vanish_chain_flower()
    {
        /*
        sizingTime += Time.deltaTime * sizingSpeed;  // �o�ߎ��ԂɊ�Â��ăT�C�Y��ω�
        float t = Mathf.Clamp01(sizingTime); // Lerp�̂��߂̐i�s�x��0����1�ɃN�����v

        this.gameObject.transform.localScale = Vector3.Lerp(initialSize, new Vector3(0.0f, 0.0f, 0.0f), t);
        currentSize = this.gameObject.transform.localScale;
        */
        this.gameObject.transform.localScale -= Vector3.one * sizingSpeed * Time.deltaTime;
        //Debug.Log("sizing");


        if (this.gameObject.transform.localScale.x < initialSize.x * destroySizeFactor
            && this.gameObject.transform.localScale.y < initialSize.y * destroySizeFactor
            && this.gameObject.transform.localScale.x < initialSize.x * destroySizeFactor)
        {
            //Destroy(this.gameObject);
        }
    }

    public void generate_vein_branch()
    {
        int i;
        float spawnX, spawnZ, setaSpawn;
        Vector3 spawnCoordinate;

        //�����p�x��ݒ肵�ă��W�A���ɕϊ�
        float setaSet = (360 / veinNum) * (Mathf.PI / 180);

        for (i = 0; i < veinNum; i++)
        {
            //�������W�ݒ�
            setaSpawn = setaSet * i;
            spawnX = (float)(veinCycleSize * Mathf.Cos(setaSpawn));
            spawnZ = (float)(veinCycleSize * Mathf.Sin(setaSpawn));
            spawnCoordinate = new Vector3(initialPos.x + spawnX, initialPos.y, initialPos.z + spawnZ);
            //Debug.Log("instantiate at" + spawnCoordinate);

            //vein�̐���
            GameObject vein = Instantiate(veinBranch, spawnCoordinate, Quaternion.identity);
            //Debug.Log("instantiate coordinate" + vein.transform.position);
            vein.GetComponent<vein_branch>().veinIndex = 0;
            vein.transform.SetParent(this.transform);
            veinList.Add(vein);
            //StartCoroutine(generate_interval());
            
        }
        branchTime = veinTime;
    }

    public void moving_vein_cycle()
    {
        //Debug.Log("movingCycle");
        List<GameObject> objectsToDestroy = new List<GameObject>(); // �폜�\��̃I�u�W�F�N�g���ꎞ�I�ɕۑ����郊�X�g
        //�����p�x��ݒ肵�ă��W�A���ɕϊ�
        float setaSet = (360 / veinNum) * (Mathf.PI / 180);
        int i=0;


        foreach (GameObject vein in veinList)
        {
            // �w�肵�����W�𒆐S�ɃT�C���g�ƃR�T�C���g�ŉ~�^�������o��
            float cycleX = Mathf.Cos(veinTime * veinCycleSpeed + (setaSet * i)) * veinCycleSize;
            float cycleZ = Mathf.Sin(veinTime * veinCycleSpeed + (setaSet * i)) * veinCycleSize;

            // Y���͎��Ԃɉ����ď㏸
            float cycleY = veinUpwardSpeed * (veinTime - branchTime);

            // �V�����ʒu���w�肵�����S�_����̑��Έʒu�ɂ�
            vein.transform.position = new Vector3(cycleX, cycleY, cycleZ) + initialPos;
            

            if(vein.transform.position.y > currentPos.y)
            {
                //Debug.Log("vein y " + vein.transform.position.y + " : flower y " + currentPos.y);
                objectsToDestroy.Add(vein);  // �폜�\��̃I�u�W�F�N�g�����X�g�ɒǉ�
                //Debug.Log("reach to destroy");
            }
            //Debug.Log(vein.transform.position);
            i++;

            if (collapse)
            {
                vein.transform.SetParent(null);
            }
        }

        //Debug.Log(this.transform.position);

        foreach (GameObject obj in objectsToDestroy)
        {
            veinList.Remove(obj);  // ���X�g����폜
            Destroy(obj);          // �I�u�W�F�N�g��j��
        }

        if (objectsToDestroy.Count >= veinNum && !collapse)
        {
            //Debug.Log("conduct destroy");
            generate_interval();
            generate_vein_branch();
        }
    }

    private void InvertMesh()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.triangles = mesh.triangles.Reverse().ToArray();
    }
    private IEnumerator generate_interval()
    {
        branchPause = true;
        yield return new WaitForSeconds(veinCycleInterval);
        branchPause = false;
    }

    private IEnumerator movement_interval()
    {
        flowerPause = true;
        yield return new WaitForSeconds(grawInterval);
        flowerPause = false;
    }
}
