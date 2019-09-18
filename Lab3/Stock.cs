using System;
using System.Threading;
using System.IO;



public class Stock
{
    string name;
    int initalValue;
    int currentValue;
    int maximumChange;
    int changes;
    int notificationThreshold;
    bool thresholdReached;
    static object lockObject = new object();
    public Notification _notification = new Notification();
    Thread thread;

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
        thread = new Thread(Activate);
        thread.Start();
    }

    public void Activate()
    {
        for (; ; )
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
            if (thresholdReached == false)
            {
                thresholdReached = true;
                _notification.saveStock(DateTime.Now, name, initalValue, currentValue);
            }
            _notification.printStock(name, currentValue, changes);
        }
    }

    static void notification_saveEvent(object sender, SaveEventArgs e)
    {
        string path = Directory.GetCurrentDirectory();
        lock (lockObject)
        {
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(path, "WriteLines.txt"), true))
            {
                outputFile.WriteLine("{0, -15} {1, -15} {2, -15} {3, -15}\n", e.dateTime.ToString(), e.stockName, e.initialValue, e.currentValue);
            }
            Console.WriteLine("Saving info: {0, -15} {1, -15} {2, -15} {3, -15}", e.dateTime.ToString(), e.stockName, e.initialValue, e.currentValue);
        }
    }

}