using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "PlayerEventSO", menuName = "Event/PlayerEventSO")]
public class PlayerEventSO : ScriptableObject
{
    public UnityAction<CharacterGeneral> OnEventRaised;

    public void RaiseEvent(CharacterGeneral character)
    {
        OnEventRaised?.Invoke(character);
    }
}
