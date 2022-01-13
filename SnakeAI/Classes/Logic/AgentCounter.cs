using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GeneticAlgorithmNS;

namespace SnakeAI {
  // SKal printes til vindue i stedet for console når vi får en winform op og køre

  // Counts up agents
  public class AgentCounter {
    private ManualResetEvent manualResetEventAgentCounter;
    public AgentCounter() {
      manualResetEventAgentCounter = new ManualResetEvent(true); // True = is paused. 
    }
    // Starts the counter in a thread which will count up and print number of agents calculated to console screen
    public void StartThread() {
      StartAgentCounterThread();
    }

    public void StartAgentCounterThread() {
      Thread agentCounterThread = new Thread(PrintCounter);
      agentCounterThread.IsBackground = true; // Will close when main thread ends
      agentCounterThread.Start();
    }

    private void PrintCounter() {
      while(true) {
        // Pauses here if reset event is reset
        manualResetEventAgentCounter.WaitOne(); 
        try {
          lock(Program.ConsoleWriteLineLock) {
            Console.Write($"Agents calculated: " +
            $"{Agent.TotalFitnessCalculations}");
            Thread.Sleep(500);
            // Move to start of line and overwrite with empty string (keeps deleting number printed)
            Console.Write("\r" + new string(' ', 30) + "\r"); 
          }
        }
        catch {
          Debug.WriteLine("Lock not working");
          //throw new Exception("Lock not working");
        }
      }
    }

    public void Pause() {
      manualResetEventAgentCounter.Reset();
    }

    public void Resume() {
      manualResetEventAgentCounter.Set();
    }
  }
}
