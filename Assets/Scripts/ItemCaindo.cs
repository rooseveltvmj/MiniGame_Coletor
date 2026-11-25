using UnityEngine;

public class ItemCaindo : MonoBehaviour
{
    public float velocidadeQueda = 5f;

    public enum TipoItem { Comum, Bonus, Bomba, Escudo, VidaExtra, Ima }
    [Header("Tipo do Item")]
    public TipoItem tipo = TipoItem.Comum;

    private Rigidbody2D rb;

    [Header("Configurações do Imã")]
    public float forcaIma = 8f; // Força de atração horizontal do imã

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0f; // Desativa gravidade do Unity para usar velocidade controlada
            rb.velocity = Vector2.down * velocidadeQueda;
        }
    }

    void Update()
    {
        // Efeito do Imã: atrai itens coletáveis na direção do jogador
        if (GameManager.Instance != null && GameManager.Instance.temIma && tipo != TipoItem.Ima)
        {
            Transform jogador = GameManager.Instance.jogadorTransform;
            if (jogador != null)
            {
                float direcaoX = jogador.position.x - transform.position.x;
                float atracao = Mathf.Clamp(direcaoX, -1f, 1f) * forcaIma;
                transform.position += new Vector3(atracao * Time.deltaTime, 0f, 0f);
            }
        }

        if (transform.position.y < -7f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Jogador"))
        {
            if (tipo == TipoItem.Bomba)
            {
                GameManager.Instance.PerderVida();
            }
            else if (tipo == TipoItem.Bonus)
            {
                GameManager.Instance.AdicionarPontoBonus(3);
            }
            else if (tipo == TipoItem.Escudo)
            {
                GameManager.Instance.AtivarEscudo(); // <--- CHAMA O ESCUDO
            }
            else if (tipo == TipoItem.VidaExtra)
            {
                GameManager.Instance.AdicionarVida();
            }
            else if (tipo == TipoItem.Ima)
            {
                GameManager.Instance.AtivarIma();
            }
            else
            {
                GameManager.Instance.AdicionarPonto();
            }
            Destroy(gameObject);
            
        }
        else if (collision.CompareTag("Limite"))
        {
            // Apenas moedas normais e bônus fazem perder vida ao cair fora. 
            // Bombas e Escudos não tiram vida ao tocar no chão.
            if (tipo == TipoItem.Comum || tipo == TipoItem.Bonus)
            {
                // Perder vida SEM escudo bloqueando (itens no chão ignoram escudo)
                if (GameManager.Instance.jogoAtivo)
                {
                    GameManager.Instance.vidas--;
                    if (AudioManager.Instance != null) AudioManager.Instance.TocarSomPerderVida();
                    if (CameraShake.Instance != null) CameraShake.Instance.TremerTela(0.15f, 0.2f);
                    GameManager.Instance.VerificarEstado();
                }
            }
            Destroy(gameObject);
        }
        
    }
}