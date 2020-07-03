using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReticleScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false; // Esconde o cursor do mouse
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Busca a posicao do cursor
        transform.position = cursorPos; // Faz o objeto seguir a posicao do cursor
    }
}
