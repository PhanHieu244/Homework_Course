using JackieSoft;
using UnityEngine;
using UnityEngine.UI;

public class CellView : MonoBehaviour, Cell.IView
{
    public Text text;

}

public class CellData : Cell.Data<CellView>
{
    public int Index;
    
    protected override void SetupData(CellView cellView)
    {
        cellView.gameObject.name = "element" + Index;
        cellView.text.text = $"{Index}";
    }
} 
