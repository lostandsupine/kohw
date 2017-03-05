using UnityEngine;
using System.Collections;

public class input_manager : MonoBehaviour {

	public static bool is_paused = false;
	private bool pause_down = false;
	private Vector2 current_direction = new Vector2(0f,0f);
	private Vector2 last_direction = new Vector2(0f,-1f);
	private float move_x = 0;
	private float move_y = 0;

	void Start () {

	}

	public Vector2 get_direction(){
		return(last_direction);
	}
		
	void Update () {
		if((Input.GetKey(KeyCode.Escape)) && !pause_down)
		{
			Time.timeScale = 1.0f - Time.timeScale;
			is_paused = !is_paused;
			pause_down = true;
		}
		move_x = 0;
		move_y = 0;
		pause_down = Input.GetKey (KeyCode.Escape);

		if (!is_paused) {
			
			if ((Input.GetKey (KeyCode.S))) {
				//GameObject.Find ("leila").GetComponent<leila_walk> ().move_leila (0,velocity);
				move_y--;
			}
			if ((Input.GetKey (KeyCode.A))) {
				//GameObject.Find ("leila").GetComponent<leila_walk> ().move_leila (1,velocity);
				move_x--;
			}
			if ((Input.GetKey (KeyCode.W))) {
				//GameObject.Find ("leila").GetComponent<leila_walk> ().move_leila (2,velocity);
				move_y++;
			}
			if ((Input.GetKey (KeyCode.D))) {
				//GameObject.Find ("leila").GetComponent<leila_walk> ().move_leila (3,velocity);
				move_x++;
			}
			/*if (move_x != 0 && move_y == 0) {
				GameObject.Find ("player").GetComponent<player_controller> ().move_player (2 + (int)move_x, velocity);
				GameObject.Find ("player").GetComponent<player_controller> ().moving = 1;
				Debug.Log("moving input");
			} else if (move_x == 0 && move_y != 0) {
				GameObject.Find ("player").GetComponent<player_controller> ().move_player (1 + (int)move_y, velocity);
				GameObject.Find ("player").GetComponent<player_controller> ().moving = 1;
				Debug.Log("moving input");
			} else if (move_x != 0 && move_y != 0) {
				GameObject.Find ("player").GetComponent<player_controller> ().move_player (2 + (int)move_x, velocity / 1.414214f);
				GameObject.Find ("player").GetComponent<player_controller> ().move_player (1 + (int)move_y, velocity / 1.414214f);
				GameObject.Find ("player").GetComponent<player_controller> ().moving = 1;
				Debug.Log("moving input");
			} else {
				GameObject.Find ("player").GetComponent<player_controller> ().moving = 0;
			}*/
			if (move_x != 0 || move_y != 0) {
				GameObject.Find ("player").GetComponent<player_controller> ().do_move((int)move_x,(int)move_y);
			}

			if (Input.GetKey (KeyCode.Space)) {
				GameObject.Find ("player").GetComponent<player_controller> ().do_jump ((int)move_x,(int)move_y);
			}
				
			if (Input.GetMouseButtonDown (0)) {
				GameObject.Find ("player").GetComponent<player_controller> ().do_attack ();
			}
			if (Input.GetMouseButton (1)) {
				GameObject.Find ("player").GetComponent<player_controller> ().do_block ();
			} else {
				//GameObject.Find ("player").GetComponent<player_controller> ().do_block_recover ();
			}

			current_direction = new Vector2 (move_x, move_y);
			if (current_direction.magnitude > 0) {
				last_direction = current_direction;
			}


		}
	}
}
