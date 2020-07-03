using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    private Transform player;       // Referencia do player
    private Vector2 target;         // Alvo

    public int projectileDamage;    // Dano do projetil
    public float projectileSpeed;   // Velocidade do projetil


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;  // Busca a referencia do player
        target = new Vector2(player.position.x, player.position.y);     // Carrega o alvo (posicao do player no momento da criacao do projetil)
    }

    // Update is called once per frame
    void Update()
    {
        // Aplica a movimentacao
        transform.position = Vector2.MoveTowards(transform.position, target, projectileSpeed * Time.deltaTime);

        // Se chegou no alvo, independente se acertou o player ou nao, destroi o projetil
        if (transform.position.x == target.x && transform.position.y == target.y)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Testa a colisao com o player
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);    // Destroi o projetil
            collision.gameObject.GetComponent<PlayerScript>().TakeDamage(projectileDamage); // Aplica o dano ao player
        }
    }
}
