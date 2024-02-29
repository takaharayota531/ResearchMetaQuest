using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pen : MonoBehaviour
{
    [Header("Pen Properties")]
    public Transform tip;
    public Material drawingMaterial;
    public Material tipMaterial;
    [Range(0.01f, 0.1f)]
    public float penWidth = 0.01f;
    public Color[] penColors = new Color[] { Color.red, Color.green, Color.blue };

    [Header("Hands & Grabble")]
    public OVRGrabber rightHand;
    public OVRGrabber leftHand;

    public OVRGrabbable grabble;

    private LineRenderer currentDrawing;
    private List<Vector3> positions = new();
    private int index;
    private int currentColorIndex;

    private void Start()
    {
        currentColorIndex = 0;
        tipMaterial.color = penColors[currentColorIndex];
    }

    private void Update()
    {
        bool isGrabbed = grabble.isGrabbed;
        bool isRightHandDrawing = isGrabbed && grabble.grabbedBy == rightHand &&
        OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger);
        bool isLeftHandDrawing = isGrabbed && grabble.grabbedBy == leftHand &&
        OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger);

        if (isRightHandDrawing || isLeftHandDrawing)
        {
            Draw();
        }
        else if (currentDrawing != null)
        {
            currentDrawing = null;
        }
        else if (OVRInput.GetDown(OVRInput.Button.One))
        {
            SwitchColor();

        }
    }

    private void Draw() {
        if (currentDrawing == null) {
            index = 0;
            currentDrawing = new GameObject().AddComponent<LineRenderer>();
            currentDrawing.material = drawingMaterial;
            currentDrawing.startColor = currentDrawing.endColor = penColors[currentColorIndex];
            currentDrawing.startWidth = currentDrawing.endWidth = penWidth;
            currentDrawing.positionCount = 1;
            currentDrawing.SetPosition(0, tip.transform.position);
        } else {
            var currentPosition = currentDrawing.GetPosition(index);
            if (Vector3.Distance(currentPosition, tip.transform.position) > 0.01f) {
                index ++ ;
                currentDrawing.positionCount = index + 1;
                currentDrawing.SetPosition(index, tip.transform.position);
            }
        }

    }
    private void SwitchColor() {
        if (currentColorIndex == penColors.Length - 1) {
            currentColorIndex = 0;
        } else { currentColorIndex++;
        }
        tipMaterial.color = penColors[currentColorIndex];
    }
}