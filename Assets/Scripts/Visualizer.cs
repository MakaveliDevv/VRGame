using UnityEngine;

[ExecuteInEditMode]
public class ColliderVisualizer : MonoBehaviour
{
    public enum GizmoColorOptions
    {
        Red,
        Green,
        Blue,
        Yellow,
        Cyan,
        Magenta,
        White,
        Black
    }

    public GizmoColorOptions gizmoColorSelection = GizmoColorOptions.Green;
    public bool fillColliderFaces = true; // New option to toggle filled shapes

    private Collider collider3D;
    private new Collider2D collider2D;

    private Color GetSelectedColor()
    {
        switch (gizmoColorSelection)
        {
            case GizmoColorOptions.Red: return Color.red;
            case GizmoColorOptions.Green: return Color.green;
            case GizmoColorOptions.Blue: return Color.blue;
            case GizmoColorOptions.Yellow: return Color.yellow;
            case GizmoColorOptions.Cyan: return Color.cyan;
            case GizmoColorOptions.Magenta: return Color.magenta;
            case GizmoColorOptions.White: return Color.white;
            case GizmoColorOptions.Black: return Color.black;
            default: return Color.green;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = GetSelectedColor();

        // 3D Collider Visualization
        collider3D = GetComponent<Collider>();
        if (collider3D != null)
        {
            if (collider3D is BoxCollider)
            {
                BoxCollider boxCollider = (BoxCollider)collider3D;
                Gizmos.matrix = Matrix4x4.TRS(boxCollider.transform.position, boxCollider.transform.rotation, boxCollider.transform.lossyScale);

                if (fillColliderFaces)
                    Gizmos.DrawCube(boxCollider.center, boxCollider.size); // Draw filled cube
                else
                    Gizmos.DrawWireCube(boxCollider.center, boxCollider.size); // Draw wireframe cube
            }
            else if (collider3D is SphereCollider)
            {
                SphereCollider sphereCollider = (SphereCollider)collider3D;
                Gizmos.matrix = Matrix4x4.TRS(sphereCollider.transform.position, sphereCollider.transform.rotation, sphereCollider.transform.lossyScale);

                if (fillColliderFaces)
                    Gizmos.DrawSphere(sphereCollider.center, sphereCollider.radius); // Draw filled sphere
                else
                    Gizmos.DrawWireSphere(sphereCollider.center, sphereCollider.radius); // Draw wireframe sphere
            }
            else if (collider3D is CapsuleCollider)
            {
                // Capsule collider is more complex, so using a bounding box approximation for fill
                CapsuleCollider capsuleCollider = (CapsuleCollider)collider3D;
                Gizmos.matrix = Matrix4x4.TRS(capsuleCollider.transform.position, capsuleCollider.transform.rotation, capsuleCollider.transform.lossyScale);
                
                if (fillColliderFaces)
                {
                    Gizmos.DrawSphere(capsuleCollider.center + Vector3.up * (capsuleCollider.height / 2 - capsuleCollider.radius), capsuleCollider.radius); // Top Sphere
                    Gizmos.DrawSphere(capsuleCollider.center - Vector3.up * (capsuleCollider.height / 2 - capsuleCollider.radius), capsuleCollider.radius); // Bottom Sphere
                    Gizmos.DrawCube(capsuleCollider.center, new Vector3(capsuleCollider.radius * 2, capsuleCollider.height - capsuleCollider.radius * 2, capsuleCollider.radius * 2)); // Body
                }
                else
                {
                    Gizmos.DrawWireSphere(capsuleCollider.center + Vector3.up * (capsuleCollider.height / 2 - capsuleCollider.radius), capsuleCollider.radius); // Wire top sphere
                    Gizmos.DrawWireSphere(capsuleCollider.center - Vector3.up * (capsuleCollider.height / 2 - capsuleCollider.radius), capsuleCollider.radius); // Wire bottom sphere
                    Gizmos.DrawWireCube(capsuleCollider.center, new Vector3(capsuleCollider.radius * 2, capsuleCollider.height - capsuleCollider.radius * 2, capsuleCollider.radius * 2)); // Wire body
                }
            }
        }

        // 2D Collider Visualization
        collider2D = GetComponent<Collider2D>();
        if (collider2D != null)
        {
            if (collider2D is BoxCollider2D)
            {
                BoxCollider2D boxCollider2D = (BoxCollider2D)collider2D;
                Gizmos.matrix = Matrix4x4.TRS(boxCollider2D.transform.position, boxCollider2D.transform.rotation, boxCollider2D.transform.lossyScale);

                if (fillColliderFaces)
                    Gizmos.DrawCube(boxCollider2D.offset, boxCollider2D.size); // Draw filled 2D box
                else
                    Gizmos.DrawWireCube(boxCollider2D.offset, boxCollider2D.size); // Draw wireframe 2D box
            }
            else if (collider2D is CircleCollider2D)
            {
                CircleCollider2D circleCollider2D = (CircleCollider2D)collider2D;
                Gizmos.matrix = Matrix4x4.TRS(circleCollider2D.transform.position, circleCollider2D.transform.rotation, circleCollider2D.transform.lossyScale);

                if (fillColliderFaces)
                    Gizmos.DrawSphere(circleCollider2D.offset, circleCollider2D.radius); // Draw filled 2D circle
                else
                    Gizmos.DrawWireSphere(circleCollider2D.offset, circleCollider2D.radius); // Draw wireframe 2D circle
            }
        }
    }
}
