using UnityEngine;
using UnityEngine.InputSystem;

public class PickupItem : MonoBehaviour
{
    public ItemData item;

    private void OnTriggerEnter2D(Collider2D col)
    {
        var pm = col.GetComponent<PlayerModifierManager>();
        if (pm)
        {
            pm.AddItem(item);

            // Spieler herausfinden
            var input = col.GetComponent<PlayerInput>();
            if (input)
            {
                int unityIndex = input.playerIndex; // 0 = erster Spieler, 1 = zweiter
                int sessionIndex = unityIndex + 1;  // → 1 oder 2

                GameSession.Instance.RegisterRunItem(sessionIndex, item);
            }

            Destroy(gameObject);
        }

    }
}

