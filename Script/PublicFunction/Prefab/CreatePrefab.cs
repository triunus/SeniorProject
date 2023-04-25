using UnityEngine;
using UnityEngine.UI;
using TMPro;

using System;
using Newtonsoft.Json.Linq;

using SkillInformaion;

// Prefab�� �����ϰ�, �����ϴ� �Լ����� ����.
// �ߺ��Ǵ� �Լ���� �������� �ſ�ſ�ſ�ſ� ����.
// ���ʷ� ������ ���� ��, �ڵ��� �κ��Լ�ȭ�� ���� ����ȭ ��Ű��.
public class CreatePrefab : MonoBehaviour
{
    public void CreateCharacterSlot(RectTransform characterPanel, string[] characterList)
    {
        GameObject[] characterSlot = new GameObject[characterList.Length];

        for (int i = 0; i < characterList.Length; i++)
        {
            characterSlot[i] = Instantiate(Resources.Load<GameObject>("Prefab/GameLobby/CharacterSlot"));
            characterSlot[i].transform.SetParent(characterPanel);
            characterSlot[i].SetActive(true);

            string character_path = "CharacterImage/Character" + characterList[i];

            // ���� ������Ʈ�� Image�� �ٲ۴�.
            // skillSlot[i].GetComponent<Image>().sprite = Resources.Load<Sprite>(skill_path);
            // �ڽ� ������Ʈ�� Image�� �ٲ۴�.
            characterSlot[i].transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(character_path);
        }
    }







    public void CreateSkillContent(RectTransform skillSlot, string[] currentSkillData, JArray skillsData)
    {
        GameObject contentSlot = Instantiate(Resources.Load<GameObject>("Prefab/SkillContent"));
        contentSlot.transform.SetParent(skillSlot.root);
        contentSlot.SetActive(true);
        contentSlot.transform.SetSiblingIndex(1);

        //        ���� �ڵ�� ����, ��ų�� ���� ���̿� ���� �������� �����ϵ��� �ϱ� ���� ����� �����̴�.
        //        content.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 300);
        //        content.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 80);

        // ��ų �������� ���� 150, -60 ��ŭ �̵��ؾ�, ��ų ������ UI�� ���� ����� ��ų ������ �߾ӿ� ��ġ�Ѵ�. 20�� �� �� �̵��� ���� ��.
        contentSlot.transform.position = skillSlot.position + new Vector3(170, -80, 0);

        string content = (string)skillsData[Int32.Parse(currentSkillData[0]) - 1]["SkillDamage"] + " "
            + ((string)skillsData[(Int32.Parse(currentSkillData[0]) - 1)]["SkillDetail"]).Split('_')[0]
            + (string)skillsData[(Int32.Parse(currentSkillData[0]) - 1)]["NumberOfTargets"]
            + ((string)skillsData[(Int32.Parse(currentSkillData[0]) - 1)]["SkillDetail"]).Split('_')[1];

        contentSlot.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = (string)skillsData[(Int32.Parse(currentSkillData[0]) - 1)]["SkillName"];
        contentSlot.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = content;

        if (skillSlot.parent.parent.tag == "SelectedInLobby" || skillSlot.parent.parent.parent.tag == "SkillListPanel" || skillSlot.parent.parent.tag == "SelectedSkillListPanel")
        {
            contentSlot.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text
                = contentSlot.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text.Replace("99+", currentSkillData[2]);
            contentSlot.transform.GetChild(2).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text
                = contentSlot.transform.GetChild(2).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text.Replace("false", currentSkillData[3]);
        }
    }


