using System;
namespace WpfApp3.Controls
{
    public enum CheckedType
    {
        Up,
        Down
    }
    public class SwitchEventArgs : EventArgs
    {
        public CheckedType CheckedType { get; set; }
    }
}
