using UnityEngine;
using UnityEngine.UI;

public class DistanceUIHandler : MonoBehaviour
{
    void Start()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.distanceUI = GetComponent<Text>();
        }
    }
}
