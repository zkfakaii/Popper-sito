using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Necesario para usar TextMeshPro

public class DialogController : MonoBehaviour
{
    public TextMeshProUGUI dialogText; // Campo para mostrar el texto usando TextMeshPro
    public List<string> sentences; // Lista de frases del diálogo
    private int sentenceIndex = 0;

    private bool isInsideTrigger = false; // Verifica si el jugador está dentro del trigger

    void Start()
    {
        dialogText.text = ""; // Vacía el texto al inicio
    }

    void Update()
    {
        if (isInsideTrigger && Input.GetKeyDown(KeyCode.Space))
        {
            DisplayNextSentence(); // Avanza en el diálogo al presionar espacio
        }
    }

    public void StartDialog()
    {
        sentenceIndex = 0;
        dialogText.text = sentences[sentenceIndex]; // Muestra la primera frase
    }

    public void DisplayNextSentence()
    {
        sentenceIndex++;
        if (sentenceIndex < sentences.Count)
        {
            dialogText.text = sentences[sentenceIndex]; // Muestra la siguiente frase
        }
        else
        {
            sentenceIndex = 0; // Reinicia al primer diálogo
            dialogText.text = sentences[sentenceIndex]; // Muestra la primera frase
        }
    }

    // Detecta cuando el jugador entra en el trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInsideTrigger = true;
            StartDialog(); // Comienza el diálogo al entrar en el trigger
        }
    }

    // Detecta cuando el jugador sale del trigger
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInsideTrigger = false;
        }
    }
}
