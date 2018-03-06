using System.CodeDom;
using System.Collections.Generic;
using DslModel;

namespace DslModelToCSharp
{
    public class StaticConstructorBuilder : IStaticConstructorBuilder
    {
        public CodeMemberMethod BuildOkResult(IList<Property> props)
        {
            var method = new CodeMemberMethod
            {
                Name = "OkResult",
                ReturnType = new CodeTypeReference(new ValidationResultBaseClass().Name)
            };

            method.Parameters.Add(new CodeParameterDeclarationExpression(props[1].Type, props[1].Name));

            var codeArgumentReferenceExpression =
                new CodeArgumentReferenceExpression(
                    $"new {new ValidationResultBaseClass().Name}(true, {props[1].Name}, new {props[2].Type}())");

            method.Attributes = MemberAttributes.Public | MemberAttributes.Final | MemberAttributes.Static;
            method.Statements.Add(new CodeMethodReturnStatement(codeArgumentReferenceExpression));

            return method;
        }

        public CodeMemberMethod BuildErrorResult(IList<Property> props)
        {
            var method = new CodeMemberMethod
            {
                Name = "ErrorResult",
                ReturnType = new CodeTypeReference(new ValidationResultBaseClass().Name)
            };

            method.Parameters.Add(new CodeParameterDeclarationExpression(props[2].Type, props[2].Name));

            var codeArgumentReferenceExpression =
                new CodeArgumentReferenceExpression(
                    $"new {new ValidationResultBaseClass().Name}(false, new {props[1].Type}(), {props[2].Name})");

            method.Attributes = MemberAttributes.Public | MemberAttributes.Final | MemberAttributes.Static;
            method.Statements.Add(new CodeMethodReturnStatement(codeArgumentReferenceExpression));

            return method;
        }
    }

    public interface IStaticConstructorBuilder
    {
        CodeMemberMethod BuildOkResult(IList<Property> props);
        CodeMemberMethod BuildErrorResult(IList<Property> userClassProperties);
    }
}