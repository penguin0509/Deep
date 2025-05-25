using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerCard
{
    public int cardID;            // 卡片 ID（1~7）
    public GameObject cardPrefab; // 對應的卡片 Prefab
}
    public class CardSlotSystem : MonoBehaviour
    {
        public List<PlayerCard> equippedCards = new List<PlayerCard>(); // 背包三格裝備卡片（順序固定）

        // 依據 slot 結果取得對應卡牌使用統計（每個 index 對應一格裝備）
        public Dictionary<int, int> GetCardUsageCount(int[] results)
        {
            Dictionary<int, int> usage = new Dictionary<int, int>();

            foreach (int index in results)
            {
                if (index >= 0 && index < equippedCards.Count)
                {
                    int usedCardID = equippedCards[index].cardID;
                    if (!usage.ContainsKey(usedCardID))
                        usage[usedCardID] = 0;
                    usage[usedCardID]++;
                }
            }

            return usage; // key = 卡片ID, value = 出現次數（1, 2, 3）
        }

        // 將 slot index 轉為對應卡牌 ID
        public int GetCardIDFromSlotIndex(int index)
        {
            if (index >= 0 && index < equippedCards.Count)
            {
                return equippedCards[index].cardID;
            }
            return -1;
        }

        // 根據卡片 ID 取得對應的 prefab
        public GameObject GetCardPrefab(int cardID)
        {
            foreach (var card in equippedCards)
            {
                if (card.cardID == cardID)
                    return card.cardPrefab;
            }
            return null;
        }
    }
