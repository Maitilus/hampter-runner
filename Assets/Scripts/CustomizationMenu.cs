using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CustomizationMenu : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI currencyText;
    public Button[] hatButtons;
    public int[] hatCosts;
    public TextMeshProUGUI[] priceTexts;

    [Header("Button Visuals")]
    public Color selectedColor = new Color(0.3f, 0.85f, 0.4f);
    public Color unselectedColor = Color.white;
    public Color lockedColor = new Color(0.5f, 0.5f, 0.5f);

    private const string CurrencyPrefKey = "TotalCurrency";
    private const string SelectedHatPrefKey = "SelectedHatIndex";
    private const string HatUnlockedPrefix = "HatUnlocked_";

    private int currentCurrency;
    private int selectedHatIndex;

    void Awake()
    {
        PlayerPrefs.SetInt(HatUnlockedPrefix + 0, 1);

        for (int i = 0; i < hatButtons.Length; i++)
        {
            int index = i;
            hatButtons[i].onClick.AddListener(() => OnHatButtonClicked(index));
        }
    }

    void OnEnable()
    {
        currentCurrency = PlayerPrefs.GetInt(CurrencyPrefKey, 0);
        selectedHatIndex = PlayerPrefs.GetInt(SelectedHatPrefKey, 0);

        if (selectedHatIndex < 0 || selectedHatIndex >= hatButtons.Length || !IsHatUnlocked(selectedHatIndex))
        {
            selectedHatIndex = 0;
        }

        RefreshUI();
    }

    private void OnHatButtonClicked(int index)
    {
        if (!IsHatUnlocked(index))
        {
            TryUnlockHat(index);
        }
        else
        {
            SelectHat(index);
        }
    }

    private void TryUnlockHat(int index)
    {
        int cost = GetHatCost(index);

        if (currentCurrency >= cost)
        {
            currentCurrency -= cost;
            PlayerPrefs.SetInt(CurrencyPrefKey, currentCurrency);
            PlayerPrefs.SetInt(HatUnlockedPrefix + index, 1);
            PlayerPrefs.Save();

            RefreshUI();
        }
        else
        {
            Debug.Log("Not enough currency to unlock this hat.");
        }
    }

    private void SelectHat(int index)
    {
        selectedHatIndex = index;
        PlayerPrefs.SetInt(SelectedHatPrefKey, selectedHatIndex);
        PlayerPrefs.Save();

        RefreshUI();
    }

    private void RefreshUI()
    {
        if (currencyText != null)
        {
            currencyText.text = "$" + currentCurrency;
        }

        for (int i = 0; i < hatButtons.Length; i++)
        {
            bool unlocked = IsHatUnlocked(i);
            Image btnImage = hatButtons[i].GetComponent<Image>();

            if (btnImage != null)
            {
                if (!unlocked)
                    btnImage.color = lockedColor;
                else
                    btnImage.color = (i == selectedHatIndex) ? selectedColor : unselectedColor;
            }

            if (priceTexts != null && i < priceTexts.Length && priceTexts[i] != null)
            {
                if (unlocked || i == 0)
                {
                    Destroy(priceTexts[i].gameObject);
                    priceTexts[i] = null;
                }
                else
                {
                    priceTexts[i].text = "$" + GetHatCost(i);
                }
            }
        }
    }

    private bool IsHatUnlocked(int index)
    {
        if (index == 0) return true;
        return PlayerPrefs.GetInt(HatUnlockedPrefix + index, 0) == 1;
    }

    private int GetHatCost(int index)
    {
        if (index == 0 || hatCosts == null || index >= hatCosts.Length) return 0;
        return hatCosts[index];
    }
}