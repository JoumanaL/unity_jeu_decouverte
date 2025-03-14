using NUnit.Framework;
using UnityEngine;

public class ennemi_patrol : MonoBehaviour
{
   // Composants physiques de l'ennemi
   public Rigidbody2D rb; // Référence au Rigidbody2D pour gérer les déplacements
   public BoxCollider2D bc; // Référence au BoxCollider2D pour détecter le sol et les obstacles

   // Variables pour la détection du sol et des obstacles
   public LayerMask listObstacleLayers; // LayerMask des obstacles et du sol
   public float groundCheckRadius = 0.15f; // Rayon de détection du sol
   public float moveSpeed = 3f; // Vitesse de déplacement de l'ennemi
   public bool isFacingRight = false; // Indique si l'ennemi fait face à la droite
   public float distanceDetection = 1f; // Distance de détection des obstacles

   public void FixedUpdate()
   {
       // Vérifie si l'ennemi est en train de tomber ou de sauter
       if (rb.linearVelocity.y != 0)
       {
           return; // Si l'ennemi est en l'air, il ne doit pas changer de direction
       }

       // Applique le mouvement à l'ennemi
       rb.linearVelocity = new Vector2(
           moveSpeed * (isFacingRight ? 1 : -1), // La direction dépend de isFacingRight
           rb.linearVelocity.y
       );

       // Vérifie si l'ennemi doit changer de direction
       if (HasNotTouchedGround() || HasCollisionWithObstacles())
       {
           Flip();
       }
   }

   // Vérifie si l'ennemi touche encore le sol ou s'il a atteint un bord
   bool HasNotTouchedGround()
   {
       // Position de détection du sol (devant l'ennemi)
       Vector2 detectionPosition = new Vector2(
           bc.bounds.center.x + ((isFacingRight ? 1 : -1) * (bc.bounds.size.x / 2)),
           bc.bounds.min.y
       );

       // Vérifie si le sol est présent en dessous du point détecté
       return !Physics2D.OverlapCircle(
           detectionPosition,
           groundCheckRadius,
           listObstacleLayers
       );
   }

   // Fonction qui inverse la direction de l'ennemi
   private void Flip()
   {
       isFacingRight = !isFacingRight; // Inverse la direction logique
       transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z); // Retourne le sprite horizontalement
   }

   // Vérifie si l'ennemi rencontre un obstacle devant lui
   bool HasCollisionWithObstacles()
   {
       // Lance un rayon depuis le centre du collider pour détecter les obstacles devant l'ennemi
       RaycastHit2D hit = Physics2D.Linecast(
           bc.bounds.center,
           bc.bounds.center + new Vector3(
               distanceDetection * (isFacingRight ? 1 : -1),
               0,
               0
           ),
           listObstacleLayers
       );

       return hit.transform != null; // Retourne vrai si un obstacle est détecté
   }

   // Affichage des gizmos pour le debug dans l'éditeur
   public void OnDrawGizmos()
   {
       if (bc != null)
       {
           // Dessine une ligne rouge pour la détection des obstacles
           Gizmos.color = Color.red;
           Gizmos.DrawLine(
               bc.bounds.center,
               bc.bounds.center + new Vector3(
                   distanceDetection * (isFacingRight ? 1 : -1),
                   0,
                   0
               )
           );

           // Dessine un cercle bleu pour la détection du sol
           Vector2 detectionPosition = new Vector2(
               bc.bounds.center.x + ((isFacingRight ? 1 : -1) * (bc.bounds.size.x / 2)),
               bc.bounds.min.y
           );
           Gizmos.color = Color.blue;
           Gizmos.DrawWireSphere(
               detectionPosition,
               groundCheckRadius
           );
       }
   }
}
