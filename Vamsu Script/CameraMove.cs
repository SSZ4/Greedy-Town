using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;
    public Vector3 offset;


    void Start()
    {
        //Debug.Log(player);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position + offset;
    }
}
