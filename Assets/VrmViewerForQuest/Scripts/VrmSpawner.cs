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

        [SerializeField]
        AnimationClip defaultAnimation;

        [SerializeField]
        BlendShapePreset defaultBlendShapePreset;

        [SerializeField]
        float defaultBlendShapeValue;

        readonly List<KeyValuePair<string, VRMImporterContext>> poolingVrmImporterContexts = new List<KeyValuePair<string, VRMImporterContext>>();

        //VRMの最大同時読み込み数
        const int MaxPoolingNum = 3;

        void Awake()
        {
            Assert.IsNotNull(vrmRoot);
            Assert.IsNotNull(defaultAnimation);
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

            var mainCamera = Camera.main;
            var defaultCullingMask = mainCamera.cullingMask;
            mainCamera.cullingMask = 1 << LayerMask.NameToLayer("Loading");
            StartCoroutine(Loading(mainCamera, defaultCullingMask, vrmMeta));
        }

        IEnumerator Loading(Camera mainCamera, int defaultCullingMask, VrmMeta vrmMeta)
        {
            //Loading表示に切り替えるために1フレーム待つ
            yield return null;

            var bytes = File.ReadAllBytes(GlobalPath.VrmHomePath + "/" + vrmMeta.VrmFileName);
            var vrmImporterContext = new VRMImporterContext();

            vrmImporterContext.ParseGlb(bytes);
            vrmImporterContext.Load();
            vrmImporterContext.Root.transform.SetParent(vrmRoot, false);
            var vrmAnimationController = vrmImporterContext.Root.AddComponent<VrmAnimationController>();
            vrmAnimationController.Play(defaultAnimation);

            //BlendShapeProxyの初期化を待つ。
            yield return null;
            
            var vrmBlendShapeProxy = vrmImporterContext.Root.GetComponent<VRMBlendShapeProxy>();
            vrmBlendShapeProxy.ImmediatelySetValue(defaultBlendShapePreset, defaultBlendShapeValue);
            vrmImporterContext.EnableUpdateWhenOffscreen();
            vrmImporterContext.ShowMeshes();
            poolingVrmImporterContexts.Add(new KeyValuePair<string, VRMImporterContext>(vrmMeta.VrmFileName, vrmImporterContext));
            mainCamera.cullingMask = defaultCullingMask;
        }
    }
}