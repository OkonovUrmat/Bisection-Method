using System;
using parserDecimal.Parser;
using System.Diagnostics;
using System.Windows.Forms;

namespace Okonov_Urmat_Bisection
{
    public class BisectionMethodClass
    {
        double eps = 0;
        public double timepl = 0;
        public decimal intervalBegin1 = 0;
        public decimal intervalEnd1 = 0;
        public decimal middle = 0;
        public double precision = 0;
        public Stopwatch stopWatch = new Stopwatch();
        public Stopwatch time = new Stopwatch();
        public int init1 = 0;
        public TimeSpan tim = new TimeSpan();
        public string elapsedTime1 = "";
        Computer computer = new Computer();
        public decimal rightbound = 0.0M, leftbound = 0.0M;
        public double epsilon = 0;
        public string func = "",b1="",b2="",b3="";
        public uint k_max = 0;
        public double max_time = 0;
        public int k_max_begin = 0;
        public double time_max_begin = 0;
        Bisection_Method b = new Bisection_Method();
        public event Action<int> increment_max;
        public event Action<int> time_max;

        public void start(decimal intervalBegin, decimal intervalEnd, double precision, uint K_maxBox, int init, double max_time, string func, System.Windows.Forms.ProgressBar progressBar)
        {
            while (Math.Abs(intervalBegin - intervalEnd) > Convert.ToDecimal(precision) && init < Convert.ToInt32(K_maxBox))
            {


                stopWatch.Start();
                init++;

                middle = (intervalBegin + intervalEnd) / 2.0M;




                if (Math.Sign(GetFunction(func, intervalBegin)) * Math.Sign(GetFunction(func, middle)) < 0.0M)
                {
                    intervalEnd = middle;

                }
                else
                {
                    intervalBegin = middle;

                }


                progressBar.Visible = true;
                progressBar.Maximum = Convert.ToInt32(init + 0.00001);
                progressBar.Value = init;
          
            

                stopWatch.Stop();
                tim = stopWatch.Elapsed;
                TimeSpan ts1 = stopWatch.Elapsed;
                elapsedTime1 = String.Format("{00}",
                ts1.Milliseconds);
                eps = Convert.ToDouble(elapsedTime1);
                if (eps < 0.01)
                {
                    timepl = 0.01 - eps;
                }
                if (eps >= max_time)
                {
                    var mb = MessageBox.Show("K_max " + K_maxBox + "\n" + "Time " + max_time + "\n" + "X*= " + middle + "\n" + "Time is up.It isn't possible to find a solution with the given time.Would you like to continue? ", "Warning", MessageBoxButtons.YesNo);
                    if (mb == DialogResult.Yes)
                    {
                        max_time += max_time;
                        time_max?.Invoke(Convert.ToInt32(max_time));

                    }
                    else if (mb == DialogResult.No)
                    {
                        b1 = "Solution hasn`t found.Time reached the limit.";
                        break;
                    }

                }


                if (init >= K_maxBox)
                {
                    var mb = MessageBox.Show("K_max " + K_maxBox + "\n" + "Time " + max_time + "\n" + "X*= " + middle + "\n" + "iterations reached the limit.It isn't possible to find a solution with the given k_max.Would you like to continue? ", "Warning", MessageBoxButtons.YesNo);
                    if (mb == DialogResult.Yes)
                    {
                        K_maxBox += K_maxBox;
                        increment_max?.Invoke(Convert.ToInt32(K_maxBox));

                    }
                    else if (mb == DialogResult.No)
                    {
                        
                        b1 = "Solution hasn`t found.iterations reached the limit.";
                        break;
                    }

                }

            }

            stopWatch.Reset();
            progressBar.Visible = false;
            timepl = Convert.ToDouble(elapsedTime1);
            intervalBegin1 = intervalBegin;
            intervalEnd1 = intervalEnd;
            init1 = init;
            k_max_begin = Convert.ToInt32(K_maxBox);
            time_max_begin = max_time;
            b3 = b1;
        }


        private decimal GetFunction(string function, decimal x1)
        {
            Computer comp = new Computer();
            return comp.Compute(function, x1);
        }
      
    }
}
