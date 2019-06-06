using UnityEngine;

namespace VrmViewer
{
    public class VrmMenu : MonoBehaviour
    {
        bool visible = true;

        void Update()
        {
            if (OVRInput.GetDown(OVRInput.Button.Start))
            {
                visible = !visible;
                ChangeVisibility(visible);
            }
        }

        void ChangeVisibility(bool visible)
        {
            var mainCamera = Camera.main.transform;
            var targetRotation = Quaternion.Euler(new Vector3(0f, mainCamera.rotation.eulerAngles.y, 0f));
            transform.SetPositionAndRotation(mainCamera.position, targetRotation);

            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(visible);
            }
        }

        public void OpenVRoidHub()
        {
            Application.OpenURL("https://hub.vroid.com/models?is_downloadable=1&characterization_allowed_user=everyone");
        }
    }
}