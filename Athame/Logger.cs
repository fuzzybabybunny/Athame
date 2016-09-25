namespace Athame
{
    public enum MessageLevel
    {
        Info,
        Debug,
        Warning,
        Error
    }

    public abstract class Logger
    {
        public abstract void Write(MessageLevel level, string line);

        public void Info(string line)
        {
            Write(MessageLevel.Info, line);
        }

        public void Debug(string line)
        {
            Write(MessageLevel.Debug, line);
        }

        public void Warning(string line)
        {
            Write(MessageLevel.Warning, line);
        }

        public void Error(string line)
        {
            Write(MessageLevel.Error, line);
        }
    }
}
