using System;
using UnityEngine;

namespace JackieSoft
{
    [Serializable]
    public class Ascending : ListView.Order
    {
        public override void SetHeaderSibling()
        {
            header.SetAsFirstSibling();
        }

        public override void SetFooterSibling()
        {
            footer.SetAsLastSibling();
        }

        public override void SetSibling(Transform transform, int index, int count)
        {
            transform.SetSiblingIndex(index + 1);
        }

        public override float CalculateVal(float val) => 1 - val;
    }
}