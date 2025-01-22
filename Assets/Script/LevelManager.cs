using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public float deltaTimeBetweenBubbleSpawn = 10;
    private float elapspedTime;
    
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadScene("PersistantObjects", LoadSceneMode.Additive);
    }

    // Update is called once per frame
    void Update()
    {
        // elapspedTime += Time.deltaTime;
        // if (elapspedTime > deltaTimeBetweenBubbleSpawn)
        // {
        //     elapspedTime = 0;
        //     var lavaBubble = Instantiate(lavaBubblePrefab, transform);
        //     lavaBubble.transform.position = new Vector3(Random.Range(-4.0f,4.0f), -1, Random.Range(-4.0f, -3.0f));
        // }
    }
}
