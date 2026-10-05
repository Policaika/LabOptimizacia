```C#
namespace SimplexMethodBMSTU
{
    class SimplexMethod
    {
        int n; // свободные переменные (столбики)
        int m; // базисные переменные ( cтроки)
        //bool
        // 1. Столбец S
        // 2. Массив с кэфами
        // 3. Базисные переменные
        // F строка
        decimal[] cols = null!; // cтолбец s
        decimal[] cols_new = null!;
        decimal[,] matrix = null!; // массив с кэфами
        decimal[] F = null!;
        decimal[] F_new = null!;
        int[] basis = null!; // базисные переменные
        decimal[,] matrix_new = null!; // массив с кэфами после преобразования
        public SimplexMethod(int n, int m)
        {
            this.n = n;
            this.m = m;
            Make();
        }
        public void Make()
        {
            cols = new decimal[m];
            matrix = new decimal[m, n];
            F = new decimal[n + 1];
            basis = new int[m];
            matrix_new = new decimal[m, n];
            F_new = new decimal[n + 1];
            cols_new = new decimal[m];
        }
        public void Input()
        {
            Console.WriteLine("Введите столбец S:");
            for (int i = 0; i < m; i++)
            {
                cols[i] = decimal.Parse(Console.ReadLine()!);
            }
            Console.WriteLine("Введите массив с кэфами:");
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    matrix[i, j] = decimal.Parse(Console.ReadLine()!);
                }
            }
            Console.WriteLine("Введите строку F:");
            for (int i = 0; i < n + 1; i++)
            {
                F[i] = decimal.Parse(Console.ReadLine()!);
            }
            Console.WriteLine("Введите базисные переменные:");
            for (int i = 0; i < m; i++)
            {
                basis[i] = int.Parse(Console.ReadLine()!);
            }
        }




        public void Print()
        {
            Console.WriteLine("Столбец S:");
            for (int i = 0; i < m; i++)
            {
                Console.Write(cols[i] + " ");
            }
            Console.WriteLine();
            Console.WriteLine("Массив с кэфами:");
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write(matrix[i, j] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("Строка F:");
            for (int i = 0; i < n + 1; i++)
            {
                Console.Write(F[i] + " ");
            }
            Console.WriteLine();
            Console.WriteLine("Базисные переменные:");
            for (int i = 0; i < m; i++)
            {
                Console.Write(basis[i] + " ");
            }
            Console.WriteLine();
        }

        // Вывод получившейся симплекс таблицы

        public void PrintSimplexTab()
        {
            Console.WriteLine("Получившаяся симплекс таблица:");
            for(int i = 0; i<m; ++i)
            {
                Console.Write(cols_new[i] + "\t");
                for(int j = 0; j<n; ++j)
                {
                   Console.Write(matrix_new[i,j] + "\t");
                }
                Console.WriteLine("");
            }
            for(int i = 0; i<n+1; ++i)
            {
                Console.Write(F_new[i] + "\t");
            }
        }

        // Поиск первого отрицательного элемента в столбце S
        int Find()
        {
            for (int i = 0; i < m; i++)
            {
                if (cols[i] < 0)
                    return i;
            }
            return -1;
        }

        // Поиск разщешающего столбца
        int Find_col()
        {
            int index = Find();
            for (int i = 0; i < n; i++)
            {
                if (matrix[index, i] < 0)
                    return i;
            }
            return -1;
        }

        // Нахождение отношения для поиска разрешающей строки
        int Find_row()
        {
            decimal a;
            decimal b = 10000000000000000000m;
            int c = 0;
            for(int i = 0; i<m; ++i)
            {
                if(matrix[i,Find_col()] > 0)
                {
                    if(cols[i]/matrix[i,Find_col()] > 0)
                        {
                            a = cols[i]/matrix[i,Find_col()];
                            if(a < b)
                            {
                                b = a;
                                c = i;
                            }
                        }  
                }

            }
            return c;
        }

        // Преобразование симплекс-таблицы
        void SimplexTab()
        {
            matrix_new[Find_row(),Find_col()] = 1/matrix[Find_row(),Find_col()];
            for(int i = 0; i<n; ++i)
            {
                if(i != Find_col())
                {
                    matrix_new[Find_row(),i] = matrix[Find_row(),i]/matrix[Find_row(),Find_col()];
                }
            }
            for(int i = 0; i<m; ++i)
            {
                if(i != Find_row())
                {
                    matrix_new[i,Find_col()] = -matrix[i,Find_col()]/matrix[Find_row(),Find_col()];
                }
            }
            for(int i = 0; i<m; ++i)
            {
                if(i != Find_row())
                {
                    for(int j = 0; j<n; ++j)
                    {
                        if(j != Find_col())
                        {
                            matrix_new[i,j] = matrix[i,j] - (matrix[Find_row(),j]*matrix[i,Find_col()])/matrix[Find_row(),Find_col()];
                        }
                    }
                }
            }
            cols_new[Find_row()] = cols[Find_row()]/matrix[Find_row(),Find_col()];
            for(int i = 0; i<m; ++i)
            {
                if(i != Find_row())
                {
                    cols_new[i] = cols[i] - (matrix[i,Find_col()]*cols[Find_row()])/matrix[Find_row(),Find_col()];
                }
            }
            F_new[n] = -F[n]/matrix_new[Find_row(),Find_col()];
            for(int i = 0; i<n; ++i)
            {
                if(i != Find_col())
                {
                    F_new[i] = F[i] - (F[Find_col()]*matrix[Find_row(),i])/matrix[Find_row(),Find_col()];
                }
            }
        }





        static void Main()
        {
            Console.Write("Введите количество строк: ");
            int m = int.Parse(Console.ReadLine()!);

            Console.Write("Введите количество столбцов: ");
            int n = int.Parse(Console.ReadLine()!);

            var simplexMethod = new SimplexMethod(n, m);
            simplexMethod.Input();
            simplexMethod.Print();
            simplexMethod.SimplexTab();
            simplexMethod.PrintSimplexTab();












        }

    }





}
```
