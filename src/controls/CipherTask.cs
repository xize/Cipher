using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cipher
{
    class CipherTask
    {
        private CipherRunnable runa;
        private long delay;
        private volatile Thread thread;

        public CipherTask(CipherRunnable run, long delay)
        {
            this.runa = run;
            this.delay = delay;
        }

        private void task()
        {
            long time = 0;
            while(true)
            {
                if(time == this.delay)
                {
                    runa.run();
                    Thread.Sleep(int.MaxValue); //stops the thread.
                    break;
                }
                time++;
                Console.WriteLine("time: " + time);
            }
        }

        public void run()
        {
            this.runa.run();
        }

        public void stop()
        {
            Thread.Sleep(int.MaxValue); //stops the thread.
        }

        public void start()
        {
            this.thread = new Thread(task);
            thread.Start();
        }

        public bool isRunning()
        {
            if(this.thread != null)
            {
                return this.thread.IsAlive;
            }
            return false;
        }
    }
}
