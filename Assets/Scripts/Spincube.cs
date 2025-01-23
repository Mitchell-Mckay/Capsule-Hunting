using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spincube : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(35,45,55) * Time.deltaTime);
    }
}
