using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    void StartPointing();
    void StopPointing();
    void Interact();
}
