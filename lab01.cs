using System.Globalization;
using System.Reflection.Metadata.Ecma335;

namespace SimplexMethodBMSTU
{
    class SimplexMethod
    {
        int n; // свободные переменные (столбики)
        int m; // базисные переменные ( cтроки)
        //bool
        // 1. Столбец S
        // 2. Массив с кэфами
        // 3. F строка(целевая функция)
        decimal[] cols = null!; // cтолбец s
        decimal[] cols_new = null!;
        decimal[,] matrix = null!; // массив с кэфами
        decimal[] F = null!;
        decimal[] F_new = null!;
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
            int col_find = Find_col();
            for(int i = 0; i<m; ++i)
            {
                if(matrix[i,col_find] != 0)
                {
                    if(cols[i]/matrix[i,col_find] > 0)
                        {
                            a = cols[i]/matrix[i,col_find];
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
        void SimplexTab(Func<int> Find_rowfunc, Func<int> Find_colfunc) 
        {
            int Find_row1 = Find_rowfunc();
            int Find_col1 = Find_colfunc();
            matrix_new[Find_row1,Find_col1] = 1/matrix[Find_row1,Find_col1];
            for(int i = 0; i<n; ++i)
            {
                if(i != Find_col1)
                {
                    matrix_new[Find_row1,i] = matrix[Find_row1,i]/matrix[Find_row1,Find_col1];
                }
            }
            for(int i = 0; i<m; ++i)
            {
                if(i != Find_row1)
                {
                    matrix_new[i,Find_col1] = -matrix[i,Find_col1]/matrix[Find_row1,Find_col1];
                }
            }
            for(int i = 0; i<m; ++i)
            {
                if(i != Find_row1)
                {
                    for(int j = 0; j<n; ++j)
                    {
                        if(j != Find_col1)
                        {
                            matrix_new[i,j] = matrix[i,j] - (matrix[Find_row1,j]*matrix[i,Find_col1])/matrix[Find_row1,Find_col1];
                        }
                    }
                }
            }
            cols_new[Find_row1] = cols[Find_row1]/matrix[Find_row1,Find_col1];
            for(int i = 0; i<m; ++i)
            {
                if(i != Find_row1)
                {
                    cols_new[i] = cols[i] - (matrix[i,Find_col1]*cols[Find_row1])/matrix[Find_row1,Find_col1];
                }
            }
            F_new[Find_col1+1] = -F[Find_col1+1]/matrix[Find_row1,Find_col1];
            F_new[0] = F[0] - ((F[Find_col1+1]*cols[Find_row1])/matrix[Find_row1,Find_col1]);
            int variable = 1;
            for(int i = 1; i<n+1; ++i)
            {
                if(i != Find_col1+1)
                {
                    F_new[i] = F[i] - (F[Find_col1+1]*matrix[Find_row1,variable])/matrix[Find_row1,Find_col1];
                    ++variable;
                }
            }
        }

        // Функция для проверки наличия решения
        int CheckSystem()
        {
            for(int i = 0; i<m; ++i)
            {
                if(cols[i] < 0)
                {
                    for(int j = 0; j<n; ++j)
                    {
                        if(matrix[i,j] < 0)
                        {
                            return 4; // Найден отрицательный элемент => разрешающий столбец, находим разрешающую строчку и продолжаем
                            //симплекс-метод
                        }
                    }
                }
            }
            return 5; // Опорное решение только, если все свободные кэфы >= 0
            // Опорное решение найдено, продолжаем симплекс-метод
        }

        // Функция для поиска нового разрешающего столбца
        int Find_col_after_general_solver()
        {
            for(int i = 0; i<n; ++i)
            {
                if(F_new[i] > 0)
                {
                    return i;
                }
            }
            return -1;
        }

        // Функция для поиска новой разрешающей строки
        int Find_row_after_general_solver()
        {
            decimal a;
            decimal c = 10000000000000000000m;
            int b = Find_col_after_general_solver();
            int g = -1;
            for(int i = 0; i<m; ++i)
            {
                if (matrix_new[i,b] != 0)
                {
                    if(cols_new[i]/matrix_new[i,b] > 0)
                    {
                        a = cols_new[i] / matrix_new[i,b];
                        if(a<c)
                        {
                            c = a;
                            g = i;
                        }
                    }
                }
            }
            return g;
        }

        // Функция для перемешивания массивов, чтобы реализовать цикл while в функции main_func_for_oprimization для применения симплекс-метода
        // пока не будет получено опорное решение
        public void Refuel_mass()
        {
            for(int i = 0; i<m; ++i)
            {
                cols[i] = cols_new[i];
                for(int j = 0; j<n; ++j)
                {
                    matrix[i,j] = matrix_new[i,j];
                }
            }
            for(int i = 0; i<n+1; ++i)
            {
                F[i] = F_new[i];
            }
        }

        // Главная функция, которая запускает весь процесс нахождения и оптимизации решения целевой функции
        void main_func_for_oprimization()
        {
            Input();
            Print();
            int check_system = CheckSystem();
            if(check_system == 5)
            {
                for(int i = 1; i<n+1; ++i)
                {
                    while(F[i] > 0)
                    {
                        SimplexTab(Find_row_after_general_solver,Find_col_after_general_solver);
                        Refuel_mass();
                    }
                    // Дописать вывод получившейся таблицы!
                }
                // Находим оптимальное решение пока не будет в строке функции(кроме столбца свободных членов) всех отрицательных кэфов
            }
            if(check_system == 4)
            {
                for(int i = 0; i<m; ++i)
                {
                    while(cols[i] < 0)
                    {
                        SimplexTab(Find_row,Find_col);
                        Refuel_mass();
                    }
                }
                // Дописать вывод получившейся таблицы
                for(int i = 1; i<n+1; ++i)
                {
                    while(F[i] > 0)
                    {
                        SimplexTab(Find_row_after_general_solver,Find_col_after_general_solver);
                        Refuel_mass();
                    }
                }
                // Дописать вывод получившейся таблицы
                // exit(0) Программа завершена!!!
            }
            else
            {
                Console.Write("Система не имеет решений");
                Environment.Exit(0);
            }
        }



        static void Main()
        {
            Console.Write("Введите количество строк: ");
            int m = int.Parse(Console.ReadLine()!);

            Console.Write("Введите количество столбцов: ");
            int n = int.Parse(Console.ReadLine()!);

            var simplexMethod = new SimplexMethod(n, m);
            //simplexMethod.CheckSystem();
            //simplexMethod.SimplexTab();
            simplexMethod.main_func_for_oprimization();
            simplexMethod.PrintSimplexTab();












        }

    }





}