using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace FractionCalculator
{
    public partial class FractionCalculator : Form
    {
        private String[] inputParams;

        public FractionCalculator()
        {
             
            InitializeComponent();

            button2.Hide();
            setStatus("Waiting for input");  
        }

        private void button1_Click(object sender, EventArgs e)
        {
            calculateTotal();
        }
            
    

        private void calculateTotal()
        {
            /*guard clauses*/
            if (textBox1.Text.Equals(""))
            {
                setStatus("empty input");
            }

            inputParams = textBox1.Text.Split(' ');

            double answer = 0;
            double tokenConverted;
            bool lookForOp;

            String[] convertedParams = new String[inputParams.Length];
            int counter = 0;

            //used to ensure we have a number-op-number format. 
            lookForOp = false;

            /*validate input*/
            foreach (string token in inputParams)
            {

                //check if mixed fraction
                if ((token.Split('|').Length - 1) == 2)
                {
                    convertedParams[counter] = Convert.ToString(convertFrToDe(token));
                    lookForOp = true;
                    counter++;
                }
                //check if decimal / number
                else if (double.TryParse(token, out tokenConverted))
                {
                    convertedParams[counter] = token;
                    lookForOp = true;
                    counter++;
                }
                //checked if operator
                else if (token.IndexOf("+") == 0 || token.IndexOf("-") == 0 || token.IndexOf("*") == 0 || token.IndexOf("/") == 0)
                {
                    convertedParams[counter] = token;
                    counter++;
                    if (!lookForOp)
                    {
                        setStatus("Mismatched number/operators");
                        break;
                    }
                    else
                    {
                        //not looking for an op anymore
                        lookForOp = false;
                    }
                }
                else if (!token.Trim().Equals(""))
                {
                    setStatus("invalid character in token " + token);
                    break;
                }
            }


            /*at this point, we should have ended at a number, so confirm that. */
            if (!lookForOp)
            {
                setStatus("Missing last number.");
                return;
            }

            /* reorder calculation */
            /*  LinkedList<string> operatorOrder = new LinkedList<string>();
              LinkedList<double> valueOrder = new LinkedList<double>();

              foreach (string arg in convertedParams)
              {
                
              }*/

            answer = Convert.ToDouble(convertedParams[0]);
            lookForOp = true;
            string holdOp = "+";

            /* calculate answer */
            for (int i = 1; i < convertedParams.Length; i++)
            {

                if (lookForOp)
                {
                    lookForOp = false;
                    holdOp = convertedParams[i];
                }
                else
                {
                    lookForOp = true;
                    answer = operate(answer, Convert.ToDouble(convertedParams[i]), holdOp);
                }
            }

            //show answer
            label2.Text = Convert.ToString(answer);

            //Show conversion button
            button2.Show();
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

        public void setStatus(string text)
        {
            label3.Text = "Status: " + text;
        }

        /* convert mixed fraction string to decimal 
         assumes only two pipes.
         assumes input validated.
         */
        public double convertFrToDe(string fraction)
        {
            double converted;
            string[] parts = fraction.Split('|');

            converted = Convert.ToDouble(parts[0]) + Convert.ToDouble(parts[1]) / Convert.ToDouble(parts[2]);
            
            return converted;
        }
        
        /* convert decimal to mixed fraction */
        public string convertDeToFr(double num)
        {
            string fractionForm = DoubleToFraction(num);

            return fractionForm;
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

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                calculateTotal();
            }
        }

        public static string DoubleToFraction(double num, double epsilon = 0.0001, int maxIterations = 20)
        {
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

            return string.Format((wholeNumberPart > 0 ? wholeNumberPart.ToString() + "|" : "") + "{0}|{1}",
                                 n.ToString(),
                                 d[t].ToString()
                                );
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Text.Equals("ToDec"))
            {
                button2.Text = "ToFrc";
                label2.Text = Convert.ToString(convertFrToDe(label2.Text));
            }
            else if(button2.Text.Equals("ToFrc"))
            {
                button2.Text = "ToDec";
                label2.Text = convertDeToFr(Convert.ToDouble(label2.Text));

            }
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

            throw new FormatException("Not a valid fraction.");
        }
    }
}
