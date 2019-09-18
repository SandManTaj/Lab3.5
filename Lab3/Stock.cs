using System;
using System.Threading;
using System.IO;


public class Stock
{
    // declare variables
    string name;
    int initalValue;
    int currentValue;
    int maximumChange;
    int changes;
    int notificationThreshold;
    bool thresholdReached;
    public Notification _notification = new Notification();
    Thread thread;
    // object used lock threads and avoid process collision
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
        // creates a new thread and starts it
        thread = new Thread(Activate);
        thread.Start();
    }

    // changes the value of a stock every .5 seconds
    public void Activate()
    {
        for (;;)
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
                // saves the stock to a file once the threshold is exceeded
                _notification.saveStock(DateTime.Now, name, initalValue, currentValue);
            }
            // prints the value of a stock every change after the threshold is exceeded
            _notification.printStock(name, currentValue, changes);
        }
    }

    // notifies the user that the stock has exceed the threshold and saves the information
    static void notification_saveEvent(object sender, SaveEventArgs e)
    {
        string path = Directory.GetCurrentDirectory();
        // locks to avoid collision
        lock (lockObject)
        {
            // creates a new file if one does not exist, writes to an existing one
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(path, "WriteLines.txt"), true))
            {
                outputFile.WriteLine("{0, -15} {1, -15} {2, -15} {3, -15}\n", e.dateTime.ToString(), e.stockName, e.initialValue, e.currentValue);
            }
            Console.WriteLine("Saving info: {0, -15} {1, -15} {2, -15} {3, -15}", e.dateTime.ToString(), e.stockName, e.initialValue, e.currentValue);
        }
    }

}