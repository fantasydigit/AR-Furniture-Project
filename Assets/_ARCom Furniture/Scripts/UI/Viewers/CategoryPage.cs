using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Polaris.Unity.Base.Utilities.UI;
public class CategoryPage : ViewableObject{
    #region PUBLIC_MEMBERS
    
    #endregion // PUBLIC_MEMBERS
    
    #region PUBLIC_METHODS
    
    public void OnBackButtonClick() {
        if (isSubCategory)
        {
            DisplayMainCategories();
        }
        else {
            ObjectViewer.Instance.ShowOneObject("FirstPanel");
        }
    }
    

    // Use this for initialization
    public override void Start () {
                
    }
	
	// Update is called once per frame
	public override void Update () {
		
	}
    public void OnEnable()
    {
        DisplayMainCategories();
    }
    public void DisplaySubCategories(string id)
    {
        isSubCategory = true;
        foreach (Transform each in GetComponentInChildren<LayoutGroup>().transform)
        {
            GameObject.Destroy(each.gameObject);
        }
        for (int i = 0; i < ParseApi.GetCategory(id).subCategories.Count; i++)
        {
            Category item = ParseApi.GetCategory(id).subCategories[i];
            GameObject obj = Resources.Load<GameObject>("Prefabs/CategoryButtonPrefab");
            GameObject clone = GameObject.Instantiate(obj);
            clone.transform.parent = GetComponentInChildren<LayoutGroup>().transform;
            clone.transform.localScale = Vector3.one;
            clone.GetComponent<CategoryButton>().Id = item.Id;
            clone.GetComponent<CategoryButton>().nameText.text = item.name;
        }
    }
    public void DisplayMainCategories()
    {
        isSubCategory = false;
        foreach (Transform each in GetComponentInChildren<LayoutGroup>().transform)
        {
            GameObject.Destroy(each.gameObject);
        }
        for (int i = 0; i < ParseApi.GetCategoryCount(); i++)
        {
            Category item = ParseApi.GetCategory(i);
            GameObject obj = Resources.Load<GameObject>("Prefabs/CategoryButtonPrefab");
            GameObject clone = GameObject.Instantiate(obj);
            clone.transform.parent = GetComponentInChildren<LayoutGroup>().transform;
            clone.transform.localScale = Vector3.one;
            clone.GetComponent<CategoryButton>().Id = item.Id;
            clone.GetComponent<CategoryButton>().nameText.text = item.name;
        }
    }
    #endregion // PUBLIC_METHODS

    #region PRIVATE_METHODS

    #endregion // PRIVATE_METHODS

    #region PRIVATE_MEMBERS
    bool isSubCategory = false;
    #endregion // PRIVATE_MEMBERS
}
