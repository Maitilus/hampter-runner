using UnityEngine;

public class PlayerHatLoader : MonoBehaviour
{
    public GameObject[] hats;

    private const string SelectedHatPrefKey = "SelectedHatIndex";

    void Start()
    {
        int selectedIndex = PlayerPrefs.GetInt(SelectedHatPrefKey, 0);

        for (int i = 0; i < hats.Length; i++)
        {
            if (hats[i] != null)
            {
                hats[i].SetActive(i == selectedIndex);
            }
        }
    }
}