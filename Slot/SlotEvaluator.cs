using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// SlotEvaluator.cs：整合卡牌與敵人攻擊邏輯（含動畫、金幣機制與註解）
// - 將拉霸 slot index（0~2）對應玩家卡背包 index → 對應卡牌 ID
// - index 3 為敵人特殊攻擊，4~5 為普通攻擊
// - 支援不同卡片強化次數與敵人攻擊強化 
// - 傳給 AttackExecutor 做實際處理
public class SlotEvaluator : MonoBehaviour
{
    // slot 結果對應的動作類型
    public enum SlotAction
    {
        PlayerSlot1,   // 玩家卡槽1
        PlayerSlot2,   // 玩家卡槽2
        PlayerSlot3,   // 玩家卡槽3
        EnemyAttack,   // 敵人普通攻擊
        EnemySpecial   // 敵人特殊攻擊
    }
    public Money moneyManager; 

    public CardSlotSystem cardSystem;                     // 玩家卡片配置系統
    public AttackExecutor attackExecutor;                 // 傷害處理器
    public UIAnimator uiAnimator;                         // UI 動畫管理器
    public DamageTextSpawner damageTextSpawner;           // 傷害數字飛出效果

    public float animationDelay = 0.3f;                   // 每段動畫之間的間隔秒數
    public int spinCost = 300;                            // 每次拉霸消耗金幣
    public float reSpinSpeedMultiplier = 2f;              // 重轉時的轉速倍率

    private bool hasReSpun = false;                       // 是否已經使用過一次重轉
    private bool isSpinning = false;                      // 是否正在轉動中

    public delegate void OnSpinStart(bool isRespin);      // 拉霸啟動事件（是否為重轉）
    public event OnSpinStart SpinStarted;

    // 試圖進行首次拉霸（從 UI 觸發）
    public void TrySpin()
    {
        if (isSpinning) return;

        if (moneyManager.money >= spinCost)
        {
            moneyManager.money -= spinCost;       // 扣除金幣
            isSpinning = true;
            SpinStarted?.Invoke(false);             // 通知 SlotManager 開始轉動
        }
        else
        {
            Debug.Log("金幣不足");
        }
    }

    // 試圖進行一次重轉（僅限一次）
    public void TryReSpin()
    {
        if (isSpinning || hasReSpun) return;

        if (moneyManager.money >= spinCost)
        {
            moneyManager.money -= spinCost;       // 扣除金幣
            hasReSpun = true;
            SpinStarted?.Invoke(true);              // 通知 SlotManager 重轉（加速）
        }
        else
        {
            Debug.Log("金幣不足（重轉）");
        }
    }

    // 啟動評估流程（由 SlotManager 結束時呼叫）
    public void Evaluate(int[] results)
    {
        StartCoroutine(EvaluateWithDelay(results));
    }

    // 主傷害處理協程，處理玩家攻擊、敵人攻擊、動畫與飛字
    private IEnumerator EvaluateWithDelay(int[] results)
    {
        Dictionary<int, int> cardUsage = new();
        int enemyAttackCount = 0;
        int enemySpecialCount = 0;

        // 將 slot 結果分類記錄：玩家卡片或敵人行動
        foreach (int index in results)
        {
            switch (index)
            {
                case 0:
                    CountCard(cardSystem.GetCardIDFromSlotIndex(0), cardUsage);
                    break;
                case 1:
                    CountCard(cardSystem.GetCardIDFromSlotIndex(1), cardUsage);
                    break;
                case 2:
                    CountCard(cardSystem.GetCardIDFromSlotIndex(2), cardUsage);
                    break;
                case 3:
                    enemySpecialCount++;
                    break;
                case 4:
                case 5:
                    enemyAttackCount++;
                    break;
            }
        }

        // 處理玩家卡牌傷害並播放動畫/飛字
        foreach (var kvp in cardUsage)
        {
            int cardID = kvp.Key;
            int count = kvp.Value;
            int damage = attackExecutor.PreviewPlayerDamage(cardID, count);
            Masterhp.HealthCurrent -= damage;
            uiAnimator.ShakeEnemy();
            if (damageTextSpawner != null && attackExecutor.masterTransform != null)
                damageTextSpawner.SpawnText("-" + damage, attackExecutor.masterTransform.position);
            yield return new WaitForSeconds(animationDelay);
        }

        // 處理敵人普通攻擊
        if (enemyAttackCount > 0)
        {
            int damage = attackExecutor.PreviewEnemyDamage(enemyAttackCount);
            Playerhp.HealthCurrent -= (int)(damage * attackExecutor.shield);
            uiAnimator.ShakePlayer();
            if (damageTextSpawner != null && attackExecutor.playerTransform != null)
                damageTextSpawner.SpawnText("-" + damage, attackExecutor.playerTransform.position);
            yield return new WaitForSeconds(animationDelay);
        }

        // 處理敵人特殊攻擊
        if (enemySpecialCount > 0)
        {
            int special = 40 + (enemySpecialCount - 1) * 15;
            Playerhp.HealthCurrent -= (int)(special * attackExecutor.shield);
            uiAnimator.ShakePlayer();
            if (damageTextSpawner != null && attackExecutor.playerTransform != null)
                damageTextSpawner.SpawnText("-" + special, attackExecutor.playerTransform.position);
        }

        isSpinning = false; // 完成轉動後解除鎖定
        hasReSpun = false;  // 重轉機會重置
    }

    // 計算卡牌出現次數用於強化傷害
    private void CountCard(int cardID, Dictionary<int, int> usage)
    {
        if (cardID < 0) return;
        if (!usage.ContainsKey(cardID)) usage[cardID] = 0;
        usage[cardID]++;
    }
}
