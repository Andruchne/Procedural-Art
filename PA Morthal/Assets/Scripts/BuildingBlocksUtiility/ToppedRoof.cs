using UnityEngine;

public class ToppedRoof : MonoBehaviour
{
    [SerializeField] GameObject top;

    public void HideTop()
    {
        top.SetActive(false);
    }
}
