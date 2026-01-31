using UnityEngine;

public class KeyTableStateSync : MonoBehaviour
{
    [Header("State source")]
    public GameObject keyMaskTable;

    [Header("State target")]
    public GameObject keyNormalTable;

    void Update()
    {
        if (keyMaskTable == null || keyNormalTable == null)
            return;

        keyNormalTable.SetActive(keyMaskTable.activeSelf);
    }
}