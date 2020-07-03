using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public string sceneName; // Nome da cena do jogo

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame() // Funcao para iniciar o jogo
    {
        SceneManager.LoadScene(sceneName); // Carrega a cena do jogo
    }

    public void QuitGame() // Funcao para sair do jogo
    {
        if (Application.isEditor) // Se esta rodando pelo editor da Unity
        {
            // IMPORTANTE: Quando compilar o projeto para rodar atraves de um executavel, comentar o remover o comando abaixo!
            UnityEditor.EditorApplication.isPlaying = false;
        }
        else // Se esta rodando independente da Unity, atraves de um executavel
            Application.Quit();
    }
}
