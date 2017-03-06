using UnityEngine;
using System.Collections;

public class bear_script : MonoBehaviour {

	private Vector3 player_position;
	private Vector3 bear_position;
	private float angle;
	private enum state {passive=0, aware=1, stalking=2, flanking=3, waiting=4, charging=4, feinting=5, attacking=6, chasing=7};
	private state bear_state = state.passive;
	private float aware_distance = 10f;
	private float aggro_distince = 5f;
	private float aggro_chance = 0.0f;
	private bool aggro;
	private float bear_velocity = 2f;
	private float rotation_velocity = 100f;

	private bool state_in_list(state state_in,state[] state_list){
		bool is_in = false;
		for (int i = 0; i < state_list.Length; i++){
			is_in = is_in || state_in == state_list [i];
		}
		return (is_in);
	}

	private float distance_to_player(){
		return ((GameObject.Find ("player").transform.position - this.transform.position).magnitude);
	}

	private void set_rotation(){
		Debug.Log ("setting rotation");
		player_position = GameObject.Find ("player").transform.position;
		bear_position = this.transform.position;
		angle = Mathf.Atan2 (player_position.y - bear_position.y, player_position.x - bear_position.x) * Mathf.Rad2Deg + 90f;
		if (angle < 0) {
			angle += 360f;
		}
		//Debug.Log (angle);
		float bear_angle = transform.localRotation.eulerAngles.z;
		//Debug.Log (bear_angle);
		if (Mathf.Abs (bear_angle - angle) < 1f) {
			this.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, angle));
		} else {
			if (bear_angle < angle) {
				if (Mathf.Abs (bear_angle - angle) < 180f) {
					this.transform.Rotate (new Vector3 (0, 0, 1) * rotation_velocity * Time.deltaTime);
				} else {
					this.transform.Rotate (new Vector3 (0, 0, -1) * rotation_velocity * Time.deltaTime);
				}
			} else {
				if (Mathf.Abs (bear_angle - angle) < 180f) {
					this.transform.Rotate (new Vector3 (0, 0, -1) * rotation_velocity * Time.deltaTime);
				} else {
					this.transform.Rotate (new Vector3 (0, 0, 1) * rotation_velocity * Time.deltaTime);
				}
			}
		}
	}

	// Use this for initialization
	void Start () {
		aggro_distince = Random.Range (1f, 5f);
		float foo = Random.Range (0f, 1f);
		aggro = (aggro_chance >= foo);
		Debug.Log (foo.ToString() + " " + aggro.ToString());
	}
	
	// Update is called once per frame
	void Update () {
		if (state_in_list(bear_state,new state[4]{state.aware,state.stalking,state.flanking,state.waiting})){
			set_rotation ();
		}
		if (bear_state == state.passive && distance_to_player () <= aware_distance) {
			Debug.Log ("bear aware");
			bear_state = state.aware;
		}
		if (bear_state == state.aware && distance_to_player () <= aggro_distince) {
			Debug.Log ("bear attacking");
			//bear_state = state.attacking;
		}
		if (bear_state == state.aware && aggro){
			Debug.Log ("bear stalking");
			//bear_state = state.stalking;
		}
		if (bear_state == state.stalking) {
			//this.transform.Translate ((Vector3.down * bear_velocity) * Time.deltaTime, Space.Self);
		}

		//Debug.Log (angle);
	}
}
