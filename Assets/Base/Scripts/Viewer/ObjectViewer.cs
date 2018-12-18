using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Polaris.Unity.Base.Utilities.UI
{
    public class ObjectViewer : MonoBehaviour
    {
        static ObjectViewer _instance;
        public static ObjectViewer Instance
        {
            get
            {
                if (_instance == null) _instance = GameObject.FindObjectOfType<ObjectViewer>();
                return _instance;
            }
        }
        public List<ViewableObject> objList;
        public ViewableObject startObj;
        public virtual void Awake() {
            prevObject = null;
            curObject = null;

        }

        // Use this for initialization
        public virtual void Start()
        {
            ShowOneObject(startObj);
        }

        // Update is called once per frame
        public virtual void Update()
        {

        }
        public virtual void AddObj(ViewableObject obj) {
            try
            {
                if (obj != null) objList.Add(obj);
            }
            catch
            {
                ;
            }
        }

        public virtual bool RemoveObject(ViewableObject obj) {
            return objList.Remove(obj);
        }
        public virtual void HideAllObjects()
        {
            if (objList == null)
                return;
            foreach (ViewableObject each in objList)
            {
                each.gameObject.SetActive(false);
            }
        }
        public virtual void ShowOneObject(ViewableObject obj) {
            foreach (ViewableObject each in objList)
            {
                if (each == obj)
                {
                    if (each.gameObject.activeSelf != true) each.gameObject.SetActive(true);
                    prevObject = curObject;
                    curObject = obj;
                }
                else {
                    if (each.gameObject.activeSelf != false) each.gameObject.SetActive(false);
                }
            }
        }
        public virtual void ShowOneObject(string objName)
        {
            foreach (ViewableObject each in objList) {
                if (each.name == objName)
                {
                    if (each.gameObject.activeSelf != true) each.gameObject.SetActive(true);
                    prevObject = curObject;
                    curObject = each;
                }
                else {
                    if (each.gameObject.activeSelf != false) each.gameObject.SetActive(false);
                }
            }
        }
        public virtual void ShowPrevObject() {
            if (prevObject == null) return;
            ShowOneObject(prevObject);
        }
        
        public ViewableObject CurrentObject{
            get { return curObject; }
        }

        protected ViewableObject prevObject, curObject;
    }
}