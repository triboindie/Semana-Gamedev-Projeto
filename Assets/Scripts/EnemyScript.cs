using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public Transform player;    // Referencia do player
    public bool canMove = true; // Controla se o inimigo pode ou nao se mover
    public int damage;          // Dano do inimigo ao encostar no player
    public float moveSpeed;     // Velocidade de movimentacao do inimigo
    public float health;        // Vida do inimigo

    // Start is called before the first frame update
    void Start()
    {
        // Busca a referencia do player
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null && canMove) // Se o player esta ativo, e se o inimigo pode se mover
        {
            // Aplica a movimentacao ao inimigo. Ele sempre ira em direcao ao player.
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se entrou em colisao com o player
        if (collision.CompareTag("Player"))
        {
            player.GetComponent<PlayerScript>().TakeDamage(damage); // Aplica dano ao player
            StartCoroutine(FreezeMovement(2));                      // Congela o inimigo temporariamente, para o player conseguir reagir
        }
    }

    IEnumerator FreezeMovement(float freezeTime) // Funcao para congelar o movimento
    {
        canMove = false;    // Por enquanto nao pode se mover
        yield return new WaitForSeconds(freezeTime); // Aguarda o tempo necessario (freezeTime)
        canMove = true;     // Agora pode se mover novamente
    }

    public void TakeDamage(float damage) // Funcao de levar dano
    {            
        // Subtrai o dano da vida atual
        health -= damage;

        // Se chegou a zero de vida, destroi o objeto
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
