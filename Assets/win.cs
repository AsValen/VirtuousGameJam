using UnityEngine;
using UnityEngine.SceneManagement;

public class win : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("goal");
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("yesGoal");
            SceneManager.LoadScene("WinMenu");
        }
    }
}
