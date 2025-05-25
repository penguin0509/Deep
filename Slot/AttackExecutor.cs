using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AttackExecutor : MonoBehaviour
{
    public float shield = 1f; // ����ĤH�����ɪ��ˮ`���v�վ�]�i�]�d�P�v�T�^
    public float curse = 0.2f; // �A�G�d�P�y���۶˪��ʤ���

    public AudioManager audioManager; // ���ĺ޲z��
    public UIAnimator uiAnimator;     // �ʵe���
    public CardSlotSystem cardSystem; // �d�P�˳ƨt��
    public Transform masterTransform; // �Ω�ˮ`�r��m
    public Transform playerTransform; // �Ω�ˮ`�r��m
    public DamageTextSpawner damageTextSpawner;

    // �C���԰��e���m�@�ޭ��v
    public void ResetState()
    {
        shield = 1f;
    }

    // �D�{���G�̾ڥd���ϥά����P�ĤH�������ư����ڧ���
    public void Execute(Dictionary<int, int> cardUsages, int enemyAttackCount, int enemySpecialCount)
    {
        // �B�z���a�d������
        foreach (var kvp in cardUsages)
        {
            int cardID = kvp.Key;
            int count = kvp.Value; // �ϥΦ���

            int damage = GetPlayerCardDamage(cardID, count);
            Masterhp.HealthCurrent -= damage; // �����ĤH��q

            audioManager.PlayPlayerAttack();
            uiAnimator.ShakeEnemy();
            if (damageTextSpawner != null && masterTransform != null)
                damageTextSpawner.SpawnText("-" + damage, masterTransform.position);
        }

        // �B�z�ĤH���q����
        if (enemyAttackCount > 0)
        {
            int damage = GetEnemyDamage(enemyAttackCount);
            Playerhp.HealthCurrent -= (int)(damage * shield);
            audioManager.PlayEnemyAttack();
            uiAnimator.ShakePlayer();
            if (damageTextSpawner != null && playerTransform != null)
                damageTextSpawner.SpawnText("-" + damage, playerTransform.position);
        }

        // �B�z�ĤH�S�����
        if (enemySpecialCount > 0)
        {
            int specialDamage = 40 + (enemySpecialCount - 1) * 15;
            Playerhp.HealthCurrent -= (int)(specialDamage * shield);
            audioManager.PlayEnemySpecial();
            uiAnimator.ShakePlayer();
            if (damageTextSpawner != null && playerTransform != null)
                damageTextSpawner.SpawnText("-" + specialDamage, playerTransform.position);
        }
    }

    public int PreviewPlayerDamage(int cardID, int count)
    {
        return GetPlayerCardDamage(cardID, count);
    }

    public int PreviewEnemyDamage(int count)
    {
        return GetEnemyDamage(count);
    }

    private int GetPlayerCardDamage(int cardID, int count)
    {
        switch (cardID)
        {
            case 1:
                return count switch { 1 => 16, 2 => 19, 3 => 22, _ => 10 };
            case 2:
                return count switch { 1 => 22, 2 => 27, 3 => 30, _ => 20 };
            case 3:
                shield = count switch { 1 => 0.5f, 2 => 0.6f, 3 => 0.7f, _ => 1f }; // ���Ѩ��@��
                return 10;
            case 4:
                return count switch { 1 => 12, 2 => 18, 3 => 25, _ => 10 };
            case 5:
                int extraCurse = count == 3 ? (int)(Playerhp.HealthCurrent * curse) : 0; // �̰��ŶA�G�|�����a��
                Playerhp.HealthCurrent -= extraCurse;
                return count switch { 1 => 14, 2 => 17, 3 => 30, _ => 15 };
            case 6:
                return count switch { 1 => 10, 2 => 15, 3 => 20, _ => 10 };
            case 7:
                return count switch { 1 => 18, 2 => 24, 3 => 35, _ => 15 };
            default:
                return 5;
        }
    }

    private int GetEnemyDamage(int count)
    {
        return count switch
        {
            1 => 15,
            2 => 30,
            3 => 50,
            _ => 10
        };
    }
}
