using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    // Référence aux données du joueur (points de vie)
    public PlayerData dataPlayer;

    // Indique si le joueur est invulnérable après avoir été touché
    public bool isInvulnerable = false;
    public float invulnerableTime = 1.5f; // Durée d'invulnérabilité après un coup
    public float invulnerableFlash = 0.2f; // Temps entre chaque clignotement pendant l'invulnérabilité
    public SpriteRenderer sr; // Référence au SpriteRenderer pour gérer l'effet visuel d'invulnérabilité

    // Événement déclenché lorsque le joueur meurt
    public VoidEventChannel onPlayerDeath;

    // Initialisation au démarrage du script
    void Start()
    {
        isInvulnerable = false; // Le joueur commence vulnérable
        dataPlayer.CurrentLifePoint = dataPlayer.maxLifePoint; // Initialise les points de vie du joueur
    }

    // Fonction qui gère la prise de dégâts du joueur
    public void Hurt(int damage = 1)
    {
        // Si le joueur est invulnérable, il ne prend pas de dégâts
        if (isInvulnerable)
        {
            return;
        }

        // Réduction des points de vie en fonction des dégâts reçus
        dataPlayer.CurrentLifePoint = dataPlayer.CurrentLifePoint - damage;

        // Si les points de vie tombent à zéro, déclenche l'événement de mort
        if (dataPlayer.CurrentLifePoint <= 0)
        {
            onPlayerDeath.Raise();
        }
        else
        {
            // Active l'invulnérabilité après avoir pris un coup
            StartCoroutine(Invulnerable());
        }
    }

    // Coroutine qui gère l'invulnérabilité temporaire après avoir pris des dégâts
    IEnumerator Invulnerable()
    {
        isInvulnerable = true; // Active l'invulnérabilité
        Color startColor = sr.color; // Sauvegarde la couleur d'origine du joueur
        WaitForSeconds invulnerableFlashWait = new WaitForSeconds(invulnerableFlash);

        // Fait clignoter le joueur pour indiquer l'invulnérabilité
        for (float i = 0; i <= invulnerableTime; i += invulnerableFlash)
        {
            if (sr.color.a == 1)
            {
                sr.color = Color.clear; // Rend le joueur invisible
            }
            else
            {
                sr.color = startColor; // Restaure la couleur initiale
            }
            yield return invulnerableFlashWait;
        }

        // Fin de l'invulnérabilité, le joueur retrouve sa couleur d'origine
        sr.color = startColor;
        isInvulnerable = false;
        yield return null;
    }
}
