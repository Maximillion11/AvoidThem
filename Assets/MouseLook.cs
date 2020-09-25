using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public Vector2 Sensitivity = new Vector2(1f, 1f);
    public float rotationCapY = 80f;

    private float rotationCountY = 0f;

    public void Update()
    {
        Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        Vector2 playerOutput = new Vector2(0, mouseInput.x * Sensitivity.x);
        Vector2 cameraOutput = new Vector2(-mouseInput.y * Sensitivity.y, 0);

        transform.Rotate(playerOutput);

        if ((rotationCountY < rotationCapY || cameraOutput.x > 0) && (rotationCountY > -rotationCapY || cameraOutput.x < 0))
        {
            float distanceToRotationCapY = rotationCapY - Mathf.Abs(rotationCountY);
            if (distanceToRotationCapY < Mathf.Abs(cameraOutput.x) && distanceToRotationCapY != 0)
            {
                cameraOutput.x = distanceToRotationCapY * Mathf.Sign(cameraOutput.x);
            }

            rotationCountY -= cameraOutput.x;
            Camera.main.transform.Rotate(cameraOutput);
        }
    }
}
