namespace StoreApi.Shared.Services.ExceptionSecurityChecker;

/// <summary>
/// This entity verify if the exception contains any
/// sensitive information that cannot be sended to the client,
/// also it verify if the exception type is allowed to be sended
/// to the client or some front-end
/// </summary>
public interface IExceptionSecurityChecker{
    
    /// <summary>
    /// Check if exception is secure
    /// </summary>
    /// <param name="_exception">exception to be checked</param>
    /// <returns>If the exception no has sensitive information, return true</returns>
    public bool IsSecure(Exception _exception);

    /// <summary>
    /// Add exception type to the trust list
    /// </summary>
    /// <param name="exceptionType">Type of exception that will be trusted</param>
    public void TrustExceptionType(Type exceptionType);

    /// <summary>
    /// Remove exception type from trusted list
    /// </summary>
    /// <param name="exceptionType">Type of exception that will be removed</param>
    public void BlockExceptionType(Type exceptionType);
}