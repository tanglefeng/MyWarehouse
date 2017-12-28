using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;

namespace Kengic.Was.CrossCutting.LinqToWcf.Types.Anonymous
{
    /// <summary>
    ///     A class representing an anonymous type.
    ///     <seealso cref="InterLinqType" />
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "http://schemas.interlinq.com/2011/03/")]
    public class AnonymousMetaType : InterLinqType
    {
        /// <summary>
        ///     Default constructor for serialization or to create an uninitialized <see cref="AnonymousMetaType" />.
        /// </summary>
        public AnonymousMetaType()
        {
            MetaProperties = new List<AnonymousMetaProperty>();
        }

        /// <summary>
        ///     Instance an instance of the class <see cref="AnonymousMetaType" />
        /// </summary>
        /// <param name="anonymousType">The <see cref="Type" /> to generate the <see cref="AnonymousMetaType" /> from.</param>
        public AnonymousMetaType(Type anonymousType)
        {
            MetaProperties = new List<AnonymousMetaProperty>();
            Initialize(anonymousType);
        }

        /// <summary>
        ///     A <see cref="List{T}" /> with the properties of the type.
        /// </summary>
        [DataMember]
        public List<AnonymousMetaProperty> MetaProperties { get; set; }

        /// <summary>
        ///     Is allways <see langword="true" />.
        ///     <see cref="InterLinqType.IsGeneric" />
        /// </summary>
        [DataMember]
        public override bool IsGeneric
        {
            get { return true; }
            set { base.IsGeneric = value; }
        }

        /// <summary>
        ///     Returns a <see cref="IEnumerable{T}" /> containing the names of all properties.
        /// </summary>
        private IEnumerable<string> PropertyNames
        {
            get { return MetaProperties.Select(p => p.Name); }
        }

        /// <summary>
        ///     Returns a <see cref="IEnumerable{T}" /> containing the names of all parameters.
        /// </summary>
        private IEnumerable<string> GenericClassParameterNames
        {
            get { return MetaProperties.Select(p => $"<{p.Name}>j__TPar"); }
        }

        /// <summary>
        ///     Returns a <see cref="IEnumerable{T}" /> containing the names of all fields.
        /// </summary>
        private IEnumerable<string> FieldNames
        {
            get { return MetaProperties.Select(p => $"<{p.Name}>i__Field"); }
        }

        /// <summary>
        ///     Initializes the <see cref="AnonymousMetaType" />.
        /// </summary>
        /// <param name="memberInfo">The <see cref="Type" /> to generate the <see cref="AnonymousMetaType" /> from.</param>
        public override void Initialize(MemberInfo memberInfo)
        {
            var representedType = memberInfo as Type;
            if (representedType == null)
            {
                throw new ArgumentException("Not of Type 'Type'", nameof(memberInfo));
            }
            Name = representedType.Name;
            foreach (
                var property in
                    representedType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty)
                )
            {
                MetaProperties.Add(new AnonymousMetaProperty(property));
            }

            //The represented type is set here, transfered to the Server as String, 
            //recreated with the same name, and taken back with getType()
            RepresentedType = representedType;
        }

        /// <summary>
        ///     Generate the a clr type at runtime.
        /// </summary>
        /// <returns>Returns the generated <see cref="Type" />.</returns>
        protected override Type CreateClrType()
        {
            return GenerateAnonymousType(DynamicAssemblyHolder.Instance.ModuleBuilder);
        }

        /// <summary>
        ///     Overrides the equality comparision.
        /// </summary>
        /// <param name="obj">Other object to compare with.</param>
        /// <returns>True, if the other <see langword="object" /> is equal to this. False, if not.</returns>
        public override bool Equals(object obj)
        {
            if ((obj == null) || (GetType() != obj.GetType()))
            {
                return false;
            }

            if (!base.Equals(obj))
            {
                return false;
            }
            var other = (AnonymousMetaType) obj;
            if (MetaProperties.Count != other.MetaProperties.Count)
            {
                return false;
            }
            for (var i = 0; i < MetaProperties.Count; i++)
            {
                if (!MetaProperties[i].Equals(other.MetaProperties[i]))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        ///     Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for the current <see langword="object" />.</returns>
        public override int GetHashCode()
        {
            var num = 51463360;
            MetaProperties.ForEach(o => num ^= EqualityComparer<AnonymousMetaProperty>.Default.GetHashCode(o));
            return num ^ base.GetHashCode();
        }

        /// <summary>
        ///     Generate the a clr type at runtime.
        /// </summary>
        /// <returns>Returns the generated <see cref="Type" />.</returns>
        private Type GenerateAnonymousType(ModuleBuilder dynamicTypeModule)
        {
            var dynamicType = dynamicTypeModule.DefineType(string.Concat(Name, Guid.NewGuid()),
                TypeAttributes.NotPublic | TypeAttributes.Sealed | TypeAttributes.Class | TypeAttributes.BeforeFieldInit);
            var typeParameters = dynamicType.DefineGenericParameters(GenericClassParameterNames.ToArray());

            var fieldNames = FieldNames.ToArray();
            var createdFields = new List<FieldBuilder>();
            for (var i = 0; i < fieldNames.Length; i++)
            {
                var field = dynamicType.DefineField(fieldNames[i], typeParameters[i],
                    FieldAttributes.Private | FieldAttributes.InitOnly);
                var attributeType = typeof (DebuggerBrowsableAttribute);
                var attribute =
                    new CustomAttributeBuilder(attributeType.GetConstructor(new[] {typeof (DebuggerBrowsableState)}),
                        new object[] {DebuggerBrowsableState.Never});
                field.SetCustomAttribute(attribute);
                createdFields.Add(field);
            }
            var propertyNames = PropertyNames.ToArray();
            var createdProperties = new List<PropertyBuilder>();
            for (var i = 0; i < propertyNames.Length; i++)
            {
                var property = GenerateProperty(dynamicType, propertyNames[i], createdFields[i]);
                createdProperties.Add(property);
            }

            GenerateClassAttributes(dynamicType, propertyNames);

            GenerateConstructor(dynamicType, propertyNames, createdFields);
            GenerateEqualsMethod(dynamicType, createdFields.ToArray());
            GenerateGetHashCodeMethod(dynamicType, createdFields.ToArray());
            GenerateToStringMethod(dynamicType, propertyNames, createdFields.ToArray());

            var createdType = dynamicType.CreateType();
            return createdType.MakeGenericType(MetaProperties.Select(
                p => (Type) p.PropertyType.GetClrVersion()
                ).ToArray());
        }

        /// <summary>
        ///     Generate a constructor with for a type.
        /// </summary>
        /// <param name="dynamicType">A <see cref="TypeBuilder" /> generate the constructor for.</param>
        /// <param name="propertyNames"><see langword="string">strings</see> to create a constructor for.</param>
        /// <param name="fields">Fields to fill in the constructor.</param>
        private static void GenerateConstructor(TypeBuilder dynamicType, string[] propertyNames,
            IList<FieldBuilder> fields)
        {
            var dynamicConstuctor =
                dynamicType.DefineConstructor(
                    MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName |
                    MethodAttributes.RTSpecialName, CallingConventions.Standard,
                    fields.Select(f => f.FieldType).ToArray());

            var ilGen = dynamicConstuctor.GetILGenerator();

            ilGen.Emit(OpCodes.Ldarg_0);
            ilGen.Emit(OpCodes.Call, typeof (object).GetConstructor(new Type[0]));

            for (var i = 0; i < propertyNames.Length; i++)
            {
                var propertyName = propertyNames[i];
                var field = fields[i];
                var parameter = dynamicConstuctor.DefineParameter(i + 1, ParameterAttributes.None, propertyName);
                ilGen.Emit(OpCodes.Ldarg_0);
                ilGen.Emit(OpCodes.Ldarg, parameter.Position);
                ilGen.Emit(OpCodes.Stfld, field);
            }

            ilGen.Emit(OpCodes.Ret);
            AddDebuggerHiddenAttribute(dynamicConstuctor);
        }

        /// <summary>
        ///     Generate a ToString method.
        /// </summary>
        /// <param name="dynamicType">A <see cref="TypeBuilder" /> to generate a ToString method for.</param>
        /// <param name="propertyNames">The names of the properties of the type.</param>
        /// <param name="fields">Fields to read in the ToString method.</param>
        private static void GenerateToStringMethod(TypeBuilder dynamicType, string[] propertyNames,
            FieldBuilder[] fields)
        {
            var method = dynamicType.DefineMethod("ToString",
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Virtual,
                CallingConventions.Standard, typeof (string), new Type[0]);
            var ilGen = method.GetILGenerator();

            var localBuilder = ilGen.DeclareLocal(typeof (StringBuilder));

            var appendObject = typeof (StringBuilder).GetMethod("Append", BindingFlags.Instance | BindingFlags.Public,
                null, new[] {typeof (object)}, null);
            var appendString = typeof (StringBuilder).GetMethod("Append", BindingFlags.Instance | BindingFlags.Public,
                null, new[] {typeof (string)}, null);
            var sbToString = typeof (object).GetMethod("ToString", BindingFlags.Instance | BindingFlags.Public, null,
                new Type[0], null);

            ilGen.Emit(OpCodes.Newobj, typeof (StringBuilder).GetConstructor(new Type[0]));
            ilGen.Emit(OpCodes.Stloc, localBuilder);
            ilGen.Emit(OpCodes.Ldloc, localBuilder);
            ilGen.Emit(OpCodes.Ldstr, "{ ");
            ilGen.EmitCall(OpCodes.Callvirt, appendString, null);
            ilGen.Emit(OpCodes.Pop);

            var first = true;

            for (var i = 0; i < fields.Length; i++)
            {
                var field = fields[i];

                ilGen.Emit(OpCodes.Ldloc, localBuilder);
                ilGen.Emit(OpCodes.Ldstr, string.Concat(first ? "" : ", ", propertyNames[i], " = "));
                ilGen.EmitCall(OpCodes.Callvirt, appendString, null);
                ilGen.Emit(OpCodes.Pop);

                ilGen.Emit(OpCodes.Ldloc, localBuilder);
                ilGen.Emit(OpCodes.Ldarg_0);
                ilGen.Emit(OpCodes.Ldfld, field);
                ilGen.Emit(OpCodes.Box, field.FieldType);
                ilGen.EmitCall(OpCodes.Callvirt, appendObject, null);
                ilGen.Emit(OpCodes.Pop);

                first = false;
            }

            ilGen.Emit(OpCodes.Ldloc, localBuilder);
            ilGen.Emit(OpCodes.Ldstr, " }");
            ilGen.EmitCall(OpCodes.Callvirt, appendString, null);
            ilGen.Emit(OpCodes.Pop);

            ilGen.Emit(OpCodes.Ldloc, localBuilder);
            ilGen.EmitCall(OpCodes.Callvirt, sbToString, null);
            ilGen.Emit(OpCodes.Ret);

            dynamicType.DefineMethodOverride(method,
                typeof (object).GetMethod("ToString", BindingFlags.Public | BindingFlags.Instance));
            AddDebuggerHiddenAttribute(method);
        }

        /// <summary>
        ///     Generates a GetHashCode method.
        /// </summary>
        /// <param name="dynamicType">A <see cref="TypeBuilder" /> to generate a GetHashCode method for.</param>
        /// <param name="fields">Fields to read in the GetHashCode method.</param>
        private static void GenerateGetHashCodeMethod(TypeBuilder dynamicType, IEnumerable<FieldBuilder> fields)
        {
            var method = dynamicType.DefineMethod("GetHashCode",
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Virtual,
                CallingConventions.Standard, typeof (int), new Type[0]);
            var ilGen = method.GetILGenerator();

            var eqEr = typeof (EqualityComparer<>);

            var localNum = ilGen.DeclareLocal(typeof (int));

            ilGen.Emit(OpCodes.Ldc_I4, new Random(unchecked((int) DateTime.Now.Ticks)).Next(int.MaxValue));
            ilGen.Emit(OpCodes.Stloc, localNum);
            foreach (var field in fields)
            {
                ilGen.Emit(OpCodes.Ldc_I4, 0xa5555529);
                ilGen.Emit(OpCodes.Ldloc, localNum);
                ilGen.Emit(OpCodes.Mul);

                var genericEqer = eqEr.MakeGenericType(field.FieldType);
                var eqDefaultMethod =
                    eqEr.GetProperty("Default", BindingFlags.Static | BindingFlags.Public).GetGetMethod();
                var genericEqDefaultMethod = TypeBuilder.GetMethod(genericEqer, eqDefaultMethod);
                ilGen.EmitCall(OpCodes.Call, genericEqDefaultMethod, null);
                ilGen.Emit(OpCodes.Ldarg_0);
                ilGen.Emit(OpCodes.Ldfld, field);

                var theTofEqualizer = eqEr.GetGenericArguments()[0];
                var eqEqualsMethod = eqEr.GetMethod("GetHashCode", BindingFlags.Public | BindingFlags.Instance, null,
                    new[] {theTofEqualizer}, null);
                var genericEqEqualsMethod = TypeBuilder.GetMethod(genericEqer, eqEqualsMethod);
                ilGen.EmitCall(OpCodes.Callvirt, genericEqEqualsMethod, null);
                ilGen.Emit(OpCodes.Add);
                ilGen.Emit(OpCodes.Stloc, localNum);
            }

            ilGen.Emit(OpCodes.Ldloc, localNum);
            ilGen.Emit(OpCodes.Ret);

            dynamicType.DefineMethodOverride(method,
                typeof (object).GetMethod("GetHashCode", BindingFlags.Public | BindingFlags.Instance));
            AddDebuggerHiddenAttribute(method);
        }

        /// <summary>
        ///     Generates a Equals method.
        /// </summary>
        /// <param name="dynamicType">A <see cref="TypeBuilder" /> to generate a Equals method for.</param>
        /// <param name="fields">Fields to read in the GetHashCode method.</param>
        private static void GenerateEqualsMethod(TypeBuilder dynamicType, IEnumerable<FieldBuilder> fields)
        {
            var method = dynamicType.DefineMethod("Equals",
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Virtual,
                CallingConventions.Standard, typeof (bool), new[] {typeof (object)});
            var parameter = method.DefineParameter(0, ParameterAttributes.None, "value");
            var ilGen = method.GetILGenerator();

            var localType = ilGen.DeclareLocal(dynamicType);

            var falseLabel = ilGen.DefineLabel(); // IL_003a
            var endLabel = ilGen.DefineLabel(); // IL_003e

            ilGen.Emit(OpCodes.Ldarg_1);
            ilGen.Emit(OpCodes.Isinst, dynamicType);
            ilGen.Emit(OpCodes.Stloc, localType);
            ilGen.Emit(OpCodes.Ldloc, localType);

            var eqEr = typeof (EqualityComparer<>);

            foreach (var field in fields)
            {
                var genericEqer = eqEr.MakeGenericType(field.FieldType);
                var eqDefaultMethod =
                    eqEr.GetProperty("Default", BindingFlags.Static | BindingFlags.Public).GetGetMethod();
                var genericEqDefaultMethod = TypeBuilder.GetMethod(genericEqer, eqDefaultMethod);
                ilGen.Emit(OpCodes.Brfalse, falseLabel);
                ilGen.EmitCall(OpCodes.Call, genericEqDefaultMethod, null);
                ilGen.Emit(OpCodes.Ldarg_0);
                ilGen.Emit(OpCodes.Ldfld, field);
                ilGen.Emit(OpCodes.Ldloc, localType);
                ilGen.Emit(OpCodes.Ldfld, field);
                var theTofEqualizer = eqEr.GetGenericArguments()[0];
                var eqEqualsMethod = eqEr.GetMethod("Equals", BindingFlags.Public | BindingFlags.Instance, null,
                    new[] {theTofEqualizer, theTofEqualizer}, null);
                var genericEqEqualsMethod = TypeBuilder.GetMethod(genericEqer, eqEqualsMethod);
                ilGen.EmitCall(OpCodes.Callvirt, genericEqEqualsMethod, null);
            }

            ilGen.Emit(OpCodes.Br_S, endLabel);
            ilGen.MarkLabel(falseLabel);
            ilGen.Emit(OpCodes.Ldc_I4_0);
            ilGen.MarkLabel(endLabel);
            ilGen.Emit(OpCodes.Ret);

            dynamicType.DefineMethodOverride(method,
                typeof (object).GetMethod("Equals", BindingFlags.Public | BindingFlags.Instance));
            AddDebuggerHiddenAttribute(method);
        }

        /// <summary>
        ///     Generates a property.
        /// </summary>
        /// <param name="dynamicType">A <see cref="TypeBuilder" /> to generate a property for.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="field">Field to access in the property.</param>
        /// <returns>Returns the created property.</returns>
        private static PropertyBuilder GenerateProperty(TypeBuilder dynamicType, string propertyName, FieldBuilder field)
        {
            var property = dynamicType.DefineProperty(propertyName, PropertyAttributes.None, field.FieldType, null);
            var getMethod = GenerateGetMethod(dynamicType, property, field);
            property.SetGetMethod(getMethod);
            return property;
        }

        /// <summary>
        ///     Generates a Get method for a property.
        /// </summary>
        /// <param name="dynamicType">A <see cref="TypeBuilder" /> to generate a Get method for.</param>
        /// <param name="property">Property to create a get method for.</param>
        /// <param name="field">Field to access in the method.</param>
        /// <returns>Returns the created method.</returns>
        private static MethodBuilder GenerateGetMethod(TypeBuilder dynamicType, PropertyBuilder property,
            FieldBuilder field)
        {
            var method = dynamicType.DefineMethod($"get_{property.Name}",
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName);
            method.SetReturnType(field.FieldType);
            var ilGen = method.GetILGenerator();

            ilGen.Emit(OpCodes.Ldarg_0);
            ilGen.Emit(OpCodes.Ldfld, field);
            ilGen.Emit(OpCodes.Ret);

            return method;
        }

        /// <summary>
        ///     Generates attributes for a type.
        /// </summary>
        /// <param name="dynamicType">A <see cref="TypeBuilder" /> to generate the attributes for.</param>
        /// <param name="propertyNames">Names of the properties.</param>
        private static void GenerateClassAttributes(TypeBuilder dynamicType, IEnumerable<string> propertyNames)
        {
            var attributeType1 = typeof (CompilerGeneratedAttribute);
            var compilerGenAttribute = new CustomAttributeBuilder(attributeType1.GetConstructor(new Type[0]),
                new object[0]);
            dynamicType.SetCustomAttribute(compilerGenAttribute);

            var attributeType2 = typeof (DebuggerDisplayAttribute);
            var sbValue = new StringBuilder("\\{ ");
            var first = true;
            foreach (var propertyName in propertyNames)
            {
                sbValue.AppendFormat("{0}{1} = ", first ? "" : ", ", propertyName);
                sbValue.Append("{");
                sbValue.Append(propertyName);
                sbValue.Append("}");
                first = false;
            }
            sbValue.Append(" }");
            var typeProperty = attributeType2.GetProperty("Type");
            var debugDisplayAttribute =
                new CustomAttributeBuilder(attributeType2.GetConstructor(new[] {typeof (string)}),
                    new object[] {sbValue.ToString()}, new[] {typeProperty}, new object[] {"<Anonymous Type>"});
            dynamicType.SetCustomAttribute(debugDisplayAttribute);
        }

        /// <summary>
        ///     Adds a <see cref="DebuggerHiddenAttribute" /> to a method.
        /// </summary>
        /// <param name="method">The method to add the attribute.</param>
        private static void AddDebuggerHiddenAttribute(MethodBuilder method)
        {
            var attributeType = typeof (DebuggerHiddenAttribute);
            var attribute = new CustomAttributeBuilder(attributeType.GetConstructor(new Type[0]), new object[0]);
            method.SetCustomAttribute(attribute);
        }

        /// <summary>
        ///     Adds a <see cref="DebuggerHiddenAttribute" /> to a constructor.
        /// </summary>
        /// <param name="constructor">The constructor to add the attribute.</param>
        private static void AddDebuggerHiddenAttribute(ConstructorBuilder constructor)
        {
            var attributeType = typeof (DebuggerHiddenAttribute);
            var attribute = new CustomAttributeBuilder(attributeType.GetConstructor(new Type[0]), new object[0]);
            constructor.SetCustomAttribute(attribute);
        }
    }
}