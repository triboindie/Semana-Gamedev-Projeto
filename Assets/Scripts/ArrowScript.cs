using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    public float arrowDamage;   // Dano da flecha
    public float arrowSpeed;    // Velocidade de movimento flecha
    public float arrowLifeTime; // Tempo de "vida" da flecha

    // Start is called before the first frame update
    void Start()
    {
        // Destroi o objeto apos um tempo especificado em arrowLifeTime
        Destroy(gameObject, arrowLifeTime);        
    }

    // Update is called once per frame
    void Update()
    {
        // Movimentacao da flecha assim que ela eh disparada
        transform.Translate(Vector2.right * arrowSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Se colidir com um objeto com a tag Enemy
        if (collision.CompareTag("Enemy"))
        {
            Destroy(gameObject); // Destroi a flecha
            collision.GetComponent<EnemyScript>().TakeDamage(arrowDamage); // Aplica dano no inimigo

        }

        // Se colidir com um objeto com a tag EnemyRanged
        if (collision.CompareTag("EnemyRanged")) 
        {
            Destroy(gameObject); // Destroi a flecha
            collision.GetComponent<EnemyRangedScript>().TakeDamage(arrowDamage); // Aplica dano no inimigo
        }

        // Se colidir com um objeto com a tag Boss
        if (collision.CompareTag("Boss"))
        {
            Destroy(gameObject); // Destroi a flecha
            collision.GetComponent<BossScript>().TakeDamage(arrowDamage); // Aplica dano no boss
        }

    }
}
