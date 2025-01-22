using DG.Tweening;
using UnityEngine;

public class LavaBubble : MonoBehaviour
{
    // Start is called before the first frame update
    private Color endColor = new Color(1, 1, 0, 0);
    private Ray ray;
    private RaycastHit hit;
    
    void Awake()
    {
        Renderer renderer = GetComponentInChildren<Renderer>();
        transform.DOMoveY(5, 10).SetEase(Ease.OutCubic).OnComplete(Destroy);
        renderer.material.DOColor(endColor, 10);
    }

    // Update is called once per frame
    void Destroy()  
    {
        Destroy(this);
    }
    

    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit))
        {
            if(Input.GetMouseButtonDown(0))
               Destroy();
        }
    }
}
