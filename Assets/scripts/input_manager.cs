using UnityEngine;
using System.Collections;

public class input_manager : MonoBehaviour {

	public static bool is_paused = false;
	private bool pause_down = false;
	private float velocity = 7f;
	private Vector2 current_direction = new Vector2(0f,0f);
	private Vector2 last_direction = new Vector2(0f,-1f);

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
		float move_x = 0;
		float move_y = 0;
		pause_down = Input.GetKey (KeyCode.Escape);

		if (!is_paused) {
			
			if (!is_paused && (Input.GetKey (KeyCode.S))) {
				//GameObject.Find ("leila").GetComponent<leila_walk> ().move_leila (0,velocity);
				move_y--;
			}
			if (!is_paused  && (Input.GetKey (KeyCode.A))) {
				//GameObject.Find ("leila").GetComponent<leila_walk> ().move_leila (1,velocity);
				move_x--;
			}
			if (!is_paused && (Input.GetKey (KeyCode.W))) {
				//GameObject.Find ("leila").GetComponent<leila_walk> ().move_leila (2,velocity);
				move_y++;
			}
			if (!is_paused && (Input.GetKey (KeyCode.D))) {
				//GameObject.Find ("leila").GetComponent<leila_walk> ().move_leila (3,velocity);
				move_x++;
			}
			if (move_x != 0 && move_y == 0) {
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
			}
				
			if (Input.GetMouseButtonDown (0)) {
				Debug.Log ("mouse button down");
				GameObject.Find ("player").GetComponent<player_controller> ().do_attack ();
			}


			current_direction = new Vector2 (move_x, move_y);
			if (current_direction.magnitude > 0) {
				last_direction = current_direction;
			}


		}
	}
}
