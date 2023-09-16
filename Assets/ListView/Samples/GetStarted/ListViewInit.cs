using System.Collections.Generic;
using JackieSoft;
using UnityEngine;
using UnityEngine.Serialization;

public class ListViewInit : MonoBehaviour
{
    [SerializeField] private ListView listView;
    [SerializeField] private int cellAmount = 500;

    private void Start()
    {
        listView.Data = new List<Cell.IData>();
        for (var cellIndex = 0; cellIndex < cellAmount; cellIndex++)
        {
            listView.Data.Add(new CellData { Index = cellIndex });
        }

        listView.Initialize();
    }
}