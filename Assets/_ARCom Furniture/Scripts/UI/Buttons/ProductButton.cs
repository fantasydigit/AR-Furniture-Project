using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Polaris.Unity.Base.Utilities.UI;

public class ProductButton : GuiButton {
    public Text nameText;
    public Text categoryText;
    public Text priceText;
    public Image mainImage;
    public Item product;
    public override void Start()
    {
        base.Start();
    }

    public override void OnClicked()
    {
        ObjectViewer.Instance.ShowOneObject("ProductDetailPage");
        ProductInDetailPage page = GameObject.FindObjectOfType<ProductInDetailPage>();
        page.ShowProduct(product);
    }
}
