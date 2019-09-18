using System;

public class SaveEventArgs : EventArgs
{
    public DateTime dateTime { get; set; }
    public string stockName { get; set; }
    public int initialValue { get; set; }
    public int currentValue { get; set; }
}