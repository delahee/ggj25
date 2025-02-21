using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ShakeManager : MonoBehaviour
{

    private static ShakeManager instance;
    public static ShakeManager Instance
    {
        get
        {
            if (instance == null)
                Debug.LogError("ShakeManager is null !!");

            return instance;
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        if (instance != null)
            Destroy(Instance.gameObject);

        instance = this;
    }

    /*public void OnClick(Transform Target)
    {
        ObjectShaker(Target, new Vector3(1,0,0), 20, 3, 1.1f, false, false);
    }*/
    public void ObjectShaker (Transform Target, Vector3 Direction, float Amplitude = 0f, int Frequency = 1, float Loss = 1f, bool Rotational = false, bool Fragmented = false)
    {
        StartCoroutine(ObjectShakerR(Target, Direction, Amplitude, Frequency, Loss, Rotational, Fragmented));
    }
    IEnumerator ObjectShakerR(Transform Target, Vector3 Direction, float Amplitude = 0f, int Frequency = 1, float Loss = 1f, bool Rotational = false, bool Fragmented = false)
    {
        Vector3 InitialCoordinates = Target.position;
        Target.Translate(Direction * Amplitude);
        bool Rotationfollow = true;
        bool Fragmentfollow = true;
        for (float i = Amplitude; i > 0.1; i = i/Loss)
        {
            if (Rotational)
            {
                if (Rotationfollow)
                {
                    Direction.x = -Direction.x;
                    Rotationfollow = false;
                }
                else
                {
                    Direction.y = -Direction.y;
                   Rotationfollow = true;
                }
            }
            else if (Fragmented)
            {
                if (Fragmentfollow)
                {
                    Direction.x = -Direction.x;
                    Fragmentfollow = false;
                }
                else
                {
                    Direction = -Direction;
                    Fragmentfollow = true;
                }
            }
            else
            {
                Direction = -Direction;
            }
            Target.position = InitialCoordinates;
            Amplitude = Amplitude/Loss;
            Target.Translate(Direction * Amplitude);
            for(int j = Frequency; j>0;j--) yield return new WaitForFixedUpdate();
        }
        Target.position = InitialCoordinates;
    }
}
