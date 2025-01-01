using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vein_cycle : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private
    float
        spiralRadius = 2.0f,
        spiralSpeed = 1.0f,
        upwardSpeed = 0.5f;

    private
    float
        time = 0.0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
    }

    private void moving_spiral(Vector3 centerPosition)
    {
        // 指定した座標を中心にサイン波とコサイン波で円運動を作り出す
        float x = Mathf.Cos(time * spiralSpeed) * spiralRadius;
        float z = Mathf.Sin(time * spiralSpeed) * spiralRadius;

        // Y軸は時間に応じて上昇
        float y = upwardSpeed * time;

        // 新しい位置を指定した中心点からの相対位置にする
        transform.position = new Vector3(x, y, z) + centerPosition;
    }

}
