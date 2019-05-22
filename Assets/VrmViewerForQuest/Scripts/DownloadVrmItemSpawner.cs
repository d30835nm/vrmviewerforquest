using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace VrmViewer
{
    public class DownloadVrmItemSpawner : MonoBehaviour
    {
        [SerializeField]
        DownloadVrmItem downloadVrmItemPrefab;

        [SerializeField]
        Transform downloadVrmItemRoot;

        [SerializeField]
        LocalVrmItemSpawner localVrmItemSpawner;

        void Awake()
        {
            Assert.IsNotNull(downloadVrmItemPrefab);
            Assert.IsNotNull(downloadVrmItemRoot);
            Assert.IsNotNull(localVrmItemSpawner);

            LoadAll();
        }

        void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus) ReLoadAll();
        }

        public void ReLoadAll()
        {
            foreach (Transform child in downloadVrmItemRoot)
            {
                Destroy(child.gameObject);
            }

            LoadAll();
        }

        void LoadAll()
        {
            var dir = new DirectoryInfo(GlobalPath.DownloadPath);
            var vrmInfos = dir.GetFiles("*.vrm", SearchOption.AllDirectories);
            var orderedVrmInfos = vrmInfos.OrderBy(f => f.LastWriteTime);

            foreach (var vrmInfo in orderedVrmInfos)
            {
                var downloadVrmItem = Instantiate(downloadVrmItemPrefab, downloadVrmItemRoot);
                downloadVrmItem.transform.SetSiblingIndex(0);
                downloadVrmItem.Initialize(vrmInfo.FullName, localVrmItemSpawner, this);
            }
        }
    }
}