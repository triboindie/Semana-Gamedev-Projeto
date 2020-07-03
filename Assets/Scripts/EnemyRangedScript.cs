using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedScript : MonoBehaviour
{
    private float shotTimer;        // Variavel auxiliar para controlar o cooldown do tiro
    private bool canShoot = true;   // Determina se o inimigo pode ou nao atirar, de acordo com o cooldown

    public Transform player;        // Referencia do transform do player
    public float moveSpeed;         // Velocidade de movimentacao
    public float shotCooldown;      // Tempo de cooldown do tiro
    public float stoppingDistance;  // Distancia que o inimigo vai manter do player
    public float retreatDistance;   // Distancia que o inimigo vai se afastar do player
    public float health;            // Vida do inimigo

    public GameObject projectile;   // Objeto (prefab) do projetil do inimigo

    // Start is called before the first frame update
    void Start()
    {        
        player = GameObject.FindGameObjectWithTag("Player").transform; // Busca a referencia do player
        shotTimer = shotCooldown;   // Inicia o processo de poder atirar
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null) // Testa se o player esta ativo
        {
            if (Vector2.Distance(transform.position, player.position) > stoppingDistance)
            {
                // Se o inimigo esta muito longe, movimenta na direcao do player
                transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
            }
            else if (Vector2.Distance(transform.position, player.position) < stoppingDistance && Vector2.Distance(transform.position, player.position) > retreatDistance)
            {
                // Se o inimigo esta na distancia ideal, fica parado e atira
                transform.position = this.transform.position;

                if (canShoot) Shoot();
            }
            else if (Vector2.Distance(transform.position, player.position) < retreatDistance)
            {
                // Se o inimigo esta muito proximo, se afasta
                transform.position = Vector2.MoveTowards(transform.position, player.position, -moveSpeed * Time.deltaTime);
            }
        }

        // Controle de cooldown de tiro do inimigo
        if (shotTimer <= 0)
        {
            canShoot = true;
        }
        else
        {
            canShoot = false;
            shotTimer -= Time.deltaTime;
        }
    }

    void Shoot() // Funcao de atirar
    {
        Instantiate(projectile, transform.position, Quaternion.identity); // Instancia o projetil
        shotTimer = shotCooldown;   // Inicia o cooldown do tiro
    }

    public void TakeDamage(float damage) // Funcao de receber dano
    {
        health -= damage; // Desconta o dano da vida atual

        // Se a vida chegar a zero, destroi o objeto
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
