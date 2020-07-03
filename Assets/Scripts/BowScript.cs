using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowScript : MonoBehaviour
{
    public Transform player;            // Referencia do player
    public float offset = 1f;           // Distancia entre o arco e o player
    public bool canShoot = true;        // Variavel auxiliar para verificar se pode ou nao atirar (de acordo com o cooldown)
    public Transform arrowPoint;        // Ponto de origem da flecha
    public float shotCooldown = .1f;    // Cooldown de tiro
    public GameObject arrow;            // Objeto (prefab) da flecha

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Rotaciona o arco de acordo com a posicao do mouse
        var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Movimenta o arco ao redor do player de acordo com a posicao do mouse
        Vector3 playerToMouseDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - player.position;
        playerToMouseDir.z = 0;
        transform.position = player.position + (offset * playerToMouseDir.normalized);
    }

    public void Shoot() // Funcao de atirar
    {
        // Verifica se ja pode atirar
        if (canShoot)
        {
            Instantiate(arrow, arrowPoint.position, arrowPoint.rotation); // Instancia a flecha
            StartCoroutine(ShotCooldown());                               // Inicia a funcao de cooldown do tiro
        }
    }

    IEnumerator ShotCooldown()
    {
        canShoot = false; // Por enquanto nao pode atirar
 
        yield return new WaitForSeconds(shotCooldown);  // Aguarda o tempo necessario (shotCooldown)

        canShoot = true;  // Depois de esperar o cooldown, podemos atirar novamente
    }
}
