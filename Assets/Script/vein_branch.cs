using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vein_branch : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public
    float
        branchDegreeMax = 30.0f,
        branchDegreeMin = 15.0f,
        durationMax = 3.0f,
        durationMin = 0.5f,
        durationDecrease = 0.5f,
        grawSpeed = 1.0f,
        probBranchGenerate = 40.0f,
        probBranchDecreaseFactor = 0.5f,
        probChracterGenerate = 15.0f,
        probGenerateInterval = 0.5f,
        radiusFactor = 3.0f,
        curvFactor = 1.1f,
        xAxisFactor = 0.3f;


    [SerializeField]
    private
    GameObject
        branch,
        leaf,
        buds;

    private
    float
        time = 0.0f,
        generateTime =0.0f;

    private
    float
        grawDuration;

    private
    float
        timeFactorCharacter = 0;

    private
    float
        timeFactorBranch = 0;

    public
    int
        veinIndex = 1;

    private
    bool
        characterPause = false,
        branchPause = false;
    void Start()
    {
        grawDuration = UnityEngine.Random.Range(durationMin, durationMax);
        if(veinIndex <= 1)
        {
            rotation_set(this.transform.root.position);
        }
        else
        {
            rotation_set(this.transform.parent.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (veinIndex != 0)
        {
            branch_moving();
        }

        if (!characterPause)
        {
            //generate_character();
        }
        if (!branchPause && veinIndex <= 4)
        {
            //generate_branch();
        }
    }


    //茎を生やす
    private void branch_moving()
    {
        //時間を平方根でsin時間経過で引き延ばすsin関数を生成
        float factorY = Mathf.Sin(Mathf.Sqrt(time) * curvFactor) * radiusFactor;
        float factorX = time * xAxisFactor;
        //オブジェクトのローカル軸で動かす
        this.transform.position += this.transform.up * factorY;
        this.transform.position += -1 * (this.transform.right * factorX);

        //時間経過で削除
        if(grawDuration <= time )
        {
            Destroy(this.gameObject);
        }
    }

    private void generate_character()
    {
        //生成乱数の生成
        int randomValue = UnityEngine.Random.Range(0, 100);
        //生成物の角度設定
        float randomDegreeX = this.transform.localRotation.eulerAngles.x + UnityEngine.Random.Range(branchDegreeMin, branchDegreeMax);
        float randomDegreeY = this.transform.localRotation.eulerAngles.y + UnityEngine.Random.Range(branchDegreeMin, branchDegreeMax);
        float randomDegreeZ = this.transform.localRotation.eulerAngles.z + UnityEngine.Random.Range(branchDegreeMin, branchDegreeMax);
        GameObject generateObject;
        

        
        timeFactorCharacter = 0;

        Debug.Log("character generate chance");

        if((float)randomValue <= probChracterGenerate)
        {
            //Debug.Log("generateCharacter");
            if (randomValue % 2 == 0)
            {
                //葉の生成
                generateObject = Instantiate(leaf, this.transform.position, Quaternion.identity);
            }
            else
            {
                //つぼみの生成
                generateObject = Instantiate(leaf, this.transform.position, Quaternion.Euler(randomDegreeX, randomDegreeY, randomDegreeZ));
            }

            generateObject.transform.SetParent(this.transform);
            StartCoroutine(character_interval());
        }

        
    }

    private void generate_branch()
    {
        //生成乱数の生成
        int randomValue = UnityEngine.Random.Range(0, 100);
        //生成物の角度設定
        float randomDegreeX = this.transform.localRotation.eulerAngles.x + UnityEngine.Random.Range(branchDegreeMin, branchDegreeMax);
        float randomDegreeY = this.transform.localRotation.eulerAngles.y + UnityEngine.Random.Range(branchDegreeMin, branchDegreeMax);
        float randomDegreeZ = this.transform.localRotation.eulerAngles.z + UnityEngine.Random.Range(branchDegreeMin, branchDegreeMax);
        GameObject generateBranch;
        Debug.Log("branch generate chance");


        //Debug.Log("generateBranch");

        if ((float)randomValue <= probBranchGenerate)
        {
            Debug.Log("branch generate");
            generateBranch = Instantiate(branch, this.transform.position, Quaternion.Euler(randomDegreeX, randomDegreeY, randomDegreeZ));
            generateBranch.transform.SetParent(this.transform);
            generateBranch.GetComponent<vein_branch>().veinIndex = veinIndex + 1;
            StartCoroutine(branch_interval());
        }
        
    }

    public void rotation_set(Vector3 centerPosition)
    {
        // オブジェクトの現在の位置を取得
        Vector3 objectPosition = this.transform.position;

        // X, Z 平面上でターゲットへのベクトルを計算
        Vector3 direction = new Vector3(centerPosition.x - objectPosition.x, 0, centerPosition.z - objectPosition.z);

        // ベクトルの長さを正規化 (1 にして方向だけを取得)
        direction.Normalize();

        // オブジェクトのX軸正方向をターゲットベクトルに合わせて回転させる
        // LookRotation の第1引数は前方向を指定 (今回は X 軸に合わせるため右方向にする)
        // 第2引数は上方向 (通常は Y 軸方向)
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up) * Quaternion.Euler(0, -90, 0);

        this.transform.rotation = targetRotation;
    }

    private IEnumerator character_interval()
    {
        characterPause = true;
        yield return new WaitForSeconds(probGenerateInterval);
        characterPause = false;
    }

    private IEnumerator branch_interval()
    {
        branchPause = true;
        yield return new WaitForSeconds(probGenerateInterval);
        Debug.Log("branch delay");
        branchPause = false;
    }


}
