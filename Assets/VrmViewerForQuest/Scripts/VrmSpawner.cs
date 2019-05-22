using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using VRM;

namespace VrmViewer
{
    public class VrmSpawner : MonoBehaviour
    {
        [SerializeField]
        Transform vrmRoot;

        readonly List<KeyValuePair<string, VRMImporterContext>> poolingVrmImporterContexts = new List<KeyValuePair<string, VRMImporterContext>>();

        //VRMの最大同時読み込み数
        const int MaxPoolingNum = 3;

        void Awake()
        {
            Assert.IsNotNull(vrmRoot);
        }

        public void SelectVrm(VrmMeta vrmMeta)
        {
            var currentVrmImporterContext = poolingVrmImporterContexts.SingleOrDefault(v => v.Key == vrmMeta.VrmFileName);
            if (!currentVrmImporterContext.Equals(default(KeyValuePair<string, VRMImporterContext>)))
            {
                foreach (var poolingVrmImporterContext in poolingVrmImporterContexts)
                {
                    poolingVrmImporterContext.Value.Root.SetActive(false);
                }

                currentVrmImporterContext.Value.Root.SetActive(true);
                return;
            }

            if (!File.Exists(GlobalPath.VrmHomePath + "/" + vrmMeta.VrmFileName))
            {
                Debug.LogError(GlobalPath.VrmHomePath + "/" + vrmMeta.VrmFileName + "のファイルが存在しません。");
                return;
            }

            if (poolingVrmImporterContexts.Count >= MaxPoolingNum)
            {
                poolingVrmImporterContexts[0].Value.Dispose();
                poolingVrmImporterContexts.RemoveAt(0);
            }

            foreach (var poolingVrmImporterContext in poolingVrmImporterContexts)
            {
                poolingVrmImporterContext.Value.Root.SetActive(false);
            }
            
            var bytes = File.ReadAllBytes(GlobalPath.VrmHomePath + "/" + vrmMeta.VrmFileName);
            var vrmImporterContext = new VRMImporterContext();

            vrmImporterContext.ParseGlb(bytes);
            vrmImporterContext.Load();
            vrmImporterContext.Root.transform.SetParent(vrmRoot, false);
            vrmImporterContext.EnableUpdateWhenOffscreen();
            vrmImporterContext.ShowMeshes();

            poolingVrmImporterContexts.Add(new KeyValuePair<string, VRMImporterContext>(vrmMeta.VrmFileName, vrmImporterContext));
        }
    }
}