using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace VrmViewer
{
    [RequireComponent(typeof(Button))]
    public class LocalVrmItem : MonoBehaviour
    {
        [SerializeField]
        Image thumbnail;

        [SerializeField]
        TextElement textElementPrefab;
            
        [SerializeField]
        Button removeButton;

        Button selectButton;
        Texture2D thumbnailTexture;
        Sprite thumbnailSprite;
        VrmMeta vrmMeta;
        DownloadVrmItemSpawner downloadVrmItemSpawner;
        VrmSpawner vrmSpawner;

        void Awake()
        {
            Assert.IsNotNull(thumbnail);
            Assert.IsNotNull(textElementPrefab);
            Assert.IsNotNull(removeButton);
            
            selectButton = GetComponent<Button>();
        }

        public void Initialize(VrmMeta vrmMeta, VrmSpawner vrmSpawner,DownloadVrmItemSpawner downloadVrmItemSpawner)
        {
            this.vrmMeta = vrmMeta;
            this.vrmSpawner = vrmSpawner;
            this.downloadVrmItemSpawner = downloadVrmItemSpawner;
            
            if (vrmMeta.Thumbnail != null)
            {
                thumbnailTexture = new Texture2D(vrmMeta.ThumbnailWidth, vrmMeta.ThumbnailHeight);
                thumbnailTexture.LoadImage(vrmMeta.Thumbnail);
                thumbnailSprite = Sprite.Create(texture: thumbnailTexture, rect: new Rect(0, 0, thumbnailTexture.width, thumbnailTexture.height),
                    pivot: new Vector2(0.5f, 0.5f));
            }
            
            thumbnail.sprite = thumbnailSprite;

            var parameterList = vrmMeta.GetParameterList();
            foreach (var parameter in parameterList)
            {
                var textElement = Instantiate(textElementPrefab, this.transform);
                textElement.Initialize(parameter.Key,parameter.Value);
            }
            
            selectButton.onClick.AddListener(() => Select());
            removeButton.onClick.AddListener(() => Remove());
        }

        void Remove()
        {
            if (File.Exists(GlobalPath.VrmHomePath + "/" + vrmMeta.VrmFileName))
            {
                File.Delete(GlobalPath.VrmHomePath + "/" + vrmMeta.VrmFileName);
            }

            if (File.Exists(GlobalPath.VrmHomePath + "/" + vrmMeta.FileName))
            {
                File.Delete(GlobalPath.VrmHomePath + "/" + vrmMeta.FileName);
            }

            downloadVrmItemSpawner.ReLoadAll();
            if(thumbnailSprite != null) Destroy(thumbnailSprite);
            if(thumbnailTexture != null) Destroy(thumbnailTexture);
            Destroy(this.gameObject);
        }

        void Select()
        {
            vrmSpawner.SelectVrm(vrmMeta);
        }
    }
}