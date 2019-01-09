using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace LABA
{
    public partial class MainForm : Form
    {
        Data data = new Data();
        Calculator calculator = new Calculator();
        bool HasChanged = false;

        public MainForm()
        {
            InitializeComponent();
            drawTable();
            this.FormClosing += MainForm_FormClosing;
        }


        private void drawTable()
        {
            dataGridView.ColumnCount = Data.COLUMNCOUNT;
            dataGridView.ColumnHeadersVisible = true;
            for(int index = 0; index < Data.COLUMNCOUNT; index++)
            {
                dataGridView.Columns[index].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView.Columns[index].Name = data.GetColumnName(index);
            }

            for (int index = 0; index < Data.ROWCOUNT; index++)
            {
                dataGridView.Rows.Add();
                dataGridView.Rows[index].HeaderCell.Value = (index + 1).ToString();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            comboBox.SelectedIndex = 0;
            data.isExpressionMode = true;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (HasChanged == false)
            {
                DialogResult dialog = MessageBox.Show(
                 "Ви дійсно хочете вийти з програми?",
                 "Завершення програми",
                 MessageBoxButtons.YesNo,
                 MessageBoxIcon.Warning
                );
                if (dialog == DialogResult.Yes)
                {
                    e.Cancel = false;
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else
            {
                DialogResult dialog = MessageBox.Show(
                 "Файл не збережений! Ви хочете вийти та зберегти?",
                 "Завершення програми",
                 MessageBoxButtons.YesNoCancel,
                 MessageBoxIcon.Warning
                );
                if (dialog == DialogResult.Yes)
                {
                    SaveFileDialog1.FileName = "myText";
                    SaveFileDialog1.DefaultExt = "txt";
                    SaveFileDialog1.Filter = "Text files (*.txt)|*.txt";
                    SaveFileDialog1.AddExtension = true;
                    if (SaveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        string path;
                        path = SaveFileDialog1.FileName;
                        data.SaveFile(path);
                        MessageBox.Show("Файл збережений!");
                        e.Cancel = false;
                    }
                    else
                        e.Cancel = true;
                }
                else if (dialog == DialogResult.No)
                {
                    e.Cancel = false;
                }
                else
                    e.Cancel = true;
            }
        }

        private void файлToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripComboBox1_Click_1(object sender, EventArgs e)
        {

        }

        private void toolStripComboBox_Click(object sender, EventArgs e)
        {

        }

        private void calculateButton_Click(object sender, EventArgs e)
        {
            int column = dataGridView.CurrentCell.ColumnIndex;
            int row = dataGridView.CurrentCell.RowIndex;
            string lastExp = null;
            if (!data.DataTable[row, column].IsEmpty)
                lastExp = data.DataTable[row, column].Expression;
            String buffer = expressionTextBox.Text;
            calculator.SetCell(data, row, column);
            calculator.AnylizeExpression(buffer);
            if(!data.DataTable[row, column].IsEmpty)
            {
                HasChanged = true;
                if(checkAll(row, column))
                    recalculateCell(row, column);
                else
                {
                    calculator.SetCell(data, row, column);
                    calculator.AnylizeExpression(lastExp);
                }
            }
            data.DataTableView(dataGridView);
        }

        private bool checkAll(int row, int column)
        {
            for (int index = 0; index < data.DataTable[row, column].Depend.Count; index++)
            {
                IntPair cell = data.DataTable[row, column].Depend[index];
                calculator.SetCell(data, cell.Row, cell.Column);
                if (!calculator.Anylize(data.DataTable[cell.Row, cell.Column].Expression))
                    return false;
                else
                {
                    return checkAll(cell.Row, cell.Column);
                }
            }
            return true;
        }

        private void recalculateCell(int row, int column)
        {
            for (int index = 0; index < data.DataTable[row, column].Depend.Count; index++)
            {
                IntPair cell = data.DataTable[row, column].Depend[index];
                calculator.SetCell(data, cell.Row, cell.Column);
                calculator.AnylizeExpression(data.DataTable[cell.Row, cell.Column].Expression);
                recalculateCell(cell.Row, cell.Column);
            }
        }

        private void відкритиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = openFileDialog1.FileName;
            data.ReadFromFile(filename);
            recalculateData();
            data.DataTableView(dataGridView);
            MessageBox.Show("Файл відкритий!");
        }

        private void recalculateData()
        {
            for(int row = 0; row < Data.ROWCOUNT; row++)
            {
                for(int column = 0; column < Data.COLUMNCOUNT; column++)
                {
                    if(!data.DataTable[row, column].IsEmpty)
                    {
                        calculator.SetCell(data, row, column);
                        calculator.AnylizeExpression(data.DataTable[row, column].Expression);
                    }
                }
            }
        }

        private void зберегтиToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SaveFileDialog1.FileName = "myText";
            SaveFileDialog1.DefaultExt = "txt";
            SaveFileDialog1.Filter = "Text files (*.txt)|*.txt";
            SaveFileDialog1.AddExtension = true;
            if (SaveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string path;
                path = SaveFileDialog1.FileName;
                data.SaveFile(path);
                MessageBox.Show("Файл збережений!");
                HasChanged = false;
            }
        }

        private void expressionTextBox_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void AddColumn_Click(object sender, EventArgs e)
        {
            int column = dataGridView.CurrentCell.ColumnIndex;
            DialogResult dialog = MessageBox.Show(
                "Ви дійсно хочете добавити стовбець?",
                "Добавлення стовбця",
                MessageBoxButtons.YesNo
                );
            if (dialog == DialogResult.Yes)
            {
                data.AddColumn(column);
                data.DataTableView(dataGridView);
                HasChanged = true;
            }
        }

        private void AddRow_Click(object sender, EventArgs e)
        {
            int row = dataGridView.CurrentCell.RowIndex;
            DialogResult dialog = MessageBox.Show(
                "Ви дійсно хочете добавити рядок?",
                "Добавлення рядка",
                MessageBoxButtons.YesNo
                );
            if (dialog == DialogResult.Yes)
            {
                data.AddRow(row);
                data.DataTableView(dataGridView);
                HasChanged = true;
            }
        }

        private void DeleteColumn_Click(object sender, EventArgs e)
        {
            int column = dataGridView.CurrentCell.ColumnIndex;
            DialogResult dialog = MessageBox.Show(
                "Ви дійсно хочете видалити стовбець?",
                "Видалення стовбця",
                MessageBoxButtons.YesNo
                );
            if (dialog == DialogResult.Yes)
            {

                data.DeleteColumn(column);
                data.DataTableView(dataGridView);
                HasChanged = true;
            }
        }

        private void DeleteRow_Click(object sender, EventArgs e)
        {
            int row = dataGridView.CurrentCell.RowIndex;
            DialogResult dialog = MessageBox.Show(
                "Ви дійсно хочете видалити рядок?",
                "Видалення рядка",
                MessageBoxButtons.YesNo
                );
            if (dialog == DialogResult.Yes)
            {
                data.DeleteRow(row);
                data.DataTableView(dataGridView);
                HasChanged = true;
            }
        }

        private void ClearTable_Click(object sender, EventArgs e)
        {
            if (HasChanged == false)
            {
                DialogResult dialog = MessageBox.Show(
                    "Ви дійсно хочете очистити таблицю?",
                    "Очищення таблиці",
                    MessageBoxButtons.YesNo
                    );
                if(dialog == DialogResult.Yes)
                {
                    data.ClearTable();
                    data.DataTableView(dataGridView);
                }
            }
            else
            {
                DialogResult dialog = MessageBox.Show(
                 "Файл не збережений! Ви хочете очистити та зберегти?",
                 "Очищення таблиці",
                 MessageBoxButtons.YesNoCancel,
                 MessageBoxIcon.Warning
                );
                if (dialog == DialogResult.Yes)
                {
                    SaveFileDialog1.FileName = "myText";
                    SaveFileDialog1.DefaultExt = "txt";
                    SaveFileDialog1.Filter = "Text files (*.txt)|*.txt";
                    SaveFileDialog1.AddExtension = true;
                    if (SaveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        string path;
                        path = SaveFileDialog1.FileName;
                        data.SaveFile(path);
                        MessageBox.Show("Файл збережений!");
                        data.ClearTable();
                        data.DataTableView(dataGridView);
                        HasChanged = false;
                    }
                }
                else if (dialog == DialogResult.No)
                {
                    data.ClearTable();
                    data.DataTableView(dataGridView);
                    HasChanged = false;
                }
            }
        }

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox.SelectedIndex == 0)
            {
                data.isExpressionMode = true;
            }
            else
            {
                data.isExpressionMode = false;
            }
            data.DataTableView(dataGridView);
        }
    }
}
