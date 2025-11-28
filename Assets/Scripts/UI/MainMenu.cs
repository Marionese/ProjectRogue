using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadSlot(int slot)
    {
        GameSession.Instance.currentSlot = slot;

        // Falls Slot existiert, lade ihn
        if (SaveSystem.SlotExists(slot))
        {
            SaveSystem.LoadGame(slot);
        }
        else
        {
            // Neuer Slot → MetaCoins bleiben 0
            Debug.Log("Starting new save slot " + slot);
        }

        // Jetzt wechselst du in die erste Spielszene
        SceneManager.LoadScene("MainScene");
    }
   
}
