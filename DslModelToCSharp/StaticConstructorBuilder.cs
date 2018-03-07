using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using DslModel;

namespace DslModelToCSharp
{
    public class StaticConstructorBuilder : IStaticConstructorBuilder
    {
        public CodeMemberMethod BuildOkResult(IList<Property> props, IList<Property> propsInMethod, string name, string genericType = "", string genericTypeArgument = "")
        {
            var codeTypeReference = new CodeTypeReference(name);
            if (genericType != string.Empty) codeTypeReference.TypeArguments.Add(genericType);

            var method = new CodeMemberMethod
            {
                Name = "OkResult",
                ReturnType = codeTypeReference
            };

            foreach (var prop in propsInMethod) {
                method.Parameters.Add(new CodeParameterDeclarationExpression(prop.Type, prop.Name));
            }

            var methodSignature = $"new {name}{genericTypeArgument}(true, {props[1].Name}, new {props[2].Type}()";
            foreach (var prop in props.Skip(3))
            {
                methodSignature += $", {prop.Name}";
            }
            methodSignature += ")";
            var codeArgumentReferenceExpression =
                new CodeArgumentReferenceExpression(methodSignature);

            method.Attributes = MemberAttributes.Public | MemberAttributes.Final | MemberAttributes.Static;
            method.Statements.Add(new CodeMethodReturnStatement(codeArgumentReferenceExpression));

            return method;
        }

        public CodeMemberMethod BuildErrorResult(IList<Property> props, IList<Property> propsInMethod, string name, string genericType = "", string genericTypeArgument = "")
        {
            var codeTypeReference = new CodeTypeReference(name);
            if (genericType != string.Empty) codeTypeReference.TypeArguments.Add(genericType);

            var method = new CodeMemberMethod
            {
                Name = "ErrorResult",
                ReturnType = codeTypeReference
            };

            foreach (var prop in propsInMethod)
            {
                method.Parameters.Add(new CodeParameterDeclarationExpression(prop.Type, prop.Name));
            }

            var methodSignature = $"new {name}{genericTypeArgument}(false, new {props[1].Type}(), {props[2].Name}";
            foreach (var prop in props.Skip(3))
            {
                methodSignature += $", {prop.Name}";
            }
            methodSignature += ")";
            var codeArgumentReferenceExpression =
                new CodeArgumentReferenceExpression(methodSignature);

            method.Attributes = MemberAttributes.Public | MemberAttributes.Final | MemberAttributes.Static;
            method.Statements.Add(new CodeMethodReturnStatement(codeArgumentReferenceExpression));

            return method;
        }
    }

    public interface IStaticConstructorBuilder
    {
        CodeMemberMethod BuildOkResult(IList<Property> propsInConstructor, IList<Property> propsInMethod, string name, string genericType = "", string genericTypeArgument = "");
        CodeMemberMethod BuildErrorResult(IList<Property> userClassProperties, IList<Property> propsInMethod, string name, string genericType = "", string genericTypeArgument = "");
    }
}