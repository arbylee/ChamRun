using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	public GameObject playerModel;
	private Vector3 offset;
	void Start () {
		offset = this.transform.position - playerModel.transform.position;
	}

	void LateUpdate () {
		if (playerModel != null) {
			Vector3 positionWithOffset = playerModel.transform.position + offset;
			this.transform.position = new Vector3 (positionWithOffset.x, this.transform.position.y, this.transform.position.z);
		}
	}
}
