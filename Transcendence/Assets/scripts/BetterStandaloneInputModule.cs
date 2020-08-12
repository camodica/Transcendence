using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class BetterStandaloneInputModule : StandaloneInputModule
{
    public PointerEventData GetLastPointerEventData(int id)
    {
        return base.GetLastPointerEventData(id);
    }
}
