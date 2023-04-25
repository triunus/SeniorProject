using UnityEngine;
using UnityEngine.UI;
using TMPro;

using System;
using Newtonsoft.Json.Linq;

using SkillInformaion;

// Prefab을 생성하고, 삭제하는 함수들의 집합.
// 중복되는 함수들과 구문들이 매우매우매우매우 많다.
// 제너럴 적으로 정리 및, 코드의 부분함수화를 통해 간략화 시키자.
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

            // 현재 컴포넌트의 Image를 바꾼당.
            // skillSlot[i].GetComponent<Image>().sprite = Resources.Load<Sprite>(skill_path);
            // 자식 컴포넌트의 Image를 바꾼당.
            characterSlot[i].transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(character_path);
        }
    }







    public void CreateSkillContent(RectTransform skillSlot, string[] currentSkillData, JArray skillsData)
    {
        GameObject contentSlot = Instantiate(Resources.Load<GameObject>("Prefab/SkillContent"));
        contentSlot.transform.SetParent(skillSlot.root);
        contentSlot.SetActive(true);
        contentSlot.transform.SetSiblingIndex(1);

        //        밑의 코드는 차후, 스킬의 설명 길이에 따라 동적으로 변동하도록 하기 위해 사용할 예정이다.
        //        content.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 300);
        //        content.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 80);

        // 스킬 슬롯으로 부터 150, -60 만큼 이동해야, 스킬 컨텐츠 UI의 좌측 상단이 스킬 슬롯의 중앙에 위치한다. 20씩 좀 더 이동해 놓은 것.
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


    // 일반 스킬에 사용
    // RectTransform을 넘김 : .root 변수와, .position(), .name이 필요하다.
    public void CreateSkillContent01(RectTransform skillSlotPosition, JArray skillsData)
    {
        GameObject contentSlot = Instantiate(Resources.Load<GameObject>("Prefab/GameLobby/SkillContent"));
        contentSlot.transform.SetParent(skillSlotPosition.root);
        contentSlot.SetActive(true);
        contentSlot.transform.SetSiblingIndex(1);

        //        밑의 코드는 차후, 스킬의 설명 길이에 따라 동적으로 변동하도록 하기 위해 사용할 예정이다.
        //        content.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 300);
        //        content.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 80);

        // 스킬 슬롯으로 부터 150, -60 만큼 이동해야, 스킬 컨텐츠 UI의 좌측 상단이 스킬 슬롯의 중앙에 위치한다. 20씩 좀 더 이동해 놓은 것.
        contentSlot.transform.position = skillSlotPosition.position + new Vector3(170, -80, 0);

        string skillNumber = skillSlotPosition.GetComponent<Image>().sprite.name.Replace("Skill", String.Empty);

        string content = (string)skillsData[Int32.Parse(skillNumber) - 1]["SkillDamage"] + " "
            + ((string)skillsData[(Int32.Parse(skillNumber) - 1)]["SkillDetail"]).Split('_')[0]
            + (string)skillsData[(Int32.Parse(skillNumber) - 1)]["NumberOfTargets"]
            + ((string)skillsData[(Int32.Parse(skillNumber) - 1)]["SkillDetail"]).Split('_')[1];

        // 스킬 이름과 내용 작성
        contentSlot.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = (string)skillsData[(Int32.Parse(skillNumber) - 1)]["SkillName"];
        contentSlot.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = content;

        // count와 NFT 여부 작성
        contentSlot.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "99+";
        contentSlot.transform.GetChild(2).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "false";
    }           // 완

    // count와 NFT 여부가 있는 경우 사용
    public void CreateSkillContent02(RectTransform skillSlotPosition, JArray skillsData)
    {
        GameObject contentSlot = Instantiate(Resources.Load<GameObject>("Prefab/GameLobby/SkillContent"));
        contentSlot.transform.SetParent(skillSlotPosition.root);
        contentSlot.SetActive(true);
        contentSlot.transform.SetSiblingIndex(1);

        //        밑의 코드는 차후, 스킬의 설명 길이에 따라 동적으로 변동하도록 하기 위해 사용할 예정이다.
        //        content.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 300);
        //        content.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 80);

        // 스킬 슬롯으로 부터 150, -60 만큼 이동해야, 스킬 컨텐츠 UI의 좌측 상단이 스킬 슬롯의 중앙에 위치한다. 20씩 좀 더 이동해 놓은 것.
        contentSlot.transform.position = skillSlotPosition.position + new Vector3(170, -80, 0);

        string skillNumber = skillSlotPosition.GetChild(0).GetComponent<Image>().sprite.name.Replace("Skill", String.Empty);

        string content = (string)skillsData[Int32.Parse(skillNumber) - 1]["SkillDamage"] + " "
            + ((string)skillsData[(Int32.Parse(skillNumber) - 1)]["SkillDetail"]).Split('_')[0]
            + (string)skillsData[(Int32.Parse(skillNumber) - 1)]["NumberOfTargets"]
            + ((string)skillsData[(Int32.Parse(skillNumber) - 1)]["SkillDetail"]).Split('_')[1];

        // 스킬 이름과 내용 작성
        contentSlot.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = (string)skillsData[(Int32.Parse(skillNumber) - 1)]["SkillName"];
        contentSlot.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = content;

        // count와 NFT 여부 작성
        contentSlot.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = skillSlotPosition.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text;
        contentSlot.transform.GetChild(2).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = skillSlotPosition.GetChild(0).name;
    }           // 완




    // 캐릭터 이미지 정보를 통해, 캐릭터가 갖는 스킬 번호 유추
    // ->  json에서 스킬 번호에 해당 하는 정보를 출력.
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






    // 로비에서 캐릭터 번호에 의해 정래지는 스킬 번호를 이용하여 프리팹 생성.
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
    }       // 완

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
    }   // 완








