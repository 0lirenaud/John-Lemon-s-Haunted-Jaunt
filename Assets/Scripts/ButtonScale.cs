using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonScale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    RectTransform rectTransformButton;
    Vector3 originalScale;

    private void Awake()
    {
        rectTransformButton = GetComponent<RectTransform>();
        originalScale = rectTransformButton.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        rectTransformButton.localScale = originalScale * 1.1f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rectTransformButton.localScale = originalScale;
    }
}
