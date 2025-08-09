using UnityEngine;

public class AnimationStats : MonoBehaviour
{
    private Animator animator;
    private SpectatorState currentState;
    private float stateTimer;
    
    private enum SpectatorState // state machine for the animation
    {
        Cheering,
        Clapping,
        Idle
    }
    void Start()
    {
        animator = GetComponent<Animator>();
        PickRandomState();
    }
    void Update()
    {
        stateTimer -= Time.deltaTime;

        if (stateTimer <= 0f)
        {
            PickRandomState();
        }
    }

    void PickRandomState()
    {
        int randomIndex = Random.Range(0, 3);
        currentState = (SpectatorState)randomIndex; // picks a state
        stateTimer = Random.Range(2f, 5f);
        ApplyState();
    }

    void ApplyState()
    {
        float randomOffset = Random.Range(0f, 6f) * 0.2f; // 0% to 100% of the animation | the 0.2 is to off set it in % of the animation, its so the animation starts at a random spot, asked for it to be per 20% because it will look a bit better

        if (currentState == SpectatorState.Cheering)
        {

            animator.Play("Cheering", 0, randomOffset); // state name must match Animator
        }
        else if (currentState == SpectatorState.Clapping)
        {
 
            animator.Play("Clapping", 0, randomOffset);
        }
        else if (currentState == SpectatorState.Idle)
        {

            animator.Play("Breathing", 0, randomOffset);
        }
    }
}