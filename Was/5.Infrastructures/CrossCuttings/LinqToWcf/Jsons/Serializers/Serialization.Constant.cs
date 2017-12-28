using System.Linq;
using System.Linq.Expressions;

namespace Kengic.Was.CrossCutting.LinqToWcf.Jsons
{
    partial class Serializer
    {
        private bool ConstantExpression(Expression expr)
        {
            var expression = expr as ConstantExpression;
            if (expression == null) { return false; }

            Prop("typeName", "constant");
            if (expression.Value == null)
            {
                Prop("value", () => _writer.WriteNull());
            }
            else {
                var value = expression.Value;
                var type = value.GetType();
                Prop("value", () =>
                {
                    _writer.WriteStartObject();
                    Prop("type", Type(type));
                    if (typeof(IQueryable).IsAssignableFrom(type))
                    {
                        Prop("value", Serialize(value.ToString(), type));
                    }
                    else
                    {
                        Prop("value", Serialize(value, type));
                    }
                    _writer.WriteEndObject();
                });
            }

            return true;
        }
    }
}
