using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public GameObject tipsPannel, gameOverPannel;
    private PlayerController4 plscript;
    // Start is called before the first frame update
    void Start()
    {
        plscript = GameObject.Find("Player").GetComponent<PlayerController4>();
        Time.timeScale = 1;

        StartCoroutine(disableTips());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (plscript.gameOver)
        {
            gameOverPannel.SetActive(true);
            tipsPannel.SetActive(false);
        }
    }

    IEnumerator disableTips()
    {
        yield return new WaitForSeconds(5);
        tipsPannel.SetActive(false);
    }
}
