using UnityEngine;
using System.Collections;

public class ComputerCar : MonoBehaviour {
	public Main main;

	public BezierSpline spline;

	public ComputerCar slave;

	public bool masterOnInnerTrack;

	public bool reverse;

	public float distance;
	
	float targetDistance;

	bool onInnerTrack;

	Vector3 targetScale = Vector3.one;

	EdgeCollider2D scoreCollider;

	// Use this for initialization
	void Start () {
		targetDistance = distance;
		scoreCollider = GetComponentInChildren<EdgeCollider2D>();
	}

	void ChangeTrack(bool onInnerTrack) {
		if (onInnerTrack) {
			targetScale = new Vector3(.88f, .78f, 1f);
		} else {
			targetScale = Vector3.one;
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if ((other.name == "Top Left") || (other.name == "Bottom Right")) {
			if (gameObject.name == "Computer Car 0") {
				onInnerTrack = (Random.Range(0, 2) == 1);
			} else if (gameObject.name == "Computer Car 1") {
				if (main.score > 5) {
					if (scoreCollider.enabled) {
						onInnerTrack = (Random.Range(0, 2) == 1);
					}
					targetDistance = 0.12f;
					scoreCollider.enabled = true;
				} else {
					onInnerTrack = masterOnInnerTrack;
				}
			} else {
				if (main.score > 15) {
					if (scoreCollider.enabled) {
						onInnerTrack = (Random.Range(0, 2) == 1);
					}
					targetDistance = 0.12f;
					GetComponentInChildren<EdgeCollider2D>().enabled = true;
				} else {
					onInnerTrack = masterOnInnerTrack;
				}
			}

			ChangeTrack(onInnerTrack);
			if (slave) {
				slave.masterOnInnerTrack = onInnerTrack;
			}
		}
	}

	public void ActOnProgress(float masterProgress) {
		float progress;
		if (reverse) {
			progress = 1f - masterProgress - distance;
			if (progress < 0f) {
				progress += 1f;
			}
		} else {
			progress = masterProgress + distance;
			if (progress > 1f) {
				progress -= 1f;
			}
		}

		if (gameObject.name == "Computer Car 0") {
			distance = Mathf.Lerp(distance, 0f, Time.deltaTime / 2);
		}

		Vector3 position = spline.GetPoint(progress);
		gameObject.transform.localPosition = position;
		
		Vector3 dir = spline.GetDirection(progress);
		float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg + 90f;
		Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		gameObject.transform.rotation = rotation;

		if (slave) {
			slave.ActOnProgress(progress);
		}
	}

	// Update is called once per frame
	void Update () {
		spline.gameObject.transform.localScale = Vector3.Lerp(
			spline.gameObject.transform.localScale, targetScale, 2 * Time.deltaTime);
		distance = Mathf.Lerp(distance, targetDistance, Time.deltaTime);
	}
}
