using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace LABA
{
    public class Data
    {
        public const int ROWCOUNT = 200;
        public const int COLUMNCOUNT = 200;

        public Cell[,] DataTable { get; set; } = new Cell[ROWCOUNT, COLUMNCOUNT];

        public bool isExpressionMode { get; set; } = true;

        public Data()
        {
            for (int row = 0; row < ROWCOUNT; row++)
            {
                for (int column = 0; column < COLUMNCOUNT; column++)
                {
                    DataTable[row, column] = new Cell();
                    DataTable[row, column].Expression = "";
                    DataTable[row, column].Value = 0;
                    DataTable[row, column].IsEmpty = true;
                }
            }
        }

        public void ClearCell(int row, int column)
        {
            foreach(IntPair cell in DataTable[row, column].Depend)
            {
                if(!DataTable[cell.Row, cell.Column].IsEmpty)
                {
                    DataTable[cell.Row, cell.Column].Expression = "";
                    DataTable[cell.Row, cell.Column].Value = 0;
                    DataTable[cell.Row, cell.Column].IsEmpty = true;
                    ClearCell(cell.Row, cell.Column);
                }
            }
        }

        public void AddColumn(int currentColumn)
        {
            for(int column = COLUMNCOUNT - 1; column > currentColumn; column--)
            {
                for(int row = 0; row < ROWCOUNT; row++)
                {
                    DataTable[row, column].Expression = DataTable[row, column - 1].Expression;
                    DataTable[row, column].Value = DataTable[row, column - 1].Value;
                    DataTable[row, column].IsEmpty = DataTable[row, column - 1].IsEmpty;
                }
            }
            for(int row = 0; row < ROWCOUNT; row++)
            {
                DataTable[row, currentColumn].Expression = "";
                DataTable[row, currentColumn].Value = 0;
                DataTable[row, currentColumn].IsEmpty = true;
            }
        }

        public void DeleteColumn(int currentColumn)
        {
            for(int row = 0; row < ROWCOUNT; row++)
            {
                if(!DataTable[row, currentColumn].IsEmpty)
                {
                    ClearCell(row, currentColumn);
                }
            }
            for (int column = currentColumn; column < COLUMNCOUNT - 1; column++)
            {
                for (int row = 0; row < ROWCOUNT; row++)
                {
                    DataTable[row, column].Expression = DataTable[row, column + 1].Expression;
                    DataTable[row, column].Value = DataTable[row, column + 1].Value;
                    DataTable[row, column].IsEmpty = DataTable[row, column + 1].IsEmpty;
                }
            }
            for (int row = 0; row < ROWCOUNT; row++)
            {
                DataTable[row, COLUMNCOUNT - 1].Expression = "";
                DataTable[row, COLUMNCOUNT - 1].Value = 0;
                DataTable[row, COLUMNCOUNT - 1].IsEmpty = true;
            }
        }

        public void AddRow(int currentRow)
        {
            for (int row = ROWCOUNT - 1; row > currentRow; row--)
            {
                for (int column = 0; column < COLUMNCOUNT; column++)
                {
                    DataTable[row, column].Expression = DataTable[row - 1, column].Expression;
                    DataTable[row, column].Value = DataTable[row - 1, column].Value;
                    DataTable[row, column].IsEmpty = DataTable[row - 1, column].IsEmpty;
                }
            }
            for (int column = 0; column < COLUMNCOUNT; column++)
            {
                DataTable[currentRow, column].Expression = "";
                DataTable[currentRow, column].Value = 0;
                DataTable[currentRow, column].IsEmpty = true;
            }
        }

        public void DeleteRow(int currentRow)
        {
            for(int column = 0; column < COLUMNCOUNT; column++)
            {
                if(!DataTable[currentRow, column].IsEmpty)
                {
                    ClearCell(currentRow, column);
                }
            }
            for(int row = currentRow; row < ROWCOUNT - 1; row++)
            {
                for(int column = 0; column < COLUMNCOUNT; column++)
                {
                    DataTable[row, column].Expression = DataTable[row + 1, column].Expression;
                    DataTable[row, column].Value = DataTable[row + 1, column].Value;
                    DataTable[row, column].IsEmpty = DataTable[row + 1, column].IsEmpty;
                }
            }
            for(int column = 0; column < COLUMNCOUNT; column++)
            {
                DataTable[ROWCOUNT - 1, column].Expression = "";
                DataTable[ROWCOUNT - 1, column].Value = 0;
                DataTable[ROWCOUNT - 1, column].IsEmpty = true;
            }
        }

        public String GetColumnName(int index)
        {
            String res = "";
            while(index >= 0)
            {
                res += ((char)(index % 26 + 65)).ToString();
                index /= 26;
                index--;
            }
            char[] arr = res.ToCharArray();
            Array.Reverse(arr);
            return new String(arr);
        }

        public void DataTableView(DataGridView dataGridView)
        {
            if (isExpressionMode)
            {
                for (int row = 0; row < ROWCOUNT; row++)
                {
                    for(int column = 0; column < COLUMNCOUNT; column++)
                    {
                        if (!DataTable[row, column].IsEmpty)
                            dataGridView.Rows[row].Cells[column].Value = DataTable[row, column].Expression;
                        else
                            dataGridView.Rows[row].Cells[column].Value = String.Empty;
                    }
                }
            }
            else
            {
                for (int row = 0; row < ROWCOUNT; row++)
                {
                    for (int column = 0; column < COLUMNCOUNT; column++)
                    {
                        if (!DataTable[row, column].IsEmpty)
                            dataGridView.Rows[row].Cells[column].Value = DataTable[row, column].Value;
                        else
                            dataGridView.Rows[row].Cells[column].Value = String.Empty;
                    }
                }
            }
            //dataGridView.AutoResizeColumns();
        }

        public void ClearTable()
        {
            for(int row = 0; row < ROWCOUNT; row++)
            {
                for(int column = 0; column < COLUMNCOUNT; column++)
                {
                    DataTable[row, column].Expression = "";
                    DataTable[row, column].Value = 0;
                    DataTable[row, column].IsEmpty = true;
                }
            }
        }

        public void SaveFile(string path)
        {
            StreamWriter streamWriter = new StreamWriter(path, false);
            for(int row = 0; row < ROWCOUNT; row++)
            {
                for(int column = 0; column < COLUMNCOUNT; column++)
                {
                    if(!DataTable[row, column].IsEmpty)
                    {
                        streamWriter.WriteLine(row.ToString() + " $ " + column.ToString() + " $ " 
                            + DataTable[row, column].Expression);
                    }
                }
            }
            streamWriter.Close();

        } 

        public void ReadFromFile(String path)
        {
            StreamReader streamReader = new StreamReader(path);
            string currentRow = streamReader.ReadLine();

            this.ClearTable();

            while(currentRow != null)
            {
                string[] spliting = currentRow.Split('$');
                int ourRow = Int32.Parse(spliting[0]);
                int ourColumn = Int32.Parse(spliting[1]);
                DataTable[ourRow, ourColumn].IsEmpty = false;
                DataTable[ourRow, ourColumn].Expression = spliting[2];

                currentRow = streamReader.ReadLine();
            }

        }

    }
}
