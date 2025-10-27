// Decompiled with JetBrains decompiler
// Type: Mortar.Font
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Mortar
{

    public class Font
    {
      public static readonly int QUICKFIND_CHARS = 256 /*0x0100*/;
      public static readonly int QUADS_IN_BUFFER = 256 /*0x0100*/;
      private Font.CharTemplate[] m_charTemplates;
      private Font.CharTemplate[] m_charLookUp = new Font.CharTemplate[Font.QUICKFIND_CHARS];
      private int m_charTemplateCnt;
      private Font.Page[] m_pages;
      private int m_pageCnt;
      private Font.Kerning[] m_kernings;
      private int m_kerningCnt;
      public uint m_hash_name;
      public int m_textureWidth;
      public int m_textureHeight;
      public float m_lineHeight;
      public float m_baseLine;
      public VertexPositionColorTexture[] quad_verts;
      public static readonly int SPACE_CHAR = 32 /*0x20*/;
      public static readonly int NEW_LINE_CHAR = 13;
      public static readonly int QUOTES_CHAR = 34;
      public static readonly int INVALID_NUMBER = -43710;

      public static bool strnicmp(Font.CharPtr p, string tst, int l)
      {
        return p.str.astring.Substring(p.idx, l).ToLower() == tst.Substring(0, l).ToLower();
      }

      public static void cpStrcpy(Font.CharPtr p, string src)
      {
        p.str.astring = p.str.astring.Substring(0, p.idx) + src;
      }

      public static int cpStrlen(Font.CharPtr p) => p.str.astring.Length - p.idx;

      private int Get_Next_Value(
        Font.CharPtr readingFrom,
        Font.CharPtr valueName,
        Action<int> intValue,
        Action<Font.CharPtr> stringValue)
      {
        int num1 = 0;
        while (readingFrom.arr(num1) != '=')
        {
          ++num1;
          if ((int) readingFrom.arr(num1) == Font.NEW_LINE_CHAR || readingFrom.Offset(num1).isEos)
            return -num1;
        }
        int i1 = num1 - 1;
        while ((int) readingFrom.arr(i1) != Font.SPACE_CHAR && i1 > 0)
          --i1;
        if (valueName.isNotNull)
        {
          int startIndex = i1 + 1;
          Font.cpStrcpy(valueName, readingFrom.Str.Substring(startIndex, num1 - startIndex));
        }
        int nextValue = num1 + 1;
        if ((int) readingFrom.arr(nextValue) == Font.NEW_LINE_CHAR || readingFrom.Offset(nextValue).isEos)
          return -nextValue;
        if (intValue == null || stringValue == null)
          return nextValue;
        bool flag = false;
        if (readingFrom.arr(nextValue) == '-')
        {
          flag = true;
          ++nextValue;
          if ((int) readingFrom.arr(nextValue) == Font.NEW_LINE_CHAR || readingFrom.Offset(nextValue).isEos)
            return -nextValue;
        }
        if ((int) readingFrom.arr(nextValue) == Font.QUOTES_CHAR)
        {
          ++nextValue;
          if ((int) readingFrom.arr(nextValue) == Font.NEW_LINE_CHAR || readingFrom.Offset(nextValue).isEos)
            return -nextValue;
          int startIndex = nextValue;
          while ((int) readingFrom.arr(nextValue) != Font.QUOTES_CHAR && readingFrom.arr(nextValue) != '.')
          {
            ++nextValue;
            if ((int) readingFrom.arr(nextValue) == Font.NEW_LINE_CHAR || readingFrom.Offset(nextValue).isEos)
              return -nextValue;
          }
          if (readingFrom.arr(nextValue) == '.')
            stringValue(new Font.CharPtr(new Font.AString(readingFrom.Str.Substring(startIndex, nextValue - startIndex) + ".tex")));
          else
            stringValue(new Font.CharPtr(new Font.AString(readingFrom.Str.Substring(startIndex, nextValue - startIndex))));
        }
        else
        {
          int num2 = nextValue;
          while ((int) readingFrom.arr(nextValue) != Font.SPACE_CHAR && (int) readingFrom.arr(nextValue) != Font.NEW_LINE_CHAR && readingFrom.arr(nextValue) != char.MinValue)
            ++nextValue;
          int num3 = 0;
          int num4 = 1;
          for (int i2 = nextValue - 1; i2 >= num2; --i2)
          {
            if (readingFrom.arr(i2) < '0' || readingFrom.arr(i2) > '9')
              return nextValue;
            num3 += num4 * ((int) readingFrom.arr(i2) - 48 /*0x30*/);
            num4 *= 10;
          }
          intValue(num3 * (flag ? -1 : 1));
        }
        return (int) readingFrom.arr(nextValue) == Font.NEW_LINE_CHAR || readingFrom.Offset(nextValue).isEos ? -nextValue : nextValue;
      }

      private bool Next_Word_Is(Font.CharPtr readingFrom, string test)
      {
        int length = test.Length;
        int num = 0;
        while (num < length && readingFrom.arr(num) != char.MinValue && (int) readingFrom.arr(num) == (int) test[num] && (int) readingFrom.arr(num) != Font.SPACE_CHAR)
          ++num;
        return num >= length;
      }

      private int Parse_Char(Font.CharPtr readingFrom, ref Font.CharTemplate newChar, int sizeLeft)
      {
        int o = 0;
        newChar = new Font.CharTemplate();
        int nextValue;
        for (; o < sizeLeft; o += nextValue)
        {
          int value = Font.INVALID_NUMBER;
          Font.CharPtr stringVal = new Font.CharPtr((Font.AString) null);
          Font.CharPtr charPtr = new Font.CharPtr(new Font.AString(""));
          nextValue = this.Get_Next_Value(readingFrom.Offset(o), charPtr, (Action<int>) (v => value = v), (Action<Font.CharPtr>) (f => stringVal = f));
          if (this.Next_Word_Is(charPtr, "id"))
          {
            if (value != Font.INVALID_NUMBER)
              newChar.id = (ushort) value;
          }
          else if (this.Next_Word_Is(charPtr, "xadvance"))
          {
            if (value != Font.INVALID_NUMBER)
              newChar.xAdvance = (float) value;
          }
          else if (this.Next_Word_Is(charPtr, "xoffset"))
          {
            if (value != Font.INVALID_NUMBER)
              newChar.xOffset = (float) value;
          }
          else if (this.Next_Word_Is(charPtr, "x"))
          {
            if (value != Font.INVALID_NUMBER)
              newChar.X = (float) value;
          }
          else if (this.Next_Word_Is(charPtr, "yoffset"))
          {
            if (value != Font.INVALID_NUMBER)
              newChar.yOffset = (float) value;
          }
          else if (this.Next_Word_Is(charPtr, "y"))
          {
            if (value != Font.INVALID_NUMBER)
              newChar.Y = (float) value;
          }
          else if (this.Next_Word_Is(charPtr, "width"))
          {
            if (value != Font.INVALID_NUMBER)
              newChar.width = (float) value;
          }
          else if (this.Next_Word_Is(charPtr, "height"))
          {
            if (value != Font.INVALID_NUMBER)
              newChar.height = (float) value;
          }
          else if (this.Next_Word_Is(charPtr, "page") && value != Font.INVALID_NUMBER)
            newChar.page = (byte) value;
          if (nextValue < 0)
            return o - nextValue + 2;
        }
        return o;
      }

      private int Parse_Page(Font.CharPtr readingFrom, ref Font.Page newPage, int sizeLeft)
      {
        int o = 0;
        newPage.pageName = (string) null;
        newPage.texture = (Texture) null;
        int nextValue;
        for (; o < sizeLeft; o += nextValue)
        {
          int value = Font.INVALID_NUMBER;
          Font.CharPtr stringVal = new Font.CharPtr((Font.AString) null);
          Font.CharPtr charPtr = new Font.CharPtr(new Font.AString(""));
          nextValue = this.Get_Next_Value(readingFrom.Offset(o), charPtr, (Action<int>) (v => value = v), (Action<Font.CharPtr>) (f => stringVal = f));
          if (this.Next_Word_Is(charPtr, "file") && stringVal.isNotNull)
            newPage.pageName = stringVal.Str;
          if (nextValue < 0)
            return o - nextValue + 2;
        }
        return o;
      }

      private int Parse_Kerning(Font.CharPtr readingFrom, ref Font.Kerning newKern, int sizeLeft)
      {
        int o = 0;
        newKern.first = 0;
        newKern.second = 0;
        newKern.amount = 0.0f;
        int nextValue;
        for (; o < sizeLeft; o += nextValue)
        {
          int value = Font.INVALID_NUMBER;
          Font.CharPtr stringVal = new Font.CharPtr((Font.AString) null);
          Font.CharPtr charPtr = new Font.CharPtr(new Font.AString(""));
          nextValue = this.Get_Next_Value(readingFrom.Offset(o), charPtr, (Action<int>) (v => value = v), (Action<Font.CharPtr>) (f => stringVal = f));
          if (this.Next_Word_Is(charPtr, "first"))
          {
            if (value != Font.INVALID_NUMBER)
              newKern.first = value;
          }
          else if (this.Next_Word_Is(charPtr, "second"))
          {
            if (value != Font.INVALID_NUMBER)
              newKern.second = value;
          }
          else if (this.Next_Word_Is(charPtr, "amount") && value != Font.INVALID_NUMBER)
            newKern.amount = (float) value;
          if (nextValue < 0)
            return o - nextValue + 2;
        }
        return o;
      }

      private Font.CharTemplate GetCharTemplate(int charID) => this.GetCharTemplate(charID, 0);

      private Font.CharTemplate GetCharTemplate(int charID, int type)
      {
        if (charID < 0)
          charID = 256 /*0x0100*/ + charID;
        if (charID < Font.QUICKFIND_CHARS && this.m_charLookUp[charID] != null)
          return this.m_charLookUp[charID];
        for (int index = 0; index < this.m_charTemplateCnt; ++index)
        {
          if ((int) this.m_charTemplates[index].id == charID)
            return this.m_charTemplates[index];
        }
        return (Font.CharTemplate) null;
      }

      private float GetKerning(uint first, uint second) => 0.0f;

      private Font.CharPtr FindAdvanceOfNextWord(Font.CharPtr str, float fontSpaceX, float wrapWidth)
      {
        Font.CharPtr advanceOfNextWord = str;
        float num = fontSpaceX;
        if ((double) wrapWidth <= 0.0)
          return new Font.CharPtr((Font.AString) null);
        while (str.isNotNull && str.isNotEos && str.Val != '\n' && str.Val != ' ')
        {
          Font.CharTemplate charTemplate = this.GetCharTemplate((int) str.Val);
          ++str.idx;
          if (charTemplate != null)
          {
            num += charTemplate.xAdvance;
            if ((double) fontSpaceX < (double) wrapWidth * 0.75 && (double) num >= (double) wrapWidth)
              return advanceOfNextWord;
          }
        }
        return (double) num > (double) wrapWidth ? advanceOfNextWord : new Font.CharPtr((Font.AString) null);
      }

      private float GetLineLength(string str) => this.GetLineLength(str, 0.0f);

      private float GetLineLength(string str, float wrapWidth)
      {
        return this.GetLineLength(str, wrapWidth, (Action<float>) null);
      }

      private float GetLineLength(string str, float wrapWidth, Action<float> charSpacing)
      {
        return this.GetLineLength(new Font.CharPtr(new Font.AString(str)), wrapWidth, charSpacing);
      }

      private float GetLineLength(Font.CharPtr str, float wrapWidth, Action<float> charSpacing)
      {
        if (charSpacing != null)
          charSpacing(0.0f);
        float num = 0.0f;
        float fontSpaceX = 0.0f;
        if ((double) wrapWidth <= 0.0)
        {
          while (str.isNotNull && str.isNotEos && str.Val != '\n')
          {
            Font.CharTemplate charTemplate = this.GetCharTemplate((int) str.arr(0));
            ++str.idx;
            if (charTemplate != null)
              fontSpaceX += charTemplate.xAdvance;
          }
        }
        else
        {
          Font.CharPtr b = new Font.CharPtr((Font.AString) null);
          while (str.isNotNull && str.isNotEos)
          {
            if (str.Val == '\n' || str.Cmp(b))
            {
              if (charSpacing == null || !str.Cmp(b))
                return fontSpaceX;
              charSpacing((wrapWidth - fontSpaceX) / num);
              return wrapWidth;
            }
            Font.CharTemplate charTemplate = this.GetCharTemplate((int) str.Val);
            ++str.idx;
            if (b.isNull)
            {
              b = this.FindAdvanceOfNextWord(str, fontSpaceX, wrapWidth);
              if (b.Cmp(str))
              {
                if (charSpacing == null || (double) num <= 0.0 || (double) fontSpaceX <= (double) wrapWidth * 0.75)
                  return fontSpaceX;
                charSpacing((wrapWidth - fontSpaceX) / num);
                return wrapWidth;
              }
            }
            if (charTemplate != null)
            {
              ++num;
              if (charTemplate.id == (ushort) 32 /*0x20*/)
                num += 2f;
              fontSpaceX += charTemplate.xAdvance;
            }
          }
        }
        return fontSpaceX;
      }

      public Font()
      {
        this.m_charTemplateCnt = 0;
        this.m_charTemplates = (Font.CharTemplate[]) null;
        this.m_pageCnt = 0;
        this.m_pages = (Font.Page[]) null;
        this.m_kerningCnt = 0;
        this.m_kernings = (Font.Kerning[]) null;
        for (int index = 0; index < Font.QUICKFIND_CHARS; ++index)
          this.m_charLookUp[index] = (Font.CharTemplate) null;
        this.quad_verts = new VertexPositionColorTexture[Font.QUADS_IN_BUFFER * 6];
        for (int index = 0; index < Font.QUADS_IN_BUFFER * 6; ++index)
          this.quad_verts[index].Position.Z = 0.0f;
      }

      public uint GetPageCount() => (uint) this.m_pageCnt;

      public Font.Page GetPage(uint pageIdx) => this.m_pages[pageIdx];

      public uint GetCharTemplateCount() => (uint) this.m_charTemplateCnt;

      public Font.CharTemplate[] GetCharTemplateArray() => this.m_charTemplates;

      public float GetLineHeight() => this.m_lineHeight;

      public float GetBaseLine() => this.m_baseLine;

      public void Load(string file)
      {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        this.Load_Internal(file);
        stopwatch.Stop();
        TimeSpan elapsed = stopwatch.Elapsed;
            string formatted = string.Format("{0:00}:{1:00}:{2:00}.{3:000}",
                                     elapsed.Hours,
                                     elapsed.Minutes,
                                     elapsed.Seconds,
                                     elapsed.Milliseconds);
        }

        private void Load_Internal(string file)
      {
        string str1 = MortarFile.LoadText(file);
        int length = str1.Length;
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        Font.FontReaderState fontReaderState = Font.FontReaderState.Idle;
        Font.FontReaderState_Common readerStateCommon = Font.FontReaderState_Common.Idle;
        Font.FontReaderState_Page fontReaderStatePage = Font.FontReaderState_Page.Idle;
        Font.FontReaderState_Chars readerStateChars = Font.FontReaderState_Chars.Idle;
        Font.FontReaderState_Char fontReaderStateChar = Font.FontReaderState_Char.Idle;
        StringBuilder stringBuilder = new StringBuilder(1048576 /*0x100000*/);
        int num1 = 1;
        int num2 = 0;
        int index1 = 0;
        Font.Page page = new Font.Page();
        int index2 = 0;
        Font.CharTemplate charTemplate = (Font.CharTemplate) null;
        for (int index3 = 0; index3 < length; ++index3)
        {
          char ch = str1[index3];
          switch (fontReaderState)
          {
            case Font.FontReaderState.Idle:
              switch (ch)
              {
                case '\t':
                case ' ':
                  string str2 = stringBuilder.ToString();
                  stringBuilder.Clear();
                  if (str2.Equals("char", StringComparison.CurrentCultureIgnoreCase))
                  {
                    fontReaderState = Font.FontReaderState.Char;
                    fontReaderStateChar = Font.FontReaderState_Char.Idle;
                    charTemplate = new Font.CharTemplate();
                    continue;
                  }
                  if (str2.Equals("page", StringComparison.CurrentCultureIgnoreCase))
                  {
                    fontReaderState = Font.FontReaderState.Page;
                    fontReaderStatePage = Font.FontReaderState_Page.Idle;
                    index1 = num2;
                    page = new Font.Page();
                    continue;
                  }
                  if (str2.Equals("chars", StringComparison.CurrentCultureIgnoreCase))
                  {
                    fontReaderState = Font.FontReaderState.Chars;
                    readerStateChars = Font.FontReaderState_Chars.Idle;
                    continue;
                  }
                  if (str2.Equals("common", StringComparison.CurrentCultureIgnoreCase))
                  {
                    fontReaderState = Font.FontReaderState.Common;
                    readerStateCommon = Font.FontReaderState_Common.Idle;
                    continue;
                  }
                  fontReaderState = !str2.Equals("info", StringComparison.CurrentCultureIgnoreCase) ? Font.FontReaderState.SkipLine : Font.FontReaderState.Info;
                  continue;
                case '\n':
                  fontReaderState = Font.FontReaderState.Idle;
                  ++num1;
                  stringBuilder.Clear();
                  continue;
                default:
                  stringBuilder.Append(ch);
                  continue;
              }
            case Font.FontReaderState.Info:
              if (ch == '\n')
              {
                fontReaderState = Font.FontReaderState.Idle;
                ++num1;
                break;
              }
              break;
            case Font.FontReaderState.Common:
              switch (ch)
              {
                case '\t':
                case '\n':
                case ' ':
                  switch (readerStateCommon)
                  {
                    case Font.FontReaderState_Common.Idle:
                    case Font.FontReaderState_Common.SkipValue:
                      readerStateCommon = Font.FontReaderState_Common.Idle;
                      break;
                    case Font.FontReaderState_Common.LineHeight:
                      float result1;
                      if (float.TryParse(stringBuilder.ToString(), out result1))
                        this.m_lineHeight = result1;
                      readerStateCommon = Font.FontReaderState_Common.Idle;
                      break;
                    case Font.FontReaderState_Common.Base:
                      float result2;
                      if (float.TryParse(stringBuilder.ToString(), out result2))
                        this.m_baseLine = result2;
                      readerStateCommon = Font.FontReaderState_Common.Idle;
                      break;
                    case Font.FontReaderState_Common.ScaleW:
                      int result3;
                      if (int.TryParse(stringBuilder.ToString(), out result3))
                        this.m_textureWidth = result3;
                      readerStateCommon = Font.FontReaderState_Common.Idle;
                      break;
                    case Font.FontReaderState_Common.ScaleH:
                      int result4;
                      if (int.TryParse(stringBuilder.ToString(), out result4))
                        this.m_textureHeight = result4;
                      readerStateCommon = Font.FontReaderState_Common.Idle;
                      break;
                    case Font.FontReaderState_Common.Pages:
                      int result5;
                      if (int.TryParse(stringBuilder.ToString(), out result5))
                      {
                        this.m_pageCnt = result5;
                        this.m_pages = new Font.Page[this.m_pageCnt];
                        for (int index4 = 0; index4 < this.m_pageCnt; ++index4)
                          this.m_pages[index4] = new Font.Page();
                      }
                      readerStateCommon = Font.FontReaderState_Common.Idle;
                      break;
                    case Font.FontReaderState_Common.Packed:
                      readerStateCommon = Font.FontReaderState_Common.Idle;
                      break;
                    case Font.FontReaderState_Common.AlphaChnl:
                      readerStateCommon = Font.FontReaderState_Common.Idle;
                      break;
                    case Font.FontReaderState_Common.RedChnl:
                      readerStateCommon = Font.FontReaderState_Common.Idle;
                      break;
                    case Font.FontReaderState_Common.GreenChnl:
                      readerStateCommon = Font.FontReaderState_Common.Idle;
                      break;
                    case Font.FontReaderState_Common.BlueChnl:
                      readerStateCommon = Font.FontReaderState_Common.Idle;
                      break;
                    default:
                      throw new Exception($"Unhandled state {(object) readerStateCommon}.");
                  }
                  if (ch == '\n')
                  {
                    fontReaderState = Font.FontReaderState.Idle;
                    ++num1;
                  }
                  stringBuilder.Clear();
                  continue;
                case '=':
                  if (readerStateCommon == Font.FontReaderState_Common.Idle)
                  {
                    string str3 = stringBuilder.ToString();
                    readerStateCommon = !str3.Equals("lineHeight", StringComparison.CurrentCultureIgnoreCase) ? (!str3.Equals("base", StringComparison.CurrentCultureIgnoreCase) ? (!str3.Equals("scaleW", StringComparison.CurrentCultureIgnoreCase) ? (!str3.Equals("scaleH", StringComparison.CurrentCultureIgnoreCase) ? (!str3.Equals("pages", StringComparison.CurrentCultureIgnoreCase) ? (!str3.Equals("packed", StringComparison.CurrentCultureIgnoreCase) ? (!str3.Equals("alphaChnl", StringComparison.CurrentCultureIgnoreCase) ? (!str3.Equals("redChnl", StringComparison.CurrentCultureIgnoreCase) ? (!str3.Equals("greenChnl", StringComparison.CurrentCultureIgnoreCase) ? (!str3.Equals("blueChnl", StringComparison.CurrentCultureIgnoreCase) ? Font.FontReaderState_Common.SkipValue : Font.FontReaderState_Common.BlueChnl) : Font.FontReaderState_Common.GreenChnl) : Font.FontReaderState_Common.RedChnl) : Font.FontReaderState_Common.AlphaChnl) : Font.FontReaderState_Common.Packed) : Font.FontReaderState_Common.Pages) : Font.FontReaderState_Common.ScaleH) : Font.FontReaderState_Common.ScaleW) : Font.FontReaderState_Common.Base) : Font.FontReaderState_Common.LineHeight;
                  }
                  else
                    fontReaderState = Font.FontReaderState.SkipLine;
                  stringBuilder.Clear();
                  continue;
                default:
                  stringBuilder.Append(ch);
                  continue;
              }
            case Font.FontReaderState.Page:
              switch (fontReaderStatePage)
              {
                case Font.FontReaderState_Page.File:
                  switch (ch)
                  {
                    case '\t':
                    case '\n':
                    case ' ':
                      string str4 = stringBuilder.ToString();
                      page.pageName = str4;
                      if (ch == '\n')
                      {
                        ++num2;
                        this.m_pages[index1] = page;
                        fontReaderState = Font.FontReaderState.Idle;
                        ++num1;
                        stringBuilder.Clear();
                        continue;
                      }
                      continue;
                    case '"':
                      fontReaderStatePage = Font.FontReaderState_Page.FileQuotes;
                      continue;
                    default:
                      stringBuilder.Append(ch);
                      continue;
                  }
                case Font.FontReaderState_Page.FileQuotes:
                  switch (ch)
                  {
                    case '\n':
                      ++num2;
                      this.m_pages[index1] = page;
                      fontReaderState = Font.FontReaderState.Idle;
                      ++num1;
                      stringBuilder.Clear();
                      continue;
                    case '"':
                      string str5 = stringBuilder.ToString();
                      page.pageName = str5;
                      fontReaderStatePage = Font.FontReaderState_Page.Idle;
                      continue;
                    default:
                      stringBuilder.Append(ch);
                      continue;
                  }
                default:
                  switch (ch)
                  {
                    case '\t':
                    case '\n':
                    case ' ':
                      switch (fontReaderStatePage)
                      {
                        case Font.FontReaderState_Page.Idle:
                        case Font.FontReaderState_Page.SkipValue:
                          fontReaderStatePage = Font.FontReaderState_Page.Idle;
                          break;
                        case Font.FontReaderState_Page.Id:
                          int result6;
                          if (int.TryParse(stringBuilder.ToString(), out result6))
                            index1 = result6;
                          fontReaderStatePage = Font.FontReaderState_Page.Idle;
                          break;
                        default:
                          throw new Exception($"Unhandled state {(object) fontReaderStatePage}.");
                      }
                      if (ch == '\n')
                      {
                        ++num2;
                        this.m_pages[index1] = page;
                        fontReaderState = Font.FontReaderState.Idle;
                        ++num1;
                      }
                      stringBuilder.Clear();
                      continue;
                    case '=':
                      if (fontReaderStatePage == Font.FontReaderState_Page.Idle)
                      {
                        string str6 = stringBuilder.ToString();
                        fontReaderStatePage = !str6.Equals("id", StringComparison.CurrentCultureIgnoreCase) ? (!str6.Equals(nameof (file), StringComparison.CurrentCultureIgnoreCase) ? Font.FontReaderState_Page.SkipValue : Font.FontReaderState_Page.File) : Font.FontReaderState_Page.Id;
                      }
                      else
                        fontReaderState = Font.FontReaderState.SkipLine;
                      stringBuilder.Clear();
                      continue;
                    default:
                      stringBuilder.Append(ch);
                      continue;
                  }
              }
            case Font.FontReaderState.Chars:
              switch (ch)
              {
                case '\t':
                case '\n':
                case ' ':
                  switch (readerStateChars)
                  {
                    case Font.FontReaderState_Chars.Idle:
                    case Font.FontReaderState_Chars.SkipValue:
                      readerStateChars = Font.FontReaderState_Chars.Idle;
                      break;
                    case Font.FontReaderState_Chars.Count:
                      int result7;
                      if (int.TryParse(stringBuilder.ToString(), out result7))
                      {
                        this.m_charTemplateCnt = result7;
                        this.m_charTemplates = new Font.CharTemplate[this.m_charTemplateCnt];
                      }
                      readerStateChars = Font.FontReaderState_Chars.Idle;
                      break;
                    default:
                      throw new Exception($"Unhandled state {(object) readerStateChars}.");
                  }
                  if (ch == '\n')
                  {
                    fontReaderState = Font.FontReaderState.Idle;
                    ++num1;
                  }
                  stringBuilder.Clear();
                  continue;
                case '=':
                  if (readerStateChars == Font.FontReaderState_Chars.Idle)
                    readerStateChars = !stringBuilder.ToString().Equals("count", StringComparison.CurrentCultureIgnoreCase) ? Font.FontReaderState_Chars.SkipValue : Font.FontReaderState_Chars.Count;
                  else
                    fontReaderState = Font.FontReaderState.SkipLine;
                  stringBuilder.Clear();
                  continue;
                default:
                  stringBuilder.Append(ch);
                  continue;
              }
            case Font.FontReaderState.Char:
              switch (ch)
              {
                case '\t':
                case '\n':
                case ' ':
                  switch (fontReaderStateChar)
                  {
                    case Font.FontReaderState_Char.Idle:
                    case Font.FontReaderState_Char.SkipValue:
                      fontReaderStateChar = Font.FontReaderState_Char.Idle;
                      break;
                    case Font.FontReaderState_Char.Id:
                      int result8;
                      if (int.TryParse(stringBuilder.ToString(), out result8))
                        charTemplate.id = (ushort) result8;
                      fontReaderStateChar = Font.FontReaderState_Char.Idle;
                      break;
                    case Font.FontReaderState_Char.X:
                      float result9;
                      if (float.TryParse(stringBuilder.ToString(), out result9))
                        charTemplate.X = result9 / (float) this.m_textureWidth;
                      fontReaderStateChar = Font.FontReaderState_Char.Idle;
                      break;
                    case Font.FontReaderState_Char.Y:
                      float result10;
                      if (float.TryParse(stringBuilder.ToString(), out result10))
                        charTemplate.Y = result10 / (float) this.m_textureHeight;
                      fontReaderStateChar = Font.FontReaderState_Char.Idle;
                      break;
                    case Font.FontReaderState_Char.Width:
                      float result11;
                      if (float.TryParse(stringBuilder.ToString(), out result11))
                        charTemplate.width = result11 / this.m_lineHeight;
                      fontReaderStateChar = Font.FontReaderState_Char.Idle;
                      break;
                    case Font.FontReaderState_Char.Height:
                      float result12;
                      if (float.TryParse(stringBuilder.ToString(), out result12))
                        charTemplate.height = result12 / this.m_lineHeight;
                      fontReaderStateChar = Font.FontReaderState_Char.Idle;
                      break;
                    case Font.FontReaderState_Char.XOffset:
                      float result13;
                      if (float.TryParse(stringBuilder.ToString(), out result13))
                        charTemplate.xOffset = result13 / this.m_lineHeight;
                      fontReaderStateChar = Font.FontReaderState_Char.Idle;
                      break;
                    case Font.FontReaderState_Char.YOffset:
                      float result14;
                      if (float.TryParse(stringBuilder.ToString(), out result14))
                        charTemplate.yOffset = result14 / this.m_lineHeight;
                      fontReaderStateChar = Font.FontReaderState_Char.Idle;
                      break;
                    case Font.FontReaderState_Char.XAdvance:
                      float result15;
                      if (float.TryParse(stringBuilder.ToString(), out result15))
                        charTemplate.xAdvance = result15 / this.m_lineHeight;
                      fontReaderStateChar = Font.FontReaderState_Char.Idle;
                      break;
                    case Font.FontReaderState_Char.Page:
                      byte result16;
                      if (byte.TryParse(stringBuilder.ToString(), out result16))
                        charTemplate.page = result16;
                      fontReaderStateChar = Font.FontReaderState_Char.Idle;
                      break;
                    case Font.FontReaderState_Char.Chnl:
                      fontReaderStateChar = Font.FontReaderState_Char.Idle;
                      break;
                    default:
                      throw new Exception($"Unhandled state {(object) fontReaderStatePage}.");
                  }
                  if (ch == '\n')
                  {
                    if ((int) charTemplate.id < Font.QUICKFIND_CHARS)
                      this.m_charLookUp[(int) charTemplate.id] = charTemplate;
                    this.m_charTemplates[index2] = charTemplate;
                    charTemplate = (Font.CharTemplate) null;
                    ++index2;
                    fontReaderState = Font.FontReaderState.Idle;
                    ++num1;
                  }
                  stringBuilder.Clear();
                  continue;
                case '=':
                  if (fontReaderStateChar == Font.FontReaderState_Char.Idle)
                  {
                    string str7 = stringBuilder.ToString();
                    fontReaderStateChar = !str7.Equals("id", StringComparison.CurrentCultureIgnoreCase) ? (!str7.Equals("x", StringComparison.CurrentCultureIgnoreCase) ? (!str7.Equals("y", StringComparison.CurrentCultureIgnoreCase) ? (!str7.Equals("width", StringComparison.CurrentCultureIgnoreCase) ? (!str7.Equals("height", StringComparison.CurrentCultureIgnoreCase) ? (!str7.Equals("xoffset", StringComparison.CurrentCultureIgnoreCase) ? (!str7.Equals("yoffset", StringComparison.CurrentCultureIgnoreCase) ? (!str7.Equals("xadvance", StringComparison.CurrentCultureIgnoreCase) ? (!str7.Equals("page", StringComparison.CurrentCultureIgnoreCase) ? (!str7.Equals("chnl", StringComparison.CurrentCultureIgnoreCase) ? Font.FontReaderState_Char.SkipValue : Font.FontReaderState_Char.Chnl) : Font.FontReaderState_Char.Page) : Font.FontReaderState_Char.XAdvance) : Font.FontReaderState_Char.YOffset) : Font.FontReaderState_Char.XOffset) : Font.FontReaderState_Char.Height) : Font.FontReaderState_Char.Width) : Font.FontReaderState_Char.Y) : Font.FontReaderState_Char.X) : Font.FontReaderState_Char.Id;
                  }
                  else
                    fontReaderState = Font.FontReaderState.SkipLine;
                  stringBuilder.Clear();
                  continue;
                default:
                  stringBuilder.Append(ch);
                  continue;
              }
            case Font.FontReaderState.SkipLine:
              if (ch == '\n')
              {
                ++num1;
                fontReaderState = Font.FontReaderState.Idle;
                break;
              }
              break;
          }
        }
        stopwatch.Stop();
        TimeSpan elapsed = stopwatch.Elapsed;
        string formatted = string.Format("{0:00}:{1:00}:{2:00}.{3:000}", elapsed.Hours, elapsed.Minutes, elapsed.Seconds, elapsed.Milliseconds);
        string fileName = Path.GetFileName(file);
        string str8 = file.Substring(0, file.Length - fileName.Length);
        for (int index5 = 0; index5 < this.m_pageCnt; ++index5)
          this.m_pages[index5].texture = Texture.Load(str8 + this.m_pages[index5].pageName);
        this.m_baseLine /= this.m_lineHeight;
      }

      public void DrawString(string stringToDraw, Vector3 pos)
      {
        this.DrawString(stringToDraw, pos, Color.White);
      }

      public void DrawString(string stringToDraw, Vector3 pos, Color colour)
      {
        this.DrawString(stringToDraw, pos, colour, 1f);
      }

      public void DrawString(string stringToDraw, Vector3 pos, Color colour, float scale)
      {
        this.DrawString(stringToDraw, pos, colour, scale, Vector2.Zero);
      }

      public void DrawString(
        string stringToDraw,
        Vector3 pos,
        Color colour,
        float scale,
        Vector2 wrap)
      {
        this.DrawString(stringToDraw, pos, colour, scale, wrap, ALIGNMENT_TYPE.ALIGN_LEFT);
      }

      public void DrawString(
        string stringToDraw,
        Vector3 pos,
        Color colour,
        float scale,
        Vector2 wrap,
        ALIGNMENT_TYPE alignType)
      {
        this.DrawString(stringToDraw, pos, colour, scale, wrap, alignType, 1f);
      }

      public void DrawString(
        string _stringToDraw,
        Vector3 pos,
        Color colour,
        float scale,
        Vector2 wrap,
        ALIGNMENT_TYPE alignType,
        float vDir)
      {
        this.DrawString(_stringToDraw, pos, colour, scale, wrap, alignType, vDir, new MortarRectangleDec?());
      }

      public void DrawString(string stringToDraw, float x, float y)
      {
        this.DrawString(stringToDraw, x, y, 0.0f);
      }

      public void DrawString(string stringToDraw, float x, float y, float z)
      {
        this.DrawString(stringToDraw, x, y, z, Color.White);
      }

      public void DrawString(string stringToDraw, float x, float y, float z, Color colour)
      {
        this.DrawString(stringToDraw, x, y, z, colour, 1f);
      }

      public void DrawString(
        string stringToDraw,
        float x,
        float y,
        float z,
        Color colour,
        float scale)
      {
        this.DrawString(stringToDraw, x, y, z, colour, scale, 0.0f);
      }

      public void DrawString(
        string stringToDraw,
        float x,
        float y,
        float z,
        Color colour,
        float scale,
        float wrapWidth)
      {
        this.DrawString(stringToDraw, x, y, z, colour, scale, wrapWidth, 0.0f);
      }

      public void DrawString(
        string stringToDraw,
        float x,
        float y,
        float z,
        Color colour,
        float scale,
        float wrapWidth,
        float textBoxHeight)
      {
        this.DrawString(stringToDraw, x, y, z, colour, scale, wrapWidth, textBoxHeight, ALIGNMENT_TYPE.ALIGN_LEFT);
      }

      public void DrawString(
        string stringToDraw,
        float x,
        float y,
        float z,
        Color colour,
        float scale,
        float wrapWidth,
        float textBoxHeight,
        ALIGNMENT_TYPE alignType)
      {
        this.DrawString(stringToDraw, new Vector3(x, y, z), colour, scale, new Vector2(wrapWidth, textBoxHeight), alignType);
      }

      public void DrawString(
        string _stringToDraw,
        Vector3 pos,
        Color colour,
        float scale,
        Vector2 wrap,
        ALIGNMENT_TYPE alignType,
        float vDir,
        MortarRectangleDec? rect)
      {
        this.DrawString(_stringToDraw, pos, colour, new Vector2(scale, scale), wrap, alignType, vDir, rect);
      }

      public void DrawString(
        string stringToDraw,
        Vector3 pos,
        Color colour,
        Vector2 scale,
        Vector2 wrap,
        ALIGNMENT_TYPE alignType,
        float vDir,
        MortarRectangleDec? rect)
      {
        wrap.X /= scale.X;
        wrap.Y /= vDir * scale.Y;
        MatrixManager.instance.Reset();
        MatrixManager.instance.Scale(new Vector3(scale.X, scale.Y, 1f));
        MatrixManager.instance.TranslateGlobal(new Vector3(pos.X, pos.Y, pos.Z));
        byte iCurrentFontPage = 0;
        this.m_pages[(int) iCurrentFontPage].texture.Set();
        float num1 = 0.0f;
        float num2 = vDir;
        int iCurrentLineStart1 = 0;
        float num3 = 0.0f;
        float num4 = 0.0f;
        for (int iChar = 0; iChar < stringToDraw.Length; ++iChar)
        {
          char charID = stringToDraw[iChar];
          Font.CharTemplate charTemplate = this.GetCharTemplate((int) charID);
          if (charTemplate != null)
          {
            float num5 = num3 + charTemplate.xAdvance;
            num1 = MathHelper.Max(num1, num5);
            bool flag = (double) wrap.X > 0.0 && (double) num5 > (double) wrap.X;
            if (charID == '\n' || flag)
            {
              if (flag)
              {
                float fLineWidth = num5;
                this.SearchForSpaceForWordWrap(stringToDraw, ref iCurrentLineStart1, ref iChar, ref fLineWidth, out int _);
                num1 = MathHelper.Max(num1, fLineWidth);
              }
              num5 = 0.0f;
              num4 += num2;
              iCurrentLineStart1 = iChar + 1;
            }
            num3 = num5;
          }
        }
        float num6 = num4 + num2;
        Vector3 zero = Vector3.Zero;
            zero.Y = (alignType & ALIGNMENT_TYPE.ALIGN_VCENTER) != ALIGNMENT_TYPE.ALIGN_VCENTER ? ((alignType & ALIGNMENT_TYPE.ALIGN_TOP) != ALIGNMENT_TYPE.ALIGN_TOP ? -num6 : 0.0f) : (float)(-(double)num6 * 0.5);
        MatrixManager.instance.TranslateLocal(zero);
        float num7 = vDir;
        float fLineWidth1 = 0.0f;
        int iCurrentLineStart2 = 0;
        int iCurrentLineEnd = stringToDraw.Length;
        float num8 = 0.0f;
        float num9 = 0.0f;
        for (int iChar = 0; iChar < stringToDraw.Length; ++iChar)
        {
          char charID = stringToDraw[iChar];
          Font.CharTemplate charTemplate = this.GetCharTemplate((int) charID);
          if (charTemplate != null)
          {
            float num10 = num8 + charTemplate.xAdvance;
            fLineWidth1 = num10;
            bool flag = (double) wrap.X > 0.0 && (double) num10 > (double) wrap.X;
            if (charID == '\n' || flag)
            {
              if (flag)
                this.SearchForSpaceForWordWrap(stringToDraw, ref iCurrentLineStart2, ref iChar, ref fLineWidth1, out iCurrentLineEnd);
              this.DrawLine(stringToDraw, iCurrentLineStart2, iCurrentLineEnd, 0.0f, num6 - num9, alignType, fLineWidth1, ref wrap, iCurrentFontPage, ref colour, vDir);
              num10 = 0.0f;
              num9 += num7;
              iCurrentLineStart2 = iCurrentLineEnd;
              iCurrentLineEnd = stringToDraw.Length;
            }
            num8 = num10;
          }
        }
        if (iCurrentLineStart2 == iCurrentLineEnd)
          return;
        this.DrawLine(stringToDraw, iCurrentLineStart2, iCurrentLineEnd, 0.0f, num6 - num9, alignType, fLineWidth1, ref wrap, iCurrentFontPage, ref colour, vDir);
      }

      private void SearchForSpaceForWordWrap(
        string stringToDraw,
        ref int iCurrentLineStart,
        ref int iChar,
        ref float fLineWidth,
        out int iCurrentLineEnd)
      {
        iCurrentLineEnd = iChar + 1;
        int num = -1;
        for (int index = iCurrentLineEnd - 1; index > iCurrentLineStart; --index)
        {
          if (stringToDraw[index] == ' ')
          {
            num = index;
            break;
          }
        }
        if (num <= -1)
          return;
        iChar = num;
        iCurrentLineEnd = iChar + 1;
        fLineWidth = 0.0f;
        for (int index = iCurrentLineStart; index < iCurrentLineEnd; ++index)
        {
          Font.CharTemplate charTemplate = this.GetCharTemplate((int) stringToDraw[index]);
          if (charTemplate != null)
            fLineWidth += charTemplate.xAdvance;
        }
      }

      private void DrawLine(
        string stringToDraw,
        int iCurrentLineStart,
        int iCurrentLineEnd,
        float fCurrentX,
        float fCurrentY,
        ALIGNMENT_TYPE alignType,
        float fLineWidth,
        ref Vector2 wrap,
        byte iCurrentFontPage,
        ref Color colour,
        float vDir)
      {
        int num1 = 0;
        float num2 = fCurrentX;
        float num3 = fCurrentY;
        float num4 = this.m_lineHeight / (float) this.m_textureHeight;
        float num5 = this.m_lineHeight / (float) this.m_textureWidth;
        int num6 = iCurrentLineEnd - iCurrentLineStart;
        float num7 = 0.0f;
        float num8 = 0.0f;
        switch (alignType & ALIGNMENT_TYPE.ALIGN_HCENTER)
        {
          case ALIGNMENT_TYPE.ALIGN_LEFT:
            num8 = 0.0f;
            break;
          case ALIGNMENT_TYPE.ALIGN_RIGHT:
            num8 = wrap.X - fLineWidth;
            break;
          case ALIGNMENT_TYPE.ALIGN_HCENTER:
            num8 = (float) (((double) wrap.X - (double) fLineWidth) * 0.5);
            break;
        }
        if ((alignType & ALIGNMENT_TYPE.ALIGN_JUSTIFY) != (ALIGNMENT_TYPE) 0)
          num7 = (wrap.X - fLineWidth) / (float) num6;
        for (int index1 = iCurrentLineStart; index1 < iCurrentLineEnd; ++index1)
        {
          Font.CharTemplate charTemplate = this.GetCharTemplate((int) stringToDraw[index1]);
          if (charTemplate != null)
          {
            if (charTemplate != null)
            {
              byte page = charTemplate.page;
              if ((int) page != (int) iCurrentFontPage)
              {
                if (num1 > 0)
                {
                  MatrixManager.instance.UploadCurrentMatrices(true);
                  Mesh.DrawTriStripEx(this.quad_verts, num1 * 6, true);
                  num1 = 0;
                }
                this.m_pages[(int) page].texture.Set();
                iCurrentFontPage = page;
              }
            }
            float num9 = charTemplate.width * num5;
            float num10 = charTemplate.height * num4;
            float x = num2 + charTemplate.xOffset + num8;
            float y = num3 - vDir * charTemplate.yOffset;
            int index2 = num1 * 6;
            this.quad_verts[index2].Position = new Vector3(x, y - vDir * charTemplate.height, 0.0f);
            this.quad_verts[index2].TextureCoordinate = new Vector2(charTemplate.X, charTemplate.Y + num10);
            if (index2 != 0)
              this.quad_verts[index2 - 1].Position = this.quad_verts[index2].Position;
            this.quad_verts[index2 + 1].Position = new Vector3(x, y, 0.0f);
            this.quad_verts[index2 + 1].TextureCoordinate = new Vector2(charTemplate.X, charTemplate.Y);
            this.quad_verts[index2 + 2].Position = new Vector3(x + charTemplate.width, y - vDir * charTemplate.height, 0.0f);
            this.quad_verts[index2 + 2].TextureCoordinate = new Vector2(charTemplate.X + num9, charTemplate.Y + num10);
            this.quad_verts[index2 + 3].Position = new Vector3(x + charTemplate.width, y, 0.0f);
            this.quad_verts[index2 + 3].TextureCoordinate = new Vector2(charTemplate.X + num9, charTemplate.Y);
            this.quad_verts[index2 + 4].Position = this.quad_verts[index2 + 3].Position;
            this.quad_verts[index2].Color = colour;
            this.quad_verts[index2 + 1].Color = colour;
            this.quad_verts[index2 + 2].Color = colour;
            this.quad_verts[index2 + 3].Color = colour;
            this.quad_verts[index2 + 4].Color = colour;
            this.quad_verts[index2 + 5].Color = colour;
            num2 += charTemplate.xAdvance + num7 * (charTemplate.id == (ushort) 32 /*0x20*/ ? 3f : 1f);
            ++num1;
            if (num1 == Font.QUADS_IN_BUFFER)
            {
              MatrixManager.instance.UploadCurrentMatrices(true);
              Mesh.DrawTriStripEx(this.quad_verts, num1 * 6, true);
              num1 = 0;
            }
          }
        }
        if (num1 <= 0)
          return;
        MatrixManager.instance.UploadCurrentMatrices(true);
        Mesh.DrawTriStripEx(this.quad_verts, num1 * 6, true);
      }

      public float MeasureString(string stringToDraw) => this.GetLineLength(stringToDraw);

      public class CharTemplate
      {
        public ushort id;
        public float X;
        public float Y;
        public float width;
        public float height;
        public float xOffset;
        public float yOffset;
        public float xAdvance;
        public byte page;
      }

      public struct Page
      {
        public string pageName;
        public Texture texture;
      }

      public struct Kerning
      {
        public int first;
        public int second;
        public float amount;
      }

      public class AString
      {
        public string astring;

        public AString(string str) => this.astring = str;
      }

      public struct CharPtr
      {
        public Font.AString str;
        public int idx;

        public CharPtr(Font.AString s)
        {
          this.str = s != null ? s : (Font.AString) null;
          this.idx = 0;
        }

        public CharPtr(Font.AString s, int i)
        {
          if (s == null)
          {
            this.str = (Font.AString) null;
            this.idx = 0;
          }
          else
          {
            this.str = s;
            this.idx = i;
          }
        }

        public void Dec() => --this.idx;

        public char Val => this.str.astring[this.idx];

        public char arr(int i)
        {
          return i + this.idx >= this.str.astring.Length ? char.MinValue : this.str.astring[this.idx + i];
        }

        public bool isNull => this.str == null;

        public bool isNotNull => this.str != null;

        public bool isEos => this.idx == this.str.astring.Length;

        public bool isNotEos => this.idx != this.str.astring.Length;

        public bool Cmp(Font.CharPtr b)
        {
          return object.ReferenceEquals((object) this.str, (object) b.str) && b.idx == this.idx;
        }

        public Font.CharPtr Offset(int o)
        {
          return new Font.CharPtr()
          {
            str = this.str,
            idx = this.idx + o
          };
        }

        public string Str => this.str.astring.Substring(this.idx);
      }

      private enum FontReaderState
      {
        Idle,
        Info,
        Common,
        Page,
        Chars,
        Char,
        SkipLine,
      }

      private enum FontReaderState_Info
      {
        Idle,
        Face,
        Size,
        Bold,
        Italic,
        Charset,
        Unicode,
        StretchH,
        Smooth,
        Padding,
        Spacing,
        Outline,
        SkipValue,
      }

      private enum FontReaderState_Common
      {
        Idle,
        LineHeight,
        Base,
        ScaleW,
        ScaleH,
        Pages,
        Packed,
        AlphaChnl,
        RedChnl,
        GreenChnl,
        BlueChnl,
        SkipValue,
      }

      private enum FontReaderState_Page
      {
        Idle,
        Id,
        File,
        FileQuotes,
        SkipValue,
      }

      private enum FontReaderState_Chars
      {
        Idle,
        Count,
        SkipValue,
      }

      private enum FontReaderState_Char
      {
        Idle,
        Id,
        X,
        Y,
        Width,
        Height,
        XOffset,
        YOffset,
        XAdvance,
        Page,
        Chnl,
        SkipValue,
      }
    }
}
