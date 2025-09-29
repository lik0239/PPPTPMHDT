using System;
using System.Globalization;
using System.Windows.Forms;

namespace MayTinhDonGian
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // Gán sự kiện cho 4 phép tính cơ bản
            btnCong.Click += btnCong_Click;
            btnTru.Click += btnTru_Click;
            btnNhan.Click += btnNhan_Click;
            btnChia.Click += btnChia_Click;

            // Gán sự kiện Xóa/Thoát
            btnXoa.Click += btnXoa_Click;
            btnThoat.Click += btnThoat_Click;

            // Áp dụng 1: ƯSCLN/BCNN (nếu bạn đã gán trong Designer thì có thể bỏ 2 dòng dưới)
            if (btnUSCLN != null) btnUSCLN.Click += btnUSCLN_Click;
            if (btnBCNN != null) btnBCNN.Click += btnBCNN_Click;

            // Chỉ đọc ô kết quả
            txtKetQua.ReadOnly = true;

            // Tuỳ chọn: Enter = Cộng
            // this.AcceptButton = btnCong;
        }

        // ====== Phần dùng cho 4 phép tính số thực ======
        private bool TryGetInputs(out double a, out double b)
        {
            a = 0; b = 0;
            var sA = txtA.Text.Trim();
            var sB = txtB.Text.Trim();

            if (!TryParseDoubleFlexible(sA, out a))
            {
                MessageBox.Show("Giá trị a không hợp lệ. Vui lòng nhập số thực.", "Lỗi nhập liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtA.Focus();
                return false;
            }

            if (!TryParseDoubleFlexible(sB, out b))
            {
                MessageBox.Show("Giá trị b không hợp lệ. Vui lòng nhập số thực.", "Lỗi nhập liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtB.Focus();
                return false;
            }

            return true;
        }

        private bool TryParseDoubleFlexible(string input, out double value)
        {
            // Chấp nhận cả dấu . và ,
            return double.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out value)
                   || double.TryParse(input, NumberStyles.Float, CultureInfo.CurrentCulture, out value);
        }

        private void ShowResult(double result)
        {
            txtKetQua.Text = result.ToString(CultureInfo.CurrentCulture);
        }

        private void btnCong_Click(object sender, EventArgs e)
        {
            if (!TryGetInputs(out var a, out var b)) return;
            ShowResult(a + b);
        }

        private void btnTru_Click(object sender, EventArgs e)
        {
            if (!TryGetInputs(out var a, out var b)) return;
            ShowResult(a - b);
        }

        private void btnNhan_Click(object sender, EventArgs e)
        {
            if (!TryGetInputs(out var a, out var b)) return;
            ShowResult(a * b);
        }

        private void btnChia_Click(object sender, EventArgs e)
        {
            if (!TryGetInputs(out var a, out var b)) return;

            if (Math.Abs(b) < 1e-12) // hoặc b == 0
            {
                MessageBox.Show("Mẫu số (b) không được bằng 0. Vui lòng nhập lại!", "Lỗi chia cho 0",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtB.Focus();
                txtB.SelectAll();
                return;
            }

            ShowResult(a / b);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            txtA.Clear();
            txtB.Clear();
            txtKetQua.Clear();
            txtA.Focus();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Bạn có thực sự muốn thoát?", "Xác nhận thoát",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Close();
            }
        }

        // ====== Áp dụng 1: ƯSCLN / BCNN (chỉ nhận SỐ NGUYÊN) ======
        private bool TryGetIntegers(out long a, out long b)
        {
            a = 0; b = 0;

            if (!long.TryParse(txtA.Text.Trim(), out a))
            {
                MessageBox.Show("Áp dụng 1 yêu cầu số nguyên cho a.", "Lỗi nhập liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtA.Focus(); txtA.SelectAll();
                return false;
            }

            if (!long.TryParse(txtB.Text.Trim(), out b))
            {
                MessageBox.Show("Áp dụng 1 yêu cầu số nguyên cho b.", "Lỗi nhập liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtB.Focus(); txtB.SelectAll();
                return false;
            }

            return true;
        }

        private static long Gcd(long x, long y)
        {
            x = Math.Abs(x);
            y = Math.Abs(y);
            if (x == 0) return y;
            if (y == 0) return x;

            while (y != 0)
            {
                long r = x % y;
                x = y;
                y = r;
            }
            return x;
        }

        private static long Lcm(long x, long y)
        {
            if (x == 0 || y == 0) return 0;
            long g = Gcd(x, y);
            checked
            {
                return Math.Abs((x / g) * y);
            }
        }

        private void btnUSCLN_Click(object sender, EventArgs e)
        {
            if (!TryGetIntegers(out var a, out var b)) return;

            if (a == 0 && b == 0)
            {
                MessageBox.Show("ƯSCLN(0, 0) không xác định.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            long kq = Gcd(a, b);
            txtKetQua.Text = $"ƯSCLN({a}, {b}) = {kq}";
        }

        private void btnBCNN_Click(object sender, EventArgs e)
        {
            if (!TryGetIntegers(out var a, out var b)) return;

            if (a == 0 && b == 0)
            {
                MessageBox.Show("BCNN(0, 0) không xác định.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                long kq = Lcm(a, b);
                txtKetQua.Text = $"BCNN({a}, {b}) = {kq}";
            }
            catch (OverflowException)
            {
                MessageBox.Show("Kết quả BCNN vượt quá phạm vi số nguyên 64-bit.",
                    "Tràn số", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        
    }
}
