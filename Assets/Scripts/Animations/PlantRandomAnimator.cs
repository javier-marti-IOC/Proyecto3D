using UnityEngine;

public class PlantRandomAnimator : MonoBehaviour
{
    [Header("Nombre exacto del estado de animación")]
    [SerializeField] private string animationStateName = "grasswind";

    [Header("Rango de velocidad aleatoria")]
    [SerializeField] private Vector2 speedRange = new Vector2(0.8f, 1.2f);

    private void Start()
    {
        Animator[] animators = GetComponentsInChildren<Animator>();

        foreach (Animator animator in animators)
        {
            if (animator == null) continue;
            
            animator.speed = Random.Range(speedRange.x, speedRange.y);
            animator.Play(animationStateName, 0, Random.value); 
            // 0 = Layer base
            // Random.value = valor aleatorio entre 0 y 1 para cambiar el inicio de la animación
        }
    }
}
