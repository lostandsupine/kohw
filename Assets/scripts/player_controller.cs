using UnityEngine;
using System.Collections;

public class player_controller : MonoBehaviour {

	public Sprite[] sprite_list;
	private int direction = 0;
	public bool moving;
	private int current_frame = 0;
	private float angle;
	private Vector3 mouse_position;
	private Vector3 player_position;
	private Vector3 weapon_position;
	private bool attacking = false;
	private float attack_start_time;
	private float attack_time_length = 0.2f;
	private bool recovering = false;
	private float recover_start_time;
	private float recover_time_length = 0.2f;
	private bool jumping = false;
	private float jump_start_time;
	private float jump_time_length = 0.15f;
	private float velocity = 7f;
	private float jump_velocity = 18f;
	private int move_x = 0;
	private int move_y = 0;
	private int jump_x = 0;
	private int jump_y = 0;
	private bool jump_recovering = false;
	private float jump_recover_start_time;
	private float jump_recover_time_length = 1f;
	private bool blocking = false;
	private float block_start_time;
	private float block_time_length = 0.2f;
	private bool block_recovering = false;
	private float block_recover_start_time;
	private float block_recover_time_length = 0.2f;


	void OnCollisionEnter2D(Collision2D coll){
		if (coll.gameObject.tag == "enemy"){
			//GameObject.Find ("game_over_text").GetComponent<game_over_script> ().game_over ();
		}
	}

	public void do_attack(){
		Debug.Log ("do attack");
		if (!attacking && !recovering && !blocking && !block_recovering) {
			attack_start_time = Time.time;
			attacking = true;
		}
	}

	public void do_block(){
		Debug.Log ("do block");
		if (!attacking && !recovering && !block_recovering) {
			block_start_time = Time.time;
			blocking = true;
		}

	}

	public void do_block_recover(){
		Debug.Log ("do block recover");
		if (!block_recovering && blocking) {
			block_recover_start_time = Time.time;
			block_recovering = true;
			blocking = false;
		}
	}

	public void do_recover(){
		Debug.Log ("do recover");
		if (!recovering && !attacking) {
			recover_start_time = Time.time;
			recovering = true;
		}
	}

	public void do_jump(int dirx, int diry){
		//Debug.Log ("do jump " + dirx.ToString () + " : " + diry.ToString());
		if (!jump_recovering) {
			if (!jumping) {
				jump_x = dirx;
				jump_y = diry;
				jump_start_time = Time.time;
			}
			jumping = true;
		}
	}

	public void do_move(int dirx, int diry){
		//Debug.Log ("do move " + dirx.ToString () + " : " + diry.ToString ());
		moving = true;
		move_x = dirx;
		move_y = diry;
	}

	private void set_weapon_rotation(){
		if (!blocking) {
			mouse_position = (Input.mousePosition - Camera.main.WorldToScreenPoint (this.transform.position)) * 100000f;
			weapon_position = Camera.main.WorldToScreenPoint (this.transform.GetChild (1).transform.GetChild (0).transform.position);
			angle = Mathf.Atan2 (mouse_position.y - weapon_position.y, mouse_position.x - weapon_position.x) * Mathf.Rad2Deg;
			//this.transform.GetChild(1).transform.GetChild(0).transform.rotation = Quaternion.Euler (new Vector3 (0, 0, angle-90));
			this.transform.GetChild (1).transform.GetChild (0).transform.RotateAround (this.transform.GetChild (1).transform.position,
				Vector3.forward,
				angle - this.transform.GetChild (1).transform.GetChild (0).transform.rotation.eulerAngles.z - 90f);
		} else {
			weapon_position = Camera.main.WorldToScreenPoint (this.transform.GetChild (1).transform.GetChild (0).transform.position);
			angle = Mathf.Atan2 (player_position.y - weapon_position.y, player_position.x - weapon_position.x) * Mathf.Rad2Deg;
			//this.transform.GetChild(1).transform.GetChild(0).transform.rotation = Quaternion.Euler (new Vector3 (0, 0, angle-90));
			this.transform.GetChild (1).transform.GetChild (0).transform.RotateAround (this.transform.GetChild (1).transform.position,
				Vector3.forward,
				angle - this.transform.GetChild (1).transform.GetChild (0).transform.rotation.eulerAngles.z + 187f);
		}
	}

