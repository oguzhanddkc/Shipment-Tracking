using System.Data.SQLite;
using System.Data;
//veritabaný olarak sqlite kullanýldý.


namespace SevkiyatTakip
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        int indexRow;
        //Rowun indexini tutacak deðiþken tanýmlandý.Ýleride kullanýlacak
        SQLiteConnection conn = new SQLiteConnection("Data Source=database.db;Version=3;");
        //Veritabaný baðlantýsý kuruldu.
        

        public void show()//Veritabanýna eriþip verileri datatable a aktaran fonksiyon.
        {
            conn.Open();// Baðlantý açýldý.
            SQLiteDataAdapter adapter = new SQLiteDataAdapter("SELECT * FROM Siparisler", conn);//Sql query.
            DataTable dataTable = new DataTable();
            DataTable dataTable2 = new DataTable();
            dataTable2.Columns.Add("SiparisNo");
            dataTable2.Columns.Add("UrunKodu");
            dataTable2.Columns.Add("FirmaAdi");
            dataTable2.Columns.Add("SiparisTarihi");
            dataTable2.Columns.Add("SevkiyatDurumu");

            //2 adet datatable var birisi sevkiyatý tamamlanmayan ürünlerin tablosu diðer ise sadece tamamlanan sevkiyatlarý gösteren tablosu.
            adapter.Fill(dataTable);

            int  numberOfColumns = dataTable.Columns.Count;
            //Datatable daki sütunlarýn sayýsýnýn tutulduðu deðiþken.
            
            
        
            foreach (DataRow row in dataTable.Rows)
            {
                for (int i = 0; i < numberOfColumns; i++)
                {
                    if(row[i].ToString() == "Tamamlandý")
                    {
                       
                        dataTable2.Rows.Add(row.ItemArray);
                        row.Delete();

                    }
                }
            //Veritabanýndan gelen verilerin tablolar arasýnda tamamlanan ve tamamlanmayan sevkiyat olarak ayrýlmasý için iç içe döngü.
                  

            }
            dataGridView1.DataSource = dataTable;//Datagridlerin veri kaynaklarý.
            dataGridView2.DataSource = dataTable2;
            conn.Close();//Baðlantý kapatýldý.
        }

        private void button1_Click(object sender, EventArgs e)
        {
            show();
            string message = "Veritabaný Listelendi!";
            MessageBox.Show(message);
            //Buttonlarýn çalýþtýðý mesajý kullanýcýya verildi.
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;
            textBox3.ReadOnly = true;
            textBox4.ReadOnly = true;
            // Sadece sevkiyatlarýn tamamlanýp tamamlanmadýðýnýn ayarlanabildiði modül olduðu için diðer textboxlar read-only olarak ayarlandý.

            show();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //Datagrid üzerinde seçilen satýrýn yandaki textboxlara verilerinin aktarýlmasý.
            
            indexRow = e.RowIndex;
            DataGridViewRow row = dataGridView1.Rows[indexRow];

            textBox1.Text = row.Cells[0].Value.ToString();
            textBox2.Text = row.Cells[1].Value.ToString();
            textBox3.Text = row.Cells[2].Value.ToString();
            textBox4.Text = row.Cells[3].Value.ToString();
            textBox5.Text = row.Cells[4].Value.ToString();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            SQLiteConnection conn = new SQLiteConnection("Data Source=database.db;Version=3;");
            conn.Open();
            SQLiteCommand command = new SQLiteCommand(conn);
            command.CommandText = "UPDATE Siparisler set SevkiyatDurumu=@SevkiyatDurumu where SiparisNo=@SiparisNo ";
            command.Prepare();
            command.Parameters.AddWithValue("@SiparisNo", textBox1.Text);
            command.Parameters.AddWithValue("@SevkiyatDurumu", textBox5.Text);
            command.ExecuteNonQuery();
            conn.Close(); 
            dataGridView1.Refresh();//Datagridin güncellenmiþ hali getirildi.
            string message = "Kayýt Güncellendi!";
            MessageBox.Show(message);
            // Sql cümlesi ile güncellenen verilerin veritabanýna aktarýlmasý iþlemi.

        }
    }
}