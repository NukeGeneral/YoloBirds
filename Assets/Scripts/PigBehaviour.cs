using UnityEngine;
using System.Collections;

public class PigBehaviour : MonoBehaviour {

    private AudioSource audioSource;
    private float health = 150f;
    public Sprite littleDamage;
    public Sprite midDamage;
    public Sprite muchDamage;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter2D(Collision2D target)
    {
        if(target.gameObject.GetComponent<Rigidbody2D>() == null)
        {
            return;
        }

        if(target.gameObject.tag == "Bird")
        {
            audioSource.Play();
            Destroy(gameObject);
        }

        else
        {
            float damage = target.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude * 2f;
            if (damage >= 15)
                audioSource.Play();

            health -= damage;

            if (health < 125f && health >=75f)
                gameObject.GetComponent<SpriteRenderer>().sprite = littleDamage;
            if (health < 75f && health >=25f)
                gameObject.GetComponent<SpriteRenderer>().sprite = midDamage;
            if (health < 25f && health > 0f)
                gameObject.GetComponent<SpriteRenderer>().sprite = muchDamage;
            if (health <= 0)
                Destroy(gameObject);
        }
    }
}
