using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Polaris.Unity.Base.Utilities.UI;

public class CategoryButton : GuiButton {
    #region PUBLIC_MEMEBERS
    public string Id;
    public Text nameText;
    #endregion // PUBLIC_MEMBERS

    #region PUBLIC_METHODS
    public override void Start()
    {
        base.Start();
    }

    public override void OnClicked()
    {
        if (ParseApi.GetCategory(Id).subCategories.Count > 0)
        {
            GameObject.FindObjectOfType<CategoryPage>().DisplaySubCategories(Id);
        }
        else {
            Category cat = ParseApi.GetCategory(Id);
            ObjectViewer.Instance.ShowOneObject("ProductsInSearchPage");
            GameObject.FindObjectOfType<ProductsInSearchPage>().ShowProducts(cat); 
        }
    }
    #endregion // PUBLIC_METHODS

}
