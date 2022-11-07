using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Поварёнок
{
    public partial class FormProduct : Form
    {
        public FormProduct()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
        private string select = "select * from Product join Manufacturer on Product.ProductManufacturerID = Manufacturer.ManufacturerID join Category on Product.ProductCategoryID = Category.CategoryID ";
        private void FormProduct_Load(object sender, EventArgs e)
        {
            loadTabel(select);
            SqlCommand sqlCommand = DBhelper.connection.CreateCommand();
            sqlCommand.CommandText = " select [CategoryName] from Category";
            SqlDataReader r = sqlCommand.ExecuteReader();
            if (r.HasRows)
            {
                while (r.Read())
                {
                    comboBoxCategory.Items.Add(r[0]);
                }
            }
            r.Close();
            comboBoxPriceSort.SelectedIndex = 0;
            comboBoxDiscount.SelectedIndex = 0;
            comboBoxCategory.SelectedIndex = 0;
        }



        private void loadTabel(String text)
        {
            dataGridView1.Rows.Clear();
            SqlCommand sqlCommand = DBhelper.connection.CreateCommand();
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = text;
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            string specifaierC = "C";
            CultureInfo culture;
            culture = CultureInfo.CreateSpecificCulture("ru-RU");
            if (sqlDataReader.HasRows)
            {

                while (sqlDataReader.Read())
                {
                    decimal cost = Convert.ToDecimal(sqlDataReader["ProductCost"]);
                    decimal discont = Convert.ToDecimal(sqlDataReader["ProductDiscountAmount"]);
                    if (sqlDataReader["ProductPhoto"].ToString().Equals(""))

                        dataGridView1.Rows.Add(Properties.Resources.заглушка,
                            sqlDataReader["ProductName"] + "\nПроизводитель: " +
                        sqlDataReader["ManufacturerName"] + "\nЦена: " + cost.ToString(specifaierC, culture) + "\nСкидка: " + discont + "%\nЦена со скидкой: " +
                        (cost - cost * discont / 100).ToString(specifaierC, culture));
                    else
                    {

                        dataGridView1.Rows.Add(sqlDataReader["ProductPhoto"],
                            sqlDataReader["ProductName"] + "\nПроизводитель: " +
                        sqlDataReader["ManufacturerName"] + "\nЦена: " + cost.ToString(specifaierC, culture) + "\nСкидка: " + discont + "%\nЦена со скидкой: " +
                        (cost - cost * discont / 100).ToString(specifaierC, culture));
                    }
                }
            }
            sqlDataReader.Close();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Height = 150;
            }
        }

        private void parameters(object sender, EventArgs e)
        { 
            string search = " and Product.ProductName like '%" + textBoxSearch.Text + "%'";
            string sortDiscount = "";
            switch (comboBoxDiscount.SelectedIndex)
            {
                case 0: sortDiscount = ""; break;
                case 1: sortDiscount = " and Product.ProductDiscountAmount between 0 and 9,99"; break;
                case 2: sortDiscount = " and Product.ProductDiscountAmount between 10 and 14,99"; break;
                case 3: sortDiscount = " and Product.ProductDiscountAmount > 15"; break;
            }
            string sortPrice = "";
            switch(comboBoxPriceSort.SelectedIndex)
            {
                case 0: sortPrice = "";break;
                case 1: sortPrice = " order by Product.ProductCost asc"; break;
                case 2: sortPrice = " order by Product.ProductCost desc"; break;
            }

            string sortCat = "";
            if (comboBoxCategory.SelectedIndex == 0)
            {sortCat = ""; 
            }
            else { sortCat = "and Category.CategoryName like '%" + comboBoxCategory.Text + "%'"; }
            
            loadTabel(select + search + sortDiscount + sortCat + sortPrice);



        }
    }
}
