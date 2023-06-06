using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Credits : MonoBehaviour
{
    private InputMaster inputMaster;
    public GameObject fadeToBlack;
    public GameObject fadeLocation;

    private void OnEnable()
    {
        inputMaster.Enable();
    }

    private void OnDisable()
    {
        inputMaster.Disable();
    }

    void Awake()
    {
        GameManager.Instance.lives = 3;
        inputMaster = new InputMaster();
        inputMaster.Player.Menu.performed += ctx => ReturnToMain();
    }

    public void PlayMusic()
    {
        AudioManager.instance.PlayMusic("creditmusic", 0f);
    }

    public void PlayWarp()
    {
        AudioManager.instance.PlayMusic("warp", 5f);
        StartCoroutine(StopWarp());
    }

    IEnumerator StopWarp()
    {
        yield return new WaitForSeconds(13);
        AudioManager.instance.StopMusic("warp", 0f);
    }

    private void ReturnToMain()
    {
        AudioManager.instance.StopMusic("creditmusic", 3f);
        Instantiate(fadeToBlack, fadeLocation.transform.position, Quaternion.identity);
        StartCoroutine(LoadMain());
    }

    IEnumerator LoadMain()
    {
        CheckpointManager.Instance.ResetCheckpoint();
        GameManager.Instance.isGameOver = false;
        yield return new WaitForSecondsRealtime(3.1f);
        SceneLoader.LoadScene(1);
    }
}
