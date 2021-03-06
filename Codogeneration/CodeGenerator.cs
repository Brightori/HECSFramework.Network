using System;
using System.Collections.Generic;

namespace HECSFramework.Core.Generator
{
    public partial class CodeGenerator
    {
        public string GenerateNetworkCommandsMap(List<Type> commands)
        {
            var tree = new TreeSyntaxNode();
            var resolvers = new TreeSyntaxNode();
            var typeToIdDictionary = new TreeSyntaxNode();
            var dictionaryBody = new TreeSyntaxNode();
            var genericMethod = new TreeSyntaxNode();

            tree.Add(new UsingSyntax("Commands"));
            tree.Add(new UsingSyntax("System"));
            tree.Add(new UsingSyntax("System.Collections.Generic", 1));
            tree.Add(new NameSpaceSyntax("HECSFramework.Core"));
            tree.Add(new LeftScopeSyntax());
            tree.Add(new TabSimpleSyntax(1, "public partial class ResolversMap"));
            tree.Add(new LeftScopeSyntax(1));
            tree.Add(new TabSimpleSyntax(2, "public Dictionary<int, ICommandResolver> Map = new Dictionary<int, ICommandResolver>"));
            tree.Add(new LeftScopeSyntax(2));
            tree.Add(resolvers);
            tree.Add(new RightScopeSyntax(2, true));
            tree.Add(new ParagraphSyntax());
            tree.Add(typeToIdDictionary);
            tree.Add(new ParagraphSyntax());
            tree.Add(InitPartialCommandResolvers());
            tree.Add(new RightScopeSyntax(1));
            tree.Add(new RightScopeSyntax(0));

            foreach (var t in commands)
                resolvers.Add(GetCommandResolver(t));

            typeToIdDictionary.Add(new TabSimpleSyntax(2, "public Dictionary<Type, int> CommandsIDs = new Dictionary<Type, int>"));
            typeToIdDictionary.Add(new LeftScopeSyntax(2));
            typeToIdDictionary.Add(dictionaryBody);
            typeToIdDictionary.Add(new RightScopeSyntax(2, true));

            for (int i = 0; i < commands.Count; i++)
            {
                Type t = commands[i];
                dictionaryBody.Add(GetCommandMethod(t));

                //if (i < commands.Count - 1)
                //    dictionaryBody.Add(new ParagraphSyntax());
            }

            return tree.ToString();
        }

        private ISyntax GetCommandMethod(Type command)
        {
            var tree = new TreeSyntaxNode();
            tree.Add(new TabSimpleSyntax(3, $"{{typeof({command.Name}), {IndexGenerator.GetIndexForType(command)}}},"));
            return tree;
        }

        private ISyntax InitPartialCommandResolvers()
        {
            var tree = new TreeSyntaxNode();
            tree.Add(new TabSimpleSyntax(2, "partial void InitPartialCommandResolvers()"));
            tree.Add(new LeftScopeSyntax(2));
            tree.Add(new TabSimpleSyntax(3, "hashTypeToResolver = Map;"));
            tree.Add(new TabSimpleSyntax(3, "typeTohash = CommandsIDs;"));
            tree.Add(new RightScopeSyntax(2));

            return tree;
        }

        private ISyntax GetCommandResolver(Type type)
        {
            return new TabSimpleSyntax(3, $"{{{IndexGenerator.GetIndexForType(type)}, new CommandResolver<{type.Name}>()}},");
        }
    }
}
