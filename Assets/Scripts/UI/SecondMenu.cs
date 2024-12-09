using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SecondMenu : MonoBehaviour
{
    // Start is called before the first frame update
    private bool keyPressed = false;
    public void StartGame()
    { 
     SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("quit");
    }
}
