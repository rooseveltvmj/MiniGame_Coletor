using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void TremerTela(float duracao = 0.25f, float intensidade = 0.3f)
    {
        StartCoroutine(EfeitoTremer(duracao, intensidade));
    }

    private IEnumerator EfeitoTremer(float duracao, float intensidade)
    {
        Vector3 posOriginal = transform.localPosition;
        float tempo = 0f;

        while (tempo < duracao)
        {
            transform.localPosition = posOriginal + (Vector3)Random.insideUnitCircle * intensidade;
            tempo += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = posOriginal;
    }
}