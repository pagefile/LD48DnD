using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private BasicShip _player = default;
    private Scene _activeScene;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
