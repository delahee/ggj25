using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Destroyer : MonoBehaviour
{
    public void DestroyIt(GameObject go) 
    {
        Button b = GetComponent<Button>();
        if(b != null )
           b.onClick.RemoveAllListeners();
        Destroy(go); 
    }
}
