using UnityEngine;
using TMPro;

public class TMP_TextShake : MonoBehaviour
{
    [SerializeField] float intensity = 2f;
    [SerializeField] float speed = 20f;

    TMP_Text text;
    Vector3[][] originalVertices;

    void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    void Start()
    {
        text.ForceMeshUpdate();
        originalVertices = new Vector3[text.textInfo.meshInfo.Length][];

        for (int i = 0; i < originalVertices.Length; i++)
        {
            originalVertices[i] = text.textInfo.meshInfo[i].vertices.Clone() as Vector3[];
        }
    }

    void Update()
    {
        text.ForceMeshUpdate();

        for (int i = 0; i < text.textInfo.characterCount; i++)
        {
            var charInfo = text.textInfo.characterInfo[i];
            if (!charInfo.isVisible) continue;

            int meshIndex = charInfo.materialReferenceIndex;
            int vertexIndex = charInfo.vertexIndex;

            Vector3 offset = new Vector3(
                Mathf.PerlinNoise(Time.time * speed, i) - 0.5f,
                Mathf.PerlinNoise(i, Time.time * speed) - 0.5f,
                0
            ) * intensity;

            var vertices = text.textInfo.meshInfo[meshIndex].vertices;

            vertices[vertexIndex + 0] = originalVertices[meshIndex][vertexIndex + 0] + offset;
            vertices[vertexIndex + 1] = originalVertices[meshIndex][vertexIndex + 1] + offset;
            vertices[vertexIndex + 2] = originalVertices[meshIndex][vertexIndex + 2] + offset;
            vertices[vertexIndex + 3] = originalVertices[meshIndex][vertexIndex + 3] + offset;
        }

        for (int i = 0; i < text.textInfo.meshInfo.Length; i++)
        {
            text.textInfo.meshInfo[i].mesh.vertices = text.textInfo.meshInfo[i].vertices;
            text.UpdateGeometry(text.textInfo.meshInfo[i].mesh, i);
        }
    }
}