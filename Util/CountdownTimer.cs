using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;

namespace OCEAdmin.Util
{
    public class CountdownTimer
    {
        public DateTime endTime;

        // Tickrate of the countdown timer.
        private int _tickTime;
        private bool _ticking;
        private Thread _tickThread;
        private NetworkCommunicator _networkPeer;

        private static List<CountdownTimer> _Instances = new List<CountdownTimer>();

        public event EventHandler<CountdownTimerEventArgs> OnTimerComplete;

        // Creates a new countdown timer with x amount of seconds.
        public CountdownTimer(NetworkCommunicator networkPeer, int seconds, int tickTime = 200)
        {
            _tickTime = tickTime;
            _networkPeer = networkPeer;
            endTime = DateTime.Now.AddSeconds(seconds);
            _Instances.Add(this);

            _ticking = true;
            _tickThread = new Thread(Tick);
            _tickThread.Start();
        }

        // Compares the current date to the end date to see if the
        // timer has been completed.
        public void Tick()
        {
            if(DateTime.Now >= endTime)
            {
                _ticking = false;
                CountdownTimerEventArgs args = new CountdownTimerEventArgs();
                args.networkPeer = _networkPeer;
                OnCountdownComplete(args);
            }

            Thread.Sleep(_tickTime);

            if (_ticking)
                Tick();
        }

        public static bool PeerHasCountdown(NetworkCommunicator peer)
        {
            foreach(CountdownTimer countdown in _Instances)
            {
                if (countdown._networkPeer.VirtualPlayer.Id == peer.VirtualPlayer.Id)
                    return true;
            }

            return false;
        }

        protected virtual void OnCountdownComplete(CountdownTimerEventArgs e)
        {
            EventHandler<CountdownTimerEventArgs> handler = OnTimerComplete;
            if (handler != null)
            {
                handler(this, e);
            }

            _Instances.Remove(this);
        }
    }

    public class CountdownTimerEventArgs : EventArgs
    {
        public NetworkCommunicator networkPeer { get; set; }
    }
}
