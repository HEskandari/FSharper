﻿using System;
using System.Collections.Generic;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.Text;
using JetBrains.Util;

namespace JetBrains.ReSharper.Psi.FSharp.Parsing
{
  public partial class FSharpLexerGenerated : ILexer<FSharpLexerState>
  {
    private TokenNodeType currTokenType;
    private static readonly Dictionary<string, TokenNodeType> keywords = new Dictionary<string, TokenNodeType>();
    protected static readonly Dictionary<NodeType, string> tokenTypesToText = new Dictionary<NodeType, string>();
    private static readonly HashSet<string> bangKeywords = new HashSet<string>();

    static FSharpLexerGenerated()
    {
      Action<string, TokenNodeType> add = (k, v) => keywords.Add(k.ToLowerInvariant(), v);
      Action<string, TokenNodeType> addBang = (k, v) =>
        {
          add(k, v);
          bangKeywords.Add(k.ToLowerInvariant().Substring(0, k.Length - 1));
        };

      // Computation expression keywords
      addBang("LET!", FSharpTokenType.LET_BANG_KEYWORD);
      addBang("USE!", FSharpTokenType.USE_BANG_KEYWORD);
      addBang("DO!", FSharpTokenType.DO_BANG_KEYWORD);
      addBang("YIELD!", FSharpTokenType.YIELD_BANG_KEYWORD);
      addBang("RETURN!", FSharpTokenType.RETURN_BANG_KEYWORD);

      add("ABSTRACT", FSharpTokenType.ABSTRACT_KEYWORD);
      add("AND", FSharpTokenType.AND_KEYWORD);
      add("AS", FSharpTokenType.AS_KEYWORD);
      add("ASSERT", FSharpTokenType.ASSERT_KEYWORD);
      add("BASE", FSharpTokenType.BASE_KEYWORD);
      add("BEGIN", FSharpTokenType.BEGIN_KEYWORD);
      add("CLASS", FSharpTokenType.CLASS_KEYWORD);
      add("DEFAULT", FSharpTokenType.DEFAULT_KEYWORD);
      add("DELEGATE", FSharpTokenType.DELEGATE_KEYWORD);
      add("DO", FSharpTokenType.DO_KEYWORD);
      add("DONE", FSharpTokenType.DONE_KEYWORD);
      add("DOWNCAST", FSharpTokenType.DOWNCAST_KEYWORD);
      add("DOWNTO", FSharpTokenType.DOWNTO_KEYWORD);
      add("ELIF", FSharpTokenType.ELIF_KEYWORD);
      add("ELSE", FSharpTokenType.ELSE_KEYWORD);
      add("END", FSharpTokenType.END_KEYWORD);
      add("EXCEPTION", FSharpTokenType.EXCEPTION_KEYWORD);
      add("EXTERN", FSharpTokenType.EXTERN_KEYWORD);
      add("FINALLY", FSharpTokenType.FINALLY_KEYWORD);
      add("FOR", FSharpTokenType.FOR_KEYWORD);
      add("FUN", FSharpTokenType.FUN_KEYWORD);
      add("FUNCTION", FSharpTokenType.FUNCTION_KEYWORD);
      add("GLOBAL", FSharpTokenType.GLOBAL_KEYWORD);
      add("IF", FSharpTokenType.IF_KEYWORD);
      add("IN", FSharpTokenType.IN_KEYWORD);
      add("INHERIT", FSharpTokenType.INHERIT_KEYWORD);
      add("INLINE", FSharpTokenType.INLINE_KEYWORD);
      add("INTERFACE", FSharpTokenType.INTERFACE_KEYWORD);
      add("INTERNAL", FSharpTokenType.INTERNAL_KEYWORD);
      add("LAZY", FSharpTokenType.LAZY_KEYWORD);
      add("LET", FSharpTokenType.LET_KEYWORD);
      add("MATCH", FSharpTokenType.MATCH_KEYWORD);
      add("MEMBER", FSharpTokenType.MEMBER_KEYWORD);
      add("MODULE", FSharpTokenType.MODULE_KEYWORD);
      add("MUTABLE", FSharpTokenType.MUTABLE_KEYWORD);
      add("NAMESPACE", FSharpTokenType.NAMESPACE_KEYWORD);
      add("NEW", FSharpTokenType.NEW_KEYWORD);
      add("NOT", FSharpTokenType.NOT_KEYWORD);
      add("NULL", FSharpTokenType.NULL_KEYWORD);
      add("OF", FSharpTokenType.OF_KEYWORD);
      add("OPEN", FSharpTokenType.OPEN_KEYWORD);
      add("OR", FSharpTokenType.OR_KEYWORD);
      add("OVERRIDE", FSharpTokenType.OVERRIDE_KEYWORD);
      add("PRIVATE", FSharpTokenType.PRIVATE_KEYWORD);
      add("PUBLIC", FSharpTokenType.PUBLIC_KEYWORD);
      add("REC", FSharpTokenType.REC_KEYWORD);
      add("RETURN", FSharpTokenType.RETURN_KEYWORD);
      add("STATIC", FSharpTokenType.STATIC_KEYWORD);
      add("STRUCT", FSharpTokenType.STRUCT_KEYWORD);
      add("THEN", FSharpTokenType.THEN_KEYWORD);
      add("TO", FSharpTokenType.TO_KEYWORD);
      add("TRUE", FSharpTokenType.TRUE_KEYWORD);
      add("TRY", FSharpTokenType.TRY_KEYWORD);
      add("TYPE", FSharpTokenType.TYPE_KEYWORD);
      add("UPCAST", FSharpTokenType.UPCAST_KEYWORD);
      add("USE", FSharpTokenType.USE_KEYWORD);
      add("VAL", FSharpTokenType.VAL_KEYWORD);
      add("VOID", FSharpTokenType.VOID_KEYWORD);
      add("WHEN", FSharpTokenType.WHEN_KEYWORD);
      add("WHILE", FSharpTokenType.WHILE_KEYWORD);
      add("WITH", FSharpTokenType.WITH_KEYWORD);
      add("YIELD", FSharpTokenType.YIELD_KEYWORD);

      // ML keywords
      add("ASR", FSharpTokenType.ASR_ML_KEYWORD);
      add("LAND", FSharpTokenType.LAND_ML_KEYWORD);
      add("LOR", FSharpTokenType.LOR_ML_KEYWORD);
      add("LSL", FSharpTokenType.LSL_ML_KEYWORD);
      add("LSR", FSharpTokenType.LSR_ML_KEYWORD);
      add("LXOR", FSharpTokenType.LXOR_ML_KEYWORD);
      add("MOD", FSharpTokenType.MOD_ML_KEYWORD);
      add("SIG", FSharpTokenType.SIG_ML_KEYWORD);

      // reserved 
      add("ATOMIC", FSharpTokenType.ATOMIC_RESERVED_KEYWORD);
      add("BREAK", FSharpTokenType.BREAK_RESERVED_KEYWORD);
      add("CHECKED", FSharpTokenType.CHECKED_RESERVED_KEYWORD);
      add("COMPONENT", FSharpTokenType.COMPONENT_RESERVED_KEYWORD);
      add("CONST", FSharpTokenType.CONST_RESERVED_KEYWORD);
      add("CONSTRAINT", FSharpTokenType.CONSTRAINT_RESERVED_KEYWORD);
      add("CONSTRUCTOR", FSharpTokenType.CONSTRUCTOR_RESERVED_KEYWORD);
      add("CONTINUE", FSharpTokenType.CONTINUE_RESERVED_KEYWORD);
      add("EAGER", FSharpTokenType.EAGER_RESERVED_KEYWORD);
      add("EVENT", FSharpTokenType.EVENT_RESERVED_KEYWORD);
      add("EXTERNAL", FSharpTokenType.EXTERNAL_RESERVED_KEYWORD);
      add("FIXED", FSharpTokenType.FIXED_RESERVED_KEYWORD);
      add("FUNCTOR", FSharpTokenType.FUNCTOR_RESERVED_KEYWORD);
      add("INCLUDE", FSharpTokenType.INCLUDE_RESERVED_KEYWORD);
      add("METHOD", FSharpTokenType.METHOD_RESERVED_KEYWORD);
      add("MIXIN", FSharpTokenType.MIXIN_RESERVED_KEYWORD);
      add("OBJECT", FSharpTokenType.OBJECT_RESERVED_KEYWORD);
      add("PARALLEL", FSharpTokenType.PARALLEL_RESERVED_KEYWORD);
      add("PROCESS", FSharpTokenType.PROCESS_RESERVED_KEYWORD);
      add("PROTECTED", FSharpTokenType.PROTECTED_RESERVED_KEYWORD);
      add("PURE", FSharpTokenType.PURE_RESERVED_KEYWORD);
      add("SEALED", FSharpTokenType.SEALED_RESERVED_KEYWORD);
      add("TAILCALL", FSharpTokenType.TAILCALL_RESERVED_KEYWORD);
      add("TRAIT", FSharpTokenType.TRAIT_RESERVED_KEYWORD);
      add("VIRTUAL", FSharpTokenType.VIRTUAL_RESERVED_KEYWORD);
      add("VOLATILE", FSharpTokenType.VOLATILE_RESERVED_KEYWORD);

      // object transformation ops (postfixed by OP because they are just too weird)
      add("BOX", FSharpTokenType.BOX_OP);
      add("HASH", FSharpTokenType.HASH_OP);
      add("SIZEOF", FSharpTokenType.SIZEOF_OP);
      add("TYPEOF", FSharpTokenType.TYPEOF_OP);
      add("TYPEDEFOF", FSharpTokenType.TYPEDEFOF_OP);
      add("UNBOX", FSharpTokenType.UNBOX_OP);
      add("REF", FSharpTokenType.REF_OP);
    }

