using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OCEAdmin
{
    public class UpdateManager
    {
        public UpdateState State;
        private const int UPDATE_TICK_RATE = 10;
        Thread thr;

        public UpdateManager()
        {
            this.SetState(UpdateState.GeneratingChecksum);

            thr = new Thread(() => this.Tick());
            thr.Start();
        }
        
        public void Tick()
        {
            while (State != UpdateState.Complete)
            {
                switch (State)
                {
                    case UpdateState.GeneratingChecksum:
                        this.GenerateChecksum();
                        break;
                    case UpdateState.ComparingChecksum:
                        this.SetState(UpdateState.Updating); // NYI: Pass thru to load until master server done
                        break;
                    case UpdateState.Updating:
                        this.SetState(UpdateState.Complete); // NYI: Pass thru to load until master server done
                        break;
                }
                // Tick rate of this thread.
                Thread.Sleep(1000 / UPDATE_TICK_RATE);
            }

            this.Finish();
            thr.Abort();
        }

        public void GenerateChecksum()
        {
            HashStorage hashStorage = new HashStorage();
            hashStorage.GenerateFromDir();
            hashStorage.ToFile();

            this.SetState(UpdateState.ComparingChecksum);
        }

        // We're all up to date or were unable to reach the update server.
        // Let's load the uniform manager with the clan data we have.
        public void Finish()
        {
            UniformManager.Instance.Populate();
        }

        // Changes the state of the update transfer.
        public void SetState(UpdateState _state)
        {
            this.State = _state;
        }
    }

    public enum UpdateState
    {
        GeneratingChecksum,
        ComparingChecksum,
        Updating,
        Complete
    }
}
