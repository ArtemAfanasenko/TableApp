using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TableApp.Interfaces;
using TableApp.Other;
using TableApp.Services;

namespace TableApp.ViewModels
{
    public class TableViewModel : BaseViewModel
    { 
        public TableViewModel()
        {
            GenerationCommand = new RelayCommand(obj =>
            {
                GenerationTable();
            });

            CalculateCommand = new RelayCommand(obj =>
            {
                TableValueToArray();
                UpdateTable();
            });
           
        }

        private void GenerationTable()
        {
            var f = new DataTable();

            for (int i = 0; i <= _column; i++)
            {
                var column = f.Columns.Add();
                if (i == 0)
                {
                    column.ColumnName = i.ToString();
                    column.ReadOnly = true;
                }
                else
                {
                    var name = TextConverter.NumberToText(i);
                    column.ColumnName = name;
                }
            }

            for (int j = 0; j < _row; j++)
            {
                f.Rows.Add((j + 1).ToString());
            }
            Dt = f.DefaultView;
        }


        private void TableValueToArray()
        {
            _cells = new string[_column][];
            for (int i = 0; i < _column; i++)
            {
                _cells[i] = new string[_row];
                for (int j = 0; j < _row; j++)
                {
                    _cells[i][j] = Dt.Table.Select()[i].Table.Rows[i].ItemArray[j + 1].ToString();
                }
            }
        }

        private void UpdateTable()
        {
            ITextConverter textConverter = new TextConverter(Cells, Column, Row);

            for (int i = 0; i < _cells.Length; i++)
            {
                for (int j = 0; j < _cells[i].Length; j++)
                {
                    _cells[i][j] = textConverter.Check(_cells[i][j]);
                }
            }

            for (int i = 0; i < _column; i++)
            {
                for (int j = 0; j < _row; j++)
                {
                    Dt[i].Row[j + 1] = _cells[i][j];
                }
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

        private DataView _dt;
        public DataView Dt
        {
            get { return _dt; }
            set
            {
                if (_dt == value) return;
                _dt = value;
                OnPropertyChanged(nameof(Dt));
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

        #region Commands
        public RelayCommand GenerationCommand { get; private set; }
        public RelayCommand CalculateCommand { get; private set; }
        #endregion
    }
}
