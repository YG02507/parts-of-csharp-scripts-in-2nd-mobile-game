using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Extension of Toggle.cs of UnityEngine.UI Class

internal sealed partial class ExtendedToggle : Toggle, IPointerDownHandler, IPointerUpHandler, IContract
{
    // Private Components

    [field: SerializeField] private RectTransform _extendedToggleRectTransform;

    [field: SerializeField] private ExtendedOutline _extendedToggleExtendedOutline;

    // Private Interfaces

    private IWritable<Kit.Hand> _writableCurrentDominantHand;

    private IWritable<Kit.Design> _writableCurrentGameDesign;

    // Private Structures

    [field: SerializeField] private Quaternion _extendedToggleRotationalOffset;

    [field: SerializeField] private Vector2 _extendedToggleStandardPoint;

    [field: SerializeField] private float _extendedToggleMaximumThickness;
}
internal sealed partial class ExtendedToggle : Toggle, IPointerDownHandler, IPointerUpHandler, IContract
{
    // Private Defined Method Called at Other Methods

    private void LatchExtendedToggle()
    {
        Kit.MakeVector(out Vector2 extendedToggleVector, _extendedToggleStandardPoint.x, _extendedToggleStandardPoint.y);

        if (this.isOn)
        {
            extendedToggleVector += _extendedToggleMaximumThickness.Half() * (_extendedToggleRotationalOffset * Vector3.down).Form();
        }

        _extendedToggleRectTransform.anchoredPosition = extendedToggleVector;

        extendedToggleVector = (this.isOn ? _extendedToggleMaximumThickness.Half() : _extendedToggleMaximumThickness) * Vector2.down;

        if (_extendedToggleRectTransform.rotation.z.Form().Equals(default))
        {
            _extendedToggleExtendedOutline.effectDistance = extendedToggleVector;
        }
        else
        {
            _extendedToggleExtendedOutline.effectDistance = -extendedToggleVector;
        }
    }
}
internal sealed partial class ExtendedToggle : Toggle, IPointerDownHandler, IPointerUpHandler, IContract
{
    // Overriden Public Interface Methods Implemented Mandatorily

    public override void OnPointerDown(PointerEventData pointerEventData)
    {
        base.OnPointerDown(pointerEventData);

        Kit.MakeVector(out Vector2 extendedToggleVector, _extendedToggleStandardPoint.x, _extendedToggleStandardPoint.y);

        if (!this.isOn)
        {
            _extendedToggleExtendedOutline.enabled = false;

            extendedToggleVector += _extendedToggleMaximumThickness * (_extendedToggleRotationalOffset * Vector3.down).Form();

            _extendedToggleRectTransform.anchoredPosition = extendedToggleVector;
        }
    }
    public override void OnPointerUp(PointerEventData pointerEventData)
    {
        base.OnPointerUp(pointerEventData);

        if (!this.isOn)
        {
            _extendedToggleExtendedOutline.enabled = true;

            LatchExtendedToggle();
        }
    }

    // Public Defined Interface Method Implemented Mandatorily

    public void ContractWriting(in GameManager gameManagerInstance)
    {
        _writableCurrentDominantHand = gameManagerInstance;

        _writableCurrentGameDesign = gameManagerInstance;
    }

    // Public Defined Methods Used by User Interface

    public void Initialize()
    {
        Kit.Relate(out _extendedToggleRectTransform, this.transform.Form());
        Kit.Relate(out _extendedToggleExtendedOutline, _extendedToggleRectTransform);

        _extendedToggleRotationalOffset = Quaternion.AngleAxis(_extendedToggleRectTransform.rotation.z, Vector3.forward);

        _extendedToggleMaximumThickness = _extendedToggleExtendedOutline.effectDistance.magnitude;

        _extendedToggleStandardPoint = _extendedToggleRectTransform.anchoredPosition;

        LatchExtendedToggle();
    }
    public void Hand(int selectedDominantHandArgument)
    {
        if (this.isOn)
        {
            _writableCurrentDominantHand.Write((Kit.Hand)selectedDominantHandArgument);
        }
    }
    public void Design(int selectedDominantDesignArgument)
    {
        if (this.isOn)
        {
            _writableCurrentGameDesign.Write((Kit.Design)selectedDominantDesignArgument);
        }
    }
    public void Latch()
    {
        LatchExtendedToggle();
    }
}
