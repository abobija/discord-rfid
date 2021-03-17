using System.Threading.Tasks;

namespace DiscordRfid
{
    public static class Extensions
    {
        public static Task ContinueWithNoop(this Task task)
        {
            return task.ContinueWith(t => { });
        }
    }
}
