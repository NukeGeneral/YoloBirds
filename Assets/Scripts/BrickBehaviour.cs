using UnityEngine;
using System.Collections;

public class BrickBehaviour : MonoBehaviour {

    private AudioSource audioSource;
    private float health = 1000000000.0f;
    public Sprite oneDamagePrefab;
    public Sprite twoDamagePrefab;
    public Sprite threeDamagePrefab;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter2D(Collision2D target)
    {
        if (target.gameObject.GetComponent<Rigidbody2D>() == null)
            return;
        if(target.collider.tag == "Bird")
        {
            float damage = target.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude * 5f;
            if (damage > 25)
                audioSource.Play();
            health = -damage;
            if (health < 75f && health >= 50f)
                gameObject.GetComponent<SpriteRenderer>().sprite = oneDamagePrefab;
            else if (health < 50 && health >= 25f)
                gameObject.GetComponent<SpriteRenderer>().sprite = twoDamagePrefab;
            else if (health < 25 && health > 0f)
                gameObject.GetComponent<SpriteRenderer>().sprite = threeDamagePrefab;
            else if (health <= 0)
                Destroy(gameObject);
        }
    }
}
