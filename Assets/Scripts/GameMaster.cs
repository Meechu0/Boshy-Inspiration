using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{

    private static GameMaster instance;
    public Vector2 lastCheckPointsPos;
    public GameObject canvas;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);

        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
    }
    private void Update()
    {
        if (canvas.active == true){
            if (Input.GetKeyDown(KeyCode.R))
            {
                ResetGame();
            }
        }       

    }
    public void ResetGame()
    {
        if(canvas.active == true)
        {
            canvas.active = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

    }
    public void die()
    {
        canvas.SetActive(true);
    }
}
