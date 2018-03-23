using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
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
            method.Statements.Add(new CodeMethodReturnStatement(new CodeSnippetExpression($@"new NotFoundObjectResult(new List<string> {{ $""Could not find Root {domainClass.Name} with ID: {{id}}"" }})")));

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
                                new CodeSnippetExpression("hookResult.Ok"),
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
            method.Statements.Add(new CodeMethodReturnStatement(new CodeSnippetExpression($@"new NotFoundObjectResult(new List<string> {{ $""Could not find Root {domainClass.Name} with ID: {{id}}"" }})")));

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

            var newListStatement = new CodeExpressionStatement(new CodeSnippetExpression($"var errorList = new List<string>()"));

            var codeStatements = new List<CodeStatement>();
            foreach (var loadParam in domainMethod.LoadParameters)
            {
                var codeExpressionStatement = new CodeExpressionStatement(new CodeSnippetExpression($"var {loadParam.Name} = await {loadParam.Type}Repository.Get{loadParam.Type}(apiCommand.{loadParam.Name}Id)"));
                var ifNullStatement = new CodeExpressionStatement(new CodeSnippetExpression($"if ({loadParam.Name} == null) errorList.Add({_nameBuilderUtil.BuildErrorMessageFor(loadParam)})"));
                codeStatements.Add(codeExpressionStatement);
                codeStatements.Add(ifNullStatement);
            }

            var ifErrorListStatement = new CodeExpressionStatement(new CodeSnippetExpression($@"if (errorList.Count > 0) return new NotFoundObjectResult(errorList)"));

            var constArguments = domainMethod.LoadParameters.Select(param => param.Name);

            string constructorSignatur = String.Empty;
            foreach (var loadParam in constArguments)
            {
                constructorSignatur += $"{loadParam}, ";
            }

            constructorSignatur = constructorSignatur.Substring(0, constructorSignatur.Length - 2);

            var newCommandStatement = new CodeExpressionStatement(new CodeSnippetExpression($"var command = new {_nameBuilderUtil.UpdateCommandName(domainClass, domainMethod)}({constructorSignatur})"));

            method.Statements.Add(new CodeSnippetExpression($"var entity = await {domainClass.Name}Repository.Get{domainClass.Name}(id)"));
            var ifEntityFoundStatements = new CodeStatement[]
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
            };
            var statements = ifEntityFoundStatements.ToList();

            statements.Insert(0, newCommandStatement);
            statements.Insert(0, ifErrorListStatement);
            statements.InsertRange(0, codeStatements);
            statements.Insert(0, newListStatement);

            var conditionalStatement = new CodeConditionStatement(
                new CodeSnippetExpression("entity != null"),
                statements.ToArray());

            method.Statements.Add(conditionalStatement);
            method.Statements.Add(new CodeMethodReturnStatement(new CodeSnippetExpression($@"new NotFoundObjectResult(new List<string> {{ $""Could not find Root {domainClass.Name} with ID: {{id}}"" }})")));

            return method;
        }
    }
}