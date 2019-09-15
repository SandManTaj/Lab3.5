using System;
using System.Threading;
using System.Collections.Generic;

public class Program
{
    public static void Main()
    {
        Stock stock1 = new Stock("Technology", 160, 5, 15);
        Stock stock2 = new Stock("Retail", 30, 2, 6);
        Stock stock3 = new Stock("Banking", 90, 4, 10);
        Stock stock4 = new Stock("Commodity", 500, 20, 50);

        StockBroker b1 = new StockBroker("Broker 1");
        b1.AddStock(stock1);
        b1.AddStock(stock2);

        StockBroker b2 = new StockBroker("Broker 2");
        b2.AddStock(stock1);
        b2.AddStock(stock3);
        b2.AddStock(stock4);

        StockBroker b3 = new StockBroker("Broker 3");
        b3.AddStock(stock1);
        b3.AddStock(stock3);

        StockBroker b4 = new StockBroker("Broker 4");
        b4.AddStock(stock1);
        b4.AddStock(stock2);
        b4.AddStock(stock3);
        b4.AddStock(stock4);
    }

}

public class Stock
{
    string name;
    int initalValue;
    int currentValue;
    int maximumChange;
    int changes;
    int notificationThreshold;
    public Notification _notification = new Notification();
    Thread thread;

    public Stock(string nm, int iv, int mc, int nt)
    {
        name = nm;
        initalValue = iv;
        currentValue = iv;
        maximumChange = mc;
        notificationThreshold = nt;
        changes = 0;
        thread = new Thread(Activate);
        thread.Start();
    }

    public void Activate()
    {
        for(;;)
        {
            Thread.Sleep(500);
            changeStockValue();
        }
    }

    void changeStockValue()
    {
        Random rand = new Random();
        changes++;
        currentValue += rand.Next(1, maximumChange);
        if (currentValue - initalValue > notificationThreshold)
        {
            _notification.printStock(name, currentValue, changes);
            initalValue = currentValue;
        }
    }

    
}

public class StockBroker
{
    string brokerName;
    List<Stock> listOfStocks = new List<Stock>();
    public StockBroker(string name)
    {
        brokerName = name;
        //_notification.stockEvent += notification_strockEvent;
    }

    public void AddStock(Stock stock)
    {
        listOfStocks.Add(stock);
        stock._notification.stockEvent += notification_strockEvent;
    }

    void notification_strockEvent(string stockName, int currentValue, int numberChanges)
    {
        Console.WriteLine("{0, -15} {1, -15} {2, -15} {3, -15}", brokerName, stockName, currentValue, numberChanges);
    }
}

public class Notification
{
    public delegate void StockNotification(string stockName, int currentValue, int numberChanges);
    public delegate void SaveNotification(DateTime dateTime, string stockName, int initialValue, int currentValue);

    public event StockNotification stockEvent;
    public event SaveNotification saveEvent;


    public Notification()
    {

    }
      

    public void printStock(string stockName, int currentValue, int numberChanges)
    {
        if (stockEvent != null)
            stockEvent?.Invoke(stockName, currentValue, numberChanges);
    }
    public void saveStock(DateTime dateTime, string stockName, int initialValue, int currentValue)
    {
        if (saveEvent != null)
            saveEvent?.Invoke(dateTime, stockName, initialValue, currentValue);
    }
}
