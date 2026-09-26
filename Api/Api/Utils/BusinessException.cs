namespace Api.Utils
{
  public class BusinessException : Exception
  {
    public BusinessException(string message) : base(message)
    {
    }
  }
}
