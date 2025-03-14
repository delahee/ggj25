using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

struct CorObject
{
    public Transform transform;
    public int instances;
    public Vector3 initPos;

    public CorObject(Transform Vtransform, int Vinstances, Vector3 InitPos = new Vector3())
    {
        this.transform = Vtransform;
        this.instances = Vinstances;
        this.initPos = InitPos;
    }
    public void Add()
    {
        instances += 1;
    }
    public void Sub()
    {
        instances -= 1;
        if (instances < 0) instances = 0;
    }
}

public class ShakeManager : MonoBehaviour
{
    private List<CorObject> CorObjList = new List<CorObject>();
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
        CorObject obj = new CorObject(Target, 1, Target.position);
        if (CorObjList.Count > 0)
        {
            for (int i = 0; i < CorObjList.Count; i++)
            {
                if (CorObjList[i].transform == Target)
                {
                    CorObjList[i].Add();
                    obj = CorObjList[i];
                }
            }
        }
        else 
        {
            obj = new CorObject(Target, 1, Target.position);
            CorObjList.Add(obj);
        }      
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
            Target.position = obj.initPos;
            Amplitude = Amplitude/Loss;
            Target.Translate(Direction * Amplitude);
            for(int j = Frequency; j>0;j--) yield return new WaitForFixedUpdate();
        }

        if (CorObjList.Count > 0)
        {
            for (int i = 0; i < CorObjList.Count; i++)
            {
                if (CorObjList[i].transform == Target)
                {
                    CorObjList[i].Add();
                }
            }
        }
        if (CorObjList.Count > 0)
        {
            for (int i = 0; i < CorObjList.Count; i++)
            {
                if (CorObjList[i].transform == Target)
                {
                    CorObjList[i].Sub();
                    Target.position = CorObjList[i].initPos;
                }
            }
        }
        else
        {
            Target.position = obj.initPos;
            Debug.Log("COROBJ NOT FOUND");
        }
        //Target.position = InitialCoordinates;
    }
}
