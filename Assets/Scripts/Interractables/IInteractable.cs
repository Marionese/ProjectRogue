using UnityEngine;
using UnityEngine.InputSystem;

public interface IInteractable
{
    void Interact(GameObject player);
    Vector3 Position { get; }
    void SetHighlight(bool state);
}
