using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSelector : MonoBehaviour
{
    public Color _color;
    public float _angle;
    public Vector3 position, ringPosition;
    public UnityEngine.UI.Image image;
    public Canvas canvas;

    public bool cursorIn;
    public void _OnEnter() { cursorIn = true; }
    public void _OnExit() { cursorIn = false; }

    private void Update()
    {
        if (cursorIn)
            _OnMouseOver();
    }

    public void _OnMouseOver()
    {
        //prend la position par rapport à l'image
        //worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        position = Input.mousePosition;
        ringPosition = position - gameObject.transform.position;

        //calcul l'angle par rapport au centre de l'image
        _angle = 360 - (Mathf.Rad2Deg * Mathf.Atan2(ringPosition.y, ringPosition.x) - 90);
        if (_angle > 360) _angle -= 360;
        if (_angle < 0) _angle += 360;

        //angle vers couleur
        _color = Color.HSVToRGB(_angle / 360, 1, 1);

        //attribution couleur
        if (image != null)
            image.color = _color;
    }


    public Vector3 ViewportToCanvasPosition(Canvas canvas, Vector3 viewportPosition)
    {
        var centerBasedViewPortPosition = viewportPosition - new Vector3(0.5f, 0.5f, 0);
        var canvasRect = canvas.GetComponent<RectTransform>();
        var scale = canvasRect.sizeDelta;
        return Vector3.Scale(centerBasedViewPortPosition, scale);
    }
}
