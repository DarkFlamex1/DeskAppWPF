using CommunityToolkit.Mvvm.Messaging.Messages;

namespace DeskAppWPF.Messages
{
    public class NavigationMessage : ValueChangedMessage<string>
    {
        public NavigationMessage(string value) : base(value)
        {
        }
    }
}
