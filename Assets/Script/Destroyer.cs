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
        
        if( go != null )
            Destroy(go); 
        else
            Destroy(b.transform.parent.gameObject);
    }

    public void DestroyItWithAnim(string anim)
    {
        GetComponent<Animator>().Play(anim);
    }
}
