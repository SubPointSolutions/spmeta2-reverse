using SPMeta2.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Reverse.Services
{
    public class ReverseFilter
    {
        public string PropertyName { get; set; }
        public string PropertyValue { get; set; }
        public string Operation { get; set; }

        public override string ToString()
        {
            return string.Format("Where '{0}' {1} '{2}'", PropertyName, Operation, PropertyValue);
        }
    }

    public enum ReverseFilterOperation
    {

    }

    public class ReverseOptionService
    {
        public List<ReverseFilter> ParseOptionFilters<TDefinition>(List<Expression<Func<TDefinition, bool>>> expressions)
            where TDefinition : DefinitionBase
        {
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

                    var operationType = body.NodeType.ToString();

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
                    var propValue = binaryBody.Arguments.FirstOrDefault().ToString();

                    var operationType = binaryBody.Method.Name;

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
