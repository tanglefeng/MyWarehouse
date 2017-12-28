using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Newtonsoft.Json;

namespace Kengic.Was.CrossCutting.LinqToWcf.Jsons
{
    internal sealed partial class Serializer
    {
        public static void Serialize(
            JsonWriter writer,
            JsonSerializer serializer,
            Expression expression)
        {
            var s = new Serializer(writer, serializer);
            s.ExpressionInternal(expression);
        }

        private readonly JsonWriter _writer;
        private readonly JsonSerializer _serializer;

        private Serializer(JsonWriter writer, JsonSerializer serializer)
        {
            _writer = writer;
            _serializer = serializer;
        }

        private Action Serialize(object value, Type type)
        {
            return () => _serializer.Serialize(_writer, value, type);
        }

        private void Prop(string name, bool value)
        {
            _writer.WritePropertyName(name);
            _writer.WriteValue(value);
        }

        private void Prop(string name, int value)
        {
            _writer.WritePropertyName(name);
            _writer.WriteValue(value);
        }

        private void Prop(string name, string value)
        {
            _writer.WritePropertyName(name);
            _writer.WriteValue(value);
        }

        private void Prop(string name, Action valueWriter)
        {
            _writer.WritePropertyName(name);
            valueWriter();
        }

        private Action Enum<TEnum>(TEnum value)
        {
            return () => EnumInternal(value);
        }

        private void EnumInternal<TEnum>(TEnum value)
        {
            _writer.WriteValue(System.Enum.GetName(typeof(TEnum), value));
        }

        private Action Enumerable<T>(IEnumerable<T> items, Func<T, Action> func)
        {
            return () => EnumerableInternal(items, func);
        }

        private void EnumerableInternal<T>(IEnumerable<T> items, Func<T, Action> func)
        {
            if (items == null) {
                _writer.WriteNull();
            }
            else {
                _writer.WriteStartArray();
                foreach (var item in items) {
                    func(item)();
                }
                _writer.WriteEndArray();
            }
        }

        private Action Expression(Expression expression)
        {
            return () => ExpressionInternal(expression);
        }

        private void ExpressionInternal(Expression expression)
        {
            if (expression == null) {
                _writer.WriteNull();
                return;
            }

            while (expression.CanReduce) {
                expression = expression.Reduce();
            }

            _writer.WriteStartObject();

            Prop("nodeType", Enum(expression.NodeType));
            Prop("type", Type(expression.Type));

            if (BinaryExpression(expression)) { goto end; }
            if (BlockExpression(expression)) { goto end; }
            if (ConditionalExpression(expression)) { goto end; }
            if (ConstantExpression(expression)) { goto end; }
            if (DebugInfoExpression(expression)) { goto end; }
            if (DefaultExpression(expression)) { goto end; }
            if (DynamicExpression(expression)) { goto end; }
            if (GotoExpression(expression)) { goto end; }
            if (IndexExpression(expression)) { goto end; }
            if (InvocationExpression(expression)) { goto end; }
            if (LabelExpression(expression)) { goto end; }
            if (LambdaExpression(expression)) { goto end; }
            if (ListInitExpression(expression)) { goto end; }
            if (LoopExpression(expression)) { goto end; }
            if (MemberExpression(expression)) { goto end; }
            if (MemberInitExpression(expression)) { goto end; }
            if (MethodCallExpression(expression)) { goto end; }
            if (NewArrayExpression(expression)) { goto end; }
            if (NewExpression(expression)) { goto end; }
            if (ParameterExpression(expression)) { goto end; }
            if (RuntimeVariablesExpression(expression)) { goto end; }
            if (SwitchExpression(expression)) { goto end; }
            if (TryExpression(expression)) { goto end; }
            if (TypeBinaryExpression(expression)) { goto end; }
            if (UnaryExpression(expression)) { goto end; }

            throw new NotSupportedException();

        end:
            _writer.WriteEndObject();
        }
    }
}
