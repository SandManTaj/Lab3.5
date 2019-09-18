using System;
using System.Collections.Generic;


public class StockBroker
{
    string brokerName;
    List<Stock> listOfStocks = new List<Stock>();
    public StockBroker(string name)
    {
        brokerName = name;
    }

    public void AddStock(Stock stock)
    {
        listOfStocks.Add(stock);
        stock._notification.stockEvent += notification_stockEvent;
    }

    void notification_stockEvent(object sender, StockEventArgs e)
    {
        Console.WriteLine("{0, -15} {1, -15} {2, -15} {3, -15}", brokerName, e.stockName, e.currentValue, e.numberChanges);
    }
}