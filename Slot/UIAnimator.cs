using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimator : MonoBehaviour
{
    public Animator playerAnimator;
    public Animator enemyAnimator;

    public void ShakePlayer() => playerAnimator?.SetTrigger("Shake");
    public void ShakeEnemy() => enemyAnimator?.SetTrigger("Shake");
    public void ResetAll()
    {
        playerAnimator?.ResetTrigger("Shake");
        enemyAnimator?.ResetTrigger("Shake");
    }
}
