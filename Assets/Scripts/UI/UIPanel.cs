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
            Debug.Log("eee sorun ne");
            gameObject.SetActive(false);
        }
    }
}