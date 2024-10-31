using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunMovement : MonoBehaviour
{
    [SerializeField]
    private GameObject _mainCamera;
    [SerializeField]
    private PlayerInputs _inputs;
    // Start is called before the first frame update
    private void Awake()
    {
        // get a reference to our main camera
        if (_mainCamera == null)
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (_inputs.aim)
        {
           transform.rotation = _mainCamera.transform.rotation;
        }
        else
        {
            transform.rotation = transform.parent.rotation;
        }
        
    }
}
