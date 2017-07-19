using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Manages the radial menu UI when tapping on a tower platform or existing tower.
/// </summary>
public class RadialMenu : MonoBehaviour 
{
	public GameObject towerPlatformMenu;	//Tower platform menu that is child to radial menu.
	public GameObject existingTowerMenu;	//Existing tower menu that is child to radial menu.

	public Button[] towerPlatformMenuButtons;
	public Button[] existingTowerMenuButtons;

	void Start ()
	{
		DisableRadialMenu();
	}

	void Update ()
	{
		//Checks for a single touch input.
		if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
		{
			RaycastToPlatformOrTower(Input.GetTouch(0).position);
		}

		//Checks for a mouse click.
		if(Input.GetMouseButtonDown(0))
		{
			RaycastToPlatformOrTower(Input.mousePosition);
		}
	}

	//Shoots a raycast in the direction of the touch or click on the screen.
	//If the raycast hits a tower or tower platform, it enables the radial menu.
	void RaycastToPlatformOrTower (Vector2 posOnScreen)
	{
		Ray ray = Camera.main.ScreenPointToRay(posOnScreen);
		RaycastHit hit;

		//Checks for a raycast hit on gameobjects with the layer 'TowerPlatform' or 'Tower'.
		if(Physics.Raycast(ray, out hit, 100, 1 << 8 | 1 << 9))
		{
			//Is the mouse cursor currently not over a UI element?
			if(!EventSystem.current.IsPointerOverGameObject())
			{
				SetRadialMenu(posOnScreen);
			}
		}
		else
		{
			//Is the mouse cursor currently not over a UI element?
			if(!EventSystem.current.IsPointerOverGameObject())
			{
				DisableRadialMenu();
			}
		}
	}

	//Enables the tower platform radial menu.
	void SetRadialMenu (Vector2 posOnScreen/*, TowerPlatform towerPlatform*/)
	{
		towerPlatformMenu.SetActive(true);

		SetTowerPlatformMenuButtons();

		transform.localPosition = new Vector3(posOnScreen.x - (Screen.width / 2), posOnScreen.y - (Screen.height / 2), 0);
	}

	//Enables the existing tower platform menu.
	/*void SetRadialMenu (Vector2 posOnScreen, Tower towerP)
	{

	}*/

	//De activates the radial menu object.
	void DisableRadialMenu ()
	{
		towerPlatformMenu.SetActive(false);
		existingTowerMenu.SetActive(false);
	}

	//Sets the tower platform buttons to disable when pressed.
	//Sets the tower platform buttons to build a tower on the platform when pressed.
	void SetTowerPlatformMenuButtons (/*TowerPlatform towerPlatform*/)
	{
		foreach(Button button in towerPlatformMenuButtons)
		{
			button.onClick.RemoveAllListeners();
			button.onClick.AddListener(DisableRadialMenu);
		}

		//towerPlatformMenuButtons[0].onClick.AddListener();
		//towerPlatformMenuButtons[0].onClick.AddListener();
		//towerPlatformMenuButtons[0].onClick.AddListener();
		//towerPlatformMenuButtons[0].onClick.AddListener();
		//towerPlatformMenuButtons[0].onClick.AddListener();
	}

	//Sets the existing tower buttons to disable when pressed.
	//Sets the upgrade button to upgrade the tower when pressed.
	//Sets the sell button to sell the tower when pressed.
	void SetExistingTowerMenuButtons (/*Tower tower*/)
	{
		foreach(Button button in existingTowerMenuButtons)
		{
			button.onClick.RemoveAllListeners();
			button.onClick.AddListener(DisableRadialMenu);
		}

		//existingTowerMenuButtons[0].onClick.AddListener();
		//existingTowerMenuButtons[0].onClick.AddListener();
	}
}
