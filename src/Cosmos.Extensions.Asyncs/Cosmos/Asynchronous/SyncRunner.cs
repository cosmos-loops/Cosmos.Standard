﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Cosmos.Exceptions;

#if !NET451
using Nito.AsyncEx.Synchronous;
#endif

namespace Cosmos.Asynchronous
{
    /// <summary>
    /// Sync runner
    /// </summary>
    public static class SyncRunner
    {
        /// <summary>
        /// For asynchronous calling
        /// </summary>
        /// <param name="task"></param>
        /// <param name="cancellationToken"></param>
        public static void ForAsynchronousCalling(Task task, CancellationToken cancellationToken = default)
        {
            if (task is null)
                throw new ArgumentNullException(nameof(task));

            task.WaitAndUnwrapException(cancellationToken);
        }

        /// <summary>
        /// For asynchronous calling
        /// </summary>
        /// <param name="taskFunc"></param>
        /// <param name="cancellationToken"></param>
        public static void ForAsynchronousCalling(Func<Task> taskFunc, CancellationToken cancellationToken = default)
        {
            if (taskFunc is null)
                throw new ArgumentNullException(nameof(taskFunc));

            Try.Create(taskFunc).GetValue().WaitAndUnwrapException(cancellationToken);
        }

        /// <summary>
        /// For asynchronous calling safety
        /// </summary>
        /// <param name="task"></param>
        /// <param name="cancellationToken"></param>
        public static void ForAsynchronousCallingSafety(Task task, CancellationToken cancellationToken = default)
        {
            task?.WaitWithoutException(cancellationToken);
        }

        /// <summary>
        /// For asynchronous calling safety
        /// </summary>
        /// <param name="taskFunc"></param>
        /// <param name="cancellationToken"></param>
        public static void ForAsynchronousCallingSafety(Func<Task> taskFunc, CancellationToken cancellationToken = default)
        {
            Try.Create(taskFunc).GetSafeValue()?.WaitWithoutException(cancellationToken);
        }

        /// <summary>
        /// For asynchronous calling safety and forget
        /// </summary>
        /// <param name="task"></param>
        /// <param name="exceptionAction"></param>
        public static void ForAsynchronousCallingSafetyAndForget(Task task, Action<Exception> exceptionAction = null)
        {
            task?.SafeFireAndForget(exceptionAction);
        }

        /// <summary>
        /// For asynchronous calling safety and forget
        /// </summary>
        /// <param name="taskFunc"></param>
        /// <param name="exceptionAction"></param>
        public static void ForAsynchronousCallingSafetyAndForget(Func<Task> taskFunc, Action<Exception> exceptionAction = null)
        {
            taskFunc?.Invoke()?.SafeFireAndForget(exceptionAction);
        }

        /// <summary>
        /// From asynchronous calling
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="task"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static TResult FromAsynchronousCalling<TResult>(Task<TResult> task, CancellationToken cancellationToken = default)
        {
            if (task is null)
                throw new ArgumentNullException(nameof(task));

            return task.WaitAndUnwrapException(cancellationToken);
        }

        /// <summary>
        /// From asynchronous calling
        /// </summary>
        /// <param name="taskFunc"></param>
        /// <param name="cancellationToken"></param>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static TResult FromAsynchronousCalling<TResult>(Func<Task<TResult>> taskFunc, CancellationToken cancellationToken = default)
        {
            if (taskFunc is null)
                throw new ArgumentNullException(nameof(taskFunc));

            var task = Try.Create(taskFunc).GetValue();

            if (task is null)
                throw new InvalidOperationException($"The task factory {nameof(taskFunc)} failed to run.");

            return task.WaitAndUnwrapException(cancellationToken);
        }

        /// <summary>
        /// From asynchronous calling safety
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="task"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static TResult FromAsynchronousCallingSafety<TResult>(Task<TResult> task, CancellationToken cancellationToken = default)
        {
            return FromAsynchronousCallingSafety(task, default(TResult), cancellationToken);
        }

        /// <summary>
        /// From asynchronous calling safety
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="taskFunc"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static TResult FromAsynchronousCallingSafety<TResult>(Func<Task<TResult>> taskFunc, CancellationToken cancellationToken = default)
        {
            return FromAsynchronousCallingSafety(taskFunc, default(TResult), cancellationToken);
        }

        /// <summary>
        /// From asynchronous calling safety
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="task"></param>
        /// <param name="defaultValue"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static TResult FromAsynchronousCallingSafety<TResult>(Task<TResult> task, TResult defaultValue, CancellationToken cancellationToken = default)
        {
            if (task is null)
                return defaultValue;

            return Try
               .Create(() => FromAsynchronousCalling(task, cancellationToken))
               .GetSafeValue(defaultValue);
        }

        /// <summary>
        /// From asynchronous calling safety
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="taskFunc"></param>
        /// <param name="defaultValue"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static TResult FromAsynchronousCallingSafety<TResult>(Func<Task<TResult>> taskFunc, TResult defaultValue, CancellationToken cancellationToken = default)
        {
            if (taskFunc is null)
                return defaultValue;

            var task = Try.Create(taskFunc).GetSafeValue();

            if (task is null)
                return defaultValue;

            return Try
               .Create(() => FromAsynchronousCalling(task, cancellationToken))
               .GetSafeValue(defaultValue);
        }

        /// <summary>
        /// From asynchronous calling safety
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="task"></param>
        /// <param name="defaultValueFunc"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static TResult FromAsynchronousCallingSafety<TResult>(Task<TResult> task, Func<TResult> defaultValueFunc, CancellationToken cancellationToken = default)
        {
            if (defaultValueFunc is null)
                return default;

            if (task is null)
                return defaultValueFunc();

            return Try
               .Create(() => FromAsynchronousCalling(task, cancellationToken))
               .GetSafeValue(defaultValueFunc);
        }

        /// <summary>
        /// From asynchronous calling safety
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="taskFunc"></param>
        /// <param name="defaultValueFunc"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static TResult FromAsynchronousCallingSafety<TResult>(Func<Task<TResult>> taskFunc, Func<TResult> defaultValueFunc, CancellationToken cancellationToken = default)
        {
            if (defaultValueFunc is null)
                return default;

            if (taskFunc is null)
                return defaultValueFunc();

            var task = Try.Create(taskFunc).GetSafeValue();

            if (task is null)
                return defaultValueFunc();

            return Try
               .Create(() => FromAsynchronousCalling(task, cancellationToken))
               .GetSafeValue(defaultValueFunc);
        }
    }
}