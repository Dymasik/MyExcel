using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LABA
{
    public class Cell
    {
        public String Expression { get; set; } = "";
        public double Value { get; set; } = 0;
        public bool IsEmpty { get; set; } = true;
        public List<IntPair> Depend = new List<IntPair>();
    }

    public class IntPair
    {
        public int Row;
        public int Column;
        public IntPair(int row, int column)
        {
            Row = row;
            Column = column;
        }
    }
}