    private TokenNodeType makeToken(TokenNodeType type)
    {
      return currTokenType = type;
    }

    public void Start()
    {
      Start(0, yy_buffer.Length, YYINITIAL);
    }

    private void Start(int startOffset, int endOffset, uint state)
    {
      yy_buffer_index = startOffset;
      yy_buffer_start = startOffset;
      yy_buffer_end = startOffset;
      yy_eof_pos = endOffset;
      yy_lexical_state = (int)state;
      currTokenType = null;
    }

    public void Advance()
    {
      locateToken();
      currTokenType = null;
    }

    public FSharpLexerState CurrentPosition
    {
      get
      {
        FSharpLexerState tokenPosition;
        tokenPosition.currTokenType = currTokenType;
        tokenPosition.yy_buffer_index = yy_buffer_index;
        tokenPosition.yy_buffer_start = yy_buffer_start;
        tokenPosition.yy_buffer_end = yy_buffer_end;
        tokenPosition.yy_lexical_state = yy_lexical_state;
        return tokenPosition;
      }
      set
      {
        currTokenType = value.currTokenType;
        yy_buffer_index = value.yy_buffer_index;
        yy_buffer_start = value.yy_buffer_start;
        yy_buffer_end = value.yy_buffer_end;
        yy_lexical_state = value.yy_lexical_state;
      }
    }

    object ILexer.CurrentPosition
    {
      get { return CurrentPosition; }
      set { CurrentPosition = (FSharpLexerState)value; }
    }

    public TokenNodeType TokenType
    {
      get
      {
        locateToken();
        return currTokenType;
      }
    }

    public int TokenStart
    {
      get
      {
        locateToken();
        return yy_buffer_start;
      }
    }

    public int TokenEnd
    {
      get
      {
        locateToken();
        return yy_buffer_end;
      }
    }

    public IBuffer Buffer { get { return yy_buffer; } }

    private void locateToken()
    {
      if (currTokenType == null)
        currTokenType = _locateToken();
    }

    internal TokenNodeType getKeyword()
    {
      var text = yytext();

      // if it's not a bang keyword, just yield it
      if (bangKeywords.Contains(text) &&
          yy_buffer_end < yy_buffer.Length &&
          yy_buffer[yy_buffer_end ] == '!')
      {
        yy_buffer_end++;
        yy_buffer_index++;
        return keywords.GetValue(text + '!');
      }
      return keywords.GetValueSafe(text);
    }
  }
}