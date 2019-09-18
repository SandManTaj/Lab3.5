/*
 * Tajbir Sandhu
 * 9/17/2019
 * CECS 475
 */

using System;
using System.Threading;
using System.Collections.Generic;
using System.IO;

public class Program
{
    public static void Main()
    {
        string path = Directory.GetCurrentDirectory();
        System.IO.File.WriteAllText(Path.Combine(path, "WriteLines.txt"), string.Empty);
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
    // class variables
    string name;
    int initalValue;
    int currentValue;
    int maximumChange;
    int changes;
    int notificationThreshold;
    bool thresholdReached;
    public Notification _notification = new Notification();
    Thread thread;
    // lock object used to avoid thread collision
    static object lockObject = new object();

    public Stock(string nm, int iv, int mc, int nt)
    {
        name = nm;
        initalValue = iv;
        currentValue = iv;
        maximumChange = mc;
        changes = 0;
        notificationThreshold = nt;
        thresholdReached = false;
        _notification.saveEvent += notification_saveEvent;
        // creates a new thread
        thread = new Thread(Activate);
        thread.Start();
    }

    // changes the event current value every .5 seconds
    public void Activate()
    {
        for(;;)
        {
            Thread.Sleep(500);
            changeStockValue();
        }
    }

    // changes the value of the stock
    void changeStockValue()
    {
        Random rand = new Random();
        changes++;
        currentValue += rand.Next(1, maximumChange);
        if (currentValue - initalValue > notificationThreshold)
        {
            if (thresholdReached == false)
            {
                thresholdReached = true;
                // raises the saveStock event
                _notification.saveStock(DateTime.Now, name, initalValue, currentValue);
            }
            // raises the printStock event
            _notification.printStock(name, currentValue, changes);
        }
    }

    // writes when the threshold is passed onto a file a prints a notification
    static void notification_saveEvent(DateTime dt, string sn, int iv, int cv)
    {
        Console.WriteLine("Saving info: {0, -15} {1, -15} {2, -15} {3, -15}", dt.ToString(), sn, iv, cv);
        string path = Directory.GetCurrentDirectory();
        lock (lockObject)
        {
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(path, "WriteLines.txt"), true))
            {
                outputFile.WriteLine("{0, -15} {1, -15} {2, -15} {3, -15}\n", dt.ToString(), sn, iv, cv);
            }
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
    }

    // adds a stock to a broker
    public void AddStock(Stock stock)
    {
        listOfStocks.Add(stock);
        // subscribes to the stockEvent
        stock._notification.stockEvent += notification_stockEvent;
    }

    // prints whenever the stock value is changed after 
    void notification_stockEvent(string stockName, int currentValue, int numberChanges)
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
