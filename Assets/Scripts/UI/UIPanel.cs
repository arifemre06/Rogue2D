using UnityEngine;



namespace DefaultNamespace.UI
{
    public class UIPanel : MonoBehaviour
    {

        public virtual void ActivatePanel()
        {
            gameObject.SetActive(true);
        }

        public virtual void DeActivatePanel()
        {
            gameObject.SetActive(false);
        }
    }
}