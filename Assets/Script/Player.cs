using UnityEngine;

public class Player : MonoBehaviour
{
    // Composants du joueur
    public Rigidbody2D rb; // Gère la physique du joueur
    public BoxCollider2D bc; // Gère les collisions du joueur

    // Variables de mouvement
    public float moveDirectionX = 0; // Direction du mouvement horizontal
    public float moveDirectionY = 0; // Direction du mouvement vertical
    public float moveSpeed = 5; // Vitesse de déplacement
    public float jumpForce = 7; // Force de saut

    // Détection du sol
    public Transform groundCheck; // Position utilisée pour détecter si le joueur touche le sol
    public float groundCheckRadius = 0.2f; // Rayon de détection du sol
    public bool isGrounded = false; // Indique si le joueur est au sol

    public LayerMask listeGroundLayers; // Définition des couches considérées comme sol
    public Animator animator; // Référence à l'Animator pour gérer les animations

    // Gestion des sauts
    public int maxAllowedJumps = 3; // Nombre maximum de sauts autorisés
    public int currentNumberJumps = 0; // Compteur de sauts en cours

    public bool isFacingRight = false; // Indique si le joueur est tourné vers la droite

    // Événements liés au joueur
    public VoidEventChannel onPlayerDeath;
    public VoidEventChannel onGameResume;
    public VoidEventChannel onGamePause;

    public bool isGamePaused = false; // Indique si le jeu est en pause

    // Abonnement aux événements lors de l'activation de l'objet
    private void OnEnable()
    {
        onPlayerDeath.OnEventRaised += Die;
        onGamePause.OnEventRaised += onPause;
        onGameResume.OnEventRaised += onResume;
    }

    // Désabonnement aux événements lors de la désactivation de l'objet
    private void OnDisable()
    {
        onPlayerDeath.OnEventRaised -= Die;
        onGamePause.OnEventRaised -= onPause;
        onGameResume.OnEventRaised -= onResume;
    }

    void Start()
    {
        // Initialisation (actuellement vide)
    }

    // Met le jeu en pause
    public void onPause()
    {
        isGamePaused = true;
    }

    // Reprend le jeu après une pause
    public void onResume()
    {
        isGamePaused = false;
    }

    // Gère la mort du joueur
    void Die()
    {
        rb.bodyType = RigidbodyType2D.Static; // Désactive les mouvements
        bc.enabled = false; // Désactive les collisions
        enabled = false; // Désactive le script
    }

    // Gère les entrées utilisateur et les animations
    void Update()
    {
        if (isGamePaused)
        {
            return;
        }

        // Mise à jour des paramètres de l'Animator
        animator.SetFloat("VelocityX", Mathf.Abs(rb.linearVelocityX));
        animator.SetFloat("VelocityY", rb.linearVelocityY);
        animator.SetBool("IsGrounded", isGrounded);

        // Détection des entrées utilisateur
        moveDirectionX = Input.GetAxis("Horizontal");

        // Gestion des sauts
        if (Input.GetButtonDown("Jump") && currentNumberJumps < maxAllowedJumps)
        {
            Jump();
            currentNumberJumps++;

            // Déclenchement d'une animation de double saut
            if (currentNumberJumps > 1)
            {
                animator.SetTrigger("DoubleJump");
            }
        }

        // Réinitialisation du compteur de sauts quand le joueur touche le sol
        if (isGrounded && !Input.GetButton("Jump"))
        {
            currentNumberJumps = 0;
        }

        // Gestion de l'orientation du joueur
        Flip();
    }

    // Gère l'orientation du joueur en fonction de la direction du mouvement
    void Flip()
    {
        if ((moveDirectionX > 0 && !isFacingRight) || (moveDirectionX < 0 && isFacingRight))
        {
            transform.Rotate(0, 180, 0);
            isFacingRight = !isFacingRight;
        }
    }

    // Applique une force de saut au joueur
    private void Jump()
    {
        rb.linearVelocity = new Vector2(
            rb.linearVelocity.x,
            jumpForce
        );
    }

    // Gère le mouvement du joueur et la détection du sol
    private void FixedUpdate()
    {
        // Déplacement horizontal du joueur
        rb.linearVelocity = new Vector2(
            moveDirectionX * moveSpeed,
            rb.linearVelocity.y
        );

        // Mise à jour de l'état du sol
        isGrounded = IsGrounded();
    }

    // Vérifie si le joueur touche le sol
    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            listeGroundLayers
        );
    }

    // Affichage des gizmos dans l'éditeur pour le debug
    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(
                groundCheck.position,
                groundCheckRadius
            );
        }
    }
}
