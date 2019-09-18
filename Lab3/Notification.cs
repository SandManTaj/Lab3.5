using System;


public class Notification
{
    // declare event handelr for when a stock changes
    public event EventHandler<StockEventArgs> stockEvent;
    // declare event handler for when a stock is saved
    public event EventHandler<SaveEventArgs> saveEvent;


    public void printStock(string stockName, int currentValue, int numberChanges)
    {
        if (stockEvent != null)
        {
            StockEventArgs e = new StockEventArgs();
            e.stockName = stockName;
            e.currentValue = currentValue;
            e.numberChanges = numberChanges;
            // raises event when printStock is called
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
            // raises event when saveStock is called
            saveEvent?.Invoke(this, e);
        }
    }
}