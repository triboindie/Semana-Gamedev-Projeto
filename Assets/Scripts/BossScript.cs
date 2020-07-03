using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    public bool rageMode;               // Vai controlar se o boss ja entrou em furia ou ainda nao
    public float ragePercent;           // Percentual de vida para o boss ativar modo furia
    private float rageHealth;           // Variavel auxiliar para controlar o modo furia

    public Animator bossAnim;           // Animator do boss
    public Transform player;            // Referencia do player
    public bool canMove = true;         // Variavel que determina se o boss pode se mover ou nao (por padrao sempre podera se mover)
    public int damage;                  // Dano do boss caso encoste no player
    public float moveSpeed;             // Velocidade de movimentacao do boss
    public float health;                // Vida do boss
    public GameObject bossProjectile;   // Referencia do projetil do boss (vamos carregar essa variavel com o prefab bossProjectile)
    public float projectileCount;       // Numero de projeteis que voce quer que o boss atire por vez

    public float shotCooldown;          // Cooldown de tiro do boss

    private float shotTimer;            // Variavel auxiliar para fazer o controle do cooldown de tiro          

    private Vector2 targetSpot;         // Proximo ponto de movimentacao do boss
    public Vector2[] movePoints;        // Lista de possiveis pontos de movimentos do boss
    

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;  // Pega a referencia do player
        GetNextSpot();  // Busca o proximo ponto de movimentacao
        rageHealth = (ragePercent * health) / 100;  // Calcula quanto de hp ele precisa chegar para ativar o modo furia
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null) // Testa se o player esta ativo na cena
        {
            // Verifica se ja esta na posicao de destino
            if (transform.position.x != targetSpot.x && transform.position.y != targetSpot.y)
            {
                // Se nao esta, vai se mover ate ela (targetSpot)
                if (canMove) transform.position = Vector2.MoveTowards(transform.position, targetSpot, moveSpeed * Time.deltaTime);
            }
            else
            {
                // Se esta, busca o proximo ponto de movimentacao
                GetNextSpot();
            }

            // Calcula o cooldown do tiro
            if (shotTimer <= 0)
            {
                Shoot();
            }
            else
            {
                shotTimer -= Time.deltaTime;
            }

            // Se atingir o hp calculado (rageHealth), entra no modo furia
            if (health <= rageHealth && !rageMode) Enrage();
        }
    }

    void Enrage() // Funcao do modo furia
    {
        rageMode = true;                // Marca que ja entrou no modo furia
        moveSpeed *= 3;                 // Multiplica a velocidade por 3 (fica 3x mais rapido)
        shotCooldown *= .75f;           // Reduz o cooldown de tiro (atira mais rapido)
        projectileCount *= 3;           // Aumenta a quantidade de projeteis x3
        bossAnim.SetTrigger("Rage");    // Altera a animacao normal para a animacao de furia
    }

    void Shoot() // Funcao de atirar
    {
        shotTimer = shotCooldown;                   // Inicia o cooldown do tiro
        float angleStep = 360f / projectileCount;   // Calcula o angulo de cada projetil com base na quantidade total (projectileCount)
        float angle = 0f;                           // Inicializa o angulo base com 0

        // Faz um loop para executar a funcao que esta dentro para cada projetil
        for (int i = 0; i <= projectileCount - 1; i++)
        {
            float xPosition = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180) * 360;     // Calcula a posicao X de acordo com o angulo
            float yPosition = transform.position.x + Mathf.Cos((angle * Mathf.PI) / 180) * 360;     // Calcula a posicao Y de acordo com o angulo

            Vector2 projectileDirection = new Vector2(xPosition, yPosition);                        // Cria um novo vetor com as posicoes x e y calculadas acima

            var projectile = Instantiate(bossProjectile, transform.position, Quaternion.identity);  // Instancia o projetil
            projectile.GetComponent<BossProjectile>().SetDirection(projectileDirection * 3);        // Seta a direcao do projetil

            angle += angleStep;                                                                     // Aumenta o angulo base para o proximo projetil conseguir usar
        }
    }

    private void GetNextSpot() // Busca o proximo ponto de movimentacao
    {
        int randomSpot = Random.Range(0, movePoints.Length);    // Busca um valor aleatorio dentre os possiveis pontos de movimentacao
        targetSpot = movePoints[randomSpot];                    // Carrega o proximo ponto (targetSpot) com um ponto aleatorio
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Testa se entrou em colisao com o player
        if (collision.CompareTag("Player"))
        {
            // Aplica o dano ao player
            collision.gameObject.GetComponent<PlayerScript>().TakeDamage(damage);
        }
    }

    public void TakeDamage(float damage) // Funcao para receber dano
    {
        health -= damage;   // Subtrai o dano da vida atual

        // Se a vida chegou a 0, destroi o objeto
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
