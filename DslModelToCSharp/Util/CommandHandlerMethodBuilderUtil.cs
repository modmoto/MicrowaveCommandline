using System.CodeDom;
using DslModel.Domain;

namespace DslModelToCSharp.Util
{
    public class CommandHandlerMethodBuilderUtil
    {
        private NameBuilderUtil _nameBuilderUtil;

        public CommandHandlerMethodBuilderUtil()
        {
            _nameBuilderUtil = new NameBuilderUtil();
        }
        public CodeTypeMember BuildGetAllMethod(DomainClass domainClass)
        {
            var method = new CodeMemberMethod();
            method.Attributes = MemberAttributes.Public | MemberAttributes.Final;

            method.Name = $"Get{domainClass.Name}s";
            method.ReturnType = new CodeTypeReference("async Task<IActionResult>");

            method.Statements.Add(new CodeSnippetExpression($"var listResult = await {domainClass.Name}Repository.Get{domainClass.Name}s()"));
            method.Statements.Add(new CodeMethodReturnStatement(new CodeSnippetExpression("new OkObjectResult(listResult)")));

            return method;
        }

        public CodeTypeMember BuildGetByIdMethod(DomainClass domainClass)
        {
            var method = new CodeMemberMethod();
            method.Attributes = MemberAttributes.Public | MemberAttributes.Final;

            method.Parameters.Add(new CodeParameterDeclarationExpression {Type = new CodeTypeReference("Guid"), Name = "id"});

            method.Name = $"Get{domainClass.Name}";
            method.ReturnType = new CodeTypeReference("async Task<IActionResult>");

            method.Statements.Add(new CodeSnippetExpression($"var result = await {domainClass.Name}Repository.Get{domainClass.Name}(id)"));
            method.Statements.Add(new CodeSnippetExpression("if (result != null) return new JsonResult(result)"));
            method.Statements.Add(new CodeMethodReturnStatement(new CodeSnippetExpression($@"new NotFoundObjectResult(new List<string> {{ $""Could not find {domainClass.Name} with ID: {{id}}"" }})")));

            return method;
        }

        public CodeTypeMember BuildCreateMethod(CreateMethod createMethod, DomainClass domainClass)
        {
            var method = new CodeMemberMethod();
            method.Attributes = MemberAttributes.Public | MemberAttributes.Final;

            var name = _nameBuilderUtil.CreateCommandName(domainClass, createMethod);
            method.Parameters.Add(new CodeParameterDeclarationExpression {Type = new CodeTypeReference(name), Name = "command"});

            method.Name = $"{createMethod.Name}{domainClass.Name}";
            method.ReturnType = new CodeTypeReference("async Task<IActionResult>");

            method.Statements.Add(new CodeSnippetExpression($"CreationResult<{domainClass.Name}> createResult = {domainClass.Name}.{createMethod.Name}(command)"));
            var conditionalStatement = new CodeConditionStatement(
                new CodeSnippetExpression("createResult.Ok"),
                new CodeStatement[]
                {
                    new CodeExpressionStatement(new CodeSnippetExpression("var hookResult = await EventStore.AppendAll(createResult.DomainEvents)")),
                    new CodeConditionStatement(
                        new CodeSnippetExpression("hookResult.Ok"),
                        new CodeStatement[] {
                            new CodeExpressionStatement(new CodeSnippetExpression($"await {domainClass.Name}Repository.{createMethod.Name}{domainClass.Name}(createResult.CreatedEntity)")), 
                            new CodeExpressionStatement(new CodeSnippetExpression(@"return new CreatedResult(""uri"", createResult.CreatedEntity)"))
                        }),
                    new CodeExpressionStatement(new CodeSnippetExpression(@"return new BadRequestObjectResult(hookResult.Errors)"))
                });

            method.Statements.Add(conditionalStatement);
            method.Statements.Add(new CodeMethodReturnStatement(new CodeSnippetExpression($@"new BadRequestObjectResult(createResult.DomainErrors)")));

            return method;
        }

