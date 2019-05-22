using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using VRM;

namespace VrmViewer
{
    [Serializable]
    public class VrmMeta
    {
        public string VrmFileName;
        public string Title;
        public string Version;
        public string Author;
        public string ContactInformation;
        public string Reference;
        public byte[] Thumbnail;
        public int ThumbnailWidth;
        public int ThumbnailHeight;

        public AllowedUser AllowedUser;
        public UssageLicense ViolentUssage;
        public UssageLicense SexualUssage;
        public UssageLicense CommercialUssage;
        public string OtherPermissionUrl;
        
        public LicenseType LicenseType;
        public string OtherLicenseUrl;
        
        public string FileName => VrmFileName + Extension;

        public const string Extension = ".vrmmeta";

        public static VrmMeta Generate(string vrmFileFullName)
        {
            var bytes = File.ReadAllBytes(vrmFileFullName);
            var vrmImporterContext = new VRMImporterContext();
            vrmImporterContext.ParseGlb(bytes);
            var meta = vrmImporterContext.ReadMeta(true);

            var vrmMeta = new VrmMeta
            {
                VrmFileName = Path.GetFileName(vrmFileFullName),
                Title = meta.Title,
                Version = meta.Version,
                Author = meta.Author,
                ContactInformation = meta.ContactInformation,
                Reference = meta.Reference,
                Thumbnail = meta.Thumbnail?.EncodeToPNG(),
                ThumbnailWidth = meta.Thumbnail ? meta.Thumbnail.width : 0,
                ThumbnailHeight = meta.Thumbnail ? meta.Thumbnail.height : 0,
                AllowedUser = meta.AllowedUser,
                ViolentUssage = meta.ViolentUssage,
                SexualUssage = meta.SexualUssage,
                CommercialUssage = meta.CommercialUssage,
                OtherPermissionUrl = meta.OtherPermissionUrl,
                LicenseType = meta.LicenseType,
                OtherLicenseUrl = meta.OtherLicenseUrl,
            };

            vrmImporterContext.Dispose();
            return vrmMeta;
        }

        public static VrmMeta Load(string vrmMetaFullName)
        {
            using (var stream = new FileStream(vrmMetaFullName, FileMode.Open))
            {
                var serializer = new XmlSerializer(typeof(VrmMeta));
                return (VrmMeta)serializer.Deserialize(stream);
            }
        }

        public void Save()
        {
            using (var stream = new FileStream(GlobalPath.VrmHomePath + "/" + VrmFileName + Extension, FileMode.Create))
            {
                var serializer = new XmlSerializer(typeof(VrmMeta));
                serializer.Serialize(stream, this);
            }
        }

        public List<KeyValuePair<string, string>> GetParameterList()
        {
            var parameterList = new List<KeyValuePair<string, string>>();

            parameterList.Add(new KeyValuePair<string, string>(nameof(Title),Title));
            parameterList.Add(new KeyValuePair<string, string>(nameof(Version),Version));
            parameterList.Add(new KeyValuePair<string, string>(nameof(Author),Author));
            parameterList.Add(new KeyValuePair<string, string>(nameof(ContactInformation),ContactInformation));
            parameterList.Add(new KeyValuePair<string, string>(nameof(Reference),Reference));
            parameterList.Add(new KeyValuePair<string, string>(nameof(AllowedUser),AllowedUser.ToString()));
            parameterList.Add(new KeyValuePair<string, string>(nameof(ViolentUssage),ViolentUssage.ToString()));
            parameterList.Add(new KeyValuePair<string, string>(nameof(SexualUssage),SexualUssage.ToString()));
            parameterList.Add(new KeyValuePair<string, string>(nameof(CommercialUssage),CommercialUssage.ToString()));
            parameterList.Add(new KeyValuePair<string, string>(nameof(OtherPermissionUrl),OtherPermissionUrl));
            parameterList.Add(new KeyValuePair<string, string>(nameof(LicenseType),LicenseType.ToString()));
            parameterList.Add(new KeyValuePair<string, string>(nameof(OtherLicenseUrl),OtherLicenseUrl));
            
            return parameterList;
        }
    }
}