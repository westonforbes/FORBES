using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FORBES.PROCESS_TIME_NAMESPACE
{
    public class PROCESS_TIME
    {
        public DateTime START_TIME { get; private set; }
        public DateTime STOP_TIME { get; private set; }
        public TimeSpan TIME_ELAPSED { get; private set; }

        public PROCESS_TIME(bool START_NOW = false)
        {
            if (START_NOW)
                START_TIME = DateTime.Now;
            else
                START_TIME = new DateTime();
            STOP_TIME = new DateTime();
            TIME_ELAPSED = new TimeSpan();
        }
        public void START() { START_TIME = DateTime.Now; }
        public TimeSpan STOP()
        {
            STOP_TIME = DateTime.Now;
            TIME_ELAPSED = STOP_TIME - START_TIME;
            return TIME_ELAPSED;
        }
    }
}