    // �Ϲ� ��ų�� ���
    // RectTransform�� �ѱ� : .root ������, .position(), .name�� �ʿ��ϴ�.
    public void CreateSkillContent01(RectTransform skillSlotPosition, JArray skillsData)
    {
        GameObject contentSlot = Instantiate(Resources.Load<GameObject>("Prefab/GameLobby/SkillContent"));
        contentSlot.transform.SetParent(skillSlotPosition.root);
        contentSlot.SetActive(true);
        contentSlot.transform.SetSiblingIndex(1);

        //        ���� �ڵ�� ����, ��ų�� ���� ���̿� ���� �������� �����ϵ��� �ϱ� ���� ����� �����̴�.
        //        content.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 300);
        //        content.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 80);

        // ��ų �������� ���� 150, -60 ��ŭ �̵��ؾ�, ��ų ������ UI�� ���� ����� ��ų ������ �߾ӿ� ��ġ�Ѵ�. 20�� �� �� �̵��� ���� ��.
        contentSlot.transform.position = skillSlotPosition.position + new Vector3(170, -80, 0);

        string skillNumber = skillSlotPosition.GetComponent<Image>().sprite.name.Replace("Skill", String.Empty);

        string content = (string)skillsData[Int32.Parse(skillNumber) - 1]["SkillDamage"] + " "
            + ((string)skillsData[(Int32.Parse(skillNumber) - 1)]["SkillDetail"]).Split('_')[0]
            + (string)skillsData[(Int32.Parse(skillNumber) - 1)]["NumberOfTargets"]
            + ((string)skillsData[(Int32.Parse(skillNumber) - 1)]["SkillDetail"]).Split('_')[1];

        // ��ų �̸��� ���� �ۼ�
        contentSlot.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = (string)skillsData[(Int32.Parse(skillNumber) - 1)]["SkillName"];
        contentSlot.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = content;

        // count�� NFT ���� �ۼ�
        contentSlot.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "99+";
        contentSlot.transform.GetChild(2).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "false";
    }           // ��

    // count�� NFT ���ΰ� �ִ� ��� ���
    public void CreateSkillContent02(RectTransform skillSlotPosition, JArray skillsData)
    {
        GameObject contentSlot = Instantiate(Resources.Load<GameObject>("Prefab/GameLobby/SkillContent"));
        contentSlot.transform.SetParent(skillSlotPosition.root);
        contentSlot.SetActive(true);
        contentSlot.transform.SetSiblingIndex(1);

        //        ���� �ڵ�� ����, ��ų�� ���� ���̿� ���� �������� �����ϵ��� �ϱ� ���� ����� �����̴�.
        //        content.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 300);
        //        content.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 80);

        // ��ų �������� ���� 150, -60 ��ŭ �̵��ؾ�, ��ų ������ UI�� ���� ����� ��ų ������ �߾ӿ� ��ġ�Ѵ�. 20�� �� �� �̵��� ���� ��.
        contentSlot.transform.position = skillSlotPosition.position + new Vector3(170, -80, 0);

        string skillNumber = skillSlotPosition.GetChild(0).GetComponent<Image>().sprite.name.Replace("Skill", String.Empty);

        string content = (string)skillsData[Int32.Parse(skillNumber) - 1]["SkillDamage"] + " "
            + ((string)skillsData[(Int32.Parse(skillNumber) - 1)]["SkillDetail"]).Split('_')[0]
            + (string)skillsData[(Int32.Parse(skillNumber) - 1)]["NumberOfTargets"]
            + ((string)skillsData[(Int32.Parse(skillNumber) - 1)]["SkillDetail"]).Split('_')[1];

        // ��ų �̸��� ���� �ۼ�
        contentSlot.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = (string)skillsData[(Int32.Parse(skillNumber) - 1)]["SkillName"];
        contentSlot.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = content;

        // count�� NFT ���� �ۼ�
        contentSlot.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = skillSlotPosition.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text;
        contentSlot.transform.GetChild(2).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = skillSlotPosition.GetChild(0).name;
    }           // ��




    // ĳ���� �̹��� ������ ����, ĳ���Ͱ� ���� ��ų ��ȣ ����
    // ->  json���� ��ų ��ȣ�� �ش� �ϴ� ������ ���.
    public void CreateCharacterSkillDetailPanel(RectTransform skillDetailPanel, JObject jObject)
    {
        //        CreateSlot(skillDetailPanel, (string)jObject["SkillNumber"]);

        GameObject tmp = Instantiate(Resources.Load<GameObject>("Prefab/GameLobby/CharacterSkillDetail"));
        tmp.transform.SetParent(skillDetailPanel);
        tmp.SetActive(true);
        tmp.transform.SetSiblingIndex(0);

        string content = (string)jObject["SkillDamage"] + " "
            + ((string)jObject["SkillDetail"]).Split('_')[0]
            + ((string)jObject["NumberOfTargets"])
            + ((string)jObject["SkillDetail"]).Split('_')[1];

        tmp.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("SkillImage/Skill" + (string)jObject["SkillNumber"]);
        tmp.transform.GetChild(0).GetChild(0).GetComponent<Image>().name = (string)jObject["SkillNumber"];
        tmp.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = (string)jObject["SkillName"];    // name
        tmp.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = content;    // content
    }






