using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ARMenuManager : MonoBehaviour
{
    [System.Serializable]
    public class FoodItem
    {
        public GameObject model;
        public Sprite photo;
        public string foodName;
        [TextArea(2, 4)] public string description;
        public string price;
    }

    [System.Serializable]
    public class FoodCategory
    {
        public string categoryName;
        public FoodItem[] items;
    }

    [Header("Categories")]
    public FoodCategory[] categories;

    [Header("Main UI")]
    public Image foodImage;
    public TMP_Text foodNameText;
    public TMP_Text foodDescText;
    public TMP_Text priceText;

    [Header("Category UI")]
    public TMP_Text[] categoryTexts;
    public RectTransform activeUnderline;
    public Color activeCategoryColor = new Color(1f, 0.835f, 0.31f);
    public Color passiveCategoryColor = new Color(0.65f, 0.65f, 0.65f);

    [Header("Dots")]
    public Image[] dots;
    public Color activeDotColor = new Color(1f, 0.835f, 0.31f);
    public Color passiveDotColor = new Color(0.54f, 0.54f, 0.54f);

    [Header("Animation")]
    public float underlineSpeed = 10f;

    private int currentCategoryIndex = 0;
    private int currentFoodIndex = 0;
    private Vector3 targetUnderlinePos;

    void Start()
    {
        HideAllModels();
        ShowFood();
        UpdateCategoryUI();
        UpdateDots();

        if (activeUnderline != null)
            targetUnderlinePos = activeUnderline.position;
    }

    void Update()
    {
        if (activeUnderline != null)
        {
            activeUnderline.position = Vector3.Lerp(
                activeUnderline.position,
                targetUnderlinePos,
                Time.deltaTime * underlineSpeed
            );
        }
    }

    public void SetCategory(int categoryIndex)
    {
        if (categoryIndex < 0 || categoryIndex >= categories.Length)
            return;

        currentCategoryIndex = categoryIndex;
        currentFoodIndex = 0;

        ShowFood();
        UpdateCategoryUI();
        UpdateDots();
    }

    public void NextFood()
    {
        FoodCategory currentCategory = categories[currentCategoryIndex];

        if (currentCategory.items.Length == 0)
            return;

        currentFoodIndex++;

        if (currentFoodIndex >= currentCategory.items.Length)
            currentFoodIndex = 0;

        ShowFood();
        UpdateDots();
    }

    public void PreviousFood()
    {
        FoodCategory currentCategory = categories[currentCategoryIndex];

        if (currentCategory.items.Length == 0)
            return;

        currentFoodIndex--;

        if (currentFoodIndex < 0)
            currentFoodIndex = currentCategory.items.Length - 1;

        ShowFood();
        UpdateDots();
    }

    void HideAllModels()
    {
        foreach (FoodCategory category in categories)
        {
            foreach (FoodItem item in category.items)
            {
                if (item.model != null)
                    item.model.SetActive(false);
            }
        }
    }

    void ShowFood()
    {
        HideAllModels();

        if (categories.Length == 0)
            return;

        FoodCategory currentCategory = categories[currentCategoryIndex];

        if (currentCategory.items.Length == 0)
            return;

        FoodItem currentItem = currentCategory.items[currentFoodIndex];

        if (currentItem.model != null)
            currentItem.model.SetActive(true);

        if (foodImage != null && currentItem.photo != null)
            foodImage.sprite = currentItem.photo;

        if (foodNameText != null)
            foodNameText.text = currentItem.foodName;

        if (foodDescText != null)
            foodDescText.text = currentItem.description;

        if (priceText != null)
            priceText.text = currentItem.price;
    }

    void UpdateCategoryUI()
    {
        for (int i = 0; i < categoryTexts.Length; i++)
        {
            if (categoryTexts[i] != null)
            {
                categoryTexts[i].color =
                    i == currentCategoryIndex ? activeCategoryColor : passiveCategoryColor;
            }
        }

        if (activeUnderline != null &&
            categoryTexts.Length > currentCategoryIndex &&
            categoryTexts[currentCategoryIndex] != null)
        {
            Vector3 newPosition = activeUnderline.position;
            newPosition.x = categoryTexts[currentCategoryIndex].transform.position.x;
            targetUnderlinePos = newPosition;
        }
    }

    void UpdateDots()
    {
        for (int i = 0; i < dots.Length; i++)
        {
            if (dots[i] == null)
                continue;

            bool isActive = i == currentFoodIndex;

            dots[i].color = isActive ? activeDotColor : passiveDotColor;
            dots[i].transform.localScale = isActive ? Vector3.one * 1.2f : Vector3.one;
        }
    }
}