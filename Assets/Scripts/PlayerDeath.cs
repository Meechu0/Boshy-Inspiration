using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    public PlayerControlls _playerController;
    private GameMaster _gameMaster;
    private void Start()
    {
        _gameMaster = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
        transform.position = _gameMaster.lastCheckPointsPos;
    }

    // Update is called once per frame
    void Update()
    { 
        
    }

}