    // �κ񿡼� ĳ���� ��ȣ�� ���� �������� ��ų ��ȣ�� �̿��Ͽ� ������ ����.
    public void CreateSkillSlot01InLobby(RectTransform skillsPanel, string[] characterSkillNumber)
    {
        for (int i = 0; i < characterSkillNumber.Length; i++)
        {
            GameObject skillSlot = Instantiate(Resources.Load<GameObject>("Prefab/GameLobby/SkillSlot01InLobby"));
            skillSlot.transform.SetParent(skillsPanel);
            skillSlot.SetActive(true);
            skillSlot.transform.SetSiblingIndex(1);

            skillSlot.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("SkillImage/Skill" + characterSkillNumber[i]);
        }
    }       // ��

    public void CreateSkillSlot02InLobby(RectTransform skillsPanel, UserSkillDataInfo[] selectedSkills)
    {
        for (int i = 0; i < selectedSkills.Length; i++)
        {
            GameObject skillSlot = Instantiate(Resources.Load<GameObject>("Prefab/GameLobby/SkillSlot02InLobby"));
            skillSlot.transform.SetParent(skillsPanel);
            skillSlot.SetActive(true);

            skillSlot.transform.name = selectedSkills[i].UniqueNumber;
            skillSlot.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("SkillImage/Skill" + selectedSkills[i].SkillNumber);   //image
            skillSlot.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = selectedSkills[i].Count;    // count
            skillSlot.transform.GetChild(0).name = selectedSkills[i].IsNFT;    // isNFT
        }
    }   // ��








/*    // �κ��丮���� �������� ������ Json������ �̿��Ͽ� ������ ����.
    public void CreateSkillSlot01InInventory(RectTransform skillsPanel, JObject jObject)
    {
        GameObject skillSlot = Instantiate(Resources.Load<GameObject>("Prefab/SkillSlot02"));
        skillSlot.transform.SetParent(skillsPanel);
        skillSlot.SetActive(true);

        skillSlot.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("SkillImage/Skill" + (string)jObject["skillNumber"]);
        skillSlot.transform.GetChild(0).name = (string)jObject["skillNumber"] + "_" + (string)jObject["uniqueNumber"] + "_" + (string)jObject["count"] + "_" + (string)jObject["isNFT"];
        skillSlot.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = (string)jObject["count"];
    }

    // �κ��丮 ��, ��ų Ŭ����, ���� �гη� ������ �����ϴ� �κ�.
    public void CreateSkillSlot02InInventory(RectTransform parentPanel, string[] skillData)
    {
        GameObject skillSlot = Instantiate(Resources.Load<GameObject>("Prefab/SkillSlot02"));
        skillSlot.transform.SetParent(parentPanel);
        skillSlot.SetActive(true);

        skillSlot.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("SkillImage/Skill" + skillData[0]);
        skillSlot.transform.GetChild(0).name = skillData[0] + "_" + skillData[1] + "_" + skillData[2] + "_" + skillData[3];
        skillSlot.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = skillData[2];
    }

    // �κ��丮 �ʱ�ȭ ��, ���� ���ð� ��� �κ�.
    public void CreateSkillSlot03InInventory(RectTransform parentPanel, string[] skillData)
    {
        GameObject skillSlot = Instantiate(Resources.Load<GameObject>("Prefab/SkillSlot02"));
        skillSlot.transform.SetParent(parentPanel);
        skillSlot.SetActive(true);

        skillSlot.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("SkillImage/Skill" + skillData[0]);
        skillSlot.transform.GetChild(0).name = skillData[0] + "_" + skillData[1] + "_" + skillData[2] + "_" + skillData[3];
        skillSlot.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = skillData[2];

        skillSlot.transform.GetChild(0).GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.6f, 0.2f);
        skillSlot.transform.GetChild(0).GetComponent<SkillClickEvent>().Checkable = false;
    }*/





