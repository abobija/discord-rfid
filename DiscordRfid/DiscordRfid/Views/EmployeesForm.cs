using DiscordRfid.Controllers;
using DiscordRfid.Models;
using DiscordRfid.Services;
using DiscordRfid.Views.Controls;
using System;
using System.Windows.Forms;

namespace DiscordRfid.Views
{
    public partial class EmployeesForm : DialogForm
    {
        public EmployeesForm()
        {
            InitializeComponent();

            Database.Instance.ModelCreated += OnEmployeeCreated;
            Shown += (o, e) => ReloadGrid();
            FormClosing += (o, e) => Database.Instance.ModelCreated -= OnEmployeeCreated;
        }

        private void OnEmployeeCreated(IModel model)
        {
            if(model is Employee)
            {
                ReloadGrid();
            }
        }

        private void ReloadGrid()
        {
            using (var con = Database.Instance.CreateConnection())
            {
                con.Open();
                var ctrl = new EmployeeController(con);
                DataGridView.DataSource = ctrl.Get(orderBy: "Id DESC");

                var lastColumn = DataGridView.Columns[DataGridView.ColumnCount - 1];
                lastColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                lastColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            using (var frm = new EmployeeForm())
            {
                frm.ShowDialog();
            }
        }
    }
}