/*    // 인벤토리에서 서버에서 가져온 Json파일을 이용하여 프리팹 생성.
    public void CreateSkillSlot01InInventory(RectTransform skillsPanel, JObject jObject)
    {
        GameObject skillSlot = Instantiate(Resources.Load<GameObject>("Prefab/SkillSlot02"));
        skillSlot.transform.SetParent(skillsPanel);
        skillSlot.SetActive(true);

        skillSlot.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("SkillImage/Skill" + (string)jObject["skillNumber"]);
        skillSlot.transform.GetChild(0).name = (string)jObject["skillNumber"] + "_" + (string)jObject["uniqueNumber"] + "_" + (string)jObject["count"] + "_" + (string)jObject["isNFT"];
        skillSlot.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = (string)jObject["count"];
    }

    // 인벤토리 내, 스킬 클릭시, 선택 패널로 프리팹 생성하는 부분.
    public void CreateSkillSlot02InInventory(RectTransform parentPanel, string[] skillData)
    {
        GameObject skillSlot = Instantiate(Resources.Load<GameObject>("Prefab/SkillSlot02"));
        skillSlot.transform.SetParent(parentPanel);
        skillSlot.SetActive(true);

        skillSlot.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("SkillImage/Skill" + skillData[0]);
        skillSlot.transform.GetChild(0).name = skillData[0] + "_" + skillData[1] + "_" + skillData[2] + "_" + skillData[3];
        skillSlot.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = skillData[2];
    }

    // 인벤토리 초기화 시, 기존 선택값 출력 부분.
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





    // 일반 생성
    public void CreateSkillSlot01InInventory_test(RectTransform skillsPanel, UserSkillDataInfo skillData)
    {
        GameObject skillSlot = Instantiate(Resources.Load<GameObject>("Prefab/GameLobby/SkillSlot02InInventory"));
        skillSlot.transform.SetParent(skillsPanel);
        skillSlot.SetActive(true);

        // SkillSlot은 UserSkillDataInfo가 갖은 4개의 변수 값을 각기 다른 방식으로 보존한다.
        skillSlot.transform.name = skillData.UniqueNumber;
        skillSlot.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("SkillImage/Skill" + skillData.SkillNumber);
        skillSlot.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = skillData.Count;
        skillSlot.transform.GetChild(0).name = skillData.IsNFT;
    }   // 완

    // 기존 선택 스킬에 대한 출력, 선택된 스킬들은 선택된 흔적이 있어야 해서 따로 만듬.
    public void CreateSkillSlot02InInventory_test(RectTransform parentPanel, UserSkillDataInfo skillData)
    {
        GameObject skillSlot = Instantiate(Resources.Load<GameObject>("Prefab/GameLobby/SkillSlot02InInventory"));
        skillSlot.transform.SetParent(parentPanel);
        skillSlot.SetActive(true);

        // SkillSlot은 UserSkillDataInfo가 갖은 4개의 변수 값을 각기 다른 방식으로 보존한다.
        skillSlot.transform.name = skillData.UniqueNumber;
        skillSlot.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("SkillImage/Skill" + skillData.SkillNumber);
        skillSlot.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = skillData.Count;
        skillSlot.transform.GetChild(0).name = skillData.IsNFT;

        skillSlot.transform.GetChild(0).GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.6f, 0.2f);
        skillSlot.transform.GetComponent<SkillSlotClickEvent>().Checkable = false;
    }   // 완








    // 상점 *** Content를 생성하는 방법에 대해서 재정의하였다. 차후, 시간이 되면 이 클래스를 참고하여 리펙토링하자.
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

            // 스킬 이름 명시
            productPanel.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = (string)skillData[Int32.Parse((string)products[i]["skillNumber"])-1]["SkillName"];

            // 스킬 이미지 패널 이름 변경, 스킬 이미지 명시
            productPanel.transform.GetChild(1).name = (string)products[i]["skillNumber"];
            productPanel.transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>("SkillImage/Skill" + (string)products[i]["skillNumber"]);

            // 스킬 가격 명시
            productPanel.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = (string)products[i]["price"];
        }
    }




    // 마켓
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
            
            // for 구문 전에 현재 시각을 구한 후 서버에서 가져온 값을 빼준다.
            productPanel.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = (string)products[i]["registrationTime"];
            productPanel.transform.GetChild(4).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = (string)products[i]["price"];
        }
    }



    // 마켓 등록
    public void CreateSkillSlot01InMarketEnroll(RectTransform skillsPanel, UserSkillDataInfo[] ownedSkills)
    {
        for (int i = 0; i < ownedSkills.Length; i++)
        {
            GameObject skillSlot = Instantiate(Resources.Load<GameObject>("Prefab/GameLobby/SkillSlot01InEnrollMenu"));
            skillSlot.transform.SetParent(skillsPanel);
            skillSlot.SetActive(true);

            // SkillSlot은 UserSkillDataInfo가 갖은 4개의 변수 값을 각기 다른 방식으로 보존한다.
            skillSlot.transform.name = ownedSkills[i].UniqueNumber;
            skillSlot.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("SkillImage/Skill" + ownedSkills[i].SkillNumber);
            skillSlot.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = ownedSkills[i].Count;
            skillSlot.transform.GetChild(0).name = ownedSkills[i].IsNFT;
        }
    } //완



    // 마켓 기록
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

            // for 구문 전에 현재 시각을 구한 후 서버에서 가져온 값을 빼준다.
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

            // for 구문 전에 현재 시각을 구한 후 서버에서 가져온 값을 빼준다.
            productPanel.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = (string)products[i]["SalesStartTime"];
            productPanel.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = (string)products[i]["salesCompletionTime"];

            productPanel.transform.GetChild(5).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = (string)products[i]["price"];
        }
    }

    


    // 삭제
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

        // Destroy가 시행 되어도, 1프레임 지나기 전에는 아직 해당 오브젝트는 사라지지 않는다.
        // 따라서 해당 패널의 부모에서 제거하여, 부모의 차일드를 사용하는 코드에 영향을 미치지 못하도록 한다.
        parentPanel.DetachChildren();
    }

    public void DestroyPartPanel(string prefabTag)
    {
        Destroy(GameObject.FindWithTag(prefabTag));
    }
}






/*
// Image의 Sprite를 변경하기 위해서, 2가지 방법을 사용한다.
// 위(본문)에서는 Assests에 존재하는 Sprite파일을 바로 사용한 것이다.
// 아래(주석)에서는 Assests에 존재하는 Texture2D 파일을 사용하여 Sprite 파일을 만들어 사용하는 것을 보여준다.
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