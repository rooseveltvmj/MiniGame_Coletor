using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float velocidade = 10f; // Controla a velocidade no Inspector
    private float limiteX = 8f;   // Limite para o jogador não sair da tela

    [Header("Controle por Toque")]
    public bool usarToque = true;  // Ativa controle por toque/mouse

    void Update()
    {
        float movimentoHorizontal = 0f;

        // Controle por toque/mouse
        if (usarToque && (Input.touchCount > 0 || Input.GetMouseButton(0)))
        {
            Vector3 posicaoTela;
            if (Input.touchCount > 0)
                posicaoTela = Input.GetTouch(0).position;
            else
                posicaoTela = Input.mousePosition;

            Vector3 posicaoMundo = Camera.main.ScreenToWorldPoint(posicaoTela);
            float direcao = posicaoMundo.x - transform.position.x;
            movimentoHorizontal = Mathf.Clamp(direcao, -1f, 1f);
        }
        else
        {
            // Controle por teclado (setas ou A/D)
            movimentoHorizontal = Input.GetAxis("Horizontal");
        }

        // Calcula a nova posição
        Vector3 novaPosicao = transform.position + new Vector3(movimentoHorizontal, 0, 0) * velocidade * Time.deltaTime;

        // Mantém o jogador dentro dos limites da tela
        novaPosicao.x = Mathf.Clamp(novaPosicao.x, -limiteX, limiteX);

        // Aplica a nova posição
        transform.position = novaPosicao;
    }
}