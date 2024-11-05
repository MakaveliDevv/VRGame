// using System.Collections;
// using UnityEngine;
// using UnityEngine.InputSystem;

// public class InputPortal : MonoBehaviour
// {
//     [SerializeField] private InputActionProperty rightHandPrimaryButton;
//     [SerializeField] private new ParticleSystem particleSystem;
//     [SerializeField] private Transform pointOfReturnPrefab;
//     [SerializeField] private GameObject portalPrefab;
//     [SerializeField] private GameObject colliderObject; // The object that will detect the collision

//     private ParticleSystem.EmissionModule emissionModule;
//     private GameObject particle;
//     private Transform spawnedPointOfReturn;
//     private bool isPointOfReturnSpawned = false;

//     private void Awake() 
//     {
//         particleSystem = GetComponentInChildren<ParticleSystem>();
//         emissionModule = particleSystem.emission;
//         emissionModule.rateOverTime = 0f;
//         emissionModule.rateOverDistance = 400f;
//         emissionModule.enabled = false;

//         particle = particleSystem.gameObject;

//         // Register the OnPointOfReturnReached event from the ColliderNotifier script
//         if (colliderObject != null)
//         {
//             if (colliderObject.TryGetComponent<ColliderNotifier>(out var notifier))
//             {
//                 notifier.OnPointOfReturnReached += SpawnPortal;
//             }
//         }
//     }

//     private void Update()
//     {
//         if (rightHandPrimaryButton.action.WasPressedThisFrame())
//         {
//             if (!isPointOfReturnSpawned)
//             {
//                 StartCoroutine(SpawnPointOfReturn());
//             }

//             emissionModule.enabled = true;
//             Debug.Log("Right hand primary button is pressed, emission activated!");
//         }
//         else if (rightHandPrimaryButton.action.WasReleasedThisFrame())
//         {
//             emissionModule.enabled = false;
//         }
//     }

//     private IEnumerator SpawnPointOfReturn()
//     {
//         spawnedPointOfReturn = Instantiate(pointOfReturnPrefab, particle.transform.position, Quaternion.identity);
//         isPointOfReturnSpawned = true;
//         Debug.Log("Point of return spawned.");

//         yield return new WaitForSeconds(0.5f);

//         if (spawnedPointOfReturn.TryGetComponent<SphereCollider>(out var sphereCollider))
//         {
//             sphereCollider.isTrigger = true;
//         }
//     }

//     private void SpawnPortal()
//     {
//         Instantiate(portalPrefab, particle.transform.position, Quaternion.identity);
//         Debug.Log("Portal spawned!");

//         Destroy(spawnedPointOfReturn.gameObject);
//         isPointOfReturnSpawned = false;
//         Debug.Log("Point of return removed and state reset.");
//     }

//     private void OnDestroy()
//     {
//         // Unregister the event to prevent memory leaks
//         if (colliderObject != null)
//         {
//             if (colliderObject.TryGetComponent<ColliderNotifier>(out var notifier))
//             {
//                 notifier.OnPointOfReturnReached -= SpawnPortal;
//             }
//         }
//     }
// }



using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputPortal : MonoBehaviour
{
    [SerializeField] private InputActionProperty rightHandPrimaryButton;
    [SerializeField] private new ParticleSystem particleSystem;
    [SerializeField] private Transform pointOfReturnPrefab;
    [SerializeField] private GameObject portalPrefab;
    [SerializeField] private GameObject colliderObject; // The object that will detect the collision

    private ParticleSystem.EmissionModule emissionModule;
    private GameObject particle;
    private Transform spawnedPointOfReturn;
    private bool isPointOfReturnSpawned = false;

    private void Awake() 
    {
        particleSystem = GetComponentInChildren<ParticleSystem>();
        emissionModule = particleSystem.emission;
        emissionModule.rateOverTime = 0f;
        emissionModule.rateOverDistance = 400f;
        emissionModule.enabled = false;

        particle = particleSystem.gameObject;
    }

    private void Update()
    {
        if (rightHandPrimaryButton.action.WasPressedThisFrame())
        {
            if (!isPointOfReturnSpawned)
            {
                StartCoroutine(SpawnPointOfReturn());
            }

            emissionModule.enabled = true;
            Debug.Log("Right hand primary button is pressed, emission activated!");
        }
        else if (rightHandPrimaryButton.action.WasReleasedThisFrame())
        {
            emissionModule.enabled = false;
        }

        // Check for collision with point of return if it has been spawned
        if (isPointOfReturnSpawned)
        {
            // Only check for collisions after a delay
            StartCoroutine(CheckForPointOfReturnCollisionWithDelay());
        }
    }

    private IEnumerator SpawnPointOfReturn()
    {
        spawnedPointOfReturn = Instantiate(pointOfReturnPrefab, particle.transform.position, Quaternion.identity);
        isPointOfReturnSpawned = true;
        Debug.Log("Point of return spawned.");

        yield return new WaitForSeconds(0.5f);

        SphereCollider sphereCollider = spawnedPointOfReturn.GetComponent<SphereCollider>();
        if (sphereCollider != null)
        {
            sphereCollider.isTrigger = true;
        }
    }

    private IEnumerator CheckForPointOfReturnCollisionWithDelay()
    {
        // Wait before starting the collision check
        yield return new WaitForSeconds(1.0f); // Adjust this duration as needed

        while (isPointOfReturnSpawned)
        {
            // Define the position and radius of the sphere to check for collisions
            Vector3 spherePosition = colliderObject.transform.position; // Position of the colliderObject
            float radius = .25f; // Adjust this radius as necessary

            // Use Physics.OverlapSphere to find colliders within the radius
            Collider[] hitColliders = Physics.OverlapSphere(spherePosition, radius);
            
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("PointOfReturn"))
                {
                    Debug.Log("Reached the point of return!");
                    SpawnPortal();
                    yield break; // Exit coroutine after spawning portal
                }
            }

            yield return null; // Wait for the next frame before checking again
        }
    }

    private void SpawnPortal() 
    {
        Instantiate(portalPrefab, particle.transform.position, Quaternion.identity);
        Debug.Log("Portal spawned!");

        Destroy(spawnedPointOfReturn.gameObject);
        isPointOfReturnSpawned = false;
        Debug.Log("Point of return removed and state reset.");
    }
}

