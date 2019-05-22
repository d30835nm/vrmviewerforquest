using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace VrmViewer
{
    [RequireComponent(typeof(Button))]
    public class DownloadVrmItem : MonoBehaviour
    {
        [SerializeField]
        Text fileNameText;

        [SerializeField]
        Text lastWriteTimeText;

        [SerializeField]
        Text importedText;

        [SerializeField]
        Button removeButton;

        string vrmFileFullName;
        LocalVrmItemSpawner localVrmItemSpawner;
        Button selectButton;

        void Awake()
        {
            Assert.IsNotNull(fileNameText);
            Assert.IsNotNull(lastWriteTimeText);
            Assert.IsNotNull(importedText);
            Assert.IsNotNull(removeButton);
            
            selectButton = GetComponent<Button>();
        }

        public void Initialize(string vrmFileFullName, LocalVrmItemSpawner localVrmItemSpawner, DownloadVrmItemSpawner downloadVrmItemSpawner)
        {
            this.vrmFileFullName = vrmFileFullName;
            this.localVrmItemSpawner = localVrmItemSpawner;
            fileNameText.text = Path.GetFileName(vrmFileFullName);
            lastWriteTimeText.text = File.GetLastWriteTime(vrmFileFullName).ToString("最終更新日時 yyyy/MM/dd HH:mm");
            removeButton.onClick.AddListener(() => Remove());

            if (File.Exists(GlobalPath.VrmHomePath + "/" + Path.GetFileName(vrmFileFullName)))
            {
                selectButton.interactable = false;
            }
            else
            {
                importedText.enabled = false;
                selectButton.onClick.AddListener(() => Select());
            }
        }

        void Select()
        {
            localVrmItemSpawner.Add(vrmFileFullName);
            importedText.enabled = true;
            selectButton.interactable = false;
        }

        void Remove()
        {
            if (File.Exists(vrmFileFullName))
            {
                File.Delete(vrmFileFullName);
            }

            Destroy(this.gameObject);
        }
    }
}