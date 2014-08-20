
namespace Server.ContextMenus
{
    public delegate void ContextCallback();

    public class CallbackEntry : ContextMenuEntry
    {
        private ContextCallback _callback;

        public CallbackEntry( int number, ContextCallback callback ) : this(number, -1, callback) { }

        public CallbackEntry( int number, int range, ContextCallback callback )
            : base(number, range)
        {
            _callback = callback;
        }

        public override void OnClick()
        {
            if( _callback != null )
                _callback();
        }
    }
}