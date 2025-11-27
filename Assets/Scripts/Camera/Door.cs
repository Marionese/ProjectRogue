using System;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [SerializeField] private string nextScene;

    private void OTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        SceneManager.LoadScene(nextScene);
    }

}

