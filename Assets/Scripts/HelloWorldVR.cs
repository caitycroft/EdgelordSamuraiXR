using UnityEngine;
using TMPro;

public class HelloWorldVR : MonoBehaviour
{
    [Header("Hello World Settings")]
    [SerializeField] private TextMeshPro textMesh;
    [SerializeField] private string message = "Hello World VR!";
    [SerializeField] private Color textColor = Color.white;
    [SerializeField] private float textSize = 0.5f;

    void Start()
    {
        if (textMesh == null)
        {
            textMesh = GetComponent<TextMeshPro>();
        }

        if (textMesh != null)
        {
            textMesh.text = message;
            textMesh.color = textColor;
            textMesh.fontSize = textSize;
            textMesh.alignment = TextAlignmentOptions.Center;

            Debug.Log("Hello World VR initialized!");
        }
        else
        {
            Debug.LogWarning("TextMeshPro component not found on " + gameObject.name);
        }
    }

    void Update()
    {
        // Make the text slowly rotate for a nice effect
        transform.Rotate(Vector3.up, 20f * Time.deltaTime);
    }
}
