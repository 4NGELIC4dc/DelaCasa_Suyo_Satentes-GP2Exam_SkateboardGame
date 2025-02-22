using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SkateboardSelection : MonoBehaviour
{
    public GameObject[] boards;
    public Button next;
    public Button prev;

    int index;

    // Start is called before the first frame update
    void Start()
    {
        index = PlayerPrefs.GetInt("boardIndex", 0);
        UpdateBoardDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        next.interactable = index < boards.Length - 1;
        prev.interactable = index > 0;
    }

    void UpdateBoardDisplay()
    {
        index = Mathf.Clamp(index, 0, boards.Length - 1);

        for (int i = 0; i < boards.Length; i++)
        {
            boards[i].SetActive(i == index);
        }
    }

    public void Next()
    {
        index++;
        UpdateBoardDisplay();
        PlayerPrefs.SetInt("boardIndex", index);
        PlayerPrefs.Save();

        GameManager.instance.SetSelectedSkateboard(index);
    }

    public void Prev()
    {
        index--;
        UpdateBoardDisplay();
        PlayerPrefs.SetInt("boardIndex", index);
        PlayerPrefs.Save();

        GameManager.instance.SetSelectedSkateboard(index);
    }

    public void Skate()
    {
        SceneManager.LoadSceneAsync("SkateboardScene");
    }
}
