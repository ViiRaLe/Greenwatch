using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakeRotation : MonoBehaviour
{
    private void Awake()
    {
        transform.rotation = Quaternion.Euler(0, Random.Range(-180, 180), 0);
    }
}
