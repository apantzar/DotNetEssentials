using Serilog.Core;
using Serilog.Events;
using Serilog;

namespace ApiTests.Configuration
{
    public class LoggingTests
    {
        [Fact]
        public void Serilog_IsConfigured_Correctly()
        {
            // Arrange
            var logEvents = new List<LogEvent>();
            var sink = new DelegatingSink(logEvents);
            var logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Sink(sink)
                .CreateLogger();

            // Act
            logger.Information("Test log message");

            // Assert
            Assert.Single(logEvents);
            Assert.Equal("Test log message", logEvents[0].MessageTemplate.Text);
        }

        private class DelegatingSink : ILogEventSink
        {
            private readonly List<LogEvent> _logEvents;

            public DelegatingSink(List<LogEvent> logEvents)
            {
                _logEvents = logEvents;
            }

            public void Emit(LogEvent logEvent)
            {
                _logEvents.Add(logEvent);
            }
        }
    }
}
