using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Book.API.Commands.V1;
using FluentValidation;
using FluentValidation.Validators;

namespace Book.API.Extensions
{
    public static class FluentValidationExtensions
    {
        public static IRuleBuilderOptions<T, IEnumerable<TElement>> AllValuesMustExistsInDb<T, TElement, TValue>(this IRuleBuilder<T, IEnumerable<TElement>> ruleBuilder, string sourcePropToCompare, Task<IEnumerable<TValue>> source)
        {
            return ruleBuilder.Must((rootObject, list, context) =>
                {
                    var sourceResult = source.GetAwaiter().GetResult()
                        .Where(element => list.Contains((TElement)element.GetType().GetProperty(sourcePropToCompare)?.GetValue(element)) );
                    
                    var available = new List<TElement>();
                    foreach (var resultElement in sourceResult)
                    {
                        var availableEl = (TElement) resultElement.GetType().GetProperty(sourcePropToCompare)?.GetValue(resultElement);
                        available.Add(availableEl);
                    }
                    
                    
                    var missing = list.Except(available);
                    context.MessageFormatter.AppendArgument("Missing", string.Join(", ", missing));
                    return sourceResult.Count().Equals(list.Count());

                })
                .WithMessage("{PropertyName} contains elements that do not exist in Db: {Missing} .");
        }
        
        
        public static IRuleBuilderOptions<T, TElement> ValueMustExistsInDb<T, TElement, TValue>(this IRuleBuilder<T, TElement> ruleBuilder, Func<TElement, Task<TValue>> sourceResultPredicate)
        {
            return ruleBuilder.Must((rootObject, element, context) =>
                {
                    if (element is null)
                        return true;
                    
                    var sourceResult = sourceResultPredicate.Invoke(element).GetAwaiter().GetResult();
                    if (sourceResult is null)
                    {
                        context.MessageFormatter.AppendArgument("Missing", string.Join(", ", element));
                        return false;
                    }
                    
                    return true;
                })
                .WithMessage("{PropertyName} contains element that do not exist in Db: {Missing} .");
        }

        private static bool IsRequestedValueExistsInDb<TElement>(object sourceValueToCompare, TElement element)
        {
            return sourceValueToCompare.Equals(element);
        }
    }
}