using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [Header("Configurações Base")]
    public int pontuacao = 0;
    public int vidas = 3;
    public bool jogoAtivo = false;

    [Header("Prefabs dos Itens")]
    public GameObject prefabItemComum;
    public GameObject prefabItemBonus;
    public GameObject prefabItemBomba;
    public GameObject prefabItemEscudo;
    public GameObject prefabItemVidaExtra;
    public GameObject prefabItemIma;          // Prefab do item Imã

    [Header("Referências")]
    public Transform jogadorTransform;        // Arraste o Jogador aqui no Inspector

    [Header("Power-ups")]
    public bool temEscudo = false;
    public GameObject indicadorEscudoVisual;
    public float duracaoEscudo = 8f;    // Duração do escudo em segundos
    public bool temIma = false;              // Imã ativo?
    public GameObject indicadorImaVisual;     // Indicador visual do imã (opcional)
    public float duracaoIma = 5f;            // Duração do imã em segundos

    [Header("Dificuldade Progressiva")]
    public float velocidadeSpawnInicial = 1.5f;      
    public float velocidadeItemInicial = 5f;         
    public int pontosParaAumentarDificuldade = 10;   
    public float multiplicadorVelocidade = 0.9f;     
    public float aumentoVelocidadeItem = 0.5f;       
    
    private float velocidadeSpawnAtual;
    private float velocidadeItemAtual;
    private int proximoMilestone = 10;               

    [Header("Interface e Menus")]
    public TextMeshProUGUI textoPontuacao;
    public TextMeshProUGUI textoVidas;
    public TextMeshProUGUI textoNivel;               
    public GameObject painelMenuInicial;
    public GameObject painelGameOver;
    public TMP_InputField inputNomeJogador;
    public TextMeshProUGUI textoTop5;

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    void Start()
    {
        // Começa pausado mostrando o Menu Inicial
        Time.timeScale = 0f;
        jogoAtivo = false;

        if (painelMenuInicial != null) painelMenuInicial.SetActive(true);
        if (painelGameOver != null) painelGameOver.SetActive(false);

        ExibirTop5();
    }

    // Chamado pelo Botão "Play" no Menu Inicial
    public void IniciarJogo()
    {
        pontuacao = 0;
        vidas = 3;
        proximoMilestone = pontosParaAumentarDificuldade;
        velocidadeSpawnAtual = velocidadeSpawnInicial;
        velocidadeItemAtual = velocidadeItemInicial;
        jogoAtivo = true;
        temEscudo = false;

        // Dentro do IniciarJogo():
        temEscudo = false;
        if (indicadorEscudoVisual != null) indicadorEscudoVisual.SetActive(false);
        temIma = false;
        if (indicadorImaVisual != null) indicadorImaVisual.SetActive(false);

        if (painelMenuInicial != null) painelMenuInicial.SetActive(false);
        if (painelGameOver != null) painelGameOver.SetActive(false);

        AtualizarInterface();
        Time.timeScale = 1f; // Despausa o jogo

        CancelInvoke("GerarItem");
        InvokeRepeating("GerarItem", 1f, velocidadeSpawnAtual);
    }

    void GerarItem()
        {
            if (!jogoAtivo) return;

            float xAleatorio = Random.Range(-8f, 8f);
            Vector3 posicao = new Vector3(xAleatorio, 6f, 0f);

            // Sorteia de 0 a 99 (Probabilidades ajustadas)
            int sorteio = Random.Range(0, 100); 
            GameObject prefabParaCriar;

            if (sorteio < 52)         // 52% Comum
            {
                prefabParaCriar = prefabItemComum;
            }
            else if (sorteio < 70)    // 18% Bônus
            {
                prefabParaCriar = prefabItemBonus;
            }
            else if (sorteio < 83)    // 13% Bomba
            {
                prefabParaCriar = prefabItemBomba;
            }
            else if (sorteio < 90)    // 7% Escudo
            {
                prefabParaCriar = prefabItemEscudo;
            }
            else if (sorteio < 96)    // 6% Vida Extra
            {
                prefabParaCriar = prefabItemVidaExtra;
            }
            else                      // 4% Imã (Ultra Raro)
            {
                prefabParaCriar = prefabItemIma;
            }

            if (prefabParaCriar != null)
            {
                GameObject item = Instantiate(prefabParaCriar, posicao, Quaternion.identity);
                ItemCaindo itemScript = item.GetComponent<ItemCaindo>();
                if (itemScript != null)
                {
                    itemScript.velocidadeQueda = velocidadeItemAtual;
                }
            }
        }

    public void AdicionarPonto()
    {
        if (!jogoAtivo) return;
        pontuacao++;
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.TocarSomColetarItem();
        }
        
        if (pontuacao >= proximoMilestone)
        {
            AumentarDificuldade();
        }
        
        AtualizarInterface();
    }

    public void AdicionarPontoBonus(int quantidade)
    {
        if (!jogoAtivo) return;
        pontuacao += quantidade;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.TocarSomColetarItem();
        }

        if (pontuacao >= proximoMilestone)
        {
            AumentarDificuldade();
        }

        AtualizarInterface();
    }

    void AumentarDificuldade()
    {
        proximoMilestone += pontosParaAumentarDificuldade;
        velocidadeSpawnAtual *= multiplicadorVelocidade;
        velocidadeItemAtual += aumentoVelocidadeItem;
        
        CancelInvoke("GerarItem");
        InvokeRepeating("GerarItem", 0.5f, velocidadeSpawnAtual);
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.TocarSomAumentoNivel();
        }
        
        Debug.Log($"🎯 DIFICULDADE AUMENTADA! Nível: {GetNivelAtual()} | Próximo marco: {proximoMilestone}");
    }

    int GetNivelAtual()
    {
        return Mathf.FloorToInt(pontuacao / pontosParaAumentarDificuldade) + 1;
    }

    public void PerderVida()
    {
        if (!jogoAtivo) return;

        if (temEscudo)
        {
            temEscudo = false;
            if (indicadorEscudoVisual != null) indicadorEscudoVisual.SetActive(false);
            
            // Toca som do escudo defendendo
            if (AudioManager.Instance != null) AudioManager.Instance.TocarSomEscudo(); 
            return; 
        }

        if (CameraShake.Instance != null) CameraShake.Instance.TremerTela();

        vidas--;
        
        // Toca som da bomba explodindo
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.TocarSomBomba();
        }
        
        AtualizarInterface();
        if (vidas <= 0) FinalizarJogo();
    }

    public void AdicionarVida()
    {
        if (!jogoAtivo) return;
        vidas++;

        // Toca som da vida extra
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.TocarSomVidaExtra();
        }

        AtualizarInterface();
    }

    /// <summary>
    /// Chamado quando um item cai no chão (não bloqueado pelo escudo).
    /// Atualiza a UI e verifica game over.
    /// </summary>
    public void VerificarEstado()
    {
        AtualizarInterface();
        if (vidas <= 0) FinalizarJogo();
    }

    void AtualizarInterface()
    {
        if (textoPontuacao != null) textoPontuacao.text = "Pontos: " + pontuacao;
        if (textoVidas != null) textoVidas.text = "Vidas: " + vidas;
        if (textoNivel != null) textoNivel.text = "Nível: " + GetNivelAtual();
    }

    public void AtivarEscudo()
    {
        temEscudo = true;
        if (indicadorEscudoVisual != null) indicadorEscudoVisual.SetActive(true);

        // Toca som do escudo
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.TocarSomEscudo();
        }

        // Reinicia o timer do escudo
        StopCoroutine("DesativarEscudoAposTempo");
        StartCoroutine("DesativarEscudoAposTempo");
    }

    private IEnumerator DesativarEscudoAposTempo()
    {
        yield return new WaitForSeconds(duracaoEscudo);
        temEscudo = false;
        if (indicadorEscudoVisual != null) indicadorEscudoVisual.SetActive(false);
    }

    public void AtivarIma()
    {
        temIma = true;
        if (indicadorImaVisual != null) indicadorImaVisual.SetActive(true);

        // Opcional: Adicionar um som para o Imã
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.TocarSomColetarItem(); // Ou crie um TocarSomIma() no AudioManager
        }

        // Reinicia o timer do imã
        StopCoroutine("DesativarImaAposTempo");
        StartCoroutine("DesativarImaAposTempo");
    }

    private IEnumerator DesativarImaAposTempo()
    {
        yield return new WaitForSeconds(duracaoIma);
        temIma = false;
        if (indicadorImaVisual != null) indicadorImaVisual.SetActive(false);
    }

    void FinalizarJogo()
    {
        jogoAtivo = false;
        CancelInvoke("GerarItem");
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.TocarSomGameOver();
        }

        // Destrói todos os itens que ainda estão caindo na tela
        ItemCaindo[] itensRestantes = FindObjectsByType<ItemCaindo>(FindObjectsSortMode.None);
        foreach (ItemCaindo item in itensRestantes)
        {
            Destroy(item.gameObject);
        }
        
        if (painelGameOver != null) painelGameOver.SetActive(true); 

        // Pausa o jogo para que nada se mova na tela de Game Over
        Time.timeScale = 0f;
    }

    // Chamado pelo Botão "Salvar" na Tela de Game Over
    public void SalvarPontuacao()
    {
        string nome = (inputNomeJogador != null && !string.IsNullOrEmpty(inputNomeJogador.text)) 
                      ? inputNomeJogador.text 
                      : "Jogador";

        SalvarNoTop5(nome, pontuacao);
        ReiniciarJogo();
    }

    void SalvarNoTop5(string novoNome, int novosPontos)
    {
        List<string> nomes = new List<string>();
        List<int> pontos = new List<int>();

        // Carrega os recordes atuais
        for (int i = 0; i < 5; i++)
        {
            nomes.Add(PlayerPrefs.GetString("TopNome_" + i, "---"));
            pontos.Add(PlayerPrefs.GetInt("TopPontos_" + i, 0));
        }

        // Adiciona a nova pontuação
        nomes.Add(novoNome);
        pontos.Add(novosPontos);

        // Ordena do maior para o menor
        for (int i = 0; i < pontos.Count - 1; i++)
        {
            for (int j = i + 1; j < pontos.Count; j++)
            {
                if (pontos[j] > pontos[i])
                {
                    int tempP = pontos[i]; pontos[i] = pontos[j]; pontos[j] = tempP;
                    string tempN = nomes[i]; nomes[i] = nomes[j]; nomes[j] = tempN;
                }
            }
        }

        // Salva os 5 melhores de volta no PlayerPrefs
        for (int i = 0; i < 5; i++)
        {
            PlayerPrefs.SetString("TopNome_" + i, nomes[i]);
            PlayerPrefs.SetInt("TopPontos_" + i, pontos[i]);
        }
        PlayerPrefs.Save();
    }

    public void ExibirTop5()
    {
        if (textoTop5 == null) return;

        string resultado = "<b>TOP 5 RECORDES</b>\n\n";
        for (int i = 0; i < 5; i++)
        {
            string nome = PlayerPrefs.GetString("TopNome_" + i, "---");
            int pts = PlayerPrefs.GetInt("TopPontos_" + i, 0);
            resultado += $"{i + 1}. {nome} - {pts} pts\n";
        }
        textoTop5.text = resultado;
    }

    public void ReiniciarJogo()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}