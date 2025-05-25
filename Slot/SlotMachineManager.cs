using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// - ��ť spinButton �I���A�P�ɭt�d�Ĥ@���P����
// - �z�L�ƥ󱵦� slotEvaluator �O�_������
// - �I�s SlotSpinner �Ұʱ���A�ñ�������ɶ�����
// - �䴩��t���v�]�w�]������֡^

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
        isRespin = true; // ��ܤw�ԹL�@���A�����I��������

        float spinSpeed = isReSpinCall ? 2f : 1f; // �[�t���v

        for (int i = 0; i < slotSpinners.Length; i++)
        {
            slotSpinners[i].StartSpinning(spinSpeed);
        }

        Invoke("EvaluateSlot", 2.5f / spinSpeed); // �ʵe�������� slot ���G
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
