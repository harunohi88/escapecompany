using UnityEngine;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        } else if (Instance != this)
        {
            Destroy(Instance.gameObject);
        }

    }
}
