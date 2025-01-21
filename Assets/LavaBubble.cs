using DG.Tweening;
using UnityEngine;

public class LavaBubble : MonoBehaviour
{
    public Vector3 movement = new Vector3(0, 50, 0);

    public float pop;
    // Start is called before the first frame update
    void Awake()
    {
        transform.DOMoveY(5, 2).SetEase(Ease.OutCubic).OnComplete(Destroy);
    }

    // Update is called once per frame
    void Destroy()
    {
        Destroy(gameObject);
    }
}
