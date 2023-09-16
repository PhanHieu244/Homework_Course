using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JackieSoft
{
    public class CellCreator : MonoBehaviour
    {
        [SerializeField] private RectTransform[] cellPrefabs;

        private GameObject _goCellPool;

        private Dictionary<Type, Queue<Cell.IView>> _cellPools;
        private Dictionary<Type, (RectTransform prefab, Vector2 size)> _cells;

        private void Awake()
        {
            _cellPools = new Dictionary<Type, Queue<Cell.IView>>();
            _cells = new Dictionary<Type, (RectTransform prefab, Vector2 size)>();

            foreach (var cellPrefab in cellPrefabs)
            {
                var layOutElement = cellPrefab.GetComponent<LayoutElement>();
                var view = cellPrefab.GetComponent<Cell.IView>().GetType();
                _cellPools.Add(view, new Queue<Cell.IView>());
                _cells.Add(view, (cellPrefab, new Vector2(layOutElement.minWidth, layOutElement.minHeight)));
            }

            _goCellPool = new GameObject(">------> cell pool <-------<", typeof(RectTransform));
            _goCellPool.transform.SetParent(transform, false);
        }

        public Cell.IView Get(Cell.IData cellData)
        {
            var type = cellData.ViewType;
            if (_cellPools[type].Count == 0)
            {
                var cellRect = Instantiate(_cells[type].prefab, _goCellPool.transform);
                cellRect.gameObject.SetActive(true);
                return cellRect.gameObject.GetComponent<Cell.IView>();
            }

            var cellView = _cellPools[type].Dequeue();
            ((Component)cellView).gameObject.SetActive(true);
            return cellView;
        }

        public Vector2 CellSize(Cell.IData cellData)
        {
            return _cells[cellData.ViewType].size;
        }

        public void Release(Cell.IData cellData, Cell.IView cellView)
        {
            var cellTransform = ((Component)cellView).transform;
            cellTransform.SetParent(_goCellPool.transform, false);
            cellTransform.gameObject.SetActive(false);
            _cellPools[cellData.ViewType].Enqueue(cellView);
        }
    }
}