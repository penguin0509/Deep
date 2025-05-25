using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerCard
{
    public int cardID;            // �d�� ID�]1~7�^
    public GameObject cardPrefab; // �������d�� Prefab
}
    public class CardSlotSystem : MonoBehaviour
    {
        public List<PlayerCard> equippedCards = new List<PlayerCard>(); // �I�]�T��˳ƥd���]���ǩT�w�^

        // �̾� slot ���G���o�����d�P�ϥβέp�]�C�� index �����@��˳ơ^
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

            return usage; // key = �d��ID, value = �X�{���ơ]1, 2, 3�^
        }

        // �N slot index �ର�����d�P ID
        public int GetCardIDFromSlotIndex(int index)
        {
            if (index >= 0 && index < equippedCards.Count)
            {
                return equippedCards[index].cardID;
            }
            return -1;
        }

        // �ھڥd�� ID ���o������ prefab
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
