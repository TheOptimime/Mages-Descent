using UnityEngine;

[RequireComponent(typeof(Health))]
public class EnemyAI : MonoBehaviour {

    Health health;
    //float internalTimer;

    Fighter player;

    Vector2 startingPoint;

	Rigidbody2D rb2d;
    //public GameObject spawnpoint;

    public enum EnemyState
    {
        Idle,
        Walking,
        Detecting,
        Attacking,
        Resting,
        Attacked
    }

    public EnemyState enemyState;

    public bool respawnEnabled;

	float timer;
    float attackRange;
	bool timerSet;
    bool hit;
	float timeLimit;

	bool isDead, respawnCalled;
	
	void Start () {
		rb2d = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
        health.maxHealth = 30;
        health.currentHealth = health.maxHealth;
        startingPoint = transform.position;
        player = FindObjectOfType<Fighter>();
        
	}
	
	
	void Update () {

        switch (enemyState)
        {
            case (EnemyState.Idle):

                break;
            case (EnemyState.Walking):

                break;
            case (EnemyState.Detecting):

                break;
            case (EnemyState.Attacking):
                float distanceFromPlayer = (transform.position - player.transform.position).magnitude;

                if(distanceFromPlayer < attackRange)
                {
                    // can attack
                }
                else
                {
                    // moves towards player
                }

                break;
            case (EnemyState.Resting):

                break;
            case (EnemyState.Attacked):

                break;
        }

		if (health.currentHealth <= 0) {
			isDead = true;
		}

		if(isDead){
			print("enemy is dead");
			transform.position = new Vector3 (0, 3000, 0);

            if (respawnEnabled)
            {
                respawnCalled = true;
            }
		}
		if (respawnCalled) {
			Respawn ();
		}

        if (hit)
        {
            enemyState = EnemyState.Attacked;
        }
    }

	void Respawn(){
		if (!timerSet) {
			timer = Time.deltaTime;
			timeLimit = Time.deltaTime + 5f;
			timerSet = true;
		}

		if (timer > timeLimit) {
			print ("respawn");
			rb2d.velocity = Vector2.zero;
			health.currentHealth = health.maxHealth;
			isDead = false;
			timerSet = false;
			respawnCalled = false;
			transform.position = startingPoint;
		} 
		else {
			//print ("timer: " + timer);
			timer += Time.deltaTime;
		}

	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Attack")
        {
            hit = true;
        }
    }

}
