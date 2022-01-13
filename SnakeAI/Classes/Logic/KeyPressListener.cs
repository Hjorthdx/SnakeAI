using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SnakeAI {
  // Integreres med winform når denne implementeret!

  // Listens for keypress

  public class KeyPressListener {

    private ManualResetEvent manualResetEventKeyPressListener;
    public bool keyPressed { get; private set; }

    public KeyPressListener() {
      manualResetEventKeyPressListener = new ManualResetEvent(false); // True = is paused. 
      keyPressed = false;
    }

    public void StartThread() {
      StartListenForKeyPressThread();
    }

    // Starts a thread which will listen for keypress while the genetic algorithm is running
    private void StartListenForKeyPressThread() {
      Thread keyPressThread = new Thread(ListenForKeyPress);
      keyPressThread.IsBackground = true; // Will close when main thread ends
      keyPressThread.Start();
    }

    private void ListenForKeyPress() {
      bool enterPressed = false;
      bool escapePressed = false;
      ConsoleKeyInfo consoleKeyPressed;

      while(true) {
        while(!(enterPressed || escapePressed)) {
          consoleKeyPressed = Console.ReadKey();
          // When key pressd. Check if match
          if(consoleKeyPressed.Key == ConsoleKey.Enter) {
            enterPressed = true;
          }
          else if(consoleKeyPressed.Key == ConsoleKey.Escape) {
            escapePressed = true; // Behøves ej
            Environment.Exit(0);
          }
        }
        // Got out of loop - key must have been pressed!
        keyPressed = true;
        Console.Clear();
        Console.WriteLine("You pressed enter!\nWaiting for calculations to finish...");
        manualResetEventKeyPressListener.WaitOne(); // If paused, thread will pause here. 
        keyPressed = false;
        Pause(); // Set pause again
        enterPressed = false;
        escapePressed = false;
      }
    }

    public void Pause() {
      manualResetEventKeyPressListener.Reset();
    }

    public void Resume() {
      manualResetEventKeyPressListener.Set();
    }
  }
}
