using SPMeta2.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Reverse.Services
{
    /// <summary>
    /// Additional fluent API for reverse filters.
    /// https://github.com/SubPointSolutions/spmeta2-reverse/issues/66
    /// </summary>
    public static class ReverseOptionsExtensions
    {
        public static ReverseOptions AddOption<TDefinition>(
            this ReverseOptions options, Expression<Func<TDefinition, bool>> filterExpression)
            where TDefinition : DefinitionBase
        {
            var definitionClassName = typeof(TDefinition).FullName;
            var existingOption = options.Options
                                        .FirstOrDefault(o => o.DefinitionClassFullName == definitionClassName);

            if (existingOption == null)
            {
                existingOption = new ReverseOption();
                existingOption.DefinitionClassFullName = definitionClassName;

                options.Options.Add(existingOption);
            }

            var parsedFilter = new ReverseOptionService().ParseOptionFilter(filterExpression);
            existingOption.Filter = parsedFilter;

            return options;
        }

        #region alternative syntax 1

        public static ReverseOptions WithWebs(
           this ReverseOptions options, Expression<Func<WebDefinition, bool>> filterExpression)
        {
            options.AddOption<WebDefinition>(filterExpression);

            return options;
        }

        public static ReverseOptions WithFields(
          this ReverseOptions options, Expression<Func<FieldDefinition, bool>> filterExpression)
        {
            options.AddOption<FieldDefinition>(filterExpression);

            return options;
        }

        public static ReverseOptions WithContentTypes(
          this ReverseOptions options, Expression<Func<ContentTypeDefinition, bool>> filterExpression)
        {
            options.AddOption<ContentTypeDefinition>(filterExpression);

            return options;
        }

        #endregion

        #region alternative syntax 2

        public static ReverseOptions Include<TDefinition>(
             this ReverseOptions options, Expression<Func<TDefinition, bool>> filterExpression)
             where TDefinition : DefinitionBase
        {
            options.AddOption<TDefinition>(filterExpression);

            return options;
        }

        #endregion
    }
}
