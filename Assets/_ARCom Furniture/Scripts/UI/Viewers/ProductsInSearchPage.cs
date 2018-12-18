using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Polaris.Unity.Base.Utilities.UI;
using LitJson;

public class ProductsInSearchPage : ViewableObject {
    #region PUBLIC_MEMBERS
    public Text searchText; 
    #endregion // PUBLIC_MEMBERS
    #region PUBLIC_METHOD
    public void ShowProducts(Category category) {
        StartCoroutine(DoShowProducts(category));
    }
    public void OnBackButtonClick() {
        //PageViewer.Instance.ShowOneObject("CategoryPage");
        ObjectViewer.Instance.ShowOneObject("CategoryPage");
    }
    #endregion //PUBLIC_METHOD

    #region MONOBEHAVIOR_METHODS
    // Use this for initialization
    public override void Start () {
		
	}
	
	// Update is called once per frame
	public override void Update () {
		
	}
    #endregion //MONOBEHAVIOR_METHODS

    #region PRIVATE_METHODS
    IEnumerator DoShowProducts(Category category)
    {
        searchText.text = category.name;
        foreach (Transform each in GetComponentInChildren<LayoutGroup>().transform)
        {
            GameObject.Destroy(each.gameObject);
        }
        // parse product
        List<Item> productsInCategory = new List<Item>();
        string strUrl = ParseApi.BASE_URL + ParseApi.API_GET_PRODUCTS + category.Id;
        Debug.Log(strUrl);
        WWW www = new WWW(strUrl);
        yield return www;
        if (www.error == null)
        {
            JsonData json = JsonMapper.ToObject(www.text);
            //string status = json ["status"].ToString();
            //int dataCnt = int.Parse(json ["count"].ToString());
            int dataCnt = json.Count;
            if (dataCnt > 0)
            {
                for (int i = 0; i < dataCnt; i++)
                {
                    Item _item = new Item();
                    _item.url = json[i]["url"].ToString();
                    _item.image = json[i]["img"].ToString();
                    _item.code = json[i]["code"].ToString();
                    _item.price = json[i]["price"].ToString();
                    _item.category = json[i]["category"].ToString();
                    _item.descr = json[i]["descr"].ToString();
                    _item.link = json[i]["link"].ToString();
                    _item.name = json[i]["name"].ToString();
                    Debug.Log(_item.category);
                    productsInCategory.Add(_item);
                }
            }
            Debug.Log(productsInCategory.Count);
        }
        else
        {
            Debug.Log("error");
        }
        for (int i = 0; i < productsInCategory.Count; i++)
        {
            Item item = productsInCategory[i];
            GameObject obj = Resources.Load<GameObject>("Prefabs/ProductButtonPrefab");
            GameObject clone = GameObject.Instantiate(obj);
            clone.transform.parent = GetComponentInChildren<LayoutGroup>().transform;
            clone.transform.localScale = Vector3.one;
            //clone.GetComponent<ProductButton>().nameText.text = item.name;
            clone.GetComponent<ProductButton>().product = item ;
            clone.GetComponent<ProductButton>().categoryText.text = item.category;
            clone.GetComponent<ProductButton>().priceText.text = item.price;
            clone.GetComponent<ProductButton>().nameText.text = item.name;

            www = new WWW(item.image);
            yield return www;
            clone.GetComponent<ProductButton>().mainImage.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
        }
        yield break;
    }
    #endregion //PRIVATE_METHODS
}
