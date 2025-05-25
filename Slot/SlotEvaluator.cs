using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// SlotEvaluator.cs�G��X�d�P�P�ĤH�����޿�]�t�ʵe�B��������P���ѡ^
// - �N���Q slot index�]0~2�^�������a�d�I�] index �� �����d�P ID
// - index 3 ���ĤH�S������A4~5 �����q����
// - �䴩���P�d���j�Ʀ��ƻP�ĤH�����j�� 
// - �ǵ� AttackExecutor ����ڳB�z
public class SlotEvaluator : MonoBehaviour
{
    // slot ���G�������ʧ@����
    public enum SlotAction
    {
        PlayerSlot1,   // ���a�d��1
        PlayerSlot2,   // ���a�d��2
        PlayerSlot3,   // ���a�d��3
        EnemyAttack,   // �ĤH���q����
        EnemySpecial   // �ĤH�S�����
    }
    public Money moneyManager; 

    public CardSlotSystem cardSystem;                     // ���a�d���t�m�t��
    public AttackExecutor attackExecutor;                 // �ˮ`�B�z��
    public UIAnimator uiAnimator;                         // UI �ʵe�޲z��
    public DamageTextSpawner damageTextSpawner;           // �ˮ`�Ʀr���X�ĪG

    public float animationDelay = 0.3f;                   // �C�q�ʵe���������j���
    public int spinCost = 300;                            // �C�����Q���Ӫ���
    public float reSpinSpeedMultiplier = 2f;              // ����ɪ���t���v

    private bool hasReSpun = false;                       // �O�_�w�g�ϥιL�@������
    private bool isSpinning = false;                      // �O�_���b��ʤ�

    public delegate void OnSpinStart(bool isRespin);      // ���Q�Ұʨƥ�]�O�_������^
    public event OnSpinStart SpinStarted;

    // �չ϶i�歺�����Q�]�q UI Ĳ�o�^
    public void TrySpin()
    {
        if (isSpinning) return;

        if (moneyManager.money >= spinCost)
        {
            moneyManager.money -= spinCost;       // ��������
            isSpinning = true;
            SpinStarted?.Invoke(false);             // �q�� SlotManager �}�l���
        }
        else
        {
            Debug.Log("��������");
        }
    }

    // �չ϶i��@������]�ȭ��@���^
    public void TryReSpin()
    {
        if (isSpinning || hasReSpun) return;

        if (moneyManager.money >= spinCost)
        {
            moneyManager.money -= spinCost;       // ��������
            hasReSpun = true;
            SpinStarted?.Invoke(true);              // �q�� SlotManager ����]�[�t�^
        }
        else
        {
            Debug.Log("���������]����^");
        }
    }

    // �Ұʵ����y�{�]�� SlotManager �����ɩI�s�^
    public void Evaluate(int[] results)
    {
        StartCoroutine(EvaluateWithDelay(results));
    }

    // �D�ˮ`�B�z��{�A�B�z���a�����B�ĤH�����B�ʵe�P���r
    private IEnumerator EvaluateWithDelay(int[] results)
    {
        Dictionary<int, int> cardUsage = new();
        int enemyAttackCount = 0;
        int enemySpecialCount = 0;

        // �N slot ���G�����O���G���a�d���μĤH���
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

        // �B�z���a�d�P�ˮ`�ü���ʵe/���r
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

        // �B�z�ĤH���q����
        if (enemyAttackCount > 0)
        {
            int damage = attackExecutor.PreviewEnemyDamage(enemyAttackCount);
            Playerhp.HealthCurrent -= (int)(damage * attackExecutor.shield);
            uiAnimator.ShakePlayer();
            if (damageTextSpawner != null && attackExecutor.playerTransform != null)
                damageTextSpawner.SpawnText("-" + damage, attackExecutor.playerTransform.position);
            yield return new WaitForSeconds(animationDelay);
        }

        // �B�z�ĤH�S�����
        if (enemySpecialCount > 0)
        {
            int special = 40 + (enemySpecialCount - 1) * 15;
            Playerhp.HealthCurrent -= (int)(special * attackExecutor.shield);
            uiAnimator.ShakePlayer();
            if (damageTextSpawner != null && attackExecutor.playerTransform != null)
                damageTextSpawner.SpawnText("-" + special, attackExecutor.playerTransform.position);
        }

        isSpinning = false; // ������ʫ�Ѱ���w
        hasReSpun = false;  // ������|���m
    }

    // �p��d�P�X�{���ƥΩ�j�ƶˮ`
    private void CountCard(int cardID, Dictionary<int, int> usage)
    {
        if (cardID < 0) return;
        if (!usage.ContainsKey(cardID)) usage[cardID] = 0;
        usage[cardID]++;
    }
}
