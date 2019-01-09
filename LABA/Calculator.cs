using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LABA
{
    class SyntaxisException : ArgumentException
    {
        public SyntaxisException()
            : base("Синтаксична помилка!")
        { }
    }

    class DivByZeroException : ArgumentException
    {
        public DivByZeroException()
            : base("Ділення на нуль!")
        { }
    }

    class WrongDg : ArgumentException
    {
        public WrongDg()
            : base("Неправильно введено число!")
        { }
    }

    class WrongRef : ArgumentException
    {
        public WrongRef()
            : base("Звернення до незрозумілого елементу чи пустої клітинки!")
        { }
    }

    class WrongCell : ArgumentException
    {
        public WrongCell()
            : base("Невірно введена назва клітинки!")
        { }
    }

    class Cycle : ArgumentException
    {
        public Cycle()
            : base("Присутній цикл, неможливо обчислити!")
        { }
    }

    public class Calculator
    {

        enum Type
        {
            None, Del, Num, Name
        };
        
        private Data data = new Data();
        string exp;
        bool AllIsGood = true;
        int Index;
        int currentRow;
        int currentColumn;
        int openBr, closeBr;
        string token;
        Type tokenType;
        List<string> depend = new List<string>();
        IntPair ourCell;
        double result;

        public void SetCell(Data data, int curRow, int curCol)
        {
            this.data = data;
            currentColumn = curCol;
            currentRow = curRow;
            ourCell = new IntPair(currentRow, currentColumn);
        }

        public double SetExp(string expstr)
        {
            exp = expstr;
            openBr = 0;
            closeBr = 0;
            //try
            //{
                if (!IsDigit(exp[exp.Length - 1]) && exp[exp.Length - 1] != ')')
                {
                    throw new SyntaxisException();
                }
                foreach (char symbol in exp)
                {
                    if (symbol == ')')
                        closeBr++;
                    if (symbol == '(')
                        openBr++;
                }
                if (openBr != closeBr)
                {
                    throw new SyntaxisException();
                }
                char[] delimetr = { '+', '-', ' ', '/', '*', '%', '|', '^', '>', '<', '(', ')' };
                string[] subExp = exp.Split(delimetr);
                for (int index = 0; index < subExp.Length; index++)
                {
                    if (subExp[index].Length <= 0)
                        continue;
                    if (IsDigit(subExp[index][0]))
                    {
                        for (int iIndex = 0; iIndex < subExp[index].Length; iIndex++)
                        {
                            if (!IsDigit(subExp[index][iIndex]) && subExp[index][iIndex] != ',')
                            {
                                throw new WrongDg();
                            }
                        }
                    }
                    else
                    {
                        int iIndex = 0, newColumn = 0;
                        string nameColumn = "";
                        while (iIndex < subExp[index].Length && !IsDigit(subExp[index][iIndex]))
                        {
                            if (iIndex > 0)
                                newColumn += 26;
                            newColumn += (int)(subExp[index][iIndex]) - (int)'A';
                            nameColumn = nameColumn + subExp[index][iIndex].ToString();
                            iIndex++;
                        }
                        if (iIndex == subExp[index].Length)
                        {
                            throw new WrongRef();
                        }
                        if (nameColumn.Length == 1)
                        {
                            if (nameColumn[0] < 'A' || nameColumn[0] > 'Z')
                            {
                                throw new WrongCell();
                            }
                        }
                        if (nameColumn.Length == 2)
                        {
                            if (nameColumn[0] < 'A' || nameColumn[0] > 'Z' || nameColumn[1] < 'A' || nameColumn[1] > 'Z')
                            {
                                throw new WrongCell();
                            }
                        }
                        if (nameColumn.Length >= 3)
                        {
                            throw new WrongCell();
                        }
                    }
                }
                Index = 0;
                GetToken();
                if (token == "")
                {
                    throw new SyntaxisException();
                }
                ExpEq(out result);
                return result;
            //}
            /*catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return 0.0;
            }*/
        }

        public bool Anylize(string expstr)
        {
            exp = expstr;
            AllIsGood = true;
            double result;
            Index = 0;
            GetToken();
            if (token == "")
            {
                return false;
            }
            ExpEq(out result);
            return AllIsGood;
        }


        public void AnylizeExpression(string expstr)
        {
            exp = expstr;
            openBr = 0;
            closeBr = 0;
            try
            {
                if (!IsDigit(exp[exp.Length - 1]) && exp[exp.Length - 1] != ')')
                {
                    AllIsGood = false;
                    throw new SyntaxisException();
                }
                foreach (char symbol in exp)
                {
                    if (symbol == ')')
                        closeBr++;
                    if (symbol == '(')
                        openBr++;
                }
                if (openBr != closeBr)
                {
                    AllIsGood = false;
                    throw new SyntaxisException();
                }
                AllIsGood = true;
                char[] delimetr = { '+', '-', ' ', '/', '*', '%', '|', '^', '>', '<', '(', ')' };
                string[] subExp = exp.Split(delimetr);
                for (int index = 0; index < subExp.Length; index++)
                {
                    if (subExp[index].Length <= 0)
                        continue;
                    if (IsDigit(subExp[index][0]))
                    {
                        for (int iIndex = 0; iIndex < subExp[index].Length; iIndex++)
                        {
                            if (!IsDigit(subExp[index][iIndex]) && subExp[index][iIndex] != ',')
                            {
                                throw new WrongDg();
                            }
                        }
                    }
                    else
                    {
                        int iIndex = 0, newColumn = 0;
                        string nameColumn = "";
                        while (iIndex < subExp[index].Length && !IsDigit(subExp[index][iIndex]))
                        {
                            if (iIndex > 0)
                                newColumn += 26;
                            newColumn += (int)(subExp[index][iIndex]) - (int)'A';
                            nameColumn = nameColumn + subExp[index][iIndex].ToString();
                            iIndex++;
                        }
                        if (iIndex == subExp[index].Length)
                        {
                            throw new WrongRef();
                        }
                        if (nameColumn.Length == 1)
                        {
                            if (nameColumn[0] < 'A' || nameColumn[0] > 'Z')
                            {
                                throw new WrongCell();
                            }
                        }
                        if (nameColumn.Length == 2)
                        {
                            if (nameColumn[0] < 'A' || nameColumn[0] > 'Z' || nameColumn[1] < 'A' || nameColumn[1] > 'Z')
                            {
                                throw new WrongCell();
                            }
                        }
                        if (nameColumn.Length >= 3)
                        {
                            throw new WrongCell();
                        }
                        string sRow = subExp[index].Substring(iIndex);
                        int newRow = Int32.Parse(sRow) - 1;
                        string cellName = nameColumn + sRow;
                        if (data.DataTable[newRow, newColumn].IsEmpty)
                        {
                            throw new WrongRef();
                        }
                        Index = 0;
                        GetToken();
                        if (token == "")
                        {
                            throw new SyntaxisException();
                        }
                        ExpEq(out result);
                    }
                }
                if (IsCycle(currentRow, currentColumn, currentRow, currentColumn))
                {
                    throw new Cycle();
                }

                Index = 0;
                GetToken();
                if (token == "")
                {
                    throw new SyntaxisException();
                }
                ExpEq(out result);
                if (AllIsGood)
                {
                    data.DataTable[currentRow, currentColumn].Expression = exp;
                    data.DataTable[currentRow, currentColumn].Value = result;
                    data.DataTable[currentRow, currentColumn].IsEmpty = false;
                    AddDependence(expstr);
                }
                else
                {
                    throw new SyntaxisException();
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        void AddDependence(string expstr)
        {
            char[] delimetr = { '+', '-', ' ', '/', '*', '%', '|', '^', '>', '<', '(', ')' };
            string[] subExp = expstr.Split(delimetr);
            for (int index = 0; index < subExp.Length; index++)
            {
                if (IsDigit(subExp[index][0]))
                {
                    continue;
                }
                else
                {
                    int iIndex = 0, newColumn = 0;
                    string nameColumn = "";
                    while (iIndex < subExp[index].Length && !IsDigit(subExp[index][iIndex]))
                    {
                        if (iIndex > 0)
                            newColumn += 26;
                        newColumn += (int)(subExp[index][iIndex]) - (int)'A';
                        nameColumn = nameColumn + subExp[index][iIndex].ToString();
                        iIndex++;
                    }
                    string sRow = subExp[index].Substring(iIndex);
                    int newRow = Int32.Parse(sRow) - 1;
                    bool contain = false;
                    foreach(IntPair cell in data.DataTable[newRow, newColumn].Depend)
                    {
                        if(cell.Row == ourCell.Row && cell.Column == ourCell.Column)
                        {
                            contain = true;
                            break;
                        }
                    }
                    if (!contain)
                    {
                        data.DataTable[newRow, newColumn].Depend.Add(ourCell);
                    }
                }
            }
        }

        void ExpEq(out double result)
        {
            string op;
            double partialResult;

            ExpFirst(out result);
            while ((op = token).Equals(">") || op.Equals("<"))
            {
                GetToken();
                ExpFirst(out partialResult);
                switch (op)
                {
                    case ">":
                        if (result > partialResult)
                            result = 1.0;
                        else
                            result = 0.0;
                        break;
                    case "<":
                        if (result > partialResult)
                            result = 0.0;
                        else
                            result = 1.0;
                        break;
                   
                }
            }
        }

        void ExpFirst(out double result)
        {
            string op;
            double partialResult;

            ExpSecond(out result);
            //MessageBox.Show(token);
            while ((op = token).Equals("+") || op.Equals("-"))
            {
                //MessageBox.Show(op);
                GetToken();
                ExpSecond(out partialResult);
                //MessageBox.Show(partialResult.ToString());
                switch (op)
                {
                    case "+":
                        result = result + partialResult;
                        break;
                    case "-":
                        result = result - partialResult;
                        break;

                }
            }
        }

        void ExpSecond(out double result)
        {
            string op;
            double partialResult = 0.0;

            ExpThird(out result);
            while ((op = token).Equals("/") || op.Equals("*") || op.Equals("%") || op.Equals("|"))
            {
                GetToken();
                ExpThird(out partialResult);
                try
                {
                    switch (op)
                    {
                        case "*":
                            result = result * partialResult;
                            break;
                        case "/":
                            if (partialResult == 0.0)
                            {
                                throw new DivByZeroException();
                            }
                            else
                                result = result / partialResult;
                            break;
                        case "|":
                            if (partialResult == 0.0)
                            {
                                throw new DivByZeroException();
                            }
                            else
                                result = (int)result / (int)partialResult;
                            break;
                        case "%":
                            if (partialResult == 0.0)
                            {
                                throw new DivByZeroException();
                            }
                            else
                                result = (int)result % (int)partialResult;
                            break;

                    }
                }
                catch(Exception e)
                {
                    AllIsGood = false;
                }
            }
        }

        void ExpThird(out double result)
        {
            double partialResult, ex;

            ExpUnar(out result);
            if (token.Equals("^"))
            {
                GetToken();
                ExpThird(out partialResult);
                ex = result;
                if(partialResult == 0.0)
                {
                    result = 1.0;
                    return;
                }
                result = Math.Pow(result, partialResult);
            }
        }

        void ExpUnar(out double result)
        {
            string op = "";
            if((tokenType == Type.Del) && token.Equals("+") || token.Equals("-"))
            {
                op = token;
                GetToken();
            }
            ExpBrecket(out result);
            if (op.Equals("-"))
                result = -result;
        }

        void ExpBrecket(out double result)
        {
            if (token.Equals("("))
            {
                GetToken();
                ExpEq(out result);
                if (!token.Equals(")"))
                {
                    AllIsGood = false;
                }
                GetToken();
            }
            else
                Atom(out result);
        }

        void Atom(out double result)
        {
            switch (tokenType)
            {
                case Type.Num:
                    try
                    {
                        result = Double.Parse(token);
                    }
                    catch (FormatException)
                    {
                        AllIsGood = false;
                        result = 0.0;
                    }
                    GetToken();
                    return;
                case Type.Name:
                    int indexOfToken = 0;
                    int column = 0;
                    while (!IsDigit(token[indexOfToken]))
                    {
                        if (indexOfToken > 0)
                            column += 26;
                        column += (int)(token[indexOfToken]) - (int)'A';
                        indexOfToken++;
                    }
                    string sRow = token.Substring(indexOfToken);
                    int row = Int32.Parse(sRow) - 1;
                    result = data.DataTable[row, column].Value;
                    GetToken();
                    return;
                case Type.Del:
                    ExpUnar(out result);
                    return;
                default:
                    //AllIsGood = false;
                    result = 0.0;
                    break;
            }
        }

        bool IsDigit(char c)
        {
            if (("1234567890".IndexOf(c) != -1))
                return true;
            return false;
        }

        void GetToken()
        {
            tokenType = Type.None;
            token = "";
            if (Index == exp.Length)
                return;
            while (Index < exp.Length && Char.IsWhiteSpace(exp[Index]))
                Index++;
            if (Index == exp.Length)
                return;
            if (IsDelim(exp[Index])){
                token += exp[Index];
                Index++;
                tokenType = Type.Del;
            }
            else if (Char.IsDigit(exp[Index]))
            {
                while (!IsDelim(exp[Index]))
                {
                    token += exp[Index];
                    Index++;
                    if (Index >= exp.Length)
                        break;
                }
                tokenType = Type.Num;
            }
            else
            {
                while (!IsDelim(exp[Index]))
                {
                    token += exp[Index];
                    Index++;
                    if (Index >= exp.Length)
                        break;
                }
                tokenType = Type.Name;
            }
        }

        bool IsDelim(char c)
        {
            if (("+-*/|^%()<>".IndexOf(c) != -1))
                return true;
            return false;
        }

        bool IsCycle(int parentRow, int parentColumn, int currentRow, int currentColumn)
        {
            if (CheckCycle(parentRow, parentColumn, currentRow, currentColumn) == true)
                return true;
            return false;
        }

        bool CheckCycle(int parentRow, int parentColumn, int currentRow, int currentColumn)
        {
            string parentCell = data.GetColumnName(parentColumn) + (parentRow + 1).ToString();
            char[] delimetr = { '+', '-', ' ', '/', '*', '%', '|', '^', '>', '<', '(', ')' };
            string[] subExp;
            if(parentRow == currentRow && parentColumn == currentColumn)
            {
                subExp = exp.Split(delimetr);
            }
            else
            {
                subExp = data.DataTable[currentRow, currentColumn].Expression.Split(delimetr);
            }
            for (int currentExp = 0; currentExp < subExp.Length; currentExp++)
            {
                if (subExp[currentExp].Length > 0 && (int)subExp[currentExp][0] >= (int)'A' && (int)subExp[currentExp][0] <= (int)'Z')
                {
                    int iIndex = 0, newColumn = 0;
                    while (!IsDigit(subExp[currentExp][iIndex]))
                    {
                        if (iIndex > 0)
                            newColumn += 26;
                        newColumn += (int)(subExp[currentExp][iIndex]) - (int)'A';
                        iIndex++;
                    }
                    string sRow = subExp[currentExp].Substring(iIndex);
                    int newRow = Int32.Parse(sRow) - 1;
                    if (subExp[currentExp].Equals(parentCell))
                        return true;
                    else
                        return IsCycle(parentRow, parentColumn, newRow, newColumn);
                }
            }
            return false;
        }
    }
}
