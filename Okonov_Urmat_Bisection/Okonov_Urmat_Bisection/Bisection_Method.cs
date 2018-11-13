using System;
using System.Collections.Generic;
using info.lundin.math;
using parserDecimal.Parser;
using System.Windows.Forms;
using System.Diagnostics;

namespace Okonov_Urmat_Bisection
{
    public partial class Bisection_Method : Form
    {
        decimal intervalBegin = 0;
        decimal intervalEnd = 0;
        double precision = 0;
        Stopwatch stopWatch = new Stopwatch();
        Stopwatch time = new Stopwatch();
        int init = 0;
        Computer computer = new Computer();
        Change_Letters c = new Change_Letters();
        string func = "";
        uint k_max = 0;
        double max_time = 0;

        ExpressionParser parser = new ExpressionParser();

        public Bisection_Method()
        {
            InitializeComponent();
            SolutionOfTaskBox.Text = "";
            ValueOfFunctionBox.Text = "";
            BapsedTimeBox.Text = "";
            AmountOfIteratinosBox.Text = "";
            AbsBox.Text = "";
            label11.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            funcBox.Text = c.Function_Verify(funcBox.Text);
            if (funcBox.Text!="")
            {
                if (LeftEndPointBox.Text == "" || RightEndPointBox.Text == "" || ToleranceBox.Text == "" || timeBox.Text == "" || K_maxBox.Text == "")
                {
                    MessageBox.Show("Input textboxes are empty! Enter the data");
                    label11.Text = "Input textboxes are empty! Enter the data";
                    return;
                }

                func = funcBox.Text.Trim().Replace(".", ",");
                func = func.ToLower();
                label11.Text = "";
                try
                {
                    k_max = UInt16.Parse(K_maxBox.Text.Trim());
                }
                catch (Exception h)
                {
                    MessageBox.Show("Character values are not allowed.Check k_max.");
                    label11.Text = "Character values are not allowed.Check k_max.";
                    return;
                }
                if (k_max <= 0)
                {
                    MessageBox.Show("k_max can`t be less or equal than zero");
                    label11.Text = "k_max can`t be less or equal than zero";
                    return;
                }
                try
                {
                    intervalBegin = Decimal.Parse(LeftEndPointBox.Text.Trim(), System.Globalization.NumberStyles.Float);
                }
                catch (Exception h)
                {
                    MessageBox.Show("Character values are not allowed.Check left bound value.");
                    label11.Text = "Character values are not allowed.Check left bound value.";
                    return;
                }
                try
                {
                    intervalEnd = Decimal.Parse(RightEndPointBox.Text.Trim(), System.Globalization.NumberStyles.Float);
                }
                catch (Exception h)
                {
                    MessageBox.Show("Character values are not allowed.Check right bound value.");
                    label11.Text = "Character values are not allowed.Check right bound value.";
                    return;
                }
                try
                {
                    precision = Double.Parse(ToleranceBox.Text);
                }
                catch (Exception h)
                {
                    MessageBox.Show("Character values are not allowed.Check the Tolerance textbox.");
                    label11.Text = "Character values are not allowed.Check the Tolerance textbox.";
                    return;
                }
                try
                {
                    max_time = Double.Parse(timeBox.Text.Trim().Replace(".", ","));
                }
                catch (Exception h)
                {
                    MessageBox.Show("Character values are not allowed.Check the max_time textbox.");
                    label11.Text = "Character values are not allowed.Check the max_time textbox.";
                    return;
                }
                string str = "-";

                if (str == ToleranceBox.Text[0].ToString() || ToleranceBox.Text.ToString() == "0")
                {
                    MessageBox.Show("Tolerance can`t be less than zero or equal it");
                    label11.Text = "Tolerance can`t be less than zero or equal it ";
                    return;
                }
                if (max_time <= 0)
                {
                    MessageBox.Show("Time limit can`t be less or equal than zero");
                    label11.Text = "Time limit can`t be less or equal than zero";
                    return;
                }
                if (funcBox.Text == "$" || funcBox.Text == "#" || funcBox.Text == "@" || funcBox.Text == "&" || funcBox.Text == "$" || funcBox.Text == "`" || funcBox.Text == "?" || funcBox.Text == ";" || funcBox.Text == ":")
                {
                    MessageBox.Show("Function incorrect, it doesn`t match to mathematical representation of the function.You are probably added wrong values or incorrect signs like #,@,$,& and etc.Please check the textbox of functions.");
                    return;
                }

                GetFunction(func, Convert.ToDecimal(intervalBegin));

                try
                {
                    GetFunction(func, Convert.ToDecimal(intervalEnd));
                }
                catch (Exception h)
                {
                    MessageBox.Show("Function incorrect.Please check the textbox of functions");
                    label11.Text = "Function incorrect";
                    return;
                }

                progressBar.Value = 0;




                if (Math.Sign(Convert.ToDecimal(GetFunction(func, intervalBegin))) * Math.Sign(Convert.ToDecimal(GetFunction(func, intervalEnd))) > 0.0M)
                {
                    MessageBox.Show("Function has same signs at ends of interval.Check endpoints of the interval [a, b]!");
                    label11.Text = "Function has same signs at ends of interval.Check endpoints of the interval [a, b]!";
                    return;
                }
                else
                {
                    BisectionMethodClass b = new BisectionMethodClass();
                    b.increment_max += UpdateIteration;
                    b.time_max += UpdateTime;
                    b.start(intervalBegin, intervalEnd, precision, k_max, init, max_time, func, progressBar);


                    stopWatch.Reset();

                    ResultPrint(b.middle, GetFunction(func, b.middle), b.elapsedTime1, b.k_max_begin, b.time_max_begin, b.init1, Math.Abs(b.intervalEnd1 - b.intervalBegin1), b.b3);
                    init = 0;

                }
            }
            else
            {
                MessageBox.Show("Function incorrect.Please check the textbox of functions");
                label11.Text = "Function incorrect";
            }
        }
        public void ResultPrint(decimal x1, decimal f1, string relError, int k_max_begin, double time_max_begin, int iter_value, decimal rel, string b1)
        {
            BisectionMethodClass b = new BisectionMethodClass();
            SolutionOfTaskBox.Text = x1.ToString();
            ValueOfFunctionBox.Text = f1.ToString();
            AmountOfIteratinosBox.Text = iter_value.ToString();
            BapsedTimeBox.Text = relError.ToString();
            AbsBox.Text = rel.ToString("0e0");
            K_maxBox.Text = k_max_begin.ToString();
            timeBox.Text = time_max_begin.ToString();
            label11.Text = b1.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SolutionOfTaskBox.Text = "";
            ValueOfFunctionBox.Text = "";
            BapsedTimeBox.Text = "";
            AmountOfIteratinosBox.Text = "";
            AbsBox.Text = "";
            label11.Text = "";
        }

        private decimal GetFunction(string function, decimal x1)
        {
            Computer comp = new Computer();
            return comp.Compute(function, x1);
        }
        private void UpdateIteration(int iteration)
        {
            Action action = () =>
            {
                K_maxBox.Text = iteration.ToString();
            };
            Invoke(action);
        }

        private void UpdateTime(int iteration)
        {
            Action action = () =>
            {
                timeBox.Text = iteration.ToString();
            };
            Invoke(action);
        }
    
    }
}

