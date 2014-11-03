using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SmallFractCalc
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            button1.Hide();
        }

        
        private void evaluateCalculation()
        {
            //confirm all fields entered.
            if (!allFieldsEntered())
            {
                return;
            }
            //confirm entries are valid.
            if (!allFieldsValid())
            {
                return;
            }

            //begin processing.
            double v1;
            double v2;
            double answer;
            v1 = FractionToDouble(textBox1.Text);
            v2 = FractionToDouble(textBox2.Text);
            answer = operate(v1, v2, comboBox1.Text);

            //display result
            if (isFraction(textBox1.Text) || isFraction(textBox2.Text))
            {
                label2.Text = DoubleToFraction(answer);
                button1.Text = "ToDec";
            }
            else
            {
                button1.Text = "ToFrc";
                label2.Text = Convert.ToString(answer);
            }
            

            button1.Show();
        }

        private bool isFraction(string text)
        {
            if (text.IndexOf("/") >= 0)
            {
                return true;
            }
            return false;
        }
        private bool allFieldsValid()
        {
            if (!comboBox1.Text.Equals("+") && !comboBox1.Text.Equals("-") && !comboBox1.Text.Equals("x") && !comboBox1.Text.Equals("/"))
            {
                return false;
            }

            if (!isFractionToDouble(textBox1.Text) || !isFractionToDouble(textBox2.Text))
            {
                return false;
            }

            return true;
        }

        private bool allFieldsEntered()
        {
            if (textBox1.Text.Equals(""))
            {
                return false;
            }
            if (textBox2.Text.Equals(""))
            {
                return false;
            }
            if (comboBox1.Text.Equals(""))
            {
                return false;
            }

            return true;
        }

        public double FractionToDouble(string fraction)
        {
            double result;

            if (double.TryParse(fraction, out result))
            {
                return result;
            }

            string[] split = fraction.Split(new char[] { ' ', '/' });

            if (split.Length == 2 || split.Length == 3)
            {
                int a, b;

                if (int.TryParse(split[0], out a) && int.TryParse(split[1], out b))
                {
                    if (split.Length == 2)
                    {
                        return (double)a / b;
                    }

                    int c;

                    if (int.TryParse(split[2], out c))
                    {
                        return a + (double)b / c;
                    }
                }
            }

            return 0;
        }

        public bool isFractionToDouble(string fraction)
        {
            double result;

            if (double.TryParse(fraction, out result))
            {
                return true;
            }

            string[] split = fraction.Split(new char[] { ' ', '/' });

            if (split.Length == 2 || split.Length == 3)
            {
                int a, b;

                if (int.TryParse(split[0], out a) && int.TryParse(split[1], out b))
                {
                    if (split.Length == 2)
                    {
                        return true;
                    }

                    int c;

                    if (int.TryParse(split[2], out c))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static string DoubleToFraction(double num, double epsilon = 0.0001, int maxIterations = 20)
        {

            //check for decimal point
            if (Convert.ToString(num).IndexOf('.') < 0)
            {
                return Convert.ToString(num);
            }

            double[] d = new double[maxIterations + 2];
            d[1] = 1;
            double z = num;
            double n = 1;
            int t = 1;

            int wholeNumberPart = (int)num;
            double decimalNumberPart = num - Convert.ToDouble(wholeNumberPart);

            while (t < maxIterations && Math.Abs(n / d[t] - num) > epsilon)
            {
                t++;
                z = 1 / (z - (int)z);
                d[t] = d[t - 1] * (int)z + d[t - 2];
                n = (int)(decimalNumberPart * d[t] + 0.5);
            }

            return string.Format((wholeNumberPart > 0 ? wholeNumberPart.ToString() + " " : "") + "{0}/{1}",
                                 n.ToString(),
                                 d[t].ToString()
                                );
        }


        private double operate(double v1, double v2, string holdOp)
        {
            double result;
            switch (holdOp)
            {
                case "+":
                    result = addNumbers(v1, v2);
                    break;
                case "-":
                    result = subNumbers(v1, v2);
                    break;
                case "*":
                    result = mulNumbers(v1, v2);
                    break;
                case "/":
                    result = divNumbers(v1, v2);
                    break;
                default:
                    result = 0;
                    break;
            }

            return result;

        }
    
        /* add two numbers */
        public double addNumbers(double v1, double v2)
        {
            return v1 + v2;
        }
        /* subtract two numbers */
        public double subNumbers(double v1, double v2)
        {
            return v1 - v2;
        }
        /* divide two numbers */
        public double divNumbers(double v1, double v2)
        {
            return v1 / v2;
        }
        /* multiply two numbers */
        public double mulNumbers(double v1, double v2)
        {
            return v1 * v2;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text.Equals("ToDec"))
            {
                button1.Text = "ToFrc";
                label2.Text = Convert.ToString(FractionToDouble(label2.Text));
            }
            else if (button1.Text.Equals("ToFrc"))
            {
                button1.Text = "ToDec";
                label2.Text = DoubleToFraction(Convert.ToDouble(label2.Text));

            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            evaluateCalculation();
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            evaluateCalculation();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                evaluateCalculation();
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                evaluateCalculation();
            }
        }

    }
}
