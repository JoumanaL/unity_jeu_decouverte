using UnityEngine;

public class ennemi : MonoBehaviour
{
    // Tableau pour stocker les points de contact lors d'une collision
    public ContactPoint2D[] listContacts = new ContactPoint2D[1];
    
    // Détecte lorsqu'un objet entre en collision avec l'ennemi
    private void OnCollisionEnter2D(Collision2D other) 
    {
        // Vérifie si l'objet en collision est le joueur
        if (other.gameObject.CompareTag("Player")) 
        { 
            // Récupère les points de contact de la collision
            other.GetContacts(listContacts);
            
            // Vérifie si le joueur est au-dessus de l'ennemi (élimination possible)
            if (listContacts[0].normal.y < -0.5f) 
            { 
                Destroy(gameObject); // Supprime l'ennemi du jeu
            } 
            else 
            {
                // Si le joueur ne vient pas d'en haut, il subit des dégâts
                PlayerHealth dataPlayer = other.gameObject.GetComponent<PlayerHealth>();
                dataPlayer.Hurt(); // Applique des dégâts au joueur
            } 
        }
    }
}
