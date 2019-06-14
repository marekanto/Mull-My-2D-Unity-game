using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public enum EnemyState{
    idle,
    walk,
    attack,
    stagger
}
public class Enemy : MonoBehaviour
{
    public EnemyState currentState;
    public float health;
    public FloatValue maxHealth;
    public string enemyName;
    public int baseAttack;
    public float moveSpeed;
    public string logMessage;
    public PlayerMovement player1;
    public GameObject player;
    // Start is called before the first frame update
    private void Awake()
    {
        health = maxHealth.initialValue;
    }
    private void TakeDamage(float damage)
    {
        if (!File.Exists(@"/Users/user/game/WriteText.txt"))
        {
            // Create a file to write to.
            using (StreamWriter sw = File.CreateText(@"/Users/user/game/WriteText.txt"))
            {
                sw.WriteLine("enemy health: " + health);
            }
        }
        using (StreamWriter sw = File.AppendText(@"/Users/user/game/WriteText.txt"))
        {
            sw.WriteLine("enemy health: " + health);
        }
        health -= damage;
        Debug.Log("enemy health: " + health);
        if (health <= 0)
        {
            this.gameObject.SetActive(false);
            Destroy(this.gameObject, 3.0f);
            Score.scoreValue += 10;
            logMessage ="Enemy died ";
            Debug.Log(logMessage);           
            using (StreamWriter sw = File.AppendText(@"/Users/user/game/WriteText.txt"))
            {
                sw.WriteLine(logMessage);
            }        
        }
    }
    public void Knock(Rigidbody2D rigidbody, float knockTime, float damage)
    {
        //Debug.Log("knockMethod");
        StartCoroutine(KnockCo(rigidbody, knockTime));
        TakeDamage(damage);
    }
    private IEnumerator KnockCo(Rigidbody2D rigidbody, float knockTime)
    {
        if (rigidbody != null)
        {
            //Debug.Log("knockCo");
            //player.GetComponent<Rigidbody2D>();
            yield return new WaitForSeconds(knockTime);
            rigidbody.velocity = Vector2.zero;
            currentState = EnemyState.idle;
            rigidbody.velocity = Vector2.zero;
            
            //GameObject.FindWithTag"Player"
        }
    }

}
