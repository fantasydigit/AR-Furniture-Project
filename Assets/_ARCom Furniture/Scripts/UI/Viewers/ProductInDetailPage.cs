using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Polaris.Unity.Base.Utilities.UI;
using TriLib;
using GoogleARCore.Examples.HelloAR;

public class ProductInDetailPage : ViewableObject {
    #region PUBLIC_MEMBERS
    public Text productNameText;
    public Text descrText;
    public Text priceText;
    public Image mainImage;
    Item item;
    #endregion // PUBLIC_MEMBERS
    
    #region PUBLIC_METHODS
    public void ShowProduct(Item item)
    {
        StartCoroutine(DoShowProduct(item));
    }
    public void OnBackButtonClick() {
        ObjectViewer.Instance.ShowOneObject("ProductsInSearchPage");
    }
    public void OnLinkButtonClick()
    {
        Application.OpenURL(item.link);
    }
    public void OnPlaceButtonClick()
    {
        GameObject.FindObjectOfType<MyAssetDownloader>().StartDownload(item.url, "fbx");
        FurnitureARController.Instance.ShowRecogPointCloud(true);
    }
    // Use this for initialization
    public override void Start () {
		
	}	
	// Update is called once per frame
	public override void Update () {
		
	}
    #endregion //PUBLIC_METHODS

    #region PRIVATE METHODS
    IEnumerator DoShowProduct(Item _item)
    {
        item = _item;
        productNameText.text = item.name;
        priceText.text = item.price;
        descrText.text = item.descr;
        WWW www = new WWW(item.image);
        yield return www;
        mainImage.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));

    }
    #endregion // PRIVATE_METHODS
}
