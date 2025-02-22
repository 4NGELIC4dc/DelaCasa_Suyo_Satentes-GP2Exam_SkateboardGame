using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject[] skateboards;  
    public int selectedSkateboardIndex = 0;
    private GameObject activeSkateboardInstance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        skateboards = Resources.LoadAll<GameObject>("Skateboards");
    }

    void Start()
    {
        if (FindObjectsOfType<GameManager>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        if (skateboards == null || skateboards.Length == 0)
        {
            skateboards = GameObject.FindGameObjectsWithTag("Skateboard");
        }

        LoadSelectedSkateboard();
    }

    public void SetSelectedSkateboard(int index)
    {
        if (skateboards == null || skateboards.Length == 0)
        {
            return;
        }

        selectedSkateboardIndex = Mathf.Clamp(index, 0, skateboards.Length - 1);
        PlayerPrefs.SetInt("boardIndex", selectedSkateboardIndex);
        PlayerPrefs.Save();
    }

    public GameObject GetSelectedSkateboardInstance()
    {
        return activeSkateboardInstance;
    }

    public void LoadSelectedSkateboard()
    {
        int boardIndex = PlayerPrefs.GetInt("boardIndex", 0);
        selectedSkateboardIndex = boardIndex;

        if (skateboards == null || skateboards.Length == 0)
        {
            return;
        }

        if (selectedSkateboardIndex < 0 || selectedSkateboardIndex >= skateboards.Length)
        {
            return;
        }

        if (skateboards[selectedSkateboardIndex] == null)
        {
            return;
        }

        if (activeSkateboardInstance != null)
        {
            Destroy(activeSkateboardInstance);
            activeSkateboardInstance = null;
        }

        activeSkateboardInstance = Instantiate(skateboards[selectedSkateboardIndex]);
        activeSkateboardInstance.SetActive(false);
    }
}
