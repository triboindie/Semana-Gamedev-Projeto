using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    private bool hasDirection;      // Variavel auxiliar
    private Vector2 moveDirection;  // direcao de movimento do projetil

    public float projectileTime;    // Tempo de "vida" do projetil
    public float projectileSpeed;   // Velocidade do projetil
    public int projectileDamage;    // Dano do projetil

    // Start is called before the first frame update
    void Start()
    {
        // Destroi o objeto apos o tempo definido em projectileTime
        Destroy(gameObject, projectileTime);
    }

    // Update is called once per frame
    void Update()
    {
        // Verifica se a direcao ja foi carregada, ou seja, se possui direcao
        if (hasDirection)
        {
            // Movimentacao do projetil
            transform.position = Vector2.MoveTowards(transform.position, moveDirection, projectileSpeed * Time.deltaTime);
        }
    }

    public void SetDirection(Vector2 direction) // Seta a direcao atraves de outro script
    {
        hasDirection = true;        // Confirma que a direcao foi recebida
        moveDirection = direction;  // Direcao eh carregada na variavel moveDirection
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Testa se colidiu com um objeto que possui a tag Player
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject); // Destroi o projetil
            collision.gameObject.GetComponent<PlayerScript>().TakeDamage(projectileDamage); // Aplica dano no player            
        }
    }
}
