using Microsoft.Data.Sqlite;
using Serilog;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiscordRfid
{
    public static class Extensions
    {
        public static int GetInt32ByName(this SqliteDataReader reader, string columnName)
        {
            return reader.GetInt32(reader.GetOrdinal(columnName));
        }

        public static Task ContinueWithNoop(this Task task)
        {
            return task.ContinueWith(t => { });
        }

        public static DialogResult Error(this Form parent, string message)
        {
            return (DialogResult) parent.Invoke((Func<DialogResult>)(() => 
                    MessageBox.Show(parent, message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                ));
        }

        public static DialogResult Error(this Form parent, Exception ex)
        {
            return (DialogResult)parent.Invoke((Func<DialogResult>)(() =>
                   MessageBox.Show(parent, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                ));
        }

        public static DialogResult Question(this Form parent, string message)
        {
            return (DialogResult) parent.Invoke((Func<DialogResult>)(() =>
                   MessageBox.Show(parent, message, parent.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                ));
        }

        public static DialogResult Information(this Form parent, string message)
        {
            return (DialogResult)parent.Invoke((Func<DialogResult>)(() =>
                  MessageBox.Show(parent, message, parent.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                ));
        }

        public static Task NoopTask()
        {
            return Task.FromResult<object>(null);
        }

        public static void SafeClose(this Form form)
        {
            form.Invoke(new MethodInvoker(delegate { form.Close(); }));
        }
    }
}
