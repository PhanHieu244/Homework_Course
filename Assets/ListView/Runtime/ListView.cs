using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JackieSoft
{
    [AddComponentMenu("Jackie Soft/List View")]
    [RequireComponent(typeof(ScrollRect), typeof(CellCreator))]
    public class ListView : MonoBehaviour
    {
        public List<Cell.IData> Data;
        [SerializeReference, SubclassSelector] private Layout _layout = new Vertical();
        [SerializeReference, SubclassSelector] private Order order = new Ascending();
        [SerializeField] public RectOffset padding;
        [SerializeField] public float spacing;

        private ScrollRect _scrollRect;
        private CellCreator _cellCreator;
        private RectTransform _tContent;
        private LayoutElement _leHeader, _leFooter;
        private Cell[] _cells;
        
        private void Awake()
        {
            _scrollRect = GetComponent<ScrollRect>();
            _cellCreator = GetComponent<CellCreator>();

            _scrollRect.viewport = GetComponent<RectTransform>();
            _tContent = new GameObject(">-------content---------<", typeof(RectTransform)).GetComponent<RectTransform>();
            _tContent.SetParent(_scrollRect.viewport, false);
            _scrollRect.content = _tContent;

            _leHeader = new GameObject(">--------Header--------<", typeof(RectTransform), typeof(LayoutElement)).GetComponent<LayoutElement>();
            _leFooter = new GameObject(">--------Footer--------<", typeof(RectTransform), typeof(LayoutElement)).GetComponent<LayoutElement>();

            _leHeader.transform.SetParent(_tContent, false);
            _leFooter.transform.SetParent(_tContent, false);

            _layout.Set(_scrollRect);
            _layout.Awake(_tContent, padding, _scrollRect.viewport.rect, order, spacing);
            order.Awake(_leHeader.GetComponent<RectTransform>(), _leFooter.GetComponent<RectTransform>());
        }

        private void OnEnable()
        {
            _scrollRect.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnDisable()
        {
            _scrollRect.onValueChanged.RemoveListener(OnValueChanged);
        }

        private int _topIndex, _bottomIndex;

        private void OnValueChanged(Vector2 val)
        {
            var pointStart = _layout.CalculatePoint(val);
 
            var pointEnd = pointStart + _layout.ListViewSize;
            var start = pointStart < _cells[0].Top ? 0 : CellAt(0, _cells.Length - 1, pointStart);
            var end = pointEnd > _cells[^1].Down ? (_cells.Length - 1) : CellAt(0, _cells.Length - 1, pointEnd);

            if (!IsDirty(start, end)) return;
            CorrectSibling();
            CorrectHeader();
            CorrectFooter();
        }

        //check need to change the list view when value change
        private bool IsDirty(int start, int end)
        {
            var dirty = false;
            if (start != _topIndex)
            {
                if (start < _topIndex)
                {
                    Active(start, _topIndex - 1);
                }
                else
                {
                    DeActive(_topIndex, start - 1);
                }
                
                dirty = true;
                _topIndex = start;
            }

            if (end == _bottomIndex) return dirty;
            if (end < _bottomIndex)
            {
                DeActive(end + 1, _bottomIndex);
            }
            else
            {
                Active(_bottomIndex + 1, end);
            }
            
            _bottomIndex = end;
            return true;
        }

        private void CorrectSibling()
        {
            var count = _tContent.childCount;
            for (var cellIndex = _topIndex; cellIndex <= _bottomIndex; cellIndex++)
                order.SetSibling(((Component)_cells[cellIndex].View).transform, cellIndex - _topIndex, count);
        }

        private int CellAt(int top, int down, float point)
        {
            //find cell by binarySearch
            while (top != down)
            {
                var middle = (top + down) / 2;
                if (point < _cells[middle].Top)
                {
                    down = middle - 1;
                }
                else if (point > _cells[middle].Down)
                {
                    top = middle + 1;
                }
                else
                {
                    return middle;
                }
            }

            return top;
        }

        private void DeActive(int beginIndex, int endIndex)
        {
            for (var i = beginIndex; i <= endIndex; i++)
            {
                if (_cells[i].View is null) continue;
                _cellCreator.Release(_cells[i].data, _cells[i].View);
                _cells[i].View = null;
            }
        }

        private void Active(int beginIndex, int endIndex)
        {
            for (var i = beginIndex; i <= endIndex; i++)
            {
                if (_cells[i].View is not null) continue;
                _cells[i].View = _cellCreator.Get(_cells[i].data);
                _cells[i].data.CrawData(_cells[i].View);
                ((Component)_cells[i].View).transform.SetParent(_tContent, false);
            }
        }

        public void Initialize()
        {
            var listViewSize = _layout.ListViewSize;
            _cells = new Cell[Data.Count];

            var contentSize = _layout.FirstPadding;

            var firstCellSize = _layout.CellSize(_cellCreator.CellSize(Data[0]));
            // first cell
            _cells[0] = new Cell
            {
                data = Data[0],
                Point = contentSize,
                Size = firstCellSize,
                Top = 0,
                Down = contentSize + firstCellSize + 0.5f * spacing,
            };

            contentSize = contentSize + firstCellSize + spacing;

            // cell 1 -> cell n-2
            for (var i = 1; i < Data.Count - 1; i++)
            {
                var cellData = Data[i];
                var cellSize = _layout.CellSize(_cellCreator.CellSize(cellData));

                _cells[i] = new Cell
                {
                    data = Data[i],
                    Point = contentSize,
                    Size = cellSize,
                    Top = contentSize - 0.5f * spacing,
                    Down = contentSize + cellSize + 0.5f * spacing,
                };

                contentSize = contentSize + cellSize + spacing;
            }

            // Last cell
            var cellLastSize = _layout.CellSize(_cellCreator.CellSize(Data[^1]));
            _cells[^1] = new Cell
            {
                data = Data[^1],
                Point = contentSize,
                Size = cellLastSize,
                Top = contentSize - 0.5f * spacing,
                Down = contentSize + cellLastSize + _layout.LastPadding,
            };

            contentSize = contentSize + cellLastSize + _layout.LastPadding;

            _topIndex = 0;
            _bottomIndex = 0;

            while (_bottomIndex < _cells.Length)
            {
                if (_cells[_bottomIndex].Point < listViewSize)
                {
                    _bottomIndex++;
                }
                else
                {
                    _bottomIndex--;
                    break;
                }
            }

            _layout.SetContentSize(contentSize);

            Active(_topIndex, _bottomIndex);

            CorrectSibling();
            CorrectHeader();
            CorrectFooter();
        }

        private void CorrectHeader()
        {
            if (_topIndex <= 0)
            {
                _leHeader.gameObject.SetActive(false);
                return;
            }
            
            _leHeader.gameObject.SetActive(true);
            _layout.SetElement(_leHeader, _cells[_topIndex].Point - _cells[0].Point - spacing);
            order.SetHeaderSibling();
        }

        private void CorrectFooter()
        {
            if (_bottomIndex >= _cells.Length - 1)
            {
                _leFooter.gameObject.SetActive(false);
                return;
            }
            
            _leFooter.gameObject.SetActive(true);
            _layout.SetElement(_leFooter, _cells[^1].Point - _cells[_bottomIndex].Point - spacing - _cells[_bottomIndex].Size + _cells[^1].Size);
            order.SetFooterSibling();
        }

        public abstract class Layout
        {
            protected RectTransform content;
            protected RectOffset padding;
            protected Rect viewport;
            protected float spacing;
            protected Order order;

            protected float contentSize;

            public float ListViewSize { get; protected set; }
            public float FirstPadding { get; protected set; }
            public float LastPadding { get; protected set; }

            public abstract float CellSize(Vector2 size);
            public abstract void Awake();
            public abstract float CalculatePoint(Vector2 val);
            public abstract void SetElement(LayoutElement layoutElement, float size);

            public virtual void SetContentSize(float size)
            {
                contentSize = size;
            }

            public void Awake(RectTransform content, RectOffset padding, Rect viewport, Order order, float spacing)
            {
                this.content = content;
                this.padding = padding;
                this.viewport = viewport;
                this.order = order;
                this.spacing = spacing;

                Awake();
            }

            public abstract void Set(ScrollRect scrollRect);
        }

        [Serializable]
        public abstract class Order
        {
            protected RectTransform header, footer;

            public void Awake(RectTransform header, RectTransform footer)
            {
                this.header = header;
                this.footer = footer;
            }

            public abstract void SetHeaderSibling();
            public abstract void SetFooterSibling();
            public abstract void SetSibling(Transform transform, int index, int count);

            public abstract float CalculateVal(float val);
        }
    }


    public class Cell
    {
        public IData data;
        public IView View;
        public float Point;
        public float Size;
        public float Top;
        public float Down;

        public interface IData
        {
            void CrawData(IView cellView);
            Type ViewType { get; }
        }
        
        public interface IView
        {
        }

        public abstract class Data<T> : IData where T : MonoBehaviour, IView
        {
            void IData.CrawData(IView cellView) => SetupData((T)cellView);
            Type IData.ViewType => typeof(T);
            protected abstract void SetupData(T cellView);
        }
    }
}