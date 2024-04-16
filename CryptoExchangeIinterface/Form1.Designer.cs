using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using CryptoExchange.Net.CommonObjects;
using CryptoExchangeRateAplication.exchanges;
namespace CryptoExchangeIinterface
{
    public partial class MainForm : Form
    {
        private ComboBox comboBoxCurrencyPairs;
        private TabControl tabControlExchanges;
        private ComboBox comboBoxExchanges;
        private System.Windows.Forms.Timer timer;
        public MainForm()
        {
            InitializeForm();
            InitializeTimer();
            timer.Start();
        }

        private void InitializeForm()
        {

            var tableLayoutPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill
            };
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));

            var currencyPairsPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight
            };
            comboBoxCurrencyPairs = new ComboBox
            {
                Width = 150,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            comboBoxCurrencyPairs.SelectedIndexChanged += async (sender, e) =>
            {
                await LoadQuotes();
            };
            currencyPairsPanel.Controls.Add(comboBoxCurrencyPairs);

            comboBoxExchanges = new ComboBox
            {
                Width = 170,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            comboBoxExchanges.SelectedIndexChanged += async (sender, e) =>
            {
                await LoadQuotes();
            };


            currencyPairsPanel.Controls.Add(comboBoxExchanges);

            tableLayoutPanel.Controls.Add(currencyPairsPanel, 0, 0);

            InitializeTimer();

            tabControlExchanges = new TabControl
            {
                Dock = DockStyle.Fill
            };

            var exchanges = new List<string> { "Bitget", "Binance", "Bybit", "Kucoin" };
            foreach (var exchange in exchanges)
            {
                var tabPage = new TabPage(exchange);
                var dataGridView = new DataGridView
                {
                    Dock = DockStyle.Fill,
                    ReadOnly = true,
                    ColumnCount = 2
                };
                dataGridView.Columns[0].Name = "Pair";
                dataGridView.Columns[1].Name = "Price";
                tabPage.Controls.Add(dataGridView);
                tabControlExchanges.TabPages.Add(tabPage);
            }

            tableLayoutPanel.Controls.Add(tabControlExchanges, 0, 1);

            Controls.Add(tableLayoutPanel);

            LoadCurrencyPairs();
            LoadExchanges();

            comboBoxCurrencyPairs.SelectedIndexChanged += async (sender, e) =>
            {
                await LoadQuotes();
            };
            comboBoxExchanges.SelectedIndexChanged += async (sender, e) =>
            {
                await LoadQuotes();
            };

            AddRow();


            Width += 30; 
            Height += 10; 
            FormBorderStyle = FormBorderStyle.FixedSingle; 
        }

        private void LoadCurrencyPairs()
        {
            var currencyPairs = new List<string> { "BTCUSDT", "ETHUSDT", "LTCUSDT", "XRPUSDT" };
            comboBoxCurrencyPairs.DataSource = currencyPairs;
        }

        private void LoadExchanges()
        {
            var exchanges = new List<string> { "Binance", "Bitget", "Bybit", "Kucoin" };
            comboBoxExchanges.DataSource = exchanges;
        }

        private async Task LoadQuotes()
        {
            string selectedPair = comboBoxCurrencyPairs?.SelectedItem?.ToString();
            string selectedExchange = comboBoxExchanges?.SelectedItem?.ToString();

            if (selectedPair == null || selectedExchange == null)
                return;
            foreach (TabPage tabPage in tabControlExchanges?.TabPages)
            {
                var dataGridView = tabPage?.Controls.OfType<DataGridView>().FirstOrDefault();

                if (dataGridView == null)
                    continue;

                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    row.Cells["Pair"].Value = selectedPair;
                    switch (selectedExchange)
                    {
                        case "Binance":
                            row.Cells["Price"].Value = await BinaceExchanger.GetCurentPriceAsync(selectedPair);
                            break;
                        case "Bitget":
                            row.Cells["Price"].Value = await BitgetExchanger.GetCurentPriceAsync(selectedPair);
                            break;
                        case "Bybit":
                            row.Cells["Price"].Value = await BybitExchanger.GetCurentPriceAsync(selectedPair);
                            break;
                        case "Kucoin":
                            row.Cells["Price"].Value = await KucoinExchanger.GetCurentPriceAsync(selectedPair);
                            break;
                    }

                    }
            }
        }
        private void InitializeTimer()
        {
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 5000;
            timer.Tick += async (sender, e) =>
            {
                await LoadQuotes();
            };
        }

        private decimal GenerateRandomPrice()
        {
            Random random = new Random();
            return (decimal)(random.NextDouble() * 1000 + 1000);
        }

        private void AddRow()
        {
            var dataGridView = (DataGridView)tabControlExchanges?.SelectedTab?.Controls[0];

            if (dataGridView == null)
                return;

            foreach (string currencyPair in comboBoxCurrencyPairs?.Items)
            {
                dataGridView.Rows.Add(currencyPair, string.Empty);
            }
        }
    }
}
