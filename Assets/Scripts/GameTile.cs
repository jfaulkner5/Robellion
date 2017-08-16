using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTile : MonoBehaviour {

    public Material GameTileMaterial;
    public Material AltGameTileMaterial;

    public MeshRenderer meshRenderer;

	// Use this for initialization
	void Start () {
		if(transform.position.x % 2 == transform.position.z %2 || transform.position.x % 2 == -transform.position.z % 2)
        {
            meshRenderer.material = GameTileMaterial;
        }
        else
        {
            meshRenderer.material = AltGameTileMaterial;
        }
	}
}
