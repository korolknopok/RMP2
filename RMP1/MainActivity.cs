using Android.App;
using Android.Media.Metrics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.Button;
using System;

namespace RMP1
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        /// <summary>
        /// Результат вычислений.
        /// </summary>
        protected TextView userResult;

        /// <summary>
        /// Складываемые числа.
        /// </summary>
        private string[] numbers = new string[2];

        /// <summary>
        /// Оператор.
        /// </summary>
        private string _operator;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            userResult = FindViewById<TextView>(Resource.Id.TextViewInput);

        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        /// <summary>
        /// Обработка нажатия на кнопку.
        /// </summary>
        /// <param name="v"></param>
        [Java.Interop.Export("ButtonClick")]
        public void ButtonClick(View v)
        {
            Button button = (Button)v;

            if ("012345678900000.".Contains(button.Text))
                AddNumber(button.Text);
            else if ("+-/Xmoddiv√%".Contains(button.Text))
                AddOperator(button.Text);
           
            else if ("=" == button.Text)
                Calculate();
            else
                DefaultView();
        }



        /// <summary>
        /// Добавляет число в TextView.
        /// </summary>
        /// <param name="value">Добавляемое число.</param>
        private void AddNumber(string value)
        {
            int index = _operator == null ? 0 : 1;
            numbers[index] += value;

            UpdateCalculatorText();
        }

        /// <summary>
        /// Добавляет оператор в TextView/
        /// </summary>
        /// <param name="value">Оператор.</param>
        private void AddOperator(string value)
        {
            if (numbers[1] != null)
            {
                Calculate(value);
                return;
            }

            _operator = value;
            UpdateCalculatorText();
        }

        /// <summary>
        /// Производит вычисления.
        /// </summary>
        /// <param name="newOperator">Оператор.</param>
        private void Calculate(string newOperator = null)
        {
            double? result = null;
            double? first = numbers[0] == null ? null : (double?)double.Parse(numbers[0]);
            double? second = numbers[1] == null ? null : (double?)double.Parse(numbers[1]);

            switch (_operator)
            {
                case "/":
                    result = first / second;
                    break;
                case "X":
                    result = first * second;
                    break;
                case "+":
                    result = first + second;
                    break;
                case "-":
                    result = first - second;
                    break;
                case "mod":
                    result = first % second;
                    break;
                case "div":
                    result = Convert.ToInt32(first / second);
                    break;
                case "√":
                    result = Math.Sqrt((double)first);
                    break;
                case "%":
                    result = first / second * 100;
                    break;

            }
            if (result != null)
            {
                numbers[0] = result.ToString();
                _operator = newOperator;
                numbers[1] = null;
                UpdateCalculatorText();
            }
        }

        /// <summary>
        /// Возврат в начальную позицию
        /// </summary>
        private void DefaultView()
        {
            numbers[0] = null;
            numbers[1] = null;
            _operator = null;
            UpdateCalculatorText();
        }
        /// <summary>
        /// Обновляет TextView.
        /// </summary>
        private void UpdateCalculatorText()
        {
            userResult.Text = $"{numbers[0]} {_operator} {numbers[1]}";
        }
    }
}
