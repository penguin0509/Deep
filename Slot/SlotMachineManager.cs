using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// - 監聽 spinButton 點擊，同時負責第一次與重轉
// - 透過事件接收 slotEvaluator 是否為重轉
// - 呼叫 SlotSpinner 啟動旋轉，並控制評估時間延遲
// - 支援轉速倍率設定（重轉較快）

public class SlotMachineManager : MonoBehaviour
{
    public Button spinButton;
    public SlotSpinner[] slotSpinners;
    public SlotEvaluator slotEvaluator;

    private bool isRespin = false;

    void Start()
    {
        spinButton.onClick.AddListener(OnSpinClicked);
        slotEvaluator.SpinStarted += OnSpinStarted;
    }

    private void OnSpinClicked()
    {
        if (isRespin == false)
        {
            slotEvaluator.TrySpin();
        }
        else
        {
            slotEvaluator.TryReSpin();
        }
    }

    private void OnSpinStarted(bool isReSpinCall)
    {
        isRespin = true; // 表示已拉過一次，之後點擊為重轉

        float spinSpeed = isReSpinCall ? 2f : 1f; // 加速倍率

        for (int i = 0; i < slotSpinners.Length; i++)
        {
            slotSpinners[i].StartSpinning(spinSpeed);
        }

        Invoke("EvaluateSlot", 2.5f / spinSpeed); // 動畫播完評估 slot 結果
    }

    private void EvaluateSlot()
    {
        int[] results = new int[slotSpinners.Length];
        for (int i = 0; i < slotSpinners.Length; i++)
        {
            results[i] = slotSpinners[i].GetSlotResult();
        }

        slotEvaluator.Evaluate(results);
    }
}