    // �Ϲ� ����
    public void CreateSkillSlot01InInventory_test(RectTransform skillsPanel, UserSkillDataInfo skillData)
    {
        GameObject skillSlot = Instantiate(Resources.Load<GameObject>("Prefab/GameLobby/SkillSlot02InInventory"));
        skillSlot.transform.SetParent(skillsPanel);
        skillSlot.SetActive(true);

        // SkillSlot�� UserSkillDataInfo�� ���� 4���� ���� ���� ���� �ٸ� ������� �����Ѵ�.
        skillSlot.transform.name = skillData.UniqueNumber;
        skillSlot.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("SkillImage/Skill" + skillData.SkillNumber);
        skillSlot.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = skillData.Count;
        skillSlot.transform.GetChild(0).name = skillData.IsNFT;
    }   // ��

    // ���� ���� ��ų�� ���� ���, ���õ� ��ų���� ���õ� ������ �־�� �ؼ� ���� ����.
    public void CreateSkillSlot02InInventory_test(RectTransform parentPanel, UserSkillDataInfo skillData)
    {
        GameObject skillSlot = Instantiate(Resources.Load<GameObject>("Prefab/GameLobby/SkillSlot02InInventory"));
        skillSlot.transform.SetParent(parentPanel);
        skillSlot.SetActive(true);

        // SkillSlot�� UserSkillDataInfo�� ���� 4���� ���� ���� ���� �ٸ� ������� �����Ѵ�.
        skillSlot.transform.name = skillData.UniqueNumber;
        skillSlot.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("SkillImage/Skill" + skillData.SkillNumber);
        skillSlot.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = skillData.Count;
        skillSlot.transform.GetChild(0).name = skillData.IsNFT;

        skillSlot.transform.GetChild(0).GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.6f, 0.2f);
        skillSlot.transform.GetComponent<SkillSlotClickEvent>().Checkable = false;
    }   // ��








    // ���� *** Content�� �����ϴ� ����� ���ؼ� �������Ͽ���. ����, �ð��� �Ǹ� �� Ŭ������ �����Ͽ� �����丵����.
    public void CreateVenderProductSlot(RectTransform parentPanel, JArray products, JArray skillData)
    {
        for (int i = 0; i < products.Count; i++)
        {

            GameObject productPanel = Instantiate(Resources.Load<GameObject>("Prefab/GameLobby/VenderProduct"));
            productPanel.transform.SetParent(parentPanel);
            // productPanel.transform.SetParent(parentParentPanel.GetChild(i/5).GetComponent<RectTransform>());
            productPanel.SetActive(true);

            // Vendoer's ProductNumber Name
            productPanel.name = (string)products[i]["productNumber"];

            // ��ų �̸� ���
            productPanel.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = (string)skillData[Int32.Parse((string)products[i]["skillNumber"])-1]["SkillName"];

            // ��ų �̹��� �г� �̸� ����, ��ų �̹��� ���
            productPanel.transform.GetChild(1).name = (string)products[i]["skillNumber"];
            productPanel.transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>("SkillImage/Skill" + (string)products[i]["skillNumber"]);

            // ��ų ���� ���
            productPanel.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = (string)products[i]["price"];
        }
    }




    // ����
    public void CreateProductsPanel(RectTransform parentPanel, int productsCount)
    {
        for(int i =0; i < (productsCount - 1)/5 + 1; i++)
        {
            GameObject productsPanel = Instantiate(Resources.Load<GameObject>("Prefab/GameLobby/ProductsPanel"));
            productsPanel.transform.SetParent(parentPanel);
            productsPanel.SetActive(false);

            productsPanel.name = "ProductsPanel0" + i;
            
            productsPanel.GetComponent<RectTransform>().position = parentPanel.position
                + new Vector3(-parentPanel.parent.gameObject.GetComponent<RectTransform>().sizeDelta.x/2, parentPanel.sizeDelta.y/2,0);
        }
    }

    public void CreateProductSlot(RectTransform parentParentPanel, JArray products, JArray skillsData)
    {
        for(int i =0; i < products.Count; i++)
        {
            GameObject productPanel = Instantiate(Resources.Load<GameObject>("Prefab/GameLobby/ProductInMarketMain"));
            productPanel.transform.SetParent(parentParentPanel.GetChild(i/5));
            // productPanel.transform.SetParent(parentParentPanel.GetChild(i/5).GetComponent<RectTransform>());
            productPanel.SetActive(true);

            // product Name
            productPanel.name = (string)products[i]["registrationNumber"];
            // product Image
            productPanel.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("SkillImage/Skill" + (string)products[i]["skillNumber"]);

            string content = (string)skillsData[(int)products[i]["skillNumber"] - 1]["SkillDamage"] + " "
                + ((string)skillsData[(int)products[i]["skillNumber"] - 1]["SkillDetail"]).Split('_')[0]
                + (string)skillsData[(int)products[i]["skillNumber"] - 1]["NumberOfTargets"]
                + ((string)skillsData[(int)products[i]["skillNumber"] - 1]["SkillDetail"]).Split('_')[1];

            // name
            productPanel.transform.GetChild(1).GetChild(0).name = (string)products[i]["skillNumber"];
            productPanel.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = (string)skillsData[(int)products[i]["skillNumber"]-1]["SkillName"];
            // content
            productPanel.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = content;
            productPanel.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = (string)skillsData[(int)products[i]["skillNumber"] - 1]["SkillRank"];
            
            // for ���� ���� ���� �ð��� ���� �� �������� ������ ���� ���ش�.
            productPanel.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = (string)products[i]["registrationTime"];
            productPanel.transform.GetChild(4).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = (string)products[i]["price"];
        }
    }



    // ���� ���
    public void CreateSkillSlot01InMarketEnroll(RectTransform skillsPanel, UserSkillDataInfo[] ownedSkills)
    {
        for (int i = 0; i < ownedSkills.Length; i++)
        {
            GameObject skillSlot = Instantiate(Resources.Load<GameObject>("Prefab/GameLobby/SkillSlot01InEnrollMenu"));
            skillSlot.transform.SetParent(skillsPanel);
            skillSlot.SetActive(true);

            // SkillSlot�� UserSkillDataInfo�� ���� 4���� ���� ���� ���� �ٸ� ������� �����Ѵ�.
            skillSlot.transform.name = ownedSkills[i].UniqueNumber;
            skillSlot.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("SkillImage/Skill" + ownedSkills[i].SkillNumber);
            skillSlot.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = ownedSkills[i].Count;
            skillSlot.transform.GetChild(0).name = ownedSkills[i].IsNFT;
        }
    } //��



    // ���� ���
    public void CreateProductOnSales(RectTransform parentParentPanel, JArray products, JArray skillsData)
    {
        for (int i = 0; i < products.Count; i++)
        {
            GameObject productPanel = Instantiate(Resources.Load<GameObject>("Prefab/GameLobby/ProductOnSales"));
            productPanel.transform.SetParent(parentParentPanel.GetChild(i / 5));
            // productPanel.transform.SetParent(parentParentPanel.GetChild(i/5).GetComponent<RectTransform>());
            productPanel.SetActive(true);

            // product Name
            productPanel.name = (string)products[i]["registrationNumber"];
            // product Image
            productPanel.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("SkillImage/Skill" + (string)products[i]["skillNumber"]);

            string content = (string)skillsData[(int)products[i]["skillNumber"] - 1]["SkillDamage"] + " "
                + ((string)skillsData[(int)products[i]["skillNumber"] - 1]["SkillDetail"]).Split('_')[0]
                + (string)skillsData[(int)products[i]["skillNumber"] - 1]["NumberOfTargets"]
                + ((string)skillsData[(int)products[i]["skillNumber"] - 1]["SkillDetail"]).Split('_')[1];

            // name
            productPanel.transform.GetChild(1).GetChild(0).name = (string)products[i]["skillNumber"];
            productPanel.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = (string)skillsData[(int)products[i]["skillNumber"] - 1]["SkillName"];
            // content
            productPanel.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = content;
            productPanel.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = (string)skillsData[(int)products[i]["skillNumber"] - 1]["SkillRank"];

            // for ���� ���� ���� �ð��� ���� �� �������� ������ ���� ���ش�.
            productPanel.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = (string)products[i]["registrationTime"];
            productPanel.transform.GetChild(4).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = (string)products[i]["price"];
        }
    }

    public void CreateProductRecordSlot(RectTransform parentParentPanel, JArray products, JArray skillsData)
    {
        for (int i = 0; i < products.Count; i++)
        {
            GameObject productPanel = Instantiate(Resources.Load<GameObject>("Prefab/GameLobby/ProductRecord"));
            productPanel.transform.SetParent(parentParentPanel.GetChild(i / 5));
            // productPanel.transform.SetParent(parentParentPanel.GetChild(i/5).GetComponent<RectTransform>());
            productPanel.SetActive(true);

            // product Name
            productPanel.name = (string)products[i]["salesCompletionNumber"];
            // product Image
            productPanel.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("SkillImage/Skill" + (string)products[i]["skillNumber"]);

            string content = (string)skillsData[(int)products[i]["skillNumber"] - 1]["SkillDamage"] + " "
                + ((string)skillsData[(int)products[i]["skillNumber"] - 1]["SkillDetail"]).Split('_')[0]
                + (string)skillsData[(int)products[i]["skillNumber"] - 1]["NumberOfTargets"]
                + ((string)skillsData[(int)products[i]["skillNumber"] - 1]["SkillDetail"]).Split('_')[1];

            // name
            productPanel.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = (string)skillsData[(int)products[i]["skillNumber"] - 1]["SkillName"];
            // content
            productPanel.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = content;
            productPanel.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = (string)skillsData[(int)products[i]["skillNumber"] - 1]["SkillRank"];

            // for ���� ���� ���� �ð��� ���� �� �������� ������ ���� ���ش�.
            productPanel.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = (string)products[i]["SalesStartTime"];
            productPanel.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = (string)products[i]["salesCompletionTime"];

            productPanel.transform.GetChild(5).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = (string)products[i]["price"];
        }
    }

    


    // ����
    public void DestroyChild(RectTransform parentPanel)
    {
        RectTransform[] children = parentPanel.GetComponentsInChildren<RectTransform>();

        if (children == null) return;

        for (int i = 1; i < children.Length; i++)
        {
            Destroy(children[i].gameObject);
        }

        parentPanel.DetachChildren();
    }

    public void DestroyChild02(RectTransform parentPanel)
    {
        if (parentPanel.childCount == 0) return;

        for (int i = 0; i < parentPanel.childCount; i++)
        {
            Destroy(parentPanel.GetChild(i).gameObject);
        }

        // Destroy�� ���� �Ǿ, 1������ ������ ������ ���� �ش� ������Ʈ�� ������� �ʴ´�.
        // ���� �ش� �г��� �θ𿡼� �����Ͽ�, �θ��� ���ϵ带 ����ϴ� �ڵ忡 ������ ��ġ�� ���ϵ��� �Ѵ�.
        parentPanel.DetachChildren();
    }

    public void DestroyPartPanel(string prefabTag)
    {
        Destroy(GameObject.FindWithTag(prefabTag));
    }
}






/*
// Image�� Sprite�� �����ϱ� ���ؼ�, 2���� ����� ����Ѵ�.
// ��(����)������ Assests�� �����ϴ� Sprite������ �ٷ� ����� ���̴�.
// �Ʒ�(�ּ�)������ Assests�� �����ϴ� Texture2D ������ ����Ͽ� Sprite ������ ����� ����ϴ� ���� �����ش�.
Debug.Log("1 : " + userImage.sprite);
Debug.Log("2 : " + userImage.sprite.name);
Debug.Log("3 : " + userImage.sprite.texture);

string image_path01 = "./Assets/Resources/UserImage/tmp" + ConvertToString(arrayOfObject[2]) + ".jpg";

Texture2D texture2D = new Texture2D(0, 0);

byte[] byteTexture = File.ReadAllBytes(image_path01);
if (byteTexture.Length > 0)
{
    texture2D.LoadImage(byteTexture);
}

userImage.sprite = Sprite.Create(texture2D, new Rect(0.0f, 0.0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f), 100.0f);

Debug.Log("1 : " + userImage.sprite);
Debug.Log("2 : " + userImage.sprite.name);
Debug.Log("3 : " + userImage.sprite.texture); */