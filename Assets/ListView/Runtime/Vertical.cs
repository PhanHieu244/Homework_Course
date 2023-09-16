using System;
using UnityEngine;
using UnityEngine.UI;

namespace JackieSoft
{
    [Serializable]
    public class Vertical : ListView.Layout
    {
        public override void Awake()
        {
            // set content
            var layoutGroup = content.gameObject.AddComponent<VerticalLayoutGroup>();
            layoutGroup.childForceExpandHeight = false;

            if (order is Descending)
            {
                layoutGroup.padding = padding;
                layoutGroup.spacing = spacing;
                layoutGroup.childAlignment = TextAnchor.LowerCenter;

                content.anchorMax = new Vector2(1, 0);
                content.anchorMin = new Vector2(0, 0);
                
                content.pivot = new Vector2(0.5f, 0);

                FirstPadding = padding.top;
                LastPadding = padding.bottom;
            }
            // default is ascending
            else
            {
                layoutGroup.padding = padding;
                layoutGroup.spacing = spacing;
                layoutGroup.childAlignment = TextAnchor.UpperCenter;

                content.anchorMax = new Vector2(1, 1);
                content.anchorMin = new Vector2(0, 1);

                content.pivot = new Vector2(0.5f, 1);

                FirstPadding = padding.bottom;
                LastPadding = padding.top;
            }

            content.offsetMin = new Vector2(0, content.offsetMin.y);
            content.offsetMax = new Vector2(0, content.offsetMax.y);

            ListViewSize = viewport.height;
        }

        public override float CellSize(Vector2 size) => size.y;
        public override float CalculatePoint(Vector2 val) => (contentSize - ListViewSize) * order.CalculateVal(val.y);
        public override void SetElement(LayoutElement layoutElement, float size) => layoutElement.minHeight = size;

        public override void SetContentSize(float size)
        {
            base.SetContentSize(size);
            var sizeDelta = content.sizeDelta;
            sizeDelta.y = size;
            content.sizeDelta = sizeDelta;
        }

        public override void Set(ScrollRect scrollRect)
        {
            scrollRect.horizontal = false;
            scrollRect.vertical = true;
        }
    }
}