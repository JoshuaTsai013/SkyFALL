using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
  public Image HeatBar;
  public Image BloodBar;


  public void SetHeatBar(float value)
  {
    HeatBar.fillAmount = value;
  }

  public void SetBloodBar(float value)
  {
    BloodBar.fillAmount = value;
  }
}
