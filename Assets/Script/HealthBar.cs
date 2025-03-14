using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    // Référence à l'image de remplissage de la barre de vie
    public Image fillImage;

    // Référence aux données du joueur (vie actuelle et max)
    public PlayerData dataPlayer;

    // Dégradé de couleur pour représenter l'état de la vie (ex : vert -> rouge)
    public Gradient lifeColorGradient;

    // Start est appelé une seule fois avant la première exécution d'Update
    void Start()
    {
        // Aucune initialisation spécifique pour l'instant
    }

    // Update est appelé une fois par frame
    void Update()
    {
        // Calcul du ratio de vie actuelle par rapport au maximum
        float lifeRatio = (float)dataPlayer.CurrentLifePoint / (float)dataPlayer.maxLifePoint;

        // Met à jour la barre de vie en fonction du ratio calculé
        fillImage.fillAmount = lifeRatio;

        // Change la couleur de la barre de vie en fonction du ratio
        fillImage.color = lifeColorGradient.Evaluate(lifeRatio);
    }
}