        public CodeTypeMember BuildUpdateMethod(DomainMethod domainMethod, DomainClass domainClass)
        {
            var method = new CodeMemberMethod();
            method.Attributes = MemberAttributes.Public | MemberAttributes.Final;

            var name = _nameBuilderUtil.UpdateCommandName(domainClass, domainMethod);
            method.Parameters.Add(new CodeParameterDeclarationExpression { Type = new CodeTypeReference("Guid"), Name = "id" });
            method.Parameters.Add(new CodeParameterDeclarationExpression { Type = new CodeTypeReference(name), Name = "command" });

            method.Name = $"{domainMethod.Name}{domainClass.Name}";
            method.ReturnType = new CodeTypeReference("async Task<IActionResult>");

            method.Statements.Add(new CodeSnippetExpression($"var entity = await {domainClass.Name}Repository.Get{domainClass.Name}(id)"));
            var conditionalStatement = new CodeConditionStatement(
                new CodeSnippetExpression("entity != null"),
                new CodeStatement[]
                {
                    new CodeExpressionStatement(new CodeSnippetExpression($"var validationResult = entity.{domainMethod.Name}(command)")),
                    new CodeConditionStatement(
                        new CodeSnippetExpression("validationResult.Ok"),
                        new CodeStatement[] {
                            new CodeExpressionStatement(new CodeSnippetExpression($"var hookResult = await EventStore.AppendAll(validationResult.DomainEvents)")),
                            new CodeConditionStatement(
                                new CodeSnippetExpression("validationResult.Ok"),
                                new CodeStatement[]
                                {
                                    new CodeExpressionStatement(new CodeSnippetExpression($"await {domainClass.Name}Repository.Update{domainClass.Name}(entity)")),
                                    new CodeExpressionStatement(new CodeSnippetExpression("return new OkResult()")),
                                }
                            ),
                            new CodeExpressionStatement(new CodeSnippetExpression("return new BadRequestObjectResult(hookResult.Errors)"))
                        }),
                    new CodeExpressionStatement(new CodeSnippetExpression(@"return new BadRequestObjectResult(validationResult.DomainErrors)"))
                });

            method.Statements.Add(conditionalStatement);
            method.Statements.Add(new CodeMethodReturnStatement(new CodeSnippetExpression($@"new NotFoundObjectResult(new List<string> {{ $""Could not find {domainClass.Name} with ID: {{id}}"" }})")));

            return method;
        }

        public CodeTypeMember BuildUpdateLoadMethod(DomainMethod domainMethod, DomainClass domainClass)
        {
            var method = new CodeMemberMethod();
            method.Attributes = MemberAttributes.Public | MemberAttributes.Final;

            var name = _nameBuilderUtil.UpdateApiCommandName(domainClass, domainMethod);
            method.Parameters.Add(new CodeParameterDeclarationExpression { Type = new CodeTypeReference("Guid"), Name = "id" });
            method.Parameters.Add(new CodeParameterDeclarationExpression { Type = new CodeTypeReference(name), Name = "apiCommand" });

            method.Name = $"{domainMethod.Name}{domainClass.Name}";
            method.ReturnType = new CodeTypeReference("async Task<IActionResult>");

            method.Statements.Add(new CodeSnippetExpression($"var entity = await {domainClass.Name}Repository.Get{domainClass.Name}(id)"));
            var conditionalStatement = new CodeConditionStatement(
                new CodeSnippetExpression("entity != null"),
                new CodeStatement[]
                {
                    new CodeExpressionStatement(new CodeSnippetExpression($"var validationResult = entity.{domainMethod.Name}(command)")),
                    new CodeConditionStatement(
                        new CodeSnippetExpression("validationResult.Ok"),
                        new CodeStatement[] {
                            new CodeExpressionStatement(new CodeSnippetExpression($"var hookResult = await EventStore.AppendAll(validationResult.DomainEvents)")),
                            new CodeConditionStatement(
                                new CodeSnippetExpression("validationResult.Ok"),
                                new CodeStatement[]
                                {
                                    new CodeExpressionStatement(new CodeSnippetExpression($"await {domainClass.Name}Repository.Update{domainClass.Name}(entity)")),
                                    new CodeExpressionStatement(new CodeSnippetExpression("return new OkResult()")),
                                }
                            ),
                            new CodeExpressionStatement(new CodeSnippetExpression("return new BadRequestObjectResult(hookResult.Errors)"))
                        }),
                    new CodeExpressionStatement(new CodeSnippetExpression(@"return new BadRequestObjectResult(validationResult.DomainErrors)"))
                });

            method.Statements.Add(conditionalStatement);
            method.Statements.Add(new CodeMethodReturnStatement(new CodeSnippetExpression($@"new NotFoundObjectResult(new List<string> {{ $""Could not find {domainClass.Name} with ID: {{id}}"" }})")));

            return method;
        }
    }
}