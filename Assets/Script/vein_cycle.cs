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
        // �w�肵�����W�𒆐S�ɃT�C���g�ƃR�T�C���g�ŉ~�^�������o��
        float x = Mathf.Cos(time * spiralSpeed) * spiralRadius;
        float z = Mathf.Sin(time * spiralSpeed) * spiralRadius;

        // Y���͎��Ԃɉ����ď㏸
        float y = upwardSpeed * time;

        // �V�����ʒu���w�肵�����S�_����̑��Έʒu�ɂ���
        transform.position = new Vector3(x, y, z) + centerPosition;
    }

}
