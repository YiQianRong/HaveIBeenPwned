// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Text;
using Microsoft.Extensions.Logging;
using Moq;

namespace HaveIBeenPwned.Service
{
    /// <summary>
    /// MockHelpers
    /// </summary>
    public static class MockHelpers
    {
        /// <summary>
        /// LogMessage
        /// </summary>
        public static StringBuilder LogMessage = new StringBuilder();

        /// <summary>
        /// MockILogger
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="logStore"></param>
        /// <returns></returns>
        public static Mock<ILogger<T>> MockILogger<T>(StringBuilder logStore = null) where T : class
        {
            logStore = logStore ?? LogMessage;
            var logger = new Mock<ILogger<T>>();
            logger.Setup(x => x.Log(It.IsAny<LogLevel>(), It.IsAny<EventId>(), It.IsAny<object>(),
                It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()))
                .Callback((LogLevel logLevel, EventId eventId, object state, Exception exception, Func<object, Exception, string> formatter) =>
                {
                    if (formatter == null)
                    {
                        logStore.Append(state.ToString());
                    }
                    else
                    {
                        logStore.Append(formatter(state, exception));
                    }
                    logStore.Append(" ");
                });
            logger.Setup(x => x.BeginScope(It.IsAny<object>())).Callback((object state) =>
                {
                    logStore.Append(state.ToString());
                    logStore.Append(" ");
                });
            logger.Setup(x => x.IsEnabled(LogLevel.Debug)).Returns(true);
            logger.Setup(x => x.IsEnabled(LogLevel.Warning)).Returns(true);

            return logger;
        }

        /// <summary>
        /// StubLogger
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static ILogger<T> StubLogger<T>()
        {
            var stub = new Mock<ILogger<T>>();
            return stub.Object;
        }
    }
}