using DiscordRfid.Controllers;
using DiscordRfid.Models;
using DiscordRfid.Views.Controls;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace DiscordRfid.Views
{
    public class EmployeesForm : ModelGridDialog<Employee>
    {
        public EmployeesForm()
        {
            Toolbox.Add(new ModelGridButton("RFID Tags", OnRfidTagsClick)
            { 
                DisableIfModelNotSelected = true
            });
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            BaseController<RfidTag>.ModelCreated += OnRfidTagCreated;
            BaseController<RfidTag>.ModelUpdated += OnRfidTagUpdated;
            BaseController<RfidTag>.ModelDeleted += OnRfidTagDeleted;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            BaseController<RfidTag>.ModelCreated -= OnRfidTagCreated;
            BaseController<RfidTag>.ModelUpdated -= OnRfidTagUpdated;
            BaseController<RfidTag>.ModelDeleted -= OnRfidTagDeleted;
        }

        private void OnRfidTagCreated(RfidTag obj) => Grid.Reload();
        private void OnRfidTagUpdated(RfidTag oldState, RfidTag newState) => Grid.Reload();
        private void OnRfidTagDeleted(RfidTag obj) => Grid.Reload();

        protected virtual void OnRfidTagsClick(MouseEventArgs e)
        {
            using (var dlg = new RfIdTagsForm(SelectedModel) { Width = 600, Height = 300 })
            {
                dlg.ShowDialog();
            }
        }
    }
}
