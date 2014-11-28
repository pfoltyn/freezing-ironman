using UnityEngine;
using System.Collections;

public class PlayerCar : MonoBehaviour {
	public Main main;

	public BezierSpline spline;

	public float duration;

	public float progress;

	public ComputerCar slave;

	Animator animator;
	GameObject outer;
	bool onInnerCircuit;

	// Use this for initialization
	void Start () {
		animator = GetComponentInParent<Animator>();
		outer = spline.gameObject;
		onInnerCircuit = false;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.name == "Score Trigger") {
			main.score++;
		} else if (other.name.StartsWith("Computer Car")) {
			main.state = GameState.GameOver;
			animator.SetBool("Bum", true);
			other.gameObject.GetComponent<Animator>().SetBool("Bum", true);
		}
	}

	private void ActOnProgress(float step) {
		progress += step;
		if (progress > 1f) {
			progress -= 1f;
		}

		Vector3 position = spline.GetPoint(progress);
		gameObject.transform.localPosition = position;
		
		Vector3 dir = spline.GetDirection(progress);
		float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg - 90f;
		Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		gameObject.transform.rotation = rotation;
	}

	// Update is called once per frame
	void Update () {
		switch (main.state) {
		case GameState.GameRun:
			int touchIndex = 0;
			while (touchIndex < Input.touchCount) {
				Touch touch = Input.GetTouch(touchIndex);
				if (touch.phase == TouchPhase.Began) {
					onInnerCircuit = !onInnerCircuit;
				}
				++touchIndex;
			}

			if (!onInnerCircuit) {
				outer.transform.localScale = Vector3.Lerp(
					outer.transform.localScale, Vector3.one, 10 * Time.deltaTime);
			} else {
				outer.transform.localScale = Vector3.Lerp(
					outer.transform.localScale, new Vector3(.88f, .78f, 1f), 10 * Time.deltaTime);
			}

			float step = Time.deltaTime / duration;
			ActOnProgress(step);
			slave.ActOnProgress(progress);
			break;
		case GameState.GameOver:
			break;
		default:
			break;
		}
	}
}
