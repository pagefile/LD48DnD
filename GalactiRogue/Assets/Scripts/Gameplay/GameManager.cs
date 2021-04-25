﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private BasicShip _player = default;

    private int[,] sectorGrid = null;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(_player);
        DontDestroyOnLoad(Camera.main);
        DontDestroyOnLoad(gameObject);
        LoadNextSector();
    }

    void LoadNextSector()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("SpaceSector");
        asyncLoad.completed += NewSectorLoaded;
        Debug.Log("Loading Scene");
    }

    private void NewSectorLoaded(AsyncOperation operation)
    {
        operation.completed -= NewSectorLoaded;
        // Do things
        Debug.Log("Scene loaded");

        // Generate the sector
        sectorGrid = SectorGenerator.GenerateSectorGrid(1000f, 100f, 50f, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if(sectorGrid != null)
        {
            for(int i = 0; i < sectorGrid.GetLength(0); i++)
            {
                for(int j = 0; j < sectorGrid.GetLength(1); j++)
                {
                    if(sectorGrid[i, j] >= 0)
                    {
                        Vector3 pos = new Vector3(i * 100f, 0f, j * 100f) - new Vector3(550f, 0f, 550f);
                        Debug.DrawLine(_player.transform.position, pos, Color.red);
                    }
                }
            }
        }
    }
}
