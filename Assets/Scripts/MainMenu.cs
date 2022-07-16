using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{

    public GameObject MainMenuContainer;
    public GameObject CreditsContainer;

    // Update is called once per frame
    public void StartGame(){
        
    }

    public void ShowCredits() {
        MainMenuContainer.SetActive(false);
        CreditsContainer.SetActive(true);
    }

    public void BackToMainMenu () {
        MainMenuContainer.SetActive(true);
        CreditsContainer.SetActive(false);
    }
}
