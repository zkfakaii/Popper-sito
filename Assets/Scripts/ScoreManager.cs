using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ScoreManager : MonoBehaviour
{
    public int carritoScore = 0;
    public int tijerasScore = 0;
    public int papasScore = 0;

    public void AddPoints(string attackType, int points)
    {
        switch (attackType)
        {
            case "Carrito":
                carritoScore += points;
                break;
            case "Tijeras":
                tijerasScore += points;
                break;
            case "Papas":
                papasScore += points;
                break;
            default:
                Debug.LogWarning("Tipo de ataque no reconocido: " + attackType);
                break;
        }
        Debug.Log($"Puntos actualizados -> Carrito: {carritoScore}, Tijeras: {tijerasScore}, Papas: {papasScore}");
    }

    public bool CanUseSpecialPapasAttack()
    {
        return papasScore >= 30;
    }

    public void UseSpecialPapasAttack()
    {
        if (papasScore >= 30)
        {
            papasScore -= 30; // Consumir 30 puntos
            Debug.Log("Usaste el ataque especial de Papas");
        }
    }

    public bool CanUseSpecialTijerasAttack()
    {
        return tijerasScore >= 30;
    }

    public void UseSpecialTijerasAttack()
    {
        if (tijerasScore >= 30)
        {
            tijerasScore -= 30; // Consumir 30 puntos
            Debug.Log("Usaste el ataque especial de Tijeras");
        }
    }


}
