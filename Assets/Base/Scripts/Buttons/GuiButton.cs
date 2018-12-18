using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace Polaris.Unity.Base.Utilities.UI
{
    [RequireComponent(typeof(Button))]
    public class GuiButton : MonoBehaviour
    {
        // Use this for initialization
        public virtual void Start()
        {
            GetComponent<Button>().onClick.AddListener(OnClicked);
            //GetComponent<Button> ().OnPointerEnter(
        }

        public virtual void OnClicked()
        {

        }
    }
}