using UnityEngine;

namespace VrmViewer
{
    public class VrmMenu : MonoBehaviour
    {
        bool visible = true;

        void Update()
        {
            //Questの場合はStartボタン、Goの場合はBackボタンをメニューボタンとする
            if (OVRInput.GetDown(OVRInput.Button.Start) || OVRInput.GetDown(OVRInput.Button.Back))
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
    }
}