using Microsoft.Extensions.Logging;

namespace FileDetailAPI.LoggerManager
{

  public interface ILoggerManager
  {
    void LogInformation(string message);
    void LogDebug(string message);
    void LogWarn(string message);
    void LogError(string message);
  }

  public class LoggerManager : ILoggerManager
  {
    private readonly ILogger<LoggerManager> _logger;

    public LoggerManager(ILogger<LoggerManager> logger)
    {
      _logger = logger;
    }

    public void LogInformation(string message)
    {
      _logger.LogInformation(message);
    }

    public void LogDebug(string message)
    {
      _logger.LogDebug(message);
    }

    public void LogWarn(string message)
    {
      _logger.LogWarning(message);
    }

    public void LogError(string message)
    {
      _logger.LogError(message);
    }


  }


}
