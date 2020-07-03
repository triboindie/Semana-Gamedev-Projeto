using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScript : MonoBehaviour
{
    private bool isPaused;              // Verifica se o jogo esta pausado

    public string gameScene;            // Nome da cena do jogo
    public string menuScene;            // Nome da cena do menu

    public GameObject pausePanel;       // Painel de pause
    public GameObject gameOverPanel;    // Painel de game over
    public GameObject UIPanel;          // Painel de UI
    public GameObject victoryPanel;     // Painel de vitoria


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Se o jogador apertou Esc
        if (Input.GetKeyDown(KeyCode.Escape))
        {            
            if (!isPaused) // Se nao esta pausado, executa o codigo para pausar
            {
                isPaused = true;            // Marca a variavel para dizer que o jogo esta pausado
                pausePanel.SetActive(true); // Ativa o painel de pause
                Cursor.visible = true;      // Mostra o cursor do mouse
                Time.timeScale = 0;         // Pausa o tempo do jogo
            }
            else // Se esta pausado, executa o codigo para despausar
            {
                isPaused = false;            // Marca a variavel para dizer que o jogo nao esta mais pausado
                pausePanel.SetActive(false); // Desativa o painel de pause
                Cursor.visible = false;      // Esconde o cursor do mouse
                Time.timeScale = 1;          // Despausa o tempo do jogo
            }
        }
    }

    public void QuitGame()
    {        
        Time.timeScale = 1;                // Despausa o tempo do jogo
        SceneManager.LoadScene(menuScene); // Carrega a cena do menu
    }

    public void Restart()
    {
        Time.timeScale = 1;                // Despausa o tempo do jogo
        SceneManager.LoadScene(gameScene); // Carrega a cena do jogo
    }

    public void Die()
    {
        Cursor.visible = true;          // Mostra o cursor do mouse
        gameOverPanel.SetActive(true);  // Ativa o painel de game over
    }

    public void Victory()
    {
        Cursor.visible = true;        // Mostra o cursor do mouse
        UIPanel.SetActive(false);     // Desativa o painel de UI
        victoryPanel.SetActive(true); // Ativa o painel de vitoria
    }
}
