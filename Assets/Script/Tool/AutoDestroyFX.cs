using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyFX : MonoBehaviour
{
    ParticleSystem fx;

    private void Start()
    {
        fx = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!fx.IsAlive())
            Destroy(gameObject);
    }
}
