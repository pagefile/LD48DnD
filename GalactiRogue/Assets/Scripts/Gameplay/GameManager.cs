using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Pagefile.System;
using Pagefile.System.Messaging;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private BasicShip _player = default;
    [SerializeField]
    private SectorData _sector = default;

    private int[,] sectorGrid = null;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(_player);
        DontDestroyOnLoad(Camera.main);
        DontDestroyOnLoad(gameObject);
        MessagePublisher.Instance.Subscribe(typeof(WarpSuccessMessage), WarpSuccessfulHandler);
        LoadNextSector();
    }

    void LoadNextSector()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("SpaceSector");
        asyncLoad.completed += NewSectorLoaded;
    }

    private void NewSectorLoaded(AsyncOperation operation)
    {
        operation.completed -= NewSectorLoaded;

        // Generate the sector
        sectorGrid = SectorGenerator.GenerateSectorGrid(_sector);
        for(int i = 0; i < sectorGrid.GetLength(0); i++)
        {
            for(int j = 0; j < sectorGrid.GetLength(1); j++)
            {
                if(sectorGrid[i, j] >= 0)
                {
                    Vector3 pos = new Vector3(i * _sector.GridSize, 0f, j * _sector.GridSize) - new Vector3(_sector.TotalSize / 2f, 0f, _sector.TotalSize / 2f);
                    GameObject poi = Instantiate(_sector.POIList[sectorGrid[i, j]], pos, Quaternion.identity);
                    MessagePublisher.Instance.PublishMessage(new POICreatedMessage(this, poi));
                }
            }
        }
    }

    private void WarpSuccessfulHandler(Message msg)
    {
        // Succesful warp! Set up the next sector
        WarpSuccessMessage warpMsg = msg as WarpSuccessMessage;
        MessagePublisher.Instance.PublishImmediate(new POIClearMessage(this));
        _player.transform.position = Vector3.zero;
        LoadNextSector();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            // Quick and dirty way out
            Application.Quit();
        }
    }
}
