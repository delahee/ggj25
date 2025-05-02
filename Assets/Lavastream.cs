using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lavastream : MonoBehaviour
{
    // Start is called before the first frame update


    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(new Vector3(0, 1.1f, 0));
        if (transform.position.z > 120) Destroy(this.gameObject);
    }
}
