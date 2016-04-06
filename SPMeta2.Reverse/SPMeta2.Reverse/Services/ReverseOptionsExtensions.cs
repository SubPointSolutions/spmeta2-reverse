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
        #region core API

        public static ReverseOptions AddFilterOption<TDefinition>(
           this ReverseOptions options, Expression<Func<TDefinition, bool>> filterExpression)
           where TDefinition : DefinitionBase
        {
            var definitionClassName = typeof(TDefinition).FullName;
            var existingOption = options.Options
                                        .FirstOrDefault(o =>
                                            o.DefinitionClassFullName == definitionClassName
                                            && o is ReverseFilterOption) as ReverseFilterOption;

            if (existingOption == null)
            {
                existingOption = new ReverseFilterOption();
                existingOption.DefinitionClassFullName = definitionClassName;

                options.Options.Add(existingOption);
            }

            var parsedFilter = new ReverseOptionService().ParseOptionFilter(filterExpression);
            existingOption.Filter = parsedFilter;

            return options;
        }

        public static ReverseOptions AddDepthOption<TDefinition>(
           this ReverseOptions options, int depth)
           where TDefinition : DefinitionBase
        {
            var definitionClassName = typeof(TDefinition).FullName;
            var existingOption = options.Options
                                        .FirstOrDefault(o =>
                                            o.DefinitionClassFullName == definitionClassName
                                            && o is ReverseDepthOption) as ReverseDepthOption;

            if (existingOption == null)
            {
                existingOption = new ReverseDepthOption();
                existingOption.DefinitionClassFullName = definitionClassName;

                options.Options.Add(existingOption);
            }

            existingOption.Depth = depth;

            return options;
        }

        #endregion

        #region alternative syntax 1

        public static ReverseOptions WithWebs(
           this ReverseOptions options, Expression<Func<WebDefinition, bool>> filterExpression)
        {
            options.AddFilterOption<WebDefinition>(filterExpression);

            return options;
        }

        public static ReverseOptions WithFields(
          this ReverseOptions options, Expression<Func<FieldDefinition, bool>> filterExpression)
        {
            options.AddFilterOption<FieldDefinition>(filterExpression);

            return options;
        }

        public static ReverseOptions WithContentTypes(
          this ReverseOptions options, Expression<Func<ContentTypeDefinition, bool>> filterExpression)
        {
            options.AddFilterOption<ContentTypeDefinition>(filterExpression);

            return options;
        }

        #endregion

        #region alternative syntax 2

        public static ReverseOptions Include<TDefinition>(
             this ReverseOptions options, Expression<Func<TDefinition, bool>> filterExpression)
             where TDefinition : DefinitionBase
        {
            options.AddFilterOption<TDefinition>(filterExpression);

            return options;
        }

        #endregion
    }
}
