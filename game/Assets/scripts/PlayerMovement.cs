using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
/*public enum PlayerState
{
    walk,
    attack,
    interact
}*/
public enum PlayerState 
{
    walk,
    attack,
    interact,
    stagger,
    idle
}

public class PlayerMovement : MonoBehaviour
{
    public PlayerState currentState;
    public float speed;
    public Rigidbody2D myRigidbody;
    private Vector3 change;
    private Animator anim;
    public FloatValue currentHealth;
    public Signal playerHealthSignal;
    // Start is called before the first frame update
    void Start()
    {
        currentState = PlayerState.walk;
        anim = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        anim.SetFloat("moveX",0);
        anim.SetFloat("moveY",-1);
    }
    /*Rigidbody2D get_player_rigid()
    {
        return myRigidbody;
    }*/
    // Update is called once per frame
    void Update()
    {
        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");
        if (Input.GetButtonDown("attack") && currentState != PlayerState.attack 
            && currentState != PlayerState.stagger)
        {
            StartCoroutine(AttackCo());
        }
        else if (currentState == PlayerState.walk || currentState == PlayerState.idle)
        {
            updateAnimationAndMove();
        }

    }
    private IEnumerator AttackCo()
    {
        anim.SetBool("Attacking", true);
        currentState = PlayerState.attack;
        yield return null;
        anim.SetBool("Attacking", false);
        yield return new WaitForSeconds(0.01f);
        currentState = PlayerState.walk;
    }
    void updateAnimationAndMove()
    {
        if (change != Vector3.zero)
        {
            MoveCharacter();
            anim.SetFloat("moveX", change.x);
            anim.SetFloat("moveY", change.y);
            anim.SetBool("moving",true);
        }
        else
        {
            anim.SetBool("moving",false);
        }
    }

    void MoveCharacter()
    {
        change.Normalize();
        myRigidbody.MovePosition
    (transform.position + change*speed*Time.deltaTime);    
            }
    public void Knock(float knockTime,float damage)
    {
        currentHealth.RuntimeValue -= damage;
        using (StreamWriter sw = File.AppendText(@"/Users/user/game/WriteText.txt"))
        {
            sw.WriteLine("mull health: " + currentHealth.RuntimeValue);
        }
        playerHealthSignal.Raise();
        if (currentHealth.RuntimeValue > 0)
        {
            StartCoroutine(KnockCo(knockTime));
        }
        else
        {
            this.gameObject.SetActive(false);
            using (StreamWriter sw = File.AppendText(@"/Users/user/game/WriteText.txt"))
            {
                sw.WriteLine("mull died");
                sw.WriteLine("Score: " + Score.scoreValue);
            }
            Application.Quit();
        
        }
 }
    private IEnumerator KnockCo(float knockTime)
    {
        if (myRigidbody != null)
        {
            yield return new WaitForSeconds(knockTime);
            myRigidbody.velocity = Vector2.zero;
            currentState = PlayerState.idle;
            myRigidbody.velocity = Vector2.zero;
            Debug.Log("xdd");
        }
    }
}
