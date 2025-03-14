using UnityEngine;
using UnityEngine.SceneManagement;

public class CurrentScenetManager : MonoBehaviour
{

    public GameObject pauseMenuScreen;

    public GameObject gameOverScreen;
    public VoidEventChannel onPlayerDeath;

    public VoidEventChannel onGameResume;
    public VoidEventChannel onGamePause;

    private void OnEnable(){
        onPlayerDeath.OnEventRaised += Die;
        
    }
    private void OnDisnable(){
        onPlayerDeath.OnEventRaised -= Die;
    }

    private void Die(){
        gameOverScreen.SetActive(true);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameOverScreen.SetActive(false);

        pauseMenuScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            if(Time.timeScale==0){
                Time.timeScale=1;
                pauseMenuScreen.SetActive(false);
                onGamePause.Raise();
            }else{
            Time.timeScale=0;
            pauseMenuScreen.SetActive(true);
            onGameResume.Raise();
            }
        }
        if (Input.GetKeyDown(KeyCode.R)){
            RestartGame();
        }
    }
    public void RestartGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ResumeGame(){
        Time.timeScale=1;
        pauseMenuScreen.SetActive(false);
    }
}
