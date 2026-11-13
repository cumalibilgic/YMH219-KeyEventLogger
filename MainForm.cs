using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace KeyEventLogger
{
    public class MainForm : Form
    {
        // Durum bilgileri
        private bool _isRecording = false;
        private readonly List<KeyRecord> _records = new List<KeyRecord>();
        private readonly Dictionary<string, int> _keyCounts = new Dictionary<string, int>();
        private readonly HashSet<Keys> _heldKeys = new HashSet<Keys>(); // şu an basılı tuşlar

        // Arayüz kontrolleri
        private ListBox lstLog;
        private Button btnStart, btnStop, btnClear, btnSave;
        private Panel pnlIndicator;
        private Label lblStatus, lblStats;

        public MainForm()
        {
            InitializeUi();
            UpdateStatus();
        }

        private void InitializeUi()
        {
            Text = "Key Event Logger (Eğitsel)";
            Size = new Size(620, 520);
            MinimumSize = new Size(560, 420);
            StartPosition = FormStartPosition.CenterScreen;

            // Form, üzerindeki kontroller odaktayken bile tuşları görsün
            KeyPreview = true;
            KeyDown += MainForm_KeyDown;
            KeyUp += MainForm_KeyUp;

            // Üst panel: göstergeler
            pnlIndicator = new Panel { Size = new Size(18, 18), Location = new Point(15, 18) };
            lblStatus = new Label { AutoSize = true, Location = new Point(42, 17), Font = new Font("Segoe UI", 11, FontStyle.Bold) };
            lblStats = new Label { AutoSize = true, Location = new Point(15, 48), Text = "Toplam tuş: 0" };

            // Butonlar
            btnStart = MakeButton("Başlat", new Point(15, 75));
            btnStop = MakeButton("Durdur", new Point(125, 75));
            btnClear = MakeButton("Temizle", new Point(235, 75));
            btnSave = MakeButton("Kaydet (.txt)", new Point(345, 75));
            btnSave.Width = 120;

            btnStart.Click += (s, e) => { _isRecording = true; UpdateStatus(); };
            btnStop.Click += (s, e) => { _isRecording = false; UpdateStatus(); };
            btnClear.Click += BtnClear_Click;
            btnSave.Click += BtnSave_Click;

            // Kayıt listesi
            lstLog = new ListBox
            {
                Location = new Point(15, 115),
                Size = new Size(575, 350),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                Font = new Font("Consolas", 10),
                HorizontalScrollbar = true
            };

            Controls.AddRange(new Control[] { pnlIndicator, lblStatus, lblStats, btnStart, btnStop, btnClear, btnSave, lstLog });
        }

        private Button MakeButton(string text, Point location)
        {
            return new Button
            {
                Text = text,
                Location = location,
                Size = new Size(100, 30),
                TabStop = false // Boşluk/Enter tuşu butonu tetiklemesin
            };
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (!_isRecording) return;

            // Tuş basılı tutulunca işletim sistemi KeyDown'u tekrar tekrar gönderir;
            // aynı basışı yalnızca bir kez kaydet (istatistik şişmesin).
            bool firstPress = _heldKeys.Add(e.KeyCode);
            if (firstPress)
                AddRecord(new KeyRecord(KeyNames.GetName(e.KeyCode), KeyRecord.KeyDownType));

            // Tuşun butonları tetiklemesini engelle
            e.Handled = true;
            e.SuppressKeyPress = true;
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            // Kayıt kapalıyken bırakılan tuş da basılı listesinden düşmeli
            bool wasHeld = _heldKeys.Remove(e.KeyCode);
            if (!_isRecording) return;

            // Kayıt, tuş basılıyken başladıysa karşılığı olmayan KeyUp'ı atla
            if (wasHeld)
                AddRecord(new KeyRecord(KeyNames.GetName(e.KeyCode), KeyRecord.KeyUpType));
            e.Handled = true;
        }

        private void AddRecord(KeyRecord record)
        {
            _records.Add(record);
            lstLog.Items.Add(record.ToString());
            lstLog.TopIndex = lstLog.Items.Count - 1; // Son kayda kaydır

            if (record.EventType == KeyRecord.KeyDownType)
            {
                _keyCounts.TryGetValue(record.KeyName, out int count);
                _keyCounts[record.KeyName] = count + 1;
                UpdateStats();
            }
        }

        private void UpdateStats()
        {
            int total = _keyCounts.Values.Sum();
            string top = string.Join(", ",
                _keyCounts.OrderByDescending(k => k.Value).Take(3).Select(k => $"{k.Key} ({k.Value})"));
            lblStats.Text = total == 0
                ? "Toplam tuş: 0"
                : $"Toplam tuş: {total}   |   En sık: {top}";
        }

        private void UpdateStatus()
        {
            if (_isRecording)
            {
                pnlIndicator.BackColor = Color.Red;
                lblStatus.Text = "KAYIT AÇIK";
                lblStatus.ForeColor = Color.Red;
            }
            else
            {
                pnlIndicator.BackColor = Color.Gray;
                lblStatus.Text = "Kayıt durdu";
                lblStatus.ForeColor = Color.Gray;
            }
            btnStart.Enabled = !_isRecording;
            btnStop.Enabled = _isRecording;
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            _records.Clear();
            _keyCounts.Clear();
            lstLog.Items.Clear();
            UpdateStats();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (_records.Count == 0)
            {
                MessageBox.Show("Kaydedilecek kayıt yok.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = "Metin dosyası (*.txt)|*.txt";
                dialog.FileName = $"keylog_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
                if (dialog.ShowDialog() != DialogResult.OK) return;

                try
                {
                    LogFileWriter.Save(dialog.FileName, _records);
                    MessageBox.Show("Kayıtlar kaydedildi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Dosya kaydedilemedi: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
