using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
  public Image HeatBar;


  public void SetHeatBar(float value)
  {
    HeatBar.fillAmount = value;
  }
}
