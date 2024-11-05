using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player")) 
        {
            StartCoroutine(ChangeScene());
        }
    }

    private IEnumerator ChangeScene () 
    {
        yield return new WaitForSeconds(.25f);

        SceneManager.LoadScene("BlankScene2");
    }
}
