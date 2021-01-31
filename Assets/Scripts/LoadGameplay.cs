using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadGameplay : MonoBehaviour
{
    public GameObject Instructions;
    public GameObject Story;

    public void Start()
    {
        Story = GameObject.Find("Story");
        Instructions = GameObject.Find("Instructions");
        ReturnToMenu();
    }
    public void LoadGameplayScene()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void OpenInstructions()
    {
        Instructions.SetActive(true);
    }

    public void OpenStory()
    {
        Story.SetActive(true);
    }

    public void ReturnToMenu()
    {
        Story.gameObject.SetActive(false);
        Instructions.gameObject.SetActive(false);
    }
}
