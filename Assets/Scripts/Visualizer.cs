using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visualizer : MonoBehaviour
{
       [Header("Collider Visualization Settings")]
    public Color colliderFillColor = new Color(0, 1, 0, 0.25f); // Green with some transparency

    private Collider targetCollider;

    private void OnValidate()
    {
        // Update the target collider reference on validation
        targetCollider = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        // Initialize the target collider when the script is enabled
        targetCollider = GetComponent<Collider>();
    }

    private void OnDrawGizmos()
    {
        if (targetCollider == null)
            return;

        Gizmos.color = colliderFillColor;

        if (targetCollider is BoxCollider boxCol)
        {
            Gizmos.matrix = Matrix4x4.TRS(boxCol.transform.position, boxCol.transform.rotation, boxCol.transform.lossyScale);
            Gizmos.DrawCube(boxCol.center, boxCol.size);
        }
        else if (targetCollider is SphereCollider sphereCol)
        {
            Gizmos.matrix = Matrix4x4.TRS(sphereCol.transform.position, sphereCol.transform.rotation, sphereCol.transform.lossyScale);
            Gizmos.DrawSphere(sphereCol.center, sphereCol.radius);
        }
        else if (targetCollider is CapsuleCollider capsuleCol)
        {
            Gizmos.matrix = Matrix4x4.TRS(capsuleCol.transform.position, capsuleCol.transform.rotation, capsuleCol.transform.lossyScale);
            DrawFilledCapsule(capsuleCol.center, capsuleCol.radius, capsuleCol.height, capsuleCol.direction);
        }
        else if (targetCollider is MeshCollider meshCol && meshCol.sharedMesh != null)
        {
            Gizmos.matrix = meshCol.transform.localToWorldMatrix;
            Gizmos.DrawMesh(meshCol.sharedMesh);
        }
    }

    private void DrawFilledCapsule(Vector3 center, float radius, float height, int direction)
    {
        float cylinderHeight = Mathf.Max(0, height / 2 - radius);
        Vector3 up = Vector3.up * cylinderHeight;

        if (direction == 0) up = Vector3.right * cylinderHeight;
        else if (direction == 2) up = Vector3.forward * cylinderHeight;

        // Draw the spheres at each end
        Gizmos.DrawSphere(center + up, radius);
        Gizmos.DrawSphere(center - up, radius);

        // Draw the cylinder between the spheres
        Gizmos.DrawCube(center, new Vector3(
            direction == 0 ? height : radius * 2,
            direction == 1 ? height : radius * 2,
            direction == 2 ? height : radius * 2
        ));
    }
}
