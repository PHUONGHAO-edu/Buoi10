using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bai_10
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        DataSet ds = new DataSet("dsQLNV");
        SqlDataAdapter daChucVu;
        SqlDataAdapter daNhanVien;

        private void Form1_Load(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = @"Data Source=(MT08);Initial Catalog=QLNV;Integrated
Security=True";
            // Dữ liệu combobox Chức vụ
            string sQueryChucVu = @"select * from chucvu";
            daChucVu = new SqlDataAdapter(sQueryChucVu, conn);
            daChucVu.Fill(ds, "tblChucVu");
            cboChucVu.DataSource = ds.Tables["tblChucVu"];
            cboChucVu.DisplayMember = "tencv";
            cboChucVu.ValueMember = "macv";

            

            // Dữ liệu datagrid Danh sách nhân viên
            string sQueryNhanVien = @"select n.*, c.tencv from nhanvien n, chucvu c where
n.macv=c.macv";
            daNhanVien = new SqlDataAdapter(sQueryNhanVien, conn);
            daNhanVien.Fill(ds, "tblDSNhanVien");
            dgDSNhanVien.DataSource = ds.Tables["tblDSNhanVien"];
            dgDSNhanVien.Columns["manv"].HeaderText = "Mã số";
            dgDSNhanVien.Columns["manv"].Width = 60;

            // … đặt tiêu đề tiếng Việt, định độ rộng cho các trường còn lại
            dgDSNhanVien.Columns["macv"].Visible = false;

            // Command Thêm nhân viên
            string sThemNV = @"insert into nhanvien values(@MaNV, @HoLot, @TenNV, @Phai,
@NgaySinh, @MaCV)";
            SqlCommand cmThemNV = new SqlCommand(sThemNV, conn);
            cmThemNV.Parameters.Add("@MaNV", SqlDbType.NVarChar, 5, "manv");
            cmThemNV.Parameters.Add("@HoLot", SqlDbType.NVarChar, 50, "holot");
            cmThemNV.Parameters.Add("@TenNV", SqlDbType.NVarChar, 10, "tennv");
            cmThemNV.Parameters.Add("@Phai", SqlDbType.NVarChar, 3, "phai");
            cmThemNV.Parameters.Add("@NgaySinh", SqlDbType.SmallDateTime, 10,
            "ngaysinh");
            cmThemNV.Parameters.Add("@MaCV", SqlDbType.NVarChar, 5, "macv");
            
            daNhanVien.InsertCommand = cmThemNV;

            //Sửa Nhân Viên
            string sSuaNV = @"update from nhanvien where manv=@MaNV";
            SqlCommand cmSuaNV = new SqlCommand(sSuaNV, conn);
            cmSuaNV.Parameters.Add("@MaNV", SqlDbType.VarChar, 5, "manv");
            cmSuaNV.Parameters.Add("@HoLot", SqlDbType.NVarChar, 50, "holot");
            cmSuaNV.Parameters.Add("@TenNV", SqlDbType.NVarChar, 10, "tennv");
            cmSuaNV.Parameters.Add("@Phai", SqlDbType.NVarChar, 3, "phai");
            cmSuaNV.Parameters.Add("@NgaySinh", SqlDbType.SmallDateTime, 10, "ngaysinh");
            cmSuaNV.Parameters.Add("@MaCV", SqlDbType.NVarChar, 5, "macv");

            daNhanVien.UpdateCommand = cmSuaNV;

            //Xóa Nhân Viên
            string sXoaNV = @"update from nhanvien where manv=@MaNV";
            SqlCommand cmXoaNV= new SqlCommand(sXoaNV, conn);
            cmXoaNV.Parameters.Add("@MaNV", SqlDbType.VarChar, 5, "manv");
            
            daNhanVien.DeleteCommand = cmXoaNV;

            //Thuộc Tính Enable các button
            btnLuu.Enabled = false;
            btnHuy.Enabled = false;
        }
        private void HienThiDatagrid(object sender, EventArgs e)
        {
            //Dữ liệu datagrid Danh sách nhân viên
            string sQueryNhanVien = @"select n.*,c.tenqq from hocsinh n, quequan c where n.maqq=c.maqq";
            
        }

        private void dgDSNhanVien_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow dr = dgDSNhanVien.SelectedRows[0];
            txtMaNV.Text = dr.Cells["manv"].Value.ToString();
            txtHoLot.Text = dr.Cells["holot"].Value.ToString();
            txtTen.Text = dr.Cells["tennv"].Value.ToString();
            if (dr.Cells["phai"].Value.ToString() == "Nam")
            {
                radNam.Checked = true;
            }
            else
            {
                radNu.Checked = true;
            }
            dtpNgaySinh.Text = dr.Cells["ngaysinh"].Value.ToString();
            cboChucVu.SelectedValue = dr.Cells["macv"].Value.ToString();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            DataRow row = ds.Tables["tblDSNhanVien"].NewRow();
            row["manv"] = txtMaNV.Text;
            row["holot"] = txtHoLot.Text;
            row["tennv"] = txtTen.Text;
            if (radNu.Checked == true)
            {
                row["phai"] = "Nữ";
            }
            else
            {
                row["phai"] = "Nam";
            }
            row["ngaysinh"] = dtpNgaySinh.Text;
            row["macv"] = cboChucVu.SelectedValue;
            ds.Tables["tblDSNhanVien"].Rows.Add(row);
            
            btnLuu.Enabled = true;
            btnHuy.Enabled = true;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                daNhanVien.Update(ds, "tblDSNhanVien");
                MessageBox.Show("Lưu thành công!", "Thông Báo");
                dgDSNhanVien.Refresh();
            }
            catch 
            {
                return;
            }
            
           



        }
        private void BtnSua_Click(object sender, EventArgs e)
        {
            DataGridViewRow dr = dgDSNhanVien.SelectedRows[0];
            dgDSNhanVien.BeginEdit(true);
            dr.Cells["manv"].Value = txtMaNV.Text;
            dr.Cells["holot"].Value = txtHoLot.Text;
            dr.Cells["tennv"].Value = txtTen.Text;
            if(radNam.Checked == true)
            {
                dr.Cells["phai"].Value = "Nam";
            }
            else
            {
                dr.Cells["phai"].Value = "Nữ";
            }
            dr.Cells["ngaysinh"].Value = dtpNgaySinh.Text;
            dr.Cells["macv"].Value = cboChucVu.Text;

        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            ds.Tables["tblDSNhanVien"].RejectChanges(); 
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult traloi;
            traloi = MessageBox.Show("Bạn có muốn thoát?", "Thông Báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if(traloi == DialogResult.OK)
            {
                Application.Exit();
            } 
            return;
                
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            DataGridViewRow dr = dgDSNhanVien.SelectedRows[0];
            dgDSNhanVien.Rows.Remove(dr);

            btnLuu.Enabled = true;
            btnHuy.Enabled = true;
        }
    }
}
