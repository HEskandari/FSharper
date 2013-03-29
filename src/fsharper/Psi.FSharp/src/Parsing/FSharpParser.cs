﻿using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.FSharp.Gen;
using JetBrains.ReSharper.Psi.FSharp.Impl.Tree;
using JetBrains.ReSharper.Psi.FSharp.Tree;
using JetBrains.ReSharper.Psi.VB.Parsing;

namespace JetBrains.ReSharper.Psi.FSharp.Parsing
{
  using System.Collections.Generic;
  using Psi.Parsing;
  using Psi.Tree;

  internal class FSharpParser : FSharpParserGenerated, IFSharpParser
  {
    private readonly ILexer<int> myOriginalLexer;

    public FSharpParser(ILexer<int> lexer, IEnumerable<PreProcessingDirective> defines)
    {
      myOriginalLexer = lexer;
      setLexer(myOriginalLexer);
    }

    public IFile ParseFile()
    {
      return (IFSharpFile)parseFSharpFile();
    }

    private static bool IsIdentifier(NodeType tokenType)
    {
      return tokenType == VBTokenType.IDENTIFIER;
    }

    public override TreeElement parseIdentifier()
    {
      if (!IsIdentifier(myLexer.TokenType))
        throw new UnexpectedToken(ParserMessages.GetExpectedMessage(ParserMessages.GetString(ParserMessages.IDS__IDENTIFIER)));
      return createToken();
    }

    public override TreeElement parseFSharpFile()
    {
      var result = TreeElementFactory.CreateCompositeElement(ElementType.F_SHARP_FILE);
      var tokenType = myLexer.TokenType;
      if (tokenType == FSharpTokenType.NAMESPACE_KEYWORD)
      {
        while (tokenType == FSharpTokenType.NAMESPACE_KEYWORD)
        {
          result.AppendNewChild(parseNamespaceDeclaration());
          tokenType = myLexer.TokenType;
        }
      }
      else if (tokenType == FSharpTokenType.MODULE_KEYWORD)
      {
        result.AppendNewChild(parsePrimaryModuleDeclaration());
      }
      else
      {
        result.AppendNewChild(parseAnonymousModuleDeclaration());
      }
      return result;
    }

    public override TreeElement parsePrimaryModuleDeclaration()
    {
      var result = TreeElementFactory.CreateCompositeElement(ElementType.PRIMARY_MODULE_DECLARATION);
      try
      {
        result.AppendNewChild(match(FSharpTokenType.MODULE_KEYWORD));
        var qualifiedNamespaceName = (IQualifiedNamespaceUsage)parseQualifiedNamespaceUsage();
        result.AppendNewChild((TreeElement)qualifiedNamespaceName.Qualifier);
        result.AppendNewChild((TreeElement)qualifiedNamespaceName.Dot);
        result.AppendNewChild((TreeElement)qualifiedNamespaceName.NameIdentifier);
        result.AppendNewChild(parseModuleBody());
      }
      catch (SyntaxError e)
      {
        if (e.ParsingResult != null)
          result.AppendNewChild(e.ParsingResult);
        e.ParsingResult = result;
        handleErrorInModuleDeclaration(result, e);
      }
      return result;
    }

    public override TreeElement parseQualifiedNamespaceUsage()
    {
      var result = TreeElementFactory.CreateCompositeElement(ElementType.QUALIFIED_NAMESPACE_USAGE);
      result.AppendNewChild(parseIdentifier());

      while (myLexer.TokenType == VBTokenType.DOT)
      {
        var qualifiedNamespaceName = TreeElementFactory.CreateCompositeElement(ElementType.QUALIFIED_NAMESPACE_USAGE);
        qualifiedNamespaceName.AppendNewChild(result);
        qualifiedNamespaceName.AppendNewChild(match(FSharpTokenType.DOT));
        qualifiedNamespaceName.AppendNewChild(parseIdentifier());

        result = qualifiedNamespaceName;
      }

      return result;
    }
  }
}