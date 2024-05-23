using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonShooterController : MonoBehaviour
{
    public GameObject shootCam;
    public GameObject cross;

    private PlayerInputs Inputs;
    // Start is called before the first frame update
    void Start()
    {
        Inputs = GetComponent<PlayerInputs>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Inputs.aim)
        {
            shootCam.SetActive(true);
            cross.SetActive(true);
        }
        else
        {
            shootCam.SetActive(false);
            cross.SetActive(false);
        }
    }
}
