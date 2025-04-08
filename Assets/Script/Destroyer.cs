using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Destroyer : MonoBehaviour
{
    public bool manual = true;
    bool destroyed = false;
    public void DestroyIt(GameObject go) 
    {
        Button b = GetComponent<Button>();
        if(b != null )
           b.onClick.RemoveAllListeners();
        
        if( go != null )
            Destroy(go); 
        else
            Destroy(b.transform.parent.gameObject);
    }

    public void DestroyItWithAnim(string anim)
    {
            destroyed = true;
            int multipop = Upgrade.multipop;
            int count = BubbleManager.Instance.PopList.Count + BubbleManager.Instance.MeltList.Count + BubbleManager.Instance.MithrilList.Count;
            Vector3 Mouse = Input.mousePosition;
            if (manual) { Upgrade.domultipop = true; manual = false; }
            if (multipop > 0 && Upgrade.domultipop)
            {
                Upgrade.domultipop = false;
                for (int i = 1; i <= multipop; i++)
                {
                    if (count > 0)
                    {
                        float distance = 999999999f;
                        bool doit = false;
                        GameObject Bgo = new GameObject();
                        if (BubbleManager.Instance.PopList.Count > 0)
                        {
                            foreach (GameObject go in BubbleManager.Instance.PopList)
                            {
                                if (go.GetComponentInChildren<Destroyer>().manual != false && go.GetComponentInChildren<Destroyer>().destroyed != true)
                                {
                                    float MBdistance = MathF.Sqrt(MathF.Pow(Mouse.x - go.transform.position.x, 2) + MathF.Pow(Mouse.y - go.transform.position.y, 2));
                                    if (MBdistance < distance)
                                    {
                                        distance = MBdistance;
                                        Bgo = go;
                                        doit = true;
                                    }
                                }
                            }
                        }
                        if (BubbleManager.Instance.MeltList.Count > 0)
                        {
                            foreach (GameObject go in BubbleManager.Instance.MeltList)
                            {
                                if (go.GetComponentInChildren<Destroyer>().manual != false && go.GetComponentInChildren<Destroyer>().destroyed != true)
                                {
                                    float MBdistance = MathF.Sqrt(MathF.Pow(Mouse.x - go.transform.position.x, 2) + MathF.Pow(Mouse.y - go.transform.position.y, 2));
                                    if (MBdistance < distance)
                                    {
                                        distance = MBdistance;
                                        Bgo = go;
                                        doit = true;
                                    }
                                }
                            }
                        }
                        if (BubbleManager.Instance.MithrilList.Count > 0)
                        {
                            foreach (GameObject go in BubbleManager.Instance.MithrilList)
                            {
                                if (go.GetComponentInChildren<Destroyer>().manual != false && go.GetComponentInChildren<Destroyer>().destroyed != true)
                                {
                                    float MBdistance = MathF.Sqrt(MathF.Pow(Mouse.x - go.transform.position.x, 2) + MathF.Pow(Mouse.y - go.transform.position.y, 2));
                                    if (MBdistance < distance)
                                    {
                                        distance = MBdistance;
                                        Bgo = go;
                                        doit = true;
                                    }
                                }
                            }
                        }
                        if (doit == true)
                        {
                            Bgo.GetComponentInChildren<Destroyer>().destroyed = true;
                            Bgo.GetComponentInChildren<Destroyer>().manual = false;
                            Bgo.GetComponentInChildren<Button>().onClick.Invoke();
                            count -= 1;
                        }
                    }
                }
            }
        GetComponentInChildren<Button>().onClick.RemoveAllListeners();
        GetComponent<Animator>().Play(anim);
    }

    private void OnDestroy()
    {
        BubbleManager.Instance.PopList.Remove(this.transform.parent.gameObject);
        BubbleManager.Instance.MeltList.Remove(this.transform.parent.gameObject);
        BubbleManager.Instance.MithrilList.Remove(this.transform.parent.gameObject);
    }
}
