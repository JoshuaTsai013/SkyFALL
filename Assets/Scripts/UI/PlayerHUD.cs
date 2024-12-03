using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
  public Image HeatBarL;
  public Image HeatBarR;
  public Image BloodBar;


  public void SetHeatBar(float value)
  {
    HeatBarL.fillAmount = value;
    HeatBarR.fillAmount = value;
  }

  public void SetBloodBar(float value)
  {
    BloodBar.fillAmount = value;
  }
}
