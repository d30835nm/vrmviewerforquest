using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using VRM;

namespace VrmViewer
{
    public class LocalVrmItemSpawner : MonoBehaviour
    {
        [SerializeField]
        LocalVrmItem localVrmItemPrefab;

        [SerializeField]
        Transform localVrmItemRoot;

        [SerializeField]
        VrmSpawner vrmSpawner;

        [SerializeField]
        DownloadVrmItemSpawner downloadVrmItemSpawner;
        
        void Awake()
        {
            Assert.IsNotNull(localVrmItemPrefab);
            Assert.IsNotNull(localVrmItemRoot);
            Assert.IsNotNull(vrmSpawner);
            Assert.IsNotNull(downloadVrmItemSpawner);
            
            LoadAll();
        }

        public void Add(string vrmFileFullName)
        {
            if (!Directory.Exists(GlobalPath.VrmHomePath))
            {
                Directory.CreateDirectory(GlobalPath.VrmHomePath);
            }
            
            File.Copy(vrmFileFullName,GlobalPath.VrmHomePath + "/" + Path.GetFileName(vrmFileFullName));
            
            var mainCamera = Camera.main;
            var defaultCullingMask = mainCamera.cullingMask;
            mainCamera.cullingMask = 1 << LayerMask.NameToLayer("Loading");
            StartCoroutine(Loading(mainCamera, defaultCullingMask, vrmFileFullName));
        }

        IEnumerator Loading(Camera mainCamera,int defaultCullingMask, string vrmFileFullName)
        {
            //Loading表示に切り替えるために1フレーム待つ
            yield return null;
            
            var vrmMeta = VrmMeta.Generate(vrmFileFullName);
            vrmMeta.Save();
            Load(vrmMeta);
            mainCamera.cullingMask = defaultCullingMask; 
        }

        void LoadAll()
        {
            if (!Directory.Exists(GlobalPath.VrmHomePath))
            {
                return;
            }

            var dir = new DirectoryInfo(GlobalPath.VrmHomePath);
            var vrmMetaInfos = dir.GetFiles("*"+VrmMeta.Extension);
            var orderedVrmMetaInfos = vrmMetaInfos.OrderBy(f => f.LastWriteTime);
            
            foreach (var vrmMetaInfo in orderedVrmMetaInfos)
            {
                Load(VrmMeta.Load(vrmMetaInfo.FullName));
            }
        }

        void Load(VrmMeta vrmMeta)
        {
            var localVrmItem = Instantiate(localVrmItemPrefab,localVrmItemRoot);
            localVrmItem.transform.SetSiblingIndex(0);
            localVrmItem.Initialize(vrmMeta, vrmSpawner, downloadVrmItemSpawner);
        }

    }
}