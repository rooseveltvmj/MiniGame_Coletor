using UnityEngine;

/// <summary>
/// Gerenciador centralizado de áudio do jogo.
/// Padrão Singleton: garante apenas 1 instância
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    
    [Header("Referências")]
    private AudioSource audioSource;
    
    [Header("Clips de Som")]
    public AudioClip somColetarItem;        // Quando jogador pega uma moeda comum
    public AudioClip somPerderVida;         // Quando perde vida no chão
    public AudioClip somGameOver;           // Quando jogo termina
    public AudioClip somAumentoNivel;       // Quando nível aumenta
    public AudioClip somBombaExplodir;      // Quando acerta uma bomba
    public AudioClip somPegarEscudo;        // Quando ativa/consome o escudo
    public AudioClip somPegarVida;          // Quando coleta o coração
    
    [Header("Volumes (0 a 1)")]
    [Range(0f, 1f)] public float volumeEfeitos = 0.7f;
    [Range(0f, 1f)] public float volumeGameOver = 0.8f;
    
    void Awake()
    {
        // Implementa Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject); // Persiste entre cenas
    }
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            Debug.LogWarning("⚠️ AudioSource não encontrado em AudioManager. Um novo AudioSource foi adicionado automaticamente.");
        }
    }
    
    /// <summary>
    /// Toca som de coletar item com pitch variado para não ficar repetitivo
    /// </summary>
    public void TocarSomColetarItem()
    {
        if (somColetarItem == null) return;
        
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(somColetarItem, volumeEfeitos);
    }
    
    /// <summary>
    /// Toca som de explocão da bomba
    /// </summary>
    public void TocarSomBomba()
    {
        if (somBombaExplodir == null) return;

        audioSource.pitch = 1f;
        audioSource.PlayOneShot(somBombaExplodir, volumeEfeitos);
    }

    /// <summary>
    /// Toca som ao coletar ou defender com o escudo
    /// </summary>
    public void TocarSomEscudo()
    {
        if (somPegarEscudo == null) return;

        audioSource.pitch = 1f;
        audioSource.PlayOneShot(somPegarEscudo, volumeEfeitos);
    }

    /// <summary>
    /// Toca som ao coletar uma vida extra
    /// </summary>
    public void TocarSomVidaExtra()
    {
        if (somPegarVida == null) return;

        audioSource.pitch = 1f;
        audioSource.PlayOneShot(somPegarVida, volumeEfeitos);
    }

    /// <summary>
    /// Toca som de perder vida
    /// </summary>
    public void TocarSomPerderVida()
    {
        if (somPerderVida == null) return;
        
        audioSource.pitch = 1f;
        audioSource.PlayOneShot(somPerderVida, volumeEfeitos);
    }
    
    /// <summary>
    /// Toca som de Game Over com volume mais alto
    /// </summary>
    public void TocarSomGameOver()
    {
        if (somGameOver == null) return;
        
        audioSource.pitch = 1f;
        audioSource.PlayOneShot(somGameOver, volumeGameOver);
    }
    
    /// <summary>
    /// Toca som de aumento de nível com pitch mais alto
    /// </summary>
    public void TocarSomAumentoNivel()
    {
        if (somAumentoNivel == null) return;
        
        audioSource.pitch = 1.2f;
        audioSource.PlayOneShot(somAumentoNivel, volumeEfeitos);
    }
    
    /// <summary>
    /// Muta/desmuta todos os sons
    /// </summary>
    public void AlternarSom(bool ativo)
    {
        audioSource.enabled = ativo;
    }
}