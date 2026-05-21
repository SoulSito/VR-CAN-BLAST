using UnityEngine;

public class PlayerScore
{
    int score;
    int bullets;
    GameSettingsManager.CantidadDeLatas quantity;

    public PlayerScore(
        int score = 0,
        int bullets = 3,
        GameSettingsManager.CantidadDeLatas quantity = GameSettingsManager.CantidadDeLatas.Aleatorio)
    {
        this.score = score;
        this.bullets = bullets;
        this.quantity = quantity;
    }

    public override string ToString()
    {
        /*string quantityAsString = "";

        switch (quantity)
        {
            case GameSettingsManager.CantidadDeLatas.Baja:
                quantityAsString = "Baja";
                break;
            case GameSettingsManager.CantidadDeLatas.Media:
                quantityAsString = "Media";
                break;
            case GameSettingsManager.CantidadDeLatas.Alta:
                quantityAsString = "Alta";
                break;
            case GameSettingsManager.CantidadDeLatas.Aleatorio:
                quantityAsString = "Aleatorio";
                break;
        }*/

        return "Puntuación: " + score +
            "\nBalas por cargador: " + bullets + 
            "\nCantidad de latas: " + quantity.ToString();
    }
}
