using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace VrmViewer
{
    public class TextElement : MonoBehaviour
    {
        [SerializeField]
        Text title;

        [SerializeField]
        Text value;

        void Awake()
        {
            Assert.IsNotNull(title);
            Assert.IsNotNull(value);
        }

        public void Initialize(string title, string value)
        {
            gameObject.name = title + "Element";
            this.title.text = title;
            this.value.text = value;
        }
    }
}

