using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource spinAudio;
    public AudioSource stopAudio;
    public AudioSource playerAttackAudio;
    public AudioSource playerSpecialAudio;
    public AudioSource enemyAttackAudio;
    public AudioSource enemySpecialAudio;

    public void PlaySpinSound() => spinAudio?.Play();
    public void PlayStopSound() => stopAudio?.Play();
    public void PlayPlayerAttack() => playerAttackAudio?.Play();
    public void PlayPlayerSpecial() => playerSpecialAudio?.Play();
    public void PlayEnemyAttack() => enemyAttackAudio?.Play();
    public void PlayEnemySpecial() => enemySpecialAudio?.Play();
}
