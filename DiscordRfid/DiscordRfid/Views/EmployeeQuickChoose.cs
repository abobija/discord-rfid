using DiscordRfid.Controllers;
using DiscordRfid.Models;
using DiscordRfid.Services;
using DiscordRfid.Views.Controls;
using System;
using System.Windows.Forms;

namespace DiscordRfid.Views
{
    public class EmployeeQuickChoose : Dialog
    {
        public Employee Employee { get; private set; }
        private ListView EmployeesListView { get; set; }

        public EmployeeQuickChoose()
        {
            Text = "Choose Employee";

            EmployeesListView = new BaseListView
            {
                Dock = DockStyle.Fill
            };

            EmployeesListView.Columns.AddRange(new ColumnHeader[]
            {
                new ColumnHeader { Text = "", Width = Width - 30 }
            });

            AddControl(EmployeesListView);
        }

        protected void AddEmployeesSafe(Employee[] employees)
        {
            void add()
            {
                SuspendLayout();
                foreach(var e in employees)
                {
                    EmployeesListView.Items.Add(new EmployeeListViewItem(e));
                }
                ResumeLayout();
            }

            if(InvokeRequired)
            {
                Invoke(new MethodInvoker(add));
            }
            else
            {
                add();
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            try
            {
                using (var con = Database.Instance.CreateConnection())
                {
                    con.Open();
                    AddEmployeesSafe(new EmployeeController(con).Get());
                }
            }
            catch(Exception ex)
            {
                this.Error(ex);
            }
        }

        protected override void OnDialogSaveClick(MouseEventArgs e)
        {
            var selections = EmployeesListView.SelectedItems;

            if (selections.Count <= 0)
            {
                this.Information("Please select employee");
                return;
            }

            Employee = (selections[0] as EmployeeListViewItem).Employee;

            base.OnDialogSaveClick(e);
        }
    }

    public class EmployeeListViewItem : ListViewItem
    {
        public Employee Employee { get; private set; }

        public EmployeeListViewItem(Employee employee) : base(new string[1])
        {
            Employee = employee;
            SubItems[0].Text = $"{Employee.FirstName} {Employee.LastName} [Tags: {Employee.TagsCount}]";
        }
    }
}
