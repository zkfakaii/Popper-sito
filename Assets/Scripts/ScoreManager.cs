using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ScoreManager : MonoBehaviour
{
    public int carritoScore = 0;
    public int tijerasScore = 0;
    public int papasScore = 0;

    // M�todo que actualiza la puntuaci�n seg�n el tipo de ataque
    public void UpdateScore(string attackType)
    {
        switch (attackType)
        {
            case "Carrito":
                carritoScore += 10;
                Debug.Log("Puntuaci�n de Carrito: " + carritoScore);
                break;
            case "CombatController":
                tijerasScore += 10;
                Debug.Log("Puntuaci�n de Tijeras: " + tijerasScore);
                break;
            case "Papas":
                papasScore += 10;
                Debug.Log("Puntuaci�n de Papas: " + papasScore);
                break;
            default:
                Debug.Log("Tipo de ataque desconocido.");
                break;
        }
    }
}
