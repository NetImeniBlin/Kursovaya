using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp4
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Menu());
        }


        public class Podkl
        {
            public string Connstring = "server=chuc.caseum.ru;port=33333;user=st_2_19_8;database=is_2_19_st8_KURS;password=33980803;";
            public string Vkl()
            {
                return Connstring;
            }
        }
    }
}
