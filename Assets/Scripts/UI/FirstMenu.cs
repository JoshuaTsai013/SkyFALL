using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class FirstMenu : MonoBehaviour
{
    // Start is called before the first frame update
    private bool keyPressed = false;
    public void PressButtonToNextMenu()
    { 
     SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
    }
    public void Update()
    { 
    if(Input.anyKeyDown)
        {
         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
        }
    
    }
 
}
