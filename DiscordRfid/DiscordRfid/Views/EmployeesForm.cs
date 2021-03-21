using DiscordRfid.Controllers;
using DiscordRfid.Models;
using DiscordRfid.Services;
using DiscordRfid.Views.Controls;
using System;
using System.Linq;
using System.Windows.Forms;

namespace DiscordRfid.Views
{
    public partial class EmployeesForm : DialogForm
    {
        public EmployeesForm()
        {
            InitializeComponent();

            Shown += (o, e) => ReloadGrid();

            BaseController<Employee>.ModelCreated += OnEmployeeCreated;
            BaseController<Employee>.ModelUpdated += OnEmployeeUpdated;
            BaseController<Employee>.ModelDeleted += OnEmployeesDeleted;

            FormClosing += (o, e) =>
            {
                BaseController<Employee>.ModelCreated -= OnEmployeeCreated;
                BaseController<Employee>.ModelUpdated -= OnEmployeeUpdated;
                BaseController<Employee>.ModelDeleted -= OnEmployeesDeleted;
            };
        }

        private void OnEmployeeCreated(Employee employee) => ReloadGrid();
        private void OnEmployeeUpdated(Employee o, Employee n) => ReloadGrid();
        private void OnEmployeesDeleted(Employee employee) => ReloadGrid();


        private void ReloadGrid()
        {
            using (var con = Database.Instance.CreateConnection())
            {
                con.Open();
                DataGridView.DataSource = new EmployeeController(con).Get(orderBy: "Id DESC").ToList();

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

        private Employee SelectedEmployee
        {
            get
            {
                var srows = DataGridView.SelectedRows;

                if (srows.Count <= 0)
                    return null; ;

                return srows[0].DataBoundItem as Employee;
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            var employee = SelectedEmployee;

            if (employee == null)
                return;

            using (var frm = new EmployeeForm(employee))
            {
                frm.ShowDialog();
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            var employee = SelectedEmployee;

            if (employee == null)
                return;

            if (this.Question(
                $"Are you sure that you want to delete selected Employee?{Environment.NewLine}{Environment.NewLine}Employee: {employee}"
                ) == DialogResult.Yes)
            {
                using (var con = Database.Instance.CreateConnection())
                {
                    con.Open();
                    new EmployeeController(con).Delete(employee);
                }
            }
        }
    }
}
