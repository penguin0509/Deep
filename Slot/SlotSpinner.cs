using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// - �I�s StartSpinning(speedMultiplier) �}�l��ʨýվ�t��
// - �۰ʨM�w�̲׸��I�P slot ���G
// - �i�z�L SlotMachineManager ����I�s�P�������G
public class SlotSpinner : MonoBehaviour
{
    public RectTransform slotTransform;
    public float baseSpeed = 50f;
    private float spinSpeed;

    private float[] yPositions = { 0f, 100f, 200f, 300f, 400f, 500f, 600f };
    private bool isSpinning = false;
    private float spinTime = 0f;
    private float spinDuration = 1.5f;

    private int slotResultIndex = 0;

    public void StartSpinning(float speedMultiplier)
    {
        spinSpeed = baseSpeed * speedMultiplier;
        spinTime = 0f;
        isSpinning = true;
    }

    void Update()
    {
        if (!isSpinning) return;

        spinTime += Time.deltaTime;
        slotTransform.anchoredPosition -= new Vector2(0f, spinSpeed * Time.deltaTime);

        if (slotTransform.anchoredPosition.y < 0f)
        {
            slotTransform.anchoredPosition += new Vector2(0f, yPositions[^1] + 100f);
        }

        if (spinTime >= spinDuration)
        {
            isSpinning = false;
            StopAtRandomPosition();
        }
    }

    private void StopAtRandomPosition()
    {
        slotResultIndex = Random.Range(0, yPositions.Length);
        slotTransform.anchoredPosition = new Vector2(0f, yPositions[slotResultIndex]);
    }

    public int GetSlotResult()
    {
        return slotResultIndex;
    }
}