	private void set_rotation(){
		mouse_position = Input.mousePosition;
		player_position = Camera.main.WorldToScreenPoint (this.transform.position);
		angle = Mathf.Atan2 (mouse_position.y - player_position.y, mouse_position.x - player_position.x) * Mathf.Rad2Deg;
		this.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, angle-90));
	}

	public void move_player(int in_direction, float velocity_in){
		//direction = in_direction;
		switch (in_direction) {
		case 0:
			this.transform.Translate ((Vector3.down * velocity_in) * Time.deltaTime, Space.World);
			break;
		case 1:
			this.transform.Translate ((Vector3.left * velocity_in) * Time.deltaTime, Space.World);
			break;
		case 2:
			this.transform.Translate ((Vector3.up * velocity_in) * Time.deltaTime, Space.World);
			break;
		case 3:
			this.transform.Translate ((Vector3.right * velocity_in) * Time.deltaTime, Space.World);
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
		if (jump_recovering) {
			if ((Time.time - jump_recover_start_time) >= jump_recover_time_length) {
				jump_recovering = false;
			}
		}
		if (jumping) {
			if ((Time.time - jump_start_time) >= jump_time_length) {
				jumping = false;
				jump_recovering = true;
				Debug.Log ("jump recovering");
				jump_recover_start_time = Time.time;
			} else {
				Debug.Log ("jumping");
				if (jump_x != 0 && jump_y == 0) {
					GameObject.Find ("player").GetComponent<player_controller> ().move_player (2 + jump_x, jump_velocity);
				} else if (jump_x == 0 && jump_y != 0) {
					GameObject.Find ("player").GetComponent<player_controller> ().move_player (1 + jump_y, jump_velocity);
				} else if (jump_x != 0 && jump_y != 0) {
					GameObject.Find ("player").GetComponent<player_controller> ().move_player (2 + jump_x, jump_velocity / 1.414214f);
					GameObject.Find ("player").GetComponent<player_controller> ().move_player (1 + jump_y, jump_velocity / 1.414214f);
				} else {

				}
			}
		}
		if (!jumping && moving) {
			Debug.Log ("moving");
			if (move_x != 0 && move_y == 0) {
				GameObject.Find ("player").GetComponent<player_controller> ().move_player (2 + move_x, velocity);
			} else if (move_x == 0 && move_y != 0) {
				GameObject.Find ("player").GetComponent<player_controller> ().move_player (1 + move_y, velocity);
			} else if (move_x != 0 && move_y != 0) {
				GameObject.Find ("player").GetComponent<player_controller> ().move_player (2 + move_x, velocity / 1.414214f);
				GameObject.Find ("player").GetComponent<player_controller> ().move_player (1 + move_y, velocity / 1.414214f);
			}
		}
		moving = false;

		set_rotation ();

		//this.transform.GetChild (1).transform.localEulerAngles = arm_rotation + new Vector3 (0, 0, 1);
		//arm_rotation = this.transform.GetChild (1).transform.localEulerAngles;
		if (blocking) {
			Debug.Log ("blocking");
			/*if (this.transform.GetChild (1).transform.localRotation.eulerAngles.z < 90f) {
				this.transform.GetChild (1).transform.RotateAround (this.transform.position , Vector3.forward, 500 * Time.deltaTime);
			} 
			if (this.transform.GetChild (1).transform.localRotation.eulerAngles.z > 90f) {
				this.transform.GetChild (1).transform.RotateAround (this.transform.position, Vector3.forward, 90-this.transform.GetChild (1).transform.localRotation.eulerAngles.z);
			} */
			if (this.transform.GetChild (1).transform.localRotation.eulerAngles.z < 23f) {
				this.transform.GetChild (1).transform.RotateAround (this.transform.GetChild (0).position , Vector3.forward, 300 * Time.deltaTime);
			} 
			if (this.transform.GetChild (1).transform.localRotation.eulerAngles.z > 23f) {
				this.transform.GetChild (1).transform.RotateAround (this.transform.GetChild (0).position, Vector3.forward, 23-this.transform.GetChild (1).transform.localRotation.eulerAngles.z);
			} 
			if ((Time.time - block_start_time) >= block_time_length) {
				do_block_recover ();
			}
		}
		if (block_recovering) {
			Debug.Log ("block recovering");
			/*if (this.transform.GetChild (1).transform.localRotation.eulerAngles.z < 100f) {
				this.transform.GetChild (1).transform.RotateAround (this.transform.position, Vector3.forward, -500 * Time.deltaTime);
			}
			if (this.transform.GetChild (1).transform.localRotation.eulerAngles.z > 180f) {
				this.transform.GetChild (1).transform.RotateAround (this.transform.position, Vector3.forward, -this.transform.GetChild (1).transform.localRotation.eulerAngles.z);
			} */
			if (this.transform.GetChild (1).transform.localRotation.eulerAngles.z < 100f) {
				this.transform.GetChild (1).transform.RotateAround (this.transform.GetChild (0).position, Vector3.forward, -300 * Time.deltaTime);
			}
			if (this.transform.GetChild (1).transform.localRotation.eulerAngles.z > 180f) {
				this.transform.GetChild (1).transform.RotateAround (this.transform.GetChild (0).position, Vector3.forward, -this.transform.GetChild (1).transform.localRotation.eulerAngles.z);
			} 
			if ((Time.time - block_recover_start_time) >= block_recover_time_length) {
				block_recovering = false;
			}
		}
		if (attacking) {
			Debug.Log ("attacking");
			if (this.transform.GetChild (1).transform.localRotation.eulerAngles.z < 60f) {
				this.transform.GetChild (1).transform.RotateAround (this.transform.GetChild (0).position , Vector3.forward, 500 * Time.deltaTime);
			} 
			if (this.transform.GetChild (1).transform.localRotation.eulerAngles.z > 60f) {
				this.transform.GetChild (1).transform.RotateAround (this.transform.GetChild (0).position, Vector3.forward, 60-this.transform.GetChild (1).transform.localRotation.eulerAngles.z);
			} 
			if ((Time.time - attack_start_time) >= attack_time_length) {
				attacking = false;
				do_recover ();
			}
		}
		if (recovering) {
			Debug.Log ("attack recovering");
			if (this.transform.GetChild (1).transform.localRotation.eulerAngles.z < 100f) {
				this.transform.GetChild (1).transform.RotateAround (this.transform.GetChild (0).position, Vector3.forward, -500 * Time.deltaTime);
			}
			if (this.transform.GetChild (1).transform.localRotation.eulerAngles.z > 180f) {
				this.transform.GetChild (1).transform.RotateAround (this.transform.GetChild (0).position, Vector3.forward, -this.transform.GetChild (1).transform.localRotation.eulerAngles.z);
			} 
			if ((Time.time - recover_start_time) >= recover_time_length) {
				recovering = false;
			}
		}
		if (!attacking && !recovering && !blocking && !block_recovering) {
			//Debug.Log ("resetting");
			this.transform.GetChild (1).transform.RotateAround (this.transform.GetChild (0).position, Vector3.forward, -this.transform.GetChild (1).transform.localRotation.eulerAngles.z);
		}
		set_weapon_rotation ();
	}
}
