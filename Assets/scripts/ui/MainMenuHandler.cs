using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenuHandler : MonoBehaviour{
    public Image fadeImage;
    bool startFade = false;
    bool finishedFade = false;
    public static bool fadeIn = true;
    public string funcToExec;



    private void Update(){
        if(startFade && fadeIn){
            fade();
        }
        else if(startFade && !fadeIn){
            fadeOut();
        }

        if(SceneManager.GetActiveScene().name == "Game" && !fadeIn){
            fadeIn = false;
            startFade = true;
        }   

        if(finishedFade && funcToExec != null && funcToExec != ""){
            if(funcToExec == "start"){
                start();
            }
            else if(funcToExec == "quit"){
                quit();
            }
            else if(funcToExec == "openSettings"){
                openSettings();
            }
            else{
                Debug.Log("Function not found [FADE]");
                Application.Quit();
            }
        }
    }

    public void setFunc(string func){
        funcToExec = func;
        startFade = true;

    }


    public void fade(){
        Color temp = fadeImage.color;
        temp.a += 1f * Time.deltaTime;
        temp.a = Mathf.Clamp(temp.a, 0, 1);
        fadeImage.color = temp;
        if(temp.a >= 1){
            finishedFade = true;
        }
    }


    public void fadeOut(){
        Color temp = fadeImage.color;
        temp.a -= 1f * Time.deltaTime;
        temp.a = Mathf.Clamp(temp.a, 0, 1);
        fadeImage.color = temp;
        if(temp.a <= 0){
            finishedFade = true;
        }
    }
    
    public void start(){
        SceneManager.LoadScene("Game");
        fadeIn = false;
        startFade = true;
    }

    public void quit(){
        Application.Quit();
    }

    public void openSettings(){

    }
}
