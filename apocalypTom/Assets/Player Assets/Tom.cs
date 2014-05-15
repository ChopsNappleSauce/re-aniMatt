using UnityEngine;
using System.Collections;

public class Tom : MonoBehaviour {

	public float moveSpeed = 10f;
	public float rotateSpeed = 10f;
	public float spawnRadius = 50f;
	public int mobCap = 100;
	public float zombieRate = 2f;
	public float resourceRate = 2f;
	public int invRows = 5;
	public float grabRadius = 3f;

	public Transform plainZombie;
	public Transform forestZombie;
	public Transform desertZombie;

	public Transform stick;
	public Transform rock;
	public Transform flax;

	public Rigidbody projectile;


	public float bulletSpeed = 1;

	private float health = 100f;
	private int mobCount = 0;
	private string weapon = "gun";
	private int ammo = 500;
	private int time = 0;
	private int counter = 0;
	private int kills = 0;
	private Vector2[,] inv;

	private Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		inv = new Vector2[invRows, invRows];
	}

	public Vector2[,] getInventory()
	{
		return inv;
	}

	public void takeDamage (float damage) {
		health -= damage;

		Debug.Log ("Health: " + health.ToString());

		if (health <= 0) {
			die();
		}
	}

	public void increaseKill () {
		kills++;
	}

	public float getHealth () {
		return health;
	}

	public int getMobCount() {
		return mobCount;
	}

	public string getWeapon() {
		return weapon;
	}

	public int getAmmo() {
		return ammo;
	}

	public int getKills() {
		return kills;
	}

	public int getTime(){
			return time;
	}

	public void decreaseMobCount() {
		mobCount--;
	}

	private void die () {
		Debug.Log ("Dead.");
		//Destroy(this);
	}

	private void trySpawnZombie() {
		if (mobCount < mobCap) {
			// pick a random point on a circle r=spawnRadius
			int deg = UnityEngine.Random.Range (0, 360);
			Vector3 circVector = new Vector3(spawnRadius, 0f, 0f);

			circVector = Quaternion.AngleAxis (deg, Vector3.up) * circVector;

			float x = transform.position.x + circVector.x;
			float z = transform.position.z + circVector.z;

			// Holy Shit Batman, can we get more verbose?
			switch (GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<GridManager>().getBiome (new Vector3(x, 0f, z))) {
				case 1:
					GameObject.Instantiate (plainZombie, new Vector3(x, 0f, z), Quaternion.Euler(270, 90, 0));
					break;
				case 2:
					GameObject.Instantiate (forestZombie, new Vector3(x, 0f, z), Quaternion.Euler(270, 90, 0));
					break;
				case 3:
					GameObject.Instantiate (desertZombie, new Vector3(x, 0f, z), Quaternion.Euler(270, 90, 0));
					break;
			}

			mobCount++;
		}
	}

	private void trySpawnResource() {
		// pick a random point on a circle r=spawnRadius
		int deg = UnityEngine.Random.Range (0, 360);
		Vector3 circVector = new Vector3(spawnRadius, 0f, 0f);
		
		circVector = Quaternion.AngleAxis (deg, Vector3.up) * circVector;
		
		float x = transform.position.x + circVector.x;
		float z = transform.position.z + circVector.z;
		
		// Holy Shit Batman, can we get more verbose?
		switch (GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<GridManager>().getBiome (new Vector3(x, 0f, z))) {
		case 1:
			GameObject.Instantiate (flax, new Vector3(x, 0f, z), Quaternion.Euler(flax.transform.eulerAngles.x,
			                                                                      flax.transform.eulerAngles.y,
			                                                                      flax.transform.eulerAngles.z));
			break;
		case 2:
			GameObject.Instantiate (stick, new Vector3(x, 0f, z), Quaternion.Euler(stick.transform.eulerAngles.x,
			                                                                       stick.transform.eulerAngles.y,
			                                                                       stick.transform.eulerAngles.z));
			break;
		case 3:
			GameObject.Instantiate (rock, new Vector3(x, 0f, z), Quaternion.Euler(rock.transform.eulerAngles.x,
			                                                                      rock.transform.eulerAngles.y,
			                                                                      rock.transform.eulerAngles.z));
			break;
		}
	}
	



	// Update is called once per frame
	void Update () {
		//gui scrGUI =;
		//GUITexture guiTexture = GetComponent

		if (health > 0) 
		{
			if (UnityEngine.Random.Range (0, 1000) < (zombieRate * 10)) {		// 2% chance to spawn a zombie per frame.
				trySpawnZombie ();
			}

			if (UnityEngine.Random.Range (0, 1000) < (resourceRate * 10)) {		// 40% chance to spawn a zombie per frame.
				trySpawnResource ();
			}

			float xDistance = 0;
			float yDistance = 0;

			if (Input.GetAxis ("Horizontal") > 0) {
					xDistance = 1;
			} else if (Input.GetAxis ("Horizontal") < 0) {
					xDistance = -1;
			}

			if (Input.GetAxis ("Vertical") > 0) {
					yDistance = 1;
			} else if (Input.GetAxis ("Vertical") < 0) {
					yDistance = -1;
			}

			//I didn't know if there was a built in time function so I just came up with this
			counter = counter + 1;
			if (counter >= 58) {
					time = time + 1;
					counter = 0;
			}

			if (Input.GetKey (KeyCode.X)) {
					transform.Translate (0f, 0f, moveSpeed * Time.deltaTime);
			} else if (Input.GetKey (KeyCode.Z)) {
					transform.Translate (0f, 0f, -moveSpeed * Time.deltaTime);
			}

			//Space to grab resources
			/*
			if(Input.GetKey(KeyCode.Space))
			{
				//Get all sticks and pick up ones close enough
				GameObject[] sticks = GameObject.FindGameObjectsWithTag("stick");
				foreach(GameObject stick in sticks)
				{
					float xSDif = stick.transform.position.x - this.transform.position.x;
					float ySDif = stick.transform.position.z - this.transform.position.z;
					float sDist = Mathf.Sqrt(Mathf.Pow(xSDif, 2f) + Mathf.Pow(ySDif, 2f));
					if(sDist < grabRadius)
					{
						bool foundSpot = false;
						for(int i = 0; i < invRows; i++)
						{
							for(int j = 0; j < invRows; j++)
							{
								//Debug.Log("[" + i + "," + j + "]:" + inv[i,j].x);
								if(inv[i,j].x < 2)
								{
									inv[i,j].x = 1;
									inv[i,j].y += 1;
									foundSpot = true;
									break;
								}
							}
						}

						if(foundSpot)
						{
							Debug.Log("Open Inv Spot");
						}
						else
						{
							Debug.Log("No Open Inv Spaces");
						}

						Destroy(stick);
					}
				}
			}
			*/

			float angle = Mathf.Atan2 (Input.mousePosition.y - Screen.height / 2, Input.mousePosition.x - Screen.width / 2) * Mathf.Rad2Deg;
			Vector3 currAngles = transform.eulerAngles;
			Quaternion target = Quaternion.Euler (currAngles.x, 180f - angle, currAngles.z);
			transform.rotation = Quaternion.Slerp (transform.rotation, target, Time.deltaTime * 15f);

			if (Input.GetMouseButtonDown (0)) {
					if (ammo > 0) {
							//anim.Play ("Strike");

							ammo--;
							Rigidbody bullet = Instantiate (projectile, transform.position, transform.rotation) as Rigidbody;
							bullet.velocity = transform.TransformDirection (new Vector3 (-bulletSpeed, 0, 0));
					}
			}

			if ((xDistance == 0) && (yDistance == 0)) {
					anim.Play ("Idle");
			}

			if ((xDistance != 0) || (yDistance != 0)) {
					anim.Play ("Run1");
			} else {
					anim.Play ("Idle");
			}

			// rotate
			Vector3 moveVec = new Vector3 (xDistance, 0f, yDistance);
			moveVec.Normalize ();
			moveVec *= moveSpeed * Time.deltaTime;
			transform.Translate (moveVec, Space.World);

			/*
			string invStr = "[";
			for(int i = 0; i < invRows; i++)
			{
				for(int j = 0; j < invRows; j++)
				{
					invStr += "(" + inv[i,j].x + "," + inv[i,j].y + ")";
				}
				invStr += "\n";
			}
			Debug.Log(invStr + "]");
			*/
		}
	}
}
