using System;


// event arguments for stockEvent
public class StockEventArgs : EventArgs
{
    public string stockName { get; set; }
    public int currentValue { get; set; }
    public int numberChanges { get; set; }
}