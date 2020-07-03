using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Vector2 direction;          // Direcao do movimento do player
    private BowScript bow;              // Referencia do arco
    private bool recovering;            // Controla se o player esta se recuperando, ou se ja pode levar dano
    private float recoveryCounter;      // Variavel auxiliar para o cooldown de levar dano
    private bool facingRight = true;    // Controla se o player esta virado para direita ou esquerda

    public int health;                  // Vida do player
    public Image[] hearts;              // Lista de imagens dos coracoes
    public Sprite fullHeart;            // Imagem do coracao cheio
    public Sprite emptyHeart;           // Imagem do coracao vazio
    public float recoveryTime;          // Tempo do cooldown de levar dano
    public float moveSpeed;             // Velocidade de movimentacao do player
    public Rigidbody2D playerRb;        // Referencia do RigidBody do player
    public Animator playerAnim;         // Referencia do Animator do player
    public GameScript gameScript;       // Referencia do Game manager


    // Start is called before the first frame update
    void Start()
    {
        bow = FindObjectOfType<BowScript>(); // Carrega a referencia do arco
    }

    // Update is called once per frame
    void Update()
    {
        // Le o comando do teclado (WASD e/ou as setas)
        direction.x = Input.GetAxisRaw("Horizontal");
        direction.y = Input.GetAxisRaw("Vertical");

        // Passa o parametro para o animator para fazer o controle das animacoes
        playerAnim.SetFloat("Speed", Mathf.Abs(direction.x) + Mathf.Abs(direction.y));

        // Faz o controle de rotacao do player (virado para a esquerda ou direita)
        Vector2 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);

        if (dir.x > 0 && !facingRight || dir.x < 0 && facingRight)
        {
            Flip();
        }

        // Le o clique do mouse
        if (Input.GetMouseButtonDown(0))
        {
            // Chama a funcao de atirar, que esta no script do arco
            bow.Shoot();
        }

        // Rotina de cooldown de levar dano
        if (recovering)
        {
            recoveryCounter += Time.deltaTime;
            if (recoveryCounter >= recoveryTime)
            {
                recoveryCounter = 0;
                recovering = false;
            }
        }
    }

    void UpdateHealthUI(int currentHealth) // Funcao para atualizar os coracoes na tela
    {
        // Para cada coracao, vamos verifica a vida do player, e escolher se devemos usar coracao cheio ou vazio
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
        }
    }

    void Flip() // Funcao para rotacionar o player
    {
        facingRight = !facingRight;     // Inverte o valor da variavel de controle
        transform.Rotate(0f, 180f, 0f); // Rotaciona o objeto do player
    }

    private void FixedUpdate()
    {
        // Aplica a movimentacao de acordo com o input do teclado (valores estao na variavel direction)
        playerRb.MovePosition(playerRb.position + (direction * moveSpeed * Time.fixedDeltaTime));
    }

    public void TakeDamage(int damage) // Funcao para levar dano
    {
        // Se nao esta se recuperando, ja pode levar o dano
        if (!recovering)
        {
            recovering = true;      // Quando levar dano, marca a variavel para iniciar o cooldown de recuperacao
            health -= damage;       // Subtrai o dano da vida
            UpdateHealthUI(health); // Atualiza a UI (coracoes)

            // Se a vida chegar a zero, destroi o objeto e chama a funcao Die() do game manager
            if (health <= 0)
            {
                Destroy(gameObject);
                gameScript.Die();
            }
        }
    }
}
