using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GrowOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public AnimationCurve curve = new();
    private bool isHovered;
    private float t;
    public int preferredWidth = 40;

    private void Update() {
        transform.localScale = Vector2.one * curve.Evaluate(t);
        t = Mathf.MoveTowards(t, isHovered ? 1 : 0, Time.deltaTime / curve.keys.Last().time);

        GetComponent<LayoutElement>().preferredWidth = curve.Evaluate(t) * preferredWidth;
    }
    public void OnPointerEnter(PointerEventData eventData) {
        isHovered = true;
    }
    public void OnPointerExit(PointerEventData eventData) {
        isHovered = false;
    }
}
