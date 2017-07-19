using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif 

public class MenuManager : MonoBehaviour {

    public GameObject MainMenu; //Main menu || menu that is displayed by default
    public GameObject OtherMenu; //menu that is toggled off by default
    public int lvlSelect; //int for scene selector. allows multiple uses in different scenes


    //loads scene dependant on int set in editor
    public void LevelStart()
    {
        SceneManager.LoadScene(lvlSelect);

    }


    // Used to toggle menu panels
    public void MenuToggle()
    {
        if (MainMenu.activeSelf)
        {
            MainMenu.SetActive(false);
            OtherMenu.SetActive(true);
        }
        else
        {
            MainMenu.SetActive(true);
            OtherMenu.SetActive(false);
        }
    }

    //Quits from application
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

   
}
