using UnityEngine;
using UnityEngine.SceneManagement;

public class Win : MonoBehaviour
{
    // public string nextSceneName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision detected!");

        if (collision.CompareTag("Player")) 
        {
            Debug.Log("Player hit the win trigger!");
            
           
            string nextSceneName = "Level2";
            
            Debug.Log("Next scene name: " + nextSceneName);

          
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
