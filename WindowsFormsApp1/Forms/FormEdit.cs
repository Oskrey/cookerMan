using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Поварёнок.View
{
    public partial class FormEdit : Form
    {
        private string article;

        public FormEdit(string article)
        {
            InitializeComponent();
            this.article = article;
        }
        private void FormEdit_Load(object sender, EventArgs e)
        {
            SqlCommand sqlCommand = DBhelper.connection.CreateCommand();
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = "select * from Product join Manufacturer on Product.ProductManufacturerID = Manufacturer.ManufacturerID join Category on Product.ProductCategoryID = Category.CategoryID where ProductArticleNumber = '" + article + "'"; 
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            string specifaierC = "C";
            CultureInfo culture;
            culture = CultureInfo.CreateSpecificCulture("ru-RU");
            if (sqlDataReader.HasRows)
            {

                while (sqlDataReader.Read())
                {
                    textBoxArticle.Text = sqlDataReader["ProductArticleNumber"].ToString();
                    textBoxName.Text = sqlDataReader["ProductName"].ToString();
                    comboBoxManufacturer.Text = sqlDataReader["ManufactorerName"].ToString();
                    comboBoxCategory.Text = sqlDataReader["CatogoryName"].ToString();
                    textBoxCost.Text = sqlDataReader["ProductCost"].ToString();
                    textBoxDiscount.Text = sqlDataReader["ProductDiscountAmount"].ToString();
                    textBoxQuantityInStock.Text = sqlDataReader["ProductQuantityInStock"].ToString();
                    textBoxDescription.Text = sqlDataReader["ProductDescription"].ToString();
                }
            }
        }
    }
}
