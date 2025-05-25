using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextSpawner : MonoBehaviour
{
    public GameObject textPrefab;

    public void SpawnText(string text, Vector3 worldPos)
    {
        GameObject obj = Instantiate(textPrefab, worldPos, Quaternion.identity);
        obj.GetComponentInChildren<TextMesh>().text = text;
        Destroy(obj, 1.5f);
    }
}
