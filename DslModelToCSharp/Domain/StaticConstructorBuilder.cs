using System.CodeDom;
using System.Collections.Generic;
using DslModel.Domain;

namespace DslModelToCSharp
{
    public class StaticConstructorBuilder : IStaticConstructorBuilder
    {
        public CodeMemberMethod BuildOkResult(IList<string> argumentsPassedToConst, IList<Property> propsInMethod, string classType)
        {
            return BuildResult(argumentsPassedToConst, propsInMethod, classType, "OkResult", true);
        }

        private static CodeMemberMethod BuildResult(IList<string> argumentsPassedToConst, IList<Property> propsInMethod,
            string classType, string resultName, bool isOk)
        {
            var codeTypeReference = new CodeTypeReference(classType);

            var method = createBaseMethod(propsInMethod, codeTypeReference, resultName);

            var methodSignature = CreateMethodCallToConstructor(argumentsPassedToConst, classType, "", isOk);

            var codeArgumentReferenceExpression = new CodeArgumentReferenceExpression(methodSignature);

            method.Attributes = MemberAttributes.Public | MemberAttributes.Final | MemberAttributes.Static;
            method.Statements.Add(new CodeMethodReturnStatement(codeArgumentReferenceExpression));

            return method;
        }

        private static CodeMemberMethod createBaseMethod(IList<Property> propsInMethod, CodeTypeReference codeTypeReference, string name)
        {
            var method = new CodeMemberMethod
            {
                Name = name,
                ReturnType = codeTypeReference
            };

            foreach (var prop in propsInMethod)
            {
                method.Parameters.Add(new CodeParameterDeclarationExpression(prop.Type, prop.Name));
            }

            return method;
        }

        private static string CreateMethodCallToConstructor(IList<string> argumentsPassedToConst, string classType, string genericType,
            bool isOk)
        {
            var methodSignature = $"new {classType}{genericType}({isOk.ToString().ToLower()}";
            foreach (var argument in argumentsPassedToConst)
            {
                methodSignature += $", {argument}";
            }

            methodSignature += ")";
            return methodSignature;
        }

        public CodeMemberMethod BuildOkResultGeneric(IList<string> argumentsPassedToConst, IList<Property> propsInMethod, string classType, string genericType)
        {
            return BuildGenericResult(argumentsPassedToConst, propsInMethod, classType, genericType, "OkResult", true);
        }

        public CodeMemberMethod BuildErrorResultGeneric(IList<string> argumentsPassedToConst, IList<Property> propsInMethod, string classType,
            string genericType)
        {
            return BuildGenericResult(argumentsPassedToConst, propsInMethod, classType, genericType, "ErrorResult", false);
        }

        private static CodeMemberMethod BuildGenericResult(IList<string> argumentsPassedToConst,
            IList<Property> propsInMethod, string classType,
            string genericType, string resultName, bool isOk)
        {
            var codeTypeReference = new CodeTypeReference(classType);

            var method = createBaseMethod(propsInMethod, codeTypeReference, resultName);
            codeTypeReference.TypeArguments.Add(genericType);
            method.ReturnType = codeTypeReference;

            var methodSignature = CreateMethodCallToConstructor(argumentsPassedToConst, classType, $"<{genericType}>", isOk);

            var codeArgumentReferenceExpression = new CodeArgumentReferenceExpression(methodSignature);

            method.Attributes = MemberAttributes.Public | MemberAttributes.Final | MemberAttributes.Static;
            method.Statements.Add(new CodeMethodReturnStatement(codeArgumentReferenceExpression));

            return method;
        }

        public CodeMemberMethod BuildErrorResult(IList<string> argumentsPassedToConst, IList<Property> propsInMethod, string classType)
        {
            return BuildResult(argumentsPassedToConst, propsInMethod, classType, "ErrorResult", false);
        }
    }

    public interface IStaticConstructorBuilder
    {
        CodeMemberMethod BuildOkResult(IList<string> argumentsPassedToConst, IList<Property> propsInMethod, string classType);
        CodeMemberMethod BuildOkResultGeneric(IList<string> argumentsPassedToConst, IList<Property> propsInMethod, string classType, string genericType);
        CodeMemberMethod BuildErrorResultGeneric(IList<string> argumentsPassedToConst, IList<Property> propsInMethod, string classType, string genericType);
        CodeMemberMethod BuildErrorResult(IList<string> argumentsPassedToConst, IList<Property> propsInMethod, string classType);
    }
}