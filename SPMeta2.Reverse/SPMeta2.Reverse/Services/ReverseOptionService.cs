using SPMeta2.Definitions;
using SPMeta2.Reverse.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Reverse.Services
{

    public class ReverseOptionService
    {
        public ReverseFilter ParseOptionFilter<TDefinition>(Expression<Func<TDefinition, bool>> expressions)
            where TDefinition : DefinitionBase
        {
            var filters = ParseOptionFilters(new Expression<Func<TDefinition, bool>>[] { expressions });

            if (!filters.Any())
            {
                throw new SPMeta2ReverseException("Cannot parse reverse filters. Expected more than one, got zero.");
            }

            return filters.First();
        }

        public List<ReverseFilter> ParseOptionFilters<TDefinition>(IEnumerable<Expression<Func<TDefinition, bool>>> expressions)
            where TDefinition : DefinitionBase
        {
            // TODO
            // crazy little sketches, must be covered by unit tests

            var result = new List<ReverseFilter>();

            foreach (var exp in expressions)
            {
                var body = exp.Body;

                if (exp.Body is BinaryExpression)
                {
                    var binaryBody = exp.Body as BinaryExpression;

                    var propExp = binaryBody.Left as MemberExpression;

                    var propName = propExp.Member.Name;
                    var propValue = string.Empty;

                    if (binaryBody.Right is ConstantExpression)
                    {
                        propValue = (binaryBody.Right as ConstantExpression).Value.ToString();

                    }
                    else
                    {
                        var objectMember = Expression.Convert(binaryBody.Right, typeof(object));

                        var getterLambda = Expression.Lambda<Func<object>>(objectMember);

                        var getter = getterLambda.Compile();
                        propValue = getter().ToString();
                    }

                    var operationType = (ReverseFilterOperationType)Enum.Parse(
                        typeof(ReverseFilterOperationType),
                        body.NodeType.ToString());

                    var filter = new ReverseFilter
                    {
                        Operation = operationType,
                        PropertyName = propName,
                        PropertyValue = propValue
                    };

                    result.Add(filter);
                }
                else if (exp.Body is MethodCallExpression)
                {
                    var binaryBody = exp.Body as MethodCallExpression;

                    var propExp = binaryBody.Object as MemberExpression;
                    //var valueExp = binaryBody.Method.Name;

                    var propName = propExp.Member.Name;

                    var argument = binaryBody.Arguments.FirstOrDefault();
                    var propValue = argument.ToString();

                    if (argument.NodeType == ExpressionType.MemberAccess)
                    {
                        var l = Expression.Lambda<Func<object>>(argument);
                        propValue = l.Compile()().ToString();
                    }

                    var operationType = (ReverseFilterOperationType)Enum.Parse(
                        typeof(ReverseFilterOperationType),
                        binaryBody.Method.Name);

                    var filter = new ReverseFilter
                    {
                        Operation = operationType,
                        PropertyName = propName,
                        PropertyValue = propValue
                    };

                    result.Add(filter);
                }
                else
                {
                    throw new Exception("unsupported exp.Body");
                }
            }

            return result;
        }

    }
}
