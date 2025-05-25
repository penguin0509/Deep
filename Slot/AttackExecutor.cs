using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AttackExecutor : MonoBehaviour
{
    public float shield = 1f; // 受到敵人攻擊時的傷害倍率調整（可因卡牌影響）
    public float curse = 0.2f; // 詛咒卡牌造成自傷的百分比

    public AudioManager audioManager; // 音效管理器
    public UIAnimator uiAnimator;     // 動畫控制器
    public CardSlotSystem cardSystem; // 卡牌裝備系統
    public Transform masterTransform; // 用於傷害字位置
    public Transform playerTransform; // 用於傷害字位置
    public DamageTextSpawner damageTextSpawner;

    // 每次戰鬥前重置護盾倍率
    public void ResetState()
    {
        shield = 1f;
    }

    // 主程式：依據卡片使用紀錄與敵人攻擊次數執行實際攻擊
    public void Execute(Dictionary<int, int> cardUsages, int enemyAttackCount, int enemySpecialCount)
    {
        // 處理玩家卡片攻擊
        foreach (var kvp in cardUsages)
        {
            int cardID = kvp.Key;
            int count = kvp.Value; // 使用次數

            int damage = GetPlayerCardDamage(cardID, count);
            Masterhp.HealthCurrent -= damage; // 扣除敵人血量

            audioManager.PlayPlayerAttack();
            uiAnimator.ShakeEnemy();
            if (damageTextSpawner != null && masterTransform != null)
                damageTextSpawner.SpawnText("-" + damage, masterTransform.position);
        }

        // 處理敵人普通攻擊
        if (enemyAttackCount > 0)
        {
            int damage = GetEnemyDamage(enemyAttackCount);
            Playerhp.HealthCurrent -= (int)(damage * shield);
            audioManager.PlayEnemyAttack();
            uiAnimator.ShakePlayer();
            if (damageTextSpawner != null && playerTransform != null)
                damageTextSpawner.SpawnText("-" + damage, playerTransform.position);
        }

        // 處理敵人特殊攻擊
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
                shield = count switch { 1 => 0.5f, 2 => 0.6f, 3 => 0.7f, _ => 1f }; // 提供防護盾
                return 10;
            case 4:
                return count switch { 1 => 12, 2 => 18, 3 => 25, _ => 10 };
            case 5:
                int extraCurse = count == 3 ? (int)(Playerhp.HealthCurrent * curse) : 0; // 最高級詛咒會扣玩家血
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
