using System;
using UnityEngine;

namespace JackieSoft
{
    [Serializable]
    public class Descending : ListView.Order
    {
        public override void SetHeaderSibling()
        {
            header.SetAsLastSibling();
        }

        public override void SetFooterSibling()
        {
            footer.SetAsFirstSibling();
        }

        public override void SetSibling(Transform transform, int index, int count)
        {
            transform.SetSiblingIndex(count - index - 2);
        }

        public override float CalculateVal(float val) => val;
    }
}