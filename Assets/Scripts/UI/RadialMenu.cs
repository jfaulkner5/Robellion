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
	private RectTransform rect;

	public GameObject towerPlatformMenu;	//Tower platform menu that is child to radial menu.
	public GameObject existingTowerMenu;	//Existing tower menu that is child to radial menu.

	public Button[] towerPlatformMenuButtons;
	public Button[] existingTowerMenuButtons;

	public float openSpeed;

	//Selected objects
	public TowerPlatform selectedTowerPlatform;
	public Tower selectedTower;

	void Start ()
	{
		rect = GetComponent<RectTransform>();

		//Disable the radial menu objects.
		towerPlatformMenu.SetActive(false);
		existingTowerMenu.SetActive(false);
	}

	void Update ()
	{
		//Checks for a single touch input.
		if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
		{
			if(GameManager.gm.canBuildOrModify)
			{
				RaycastToPlatformOrTower(Input.GetTouch(0).position);
			}
		}

		//Checks for a mouse click.
		if(Input.GetMouseButtonDown(0))
		{
			if(GameManager.gm.canBuildOrModify)
			{
				RaycastToPlatformOrTower(Input.mousePosition);
			}
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
				if(hit.collider.gameObject.layer == 8)
				{
					if(!hit.collider.GetComponent<TowerPlatform>().hasTower)
					{
						SetRadialMenu(posOnScreen, hit.collider.GetComponent<TowerPlatform>());
					}
				}
				else if(hit.collider.gameObject.layer == 9)
				{
					SetRadialMenu(posOnScreen, hit.collider.GetComponent<Tower>());
				}
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
	void SetRadialMenu (Vector2 posOnScreen, TowerPlatform towerPlatform)
	{
		towerPlatformMenu.SetActive(true);
		StartCoroutine(RadialMenuOpenAnimation());

		SetTowerPlatformMenuButtons(towerPlatform);

		//Set the radial menu position to the same as the touch position on screen.
		transform.localPosition = new Vector3(posOnScreen.x - (Screen.width / 2), posOnScreen.y - (Screen.height / 2), 0);

		FixRadialMenuPosition();
	}

	//Enables the existing tower platform menu.
	void SetRadialMenu (Vector2 posOnScreen, Tower tower)
	{
		existingTowerMenu.SetActive(true);
		StartCoroutine(RadialMenuOpenAnimation());

		SetExistingTowerMenuButtons(tower);

		//Set the radial menu position to the same as the touch position on screen.
		transform.localPosition = new Vector3(posOnScreen.x - (Screen.width / 2), posOnScreen.y - (Screen.height / 2), 0);

		FixRadialMenuPosition();
	}

	//Increases the scale of the radial menu over time to 1,1,1.
	IEnumerator RadialMenuOpenAnimation ()
	{
		transform.localScale = Vector3.zero;

		while(transform.localScale.x < 1.0f)
		{
			transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.one, openSpeed * Time.deltaTime);
			yield return null;
		}

		transform.localScale = Vector3.one;
	}

	//Decreases the scale of the radial menu over time to 0,0,0.
	IEnumerator RadialMenuCloseAnimation ()
	{
		transform.localScale = Vector3.one;

		while(transform.localScale.x > 0.0f)
		{
			transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.zero, openSpeed * Time.deltaTime);
			yield return null;
		}

		transform.localScale = Vector3.zero;

		//Disable the radial menu objects.
		towerPlatformMenu.SetActive(false);
		existingTowerMenu.SetActive(false);
	}

	//De activates the radial menu object.
	void DisableRadialMenu ()
	{
		StartCoroutine(RadialMenuCloseAnimation());
	}

	//Sets the tower platform buttons to disable when pressed.
	//Sets the tower platform buttons to build a tower on the platform when pressed.
	void SetTowerPlatformMenuButtons (TowerPlatform towerPlatform)
	{
		foreach(Button button in towerPlatformMenuButtons)
		{
			button.onClick.RemoveAllListeners();
			button.onClick.AddListener(DisableRadialMenu);
		}

		towerPlatformMenuButtons[0].onClick.AddListener(() => towerPlatform.BuildTower(TowerType.RobotArm));
		towerPlatformMenuButtons[1].onClick.AddListener(() => towerPlatform.BuildTower(TowerType.Crusher));
		towerPlatformMenuButtons[2].onClick.AddListener(() => towerPlatform.BuildTower(TowerType.Lazer));
		towerPlatformMenuButtons[3].onClick.AddListener(() => towerPlatform.BuildTower(TowerType.AcidEtcher));
		//towerPlatformMenuButtons[4].onClick.AddListener(towerPlatform.BuildTower(TowerType.TeslaCoil));
	}

	//Sets the existing tower buttons to disable when pressed.
	//Sets the upgrade button to upgrade the tower when pressed.
	//Sets the sell button to sell the tower when pressed.
	void SetExistingTowerMenuButtons (Tower tower)
	{
		foreach(Button button in existingTowerMenuButtons)
		{
			button.onClick.RemoveAllListeners();
			button.onClick.AddListener(DisableRadialMenu);
		}

		//existingTowerMenuButtons[0].onClick.AddListener();
		//existingTowerMenuButtons[0].onClick.AddListener();
	}

	//Makes sure that the radial menu doesn't go off screen.
	void FixRadialMenuPosition ()
	{
		float clampX = Mathf.Clamp(transform.localPosition.x, (-Screen.width / 2) + (rect.sizeDelta.x / 2) + 10, (Screen.width / 2) - (rect.sizeDelta.x / 2) - 10);
		float clampY = Mathf.Clamp(transform.localPosition.y, (-Screen.height / 2) + (rect.sizeDelta.y / 2) + 10, (Screen.height / 2) - (rect.sizeDelta.y / 2) - 10);

		transform.localPosition = new Vector3(clampX, clampY, 0);
	}
}
