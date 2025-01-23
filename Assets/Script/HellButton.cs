using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HellButton : Button
{
    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Hover");
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Confirm");
    }
}
