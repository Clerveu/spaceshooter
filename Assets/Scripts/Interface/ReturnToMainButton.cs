using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ReturnToMainButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        GameManager.Instance.isExitingToMenu = true;
        SceneLoader.LoadScene(1);
        Time.timeScale = 1;
        AudioManager.instance.StopAll();
        AudioManager.instance.UnpauseAll();
    }

}
