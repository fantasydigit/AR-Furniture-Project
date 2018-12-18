using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Polaris.Unity.Base.Utilities.UI;
public class PageViewer : ObjectViewer {
    #region PUBLIC_METHODES
    public override void HideAllObjects()
    {
        if (objList == null)
            return;
        foreach (ViewableObject each in objList)
        {
            EasyTweenPage page = (EasyTweenPage)each;
            if (page.easyTween.IsObjectOpened()) {
                page.easyTween.OpenCloseObjectAnimation();
            }
        }
    }
    public override void ShowOneObject(ViewableObject obj)
    {
        foreach (ViewableObject each in objList)
        {
            if (each == obj)
            {
                EasyTweenPage page = (EasyTweenPage)each;
                if (!page.easyTween.IsObjectOpened())
                {
                    StartCoroutine(DelayObjectAnimation(page));
                }
                prevObject = curObject;
                curObject = obj;
            }
            else
            {
                EasyTweenPage page = (EasyTweenPage)each;
                if (page.easyTween.IsObjectOpened())
                {
                    page.easyTween.OpenCloseObjectAnimation();
                }
            }
        }
    }
    public override void ShowOneObject(string objName)
    {
        foreach (ViewableObject each in objList)
        {
            if (each.name == objName)
            {
                ShowOneObject(each);
            }
        }
    }
    #endregion // PUBLIC_METHODS

    #region PRIVATE_METHODS
    IEnumerator DelayObjectAnimation(EasyTweenPage page) {
        yield return new WaitForSeconds(0.3f);
        page.easyTween.OpenCloseObjectAnimation();
    }
    #endregion // PRIVATE_METHODS
}
