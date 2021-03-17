using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TableApp.Interfaces;
using TableApp.Other;

namespace TableApp.Services
{
    public class TextConverter : BaseViewModel, ITextConverter
    { 
        public TextConverter(string[][] cells, int column, int row)
        {
            Cells = cells;
            Column = column;
            Row = row;
        }

        public bool CheckTextToNumber(string text)
        {
            string t = text.Select(c => c - 'A' + 1).Aggregate((sum, next) => sum * 26 + next).ToString();
            foreach (char item in t)
            {
                if (!char.IsDigit(item))
                    return false;
            }
            return true;
        }

        public int TextToNumber(string text) => text.Select(c => c - 'A' + 1).Aggregate((sum, next) => sum * 26 + next);

        public static string NumberToText(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = String.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName;
        }

        public string Check(string inputString)
        {
            char secondCharacter = ' ';
            if (!string.IsNullOrEmpty(inputString))
            {
                char firstCharacter = inputString[0];
                if (inputString.Length>1)
                {
                    secondCharacter = inputString[1];
                }
                if (firstCharacter == '=')
                {
                    if (secondCharacter == '=')
                    {
                        return "";
                    }
                    return ArithmeticOperation(inputString);
                }
                else if (firstCharacter == '\'')
                {
                    return inputString.Substring(1);
                }
                else
                {
                    return inputString;
                }
            }
            else
            {
                return inputString;
            }
        }

        public string ArithmeticOperation(string inputString)
        {
            var t = inputString.Substring(1);
            string firstPart = "";
            string secondPart = "";
            string thirdPart = "";
            int indexForSign = 0;
            double[] arrayValue;

            for (int i = 0; i < t.Length; i++)
            {
                if (t[i] == '+' || t[i] == '-' || t[i] == '*' || t[i] == '/')
                {
                    indexForSign++;
                }
            }
            if (indexForSign > 0)
            {
                arrayValue = new double[indexForSign + 1];
            }
            else
            {
                arrayValue = new double[1];
            }

            char[] arraySign = new char[indexForSign];

            int index = 0;

            for (int i = 0; i < t.Length; i++)
            {
                if (char.IsLetterOrDigit(t[i]))
                {
                    if (char.IsDigit(t[i]))
                    {
                        firstPart += t[i];
                    }
                    else if (char.IsLetter(t[i]))
                    {
                        secondPart += t[i];
                    }
                }
                else if (indexForSign > 0 && t[i] == '+' || t[i] == '-' || t[i] == '*' || t[i] == '/')
                {
                    if (!string.IsNullOrEmpty(firstPart))
                    {
                        thirdPart += t[i];
                        var coloumn = Convert.ToInt32(firstPart) - 1;

                        if (secondPart == "")
                        {
                            arrayValue[index] = Convert.ToDouble(firstPart);
                        }
                        else
                        {
                            if (CheckTextToNumber(secondPart))
                            {
                                var row = TextToNumber(secondPart.ToUpper()) - 1;
                                if (!string.IsNullOrEmpty(_cells[coloumn][row]))
                                {
                                    arrayValue[index] = Convert.ToDouble(_cells[coloumn][row]);
                                }
                                else
                                {
                                    MessageBox.Show("Неверные данные! Повторите попытку!");
                                    arrayValue = new double[] { };
                                }
                            }
                        }
                        arraySign[index] = thirdPart[0];
                        firstPart = "";
                        secondPart = "";
                        thirdPart = "";
                        index++;
                    }
                    else
                    {
                        MessageBox.Show("Неверные данные! Повторите попытку");
                        arrayValue = new double[] { };
                    }
                }

            }

            if (indexForSign == index)
            {
                if (!string.IsNullOrEmpty(firstPart))
                {
                    var coloumn = Convert.ToInt32(firstPart) - 1;

                    if (secondPart == "")
                    {
                        arrayValue[index] = Convert.ToDouble(firstPart);
                    }
                    else
                    {
                        if (CheckTextToNumber(secondPart))
                        {
                            var row = TextToNumber(secondPart.ToUpper()) - 1;
                            if (!string.IsNullOrEmpty(_cells[coloumn][row]))
                            {
                                arrayValue[index] = Convert.ToDouble(_cells[coloumn][row]);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Неверные данные! Повторите попытку!");
                            arrayValue = new double[] { };
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Неверные данные! Повторите попытку");
                    arrayValue = new double[] { };
                }

                if (arraySign.Length > 0 && arrayValue.Length > 0)
                {
                    for (int i = 0; i < arraySign.Length; i++)
                    {
                        if (arraySign[i] == '+')
                        {
                            arrayValue[i + 1] = (arrayValue[i] + arrayValue[i + 1]);
                        }
                        if (arraySign[i] == '-')
                        {
                            arrayValue[i + 1] = (arrayValue[i] - arrayValue[i + 1]);
                        }
                        if (arraySign[i] == '*')
                        {
                            arrayValue[i + 1] = (arrayValue[i] * arrayValue[i + 1]);
                        }
                        if (arraySign[i] == '/')
                        {
                            arrayValue[i + 1] = (arrayValue[i] / arrayValue[i + 1]);
                        }
                    }
                }
            }

            string cellValue = "";

            if (arrayValue.Length < 1)
            {
                return cellValue;
            }
            else
            {
                cellValue = arrayValue[indexForSign].ToString();
                return cellValue;
            }
        }

        private string[][] _cells;
        public string[][] Cells
        {
            get { return _cells; }
            set
            {
                if (_cells == value) return;
                _cells = value;
                OnPropertyChanged(nameof(Cells));
            }
        }

        private int _column;
        public int Column
        {
            get { return _column; }
            set
            {
                if (_column == value) return;
                _column = value;
                OnPropertyChanged(nameof(Column));
            }
        }

        private int _row;
        public int Row
        {
            get { return _row; }
            set
            {
                if (_row == value) return;
                _row = value;
                OnPropertyChanged(nameof(Row));
            }
        }
    }
}
