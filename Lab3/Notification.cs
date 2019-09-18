using System;

public class Notification
{
    public event EventHandler<StockEventArgs> stockEvent;
    public event EventHandler<SaveEventArgs> saveEvent;

    public Notification()
    {

    }


    public void printStock(string stockName, int currentValue, int numberChanges)
    {
        if (stockEvent != null)
        {
            StockEventArgs e = new StockEventArgs();
            e.stockName = stockName;
            e.currentValue = currentValue;
            e.numberChanges = numberChanges;
            stockEvent?.Invoke(this, e);
        }
    }
    public void saveStock(DateTime dateTime, string stockName, int initialValue, int currentValue)
    {
        if (saveEvent != null)
        {
            SaveEventArgs e = new SaveEventArgs();
            e.dateTime = dateTime;
            e.stockName = stockName;
            e.initialValue = initialValue;
            e.currentValue = currentValue;
            saveEvent?.Invoke(this, e);
        }
    }
}