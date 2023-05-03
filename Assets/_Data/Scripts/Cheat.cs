using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cheat : MonoBehaviour
{
    [SerializeField] private List<Vector2> endPoints;
    private GameObject player;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            this.TeleportToEndMap();
        }
    }

    private void TeleportToEndMap()
    {
        this.player = GameObject.Find("Player");
        if (this.player == null || this.endPoints.Count == 0) return;

        if (SceneManager.GetActiveScene().name == "Scene_Level1_Forest")
        {
            this.player.transform.position = this.endPoints[0];
        }
        else if (SceneManager.GetActiveScene().name == "Scene_Level2_Dungeon")
        {
            this.player.transform.position = this.endPoints[1];
        }
        else if (SceneManager.GetActiveScene().name == "Scene_Level3_Heaven")
        {
            this.player.transform.position = this.endPoints[2];
        }
    }
}
