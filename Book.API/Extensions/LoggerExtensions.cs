using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace Book.API.Extensions
{
    public static class LoggerExtensions
    {
        public static void LogInfoMethodStarted<TResponseMethodType>(this ILogger logger,
            Type methodClass, string methodName, object[] methodParams)
        {
            var methodNameFromClass = methodClass.GetMethod(methodName)?.Name;
            if (methodNameFromClass is null)
                throw new ArgumentException($"Passed MethodName({methodName}) not exists for that class({methodClass.FullName})");
            
            
            logger.LogInformation("{MethodClass}: {MethodName} : Requesting result {ResultType} with params {@MethodParams} ", 
                methodClass.FullName, methodNameFromClass , typeof(TResponseMethodType), methodParams);
        }
        
        public static void LogInfoMethodEnded<TResponse>(this ILogger logger,
            Type methodClass, string methodName, TResponse result)
        {
            var methodNameFromClass = methodClass.GetMethod(methodName)?.Name;
            if (methodNameFromClass is null)
                throw new ArgumentException($"Passed MethodName({methodName}) not exists for that class({methodClass.FullName})");
            
            // if (typeof(TResponse) == typeof(IEnumerable<Domain.Book>))
            if (result is ICollection collectionResult)
            {
                logger.LogInformation("{MethodClass}: {MethodName} : Request returned {ResultType} Count {Count}", 
                    methodClass.FullName, methodNameFromClass, typeof(TResponse), collectionResult.Count);
            }
            else
            {
                logger.LogInformation(
                    "{MethodClass}: {MethodName} : Request returned {ResultType} : {@Result}",
                    methodClass.FullName, methodNameFromClass, typeof(TResponse), result);
            }
        }
        
        public static void LogWarningNotFound<TResponseMethodType>(this ILogger logger,
            Type methodClass, string methodName, object[] methodParams)
        {
            var methodNameFromClass = methodClass.GetMethod(methodName)?.Name;
            if (methodNameFromClass is null)
                throw new ArgumentException($"Passed MethodName({methodName}) not exists for that class({methodClass.FullName})");

            if ( typeof(TResponseMethodType).GetInterface(nameof(IEnumerable)) != null)
            {
                logger.LogWarning("{MethodClass}: {MethodName} : Requested {ResultType} not found", 
                    methodClass.FullName, methodNameFromClass , typeof(TResponseMethodType));
            }
            else
            {
                logger.LogWarning("{MethodClass}: {MethodName}: Requested {ResultType} {@MethodParams} not found", 
                    methodClass.FullName, methodNameFromClass , typeof(TResponseMethodType), methodParams);
            }
        }
        
        public static void LogInfoCreateMethodStarted<TResponseMethodType>(this ILogger logger,
            Type methodClass, string methodName, object[] methodParams)
        {
            var methodNameFromClass = methodClass.GetMethod(methodName)?.Name;
            if (methodNameFromClass is null)
                throw new ArgumentException($"Passed MethodName({methodName}) not exists for that class({methodClass.FullName})");

            logger.LogInformation("{MethodClass}: {MethodName}: Requesting adding new {ResultType} : Value {@MethodParams}", 
                methodClass.FullName, methodNameFromClass , typeof(TResponseMethodType), methodParams);
        }
        
        
        public static void LogInfoCreateMethodEnded<TResponse>(this ILogger logger, 
            Type methodClass, string methodName, TResponse result)
        {
            var methodNameFromClass = methodClass.GetMethod(methodName)?.Name;
            if (methodNameFromClass is null)
                throw new ArgumentException($"Passed MethodName({methodName}) not exists for that class({methodClass.FullName})");
            
            logger.LogInformation("{MethodClass}: {MethodName}: Added new {ResultType} to be tracked by EF with Value {@Result}", 
                methodClass.FullName, methodNameFromClass , typeof(TResponse), result);
        }
        
        
        public static void LogWarningCreateMethodNullData<TResponseMethodType>(this ILogger logger,
            Type methodClass, string methodName)
        {
            var methodNameFromClass = methodClass.GetMethod(methodName)?.Name;
            if (methodNameFromClass is null)
                throw new ArgumentException($"Passed MethodName({methodName}) not exists for that class({methodClass.FullName})");
            
            logger.LogWarning("{MethodClass}: {MethodName}: {ResultType} to add is {MethodParam}", 
                methodClass.FullName, methodNameFromClass , typeof(TResponseMethodType), null);
        }
        
    }
}