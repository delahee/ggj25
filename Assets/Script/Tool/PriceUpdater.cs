using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PriceUpdater : MonoBehaviour
{
    public TextMeshProUGUI price;

    public string PriceFormatter(float value, BubbleType type = BubbleType.Pop)
    {
        return $"{value} {type.ToString()}";
    }
    public string PriceFormatter(float pop, float melt, float mithril)
    {
        string res = "";
        if (pop > 0)
            res += $"{pop} Pop";
        if (melt > 0)
            res += $" {melt} Melt";
        if (mithril > 0)
            res += $" {mithril} Mithril";
        
        //if (res != "")
            return res;

        //return $"pop : {pop}, melt : {melt}, mithril : {mithril}";
    }
}
