namespace NTT.Exceptions.ExceptionTypes;

[Serializable]
public class OperationalException : Exception
{
    public OperationalException()
    {
    }

    public OperationalException(string message) : base(message)
    {
    }

    public OperationalException(string message, Exception innerException) : base(message, innerException)
    {
    }
}