using System.Data.SQLite;
using System.Data;
//veritaban� olarak sqlite kullan�ld�.


namespace SevkiyatTakip
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        int indexRow;
        //Rowun indexini tutacak de�i�ken tan�mland�.�leride kullan�lacak
        SQLiteConnection conn = new SQLiteConnection("Data Source=database.db;Version=3;");
        //Veritaban� ba�lant�s� kuruldu.
        

        public void show()//Veritaban�na eri�ip verileri datatable a aktaran fonksiyon.
        {
            conn.Open();// Ba�lant� a��ld�.
            SQLiteDataAdapter adapter = new SQLiteDataAdapter("SELECT * FROM Siparisler", conn);//Sql query.
            DataTable dataTable = new DataTable();
            DataTable dataTable2 = new DataTable();
            dataTable2.Columns.Add("SiparisNo");
            dataTable2.Columns.Add("UrunKodu");
            dataTable2.Columns.Add("FirmaAdi");
            dataTable2.Columns.Add("SiparisTarihi");
            dataTable2.Columns.Add("SevkiyatDurumu");

            //2 adet datatable var birisi sevkiyat� tamamlanmayan �r�nlerin tablosu di�er ise sadece tamamlanan sevkiyatlar� g�steren tablosu.
            adapter.Fill(dataTable);

            int  numberOfColumns = dataTable.Columns.Count;
            //Datatable daki s�tunlar�n say�s�n�n tutuldu�u de�i�ken.
            
            
        
            foreach (DataRow row in dataTable.Rows)
            {
                for (int i = 0; i < numberOfColumns; i++)
                {
                    if(row[i].ToString() == "Tamamland�")
                    {
                       
                        dataTable2.Rows.Add(row.ItemArray);
                        row.Delete();

                    }
                }
            //Veritaban�ndan gelen verilerin tablolar aras�nda tamamlanan ve tamamlanmayan sevkiyat olarak ayr�lmas� i�in i� i�e d�ng�.
                  

            }
            dataGridView1.DataSource = dataTable;//Datagridlerin veri kaynaklar�.
            dataGridView2.DataSource = dataTable2;
            conn.Close();//Ba�lant� kapat�ld�.
        }

        private void button1_Click(object sender, EventArgs e)
        {
            show();
            string message = "Veritaban� Listelendi!";
            MessageBox.Show(message);
            //Buttonlar�n �al��t��� mesaj� kullan�c�ya verildi.
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;
            textBox3.ReadOnly = true;
            textBox4.ReadOnly = true;
            // Sadece sevkiyatlar�n tamamlan�p tamamlanmad���n�n ayarlanabildi�i mod�l oldu�u i�in di�er textboxlar read-only olarak ayarland�.

            show();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //Datagrid �zerinde se�ilen sat�r�n yandaki textboxlara verilerinin aktar�lmas�.
            
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
            dataGridView1.Refresh();//Datagridin g�ncellenmi� hali getirildi.
            string message = "Kay�t G�ncellendi!";
            MessageBox.Show(message);
            // Sql c�mlesi ile g�ncellenen verilerin veritaban�na aktar�lmas� i�lemi.

        }
    }
}