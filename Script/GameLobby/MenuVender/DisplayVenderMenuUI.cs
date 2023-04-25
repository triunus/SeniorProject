using UnityEngine;

using System.IO;
using Newtonsoft.Json.Linq;

public class DisplayVenderMenuUI : MonoBehaviour
{
    private CreatePrefab createPrefab;

    [SerializeField]
    private RectTransform showcaseOfProduct;

    private void Awake()
    {
        createPrefab = GameObject.FindWithTag("GameManager").GetComponent<CreatePrefab>();
    }

    public void DisplayProductsList(string productsStinrg)
    {
        JArray products = JArray.Parse(productsStinrg);

        JArray skillData = JArray.Parse(File.ReadAllText("./Assets/GameData/SkillsInfo/SkillsInfo.json"));

        createPrefab.DestroyChild02(showcaseOfProduct);

        createPrefab.CreateVenderProductSlot(showcaseOfProduct, products, skillData);
    }
}
