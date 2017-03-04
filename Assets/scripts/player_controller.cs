using UnityEngine;
using System.Collections;

public class player_controller : MonoBehaviour {

	public Sprite[] sprite_list;
	private int direction = 0;
	public int moving = 0;
	private int current_frame = 0;
	private float angle;
	private Vector3 mouse_position;
	private Vector3 player_position;
	private Vector3 weapon_position;
	private bool attacking = false;
	private float attack_start_time;
	private float attack_time_length = 0.25f;
	private bool recovering = false;
	private float recover_start_time;
	private float recover_time_length = 0.25f;


	void OnCollisionEnter2D(Collision2D coll){
		if (coll.gameObject.tag == "enemy"){
			//GameObject.Find ("game_over_text").GetComponent<game_over_script> ().game_over ();
		}
	}

	public void do_attack(){
		Debug.Log ("do attack");
		if (!attacking && !recovering) {
			attack_start_time = Time.time;
		}
		attacking = true;
	}

	public void do_recover(){
		Debug.Log ("do recover");
		if (!recovering && !attacking) {
			recover_start_time = Time.time;
		}
		recovering = true;
	}

	private void set_weapon_rotation(){
		mouse_position = Input.mousePosition;
		weapon_position = Camera.main.WorldToScreenPoint (this.transform.GetChild(1).transform.GetChild(0).transform.position);
		angle = Mathf.Atan2 (mouse_position.y - weapon_position.y, mouse_position.x - weapon_position.x) * Mathf.Rad2Deg;
		this.transform.GetChild(1).transform.GetChild(0).transform.rotation = Quaternion.Euler (new Vector3 (0, 0, angle-90));
	}

	private void set_rotation(){
		mouse_position = Input.mousePosition;
		player_position = Camera.main.WorldToScreenPoint (this.transform.position);
		angle = Mathf.Atan2 (mouse_position.y - player_position.y, mouse_position.x - player_position.x) * Mathf.Rad2Deg;
		this.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, angle-90));
	}

	public void move_player(int in_direction, float velocity_in){
		direction = in_direction;
		//moving = 1;
		switch (in_direction) {
		case 0:
			this.transform.Translate ((Vector3.down * velocity_in) * Time.deltaTime,Space.World);
			break;
		case 1:
			this.transform.Translate ((Vector3.left * velocity_in) * Time.deltaTime,Space.World);
			break;
		case 2:
			this.transform.Translate ((Vector3.up * velocity_in) * Time.deltaTime,Space.World);
			break;
		case 3:
			this.transform.Translate ((Vector3.right * velocity_in) * Time.deltaTime,Space.World);
			break;
		}
	}

	public void translate_player(int in_direction, float velocity_in){
		direction = in_direction;
		switch (in_direction) {
		case 0:
			this.transform.Translate ((Vector3.down * velocity_in));
			break;
		case 1:
			this.transform.Translate ((Vector3.left * velocity_in));
			break;
		case 2:
			this.transform.Translate ((Vector3.up * velocity_in));
			break;
		case 3:
			this.transform.Translate ((Vector3.right * velocity_in));
			break;
		}
	}

	void Start () {

	}

	void Update () {
		/*if (1 == moving) {
			current_frame = (int)((15 * Time.fixedTime) % 12);
			GetComponent<SpriteRenderer> ().sprite = sprite_list [current_frame];
		} else {
			current_frame = 0;
			GetComponent<SpriteRenderer> ().sprite = sprite_list [current_frame];
		}*/
		set_rotation ();
		//set_weapon_rotation ();
		//this.transform.GetChild (1).transform.localEulerAngles = arm_rotation + new Vector3 (0, 0, 1);
		//arm_rotation = this.transform.GetChild (1).transform.localEulerAngles;
		if (attacking) {
			if (this.transform.GetChild (1).transform.localRotation.eulerAngles.z < 90f) {
				this.transform.GetChild (1).transform.RotateAround (this.transform.position, Vector3.forward, 500 * Time.deltaTime);
			} 
			if (this.transform.GetChild (1).transform.localRotation.eulerAngles.z > 90f) {
				this.transform.GetChild (1).transform.RotateAround (this.transform.position, Vector3.forward, 90-this.transform.GetChild (1).transform.localRotation.eulerAngles.z);
			} 
			if ((Time.time - attack_start_time) >= attack_time_length) {
				attacking = false;
				do_recover ();
			}
		}
		if (recovering) {
			if (this.transform.GetChild (1).transform.localRotation.eulerAngles.z < 100f) {
				this.transform.GetChild (1).transform.RotateAround (this.transform.position, Vector3.forward, -500 * Time.deltaTime);
			}
			if (this.transform.GetChild (1).transform.localRotation.eulerAngles.z > 180f) {
				this.transform.GetChild (1).transform.RotateAround (this.transform.position, Vector3.forward, -this.transform.GetChild (1).transform.localRotation.eulerAngles.z);
			} 
			if ((Time.time - recover_start_time) >= recover_time_length) {
				recovering = false;
			}
		}
		if (!attacking && !recovering) {
			this.transform.GetChild (1).transform.RotateAround (this.transform.position, Vector3.forward, -this.transform.GetChild (1).transform.localRotation.eulerAngles.z);
		}
	}
}
