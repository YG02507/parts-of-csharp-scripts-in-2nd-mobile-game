using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Extension of Button.cs of UnityEngine.UI Class

internal sealed partial class ExtendedButton : Button, IPointerDownHandler, IPointerUpHandler
{
    // Private Components

    [field: SerializeField] private RectTransform _extendedButtonRectTransform;

    [field: SerializeField] private ExtendedOutline _extendedButtonExtendedOutline;

    // Private Structures

    [field: SerializeField] private Quaternion _extendedButtonRotationalOffset;

    [field: SerializeField] private float _pushVerticalDisplacement;
}
internal sealed partial class ExtendedButton : Button, IPointerDownHandler, IPointerUpHandler
{
    // Protected Message of MonoBehaviour Class

    protected override void Awake()
    {
        base.Awake();

        Relate();

        Initialize();
    }
}
internal sealed partial class ExtendedButton : Button, IPointerDownHandler, IPointerUpHandler
{
    // Private Defined Methods Called at Awake Message

    private void Relate()
    {
        Kit.Relate(out _extendedButtonRectTransform, this.transform.Form());
        Kit.Relate(out _extendedButtonExtendedOutline, _extendedButtonRectTransform);
    }
    private void Initialize()
    {
        _extendedButtonRotationalOffset = Quaternion.AngleAxis(_extendedButtonRectTransform.rotation.z, Vector3.forward);

        _pushVerticalDisplacement = (_extendedButtonRotationalOffset * Vector3.down).y;
        _pushVerticalDisplacement *= _extendedButtonExtendedOutline.effectDistance.magnitude;
    }
}
internal sealed partial class ExtendedButton : Button, IPointerDownHandler, IPointerUpHandler
{
    // Overriden Public Interface Methods Implemented Mandatorily

    public override void OnPointerDown(PointerEventData pointerEventData)
    {
        base.OnPointerDown(pointerEventData);

        if (this.interactable)
        {
            _extendedButtonExtendedOutline.enabled = false;

            _extendedButtonRectTransform.anchoredPosition -= _pushVerticalDisplacement * Vector2.down;
        }
    }
    public override void OnPointerUp(PointerEventData pointerEventData)
    {
        base.OnPointerUp(pointerEventData);

        if (!_extendedButtonExtendedOutline.enabled)
        {
            _extendedButtonExtendedOutline.enabled = true;

            _extendedButtonRectTransform.anchoredPosition -= _pushVerticalDisplacement * Vector2.up;
        }
    }
}
