using System.Collections.Generic;
using UnityEngine;

public enum VFXType
{
    Important,
    Trash,
    Combo,
}

public class VFXPoolManager : MonoBehaviour
{
    public static VFXPoolManager Instance;

    public GameObject[] VFXPrefabs;
    private Dictionary<VFXType, Queue<GameObject>> VFXPool = new();
    public int PoolSize = 10;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        for (int i = 0; i < VFXPrefabs.Length; i++)
        {
            VFXPool.Add((VFXType)i, new Queue<GameObject>());
            for (int j = 0; j < PoolSize; j++)
            {
                GameObject obj = Instantiate(VFXPrefabs[i]);
                obj.SetActive(false);
                VFXPool[(VFXType)i].Enqueue(obj);
            }
        }
    }

    public void VFX(VFXType type, Vector3 position)
    {
        GameObject vfx = VFXPool[type].Dequeue();
        vfx.SetActive(true);
        vfx.transform.position = position;

        VFXPool[type].Enqueue(vfx);
    }
}
