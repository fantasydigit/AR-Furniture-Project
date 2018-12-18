using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System;

public class Item {
	public string Id;
    public string url;
    public string name;
    public string image;
    public string code;
    public string price;
    public string category;
    public string descr;
    public string link;
    public Item() {
		Id = "";
        url = ""; // fbx
        image = "";
        code = "";
        price = "";
        category = "";
        descr = "";
        link = "";
	}
}
public class Category {
    public string Id;
    public string name;
    public List<Category> subCategories;
    public Category() {
        Id = "";
        name = "";
        subCategories = new List<Category>();
    }
}
public class ParseApi : MonoBehaviour {
    #region PUBLIC MEMBERS
    public static bool IsParsingFinished {
        get { return isParsingFinished; }
    }
    public static string currentCategory = string.Empty;
    #endregion // PUBLIC_MEMBERS

    #region PRIVATE_MEMBERS
    public static string BASE_URL = "https://arcom.agselling.com/";
    public static string API_GET_PRODUCTS = "json2.php?id=";
    private string API_GET_CATOGORY = "jcat.php";
    private static bool isParsingFinished = false;
	private static List<Item> dataList = new List<Item> ();
    private static List<Category> categoryList = new List<Category>();
    #endregion // PRIVATE_MEMEBERS

    #region MONOBEHAVIOR_METHODS
    // Use this for initialization
    void Start () {
		StartCoroutine (GetData());
	}
    #endregion // MONOBEHAVIOR_METHODS

    #region PRIVATE_METHODS
    IEnumerator GetData() {
        string strUrl;
        WWW www;
        /*
        // parse products json
        strUrl = BASE_URL + API_GET_PRODUCTS;	
		Debug.Log (strUrl);
		www = new WWW (strUrl);
		yield return www;
		if (www.error == null) {
			JsonData json = JsonMapper.ToObject (www.text);
            //string status = json ["status"].ToString();
            //int dataCnt = int.Parse(json ["count"].ToString());
            int dataCnt = json.Count;
            if (dataCnt > 0) {
				for (int i = 0; i < dataCnt; i++) {
					Item _item = new Item ();
					_item.url = json[i]["url"].ToString();
                    _item.image = json[i]["img"].ToString();
                    _item.code = json[i]["code"].ToString();
                    _item.price = json[i]["price"].ToString();
                    _item.category = json[i]["category"].ToString();
                    _item.descr = json[i]["descr"].ToString();
                    _item.link = json[i]["link"].ToString();
                    _item.name = json[i]["name"].ToString();
                    Debug.Log(_item.category);
                    dataList.Add (_item);
				}
			}
			Debug.Log (dataList.Count);
        } else {
			Debug.Log ("error");
		}*/
        // parse cateogry json
        strUrl = BASE_URL + API_GET_CATOGORY;
        Debug.Log(strUrl);
        www = new WWW(strUrl);
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
                    Category _item = new Category();
                    _item.Id = json[i]["id"].ToString();
                    _item.name = json[i]["cat"].ToString();
                    JsonData sub_json = json[i]["sub"]; 
                    if (sub_json.Count > 0) {
                        for (int j = 0; j < sub_json.Count; j++) {
                            Category sub_item = new Category();
                            sub_item.Id = sub_json[j]["id"].ToString();
                            sub_item.name = sub_json[j]["cat"].ToString();
                            _item.subCategories.Add(sub_item);
                        }
                    }
                    Debug.Log( "main category count:" + dataCnt);
                    categoryList.Add(_item);
                }
            }
            Debug.Log(dataList.Count);
        }
        else
        {
            Debug.Log("error");
        }
        isParsingFinished = true;
	}
    #endregion // PRIVATE_METHODS

    #region PUBLIC_METHODS
    public static Item GetItem(int index)
    {
        return dataList[index];
    }
    public static int GetItemCount() {
        return dataList.Count;
    }
    public static Category GetCategory(int index)
    {
        return categoryList[index];
    }
    public static Category GetCategory(string id)
    {
        foreach (Category each in categoryList) {
            if (each.Id == id) {
                return each;
            }
            foreach (Category sub_cat in each.subCategories) {
                if (sub_cat.Id == id) {
                    return sub_cat;
                }
            }
        }
        return null;
    }
    public static int GetCategoryCount()
    {
        return categoryList.Count;
    }
    #endregion //PUBLIC_METHODS
}
