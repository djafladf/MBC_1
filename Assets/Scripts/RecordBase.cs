using UnityEngine;
using UnityEngine.EventSystems;

public class RecordBase : MonoBehaviour, IPointerDownHandler
{

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!DTW.inst.OnRecord) return;

    }
}
