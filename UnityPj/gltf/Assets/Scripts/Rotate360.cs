using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate360 : MonoBehaviour
{
    private GameObject ObjectToRotate;
    public float RotateCycle = 60.0f;
    private float RotationVolume;

    // Start is called before the first frame update
    void Start()
    {
        ObjectToRotate = this.gameObject;
        RotationVolume = 360 / RotateCycle;
    }

    // Update is called once per frame
    void Update()
    {
        ObjectToRotate.transform.Rotate(new Vector3(0, RotationVolume, 0) * Time.deltaTime);
    }
}
