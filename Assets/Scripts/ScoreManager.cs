using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreManager : MonoBehaviour
{
    public int carritoScore = 0;
    public int tijerasScore = 0;
    public int papasScore = 0;
    public Image carrito;
    public List <Sprite> Carritosp;
    public Image tijeras;
    public List <Sprite> tijerassp;
    public Image  papas;
    public List <Sprite>  papassp;

    public void Start(){
        UpdateUI();
    }

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

        UpdateUI();
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
            UpdateUI();
        }
    }

      public void UpdateUI()
    {
        carrito.sprite = carritoScore < 10 ? Carritosp[0] : carritoScore < 20 ? Carritosp[1] : carritoScore < 30? Carritosp[2] : Carritosp[3];
        tijeras.sprite = tijerasScore < 10 ? tijerassp[0] : tijerasScore < 20 ? tijerassp[1] : tijerasScore < 30? tijerassp[2] : tijerassp[3];
        papas.sprite =  papasScore < 10 ?  papassp[0] :  papasScore < 20 ?  papassp[1] :  papasScore < 30?  papassp[2] :  papassp[3];
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
            UpdateUI();
        }
    }
    public void UseSpecialCarritoAttack()
    {
        if (carritoScore >= 30)
        {
            carritoScore -= 30; // Consumir 30 puntos
            Debug.Log("Usaste el ataque especial de Tijeras");
            UpdateUI();
        }
    }


}
