using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Irony.Parsing {

  public enum WikiBlockType {
    EscapedText,
    CodeBlock,
    Anchor,
    LinkToAnchor,
    Url,
    FileLink, //looks like it is the same as Url
    Image,
  }

  public class WikiBlockTerminal : WikiTerminalBase {
    public readonly WikiBlockType BlockType;

    public WikiBlockTerminal(string name, WikiBlockType blockType, string openTag, string closeTag, string htmlElementName)
          : base(name, WikiTermType.Block, openTag, closeTag, htmlElementName) { 
      BlockType = blockType;
    }

    public override Token TryMatch(ParsingContext context, ISourceStream source) {
      if (!source.MatchSymbol(OpenTag)) return null;
      source.PreviewPosition += OpenTag.Length;
      var endPos = source.IndexOf(CloseTag, source.PreviewPosition);
      string content; 
      if(endPos > 0) {
        content = source.GetText(source.PreviewPosition, endPos - source.PreviewPosition); 
        source.PreviewPosition = endPos + CloseTag.Length;
      } else {
        content = source.GetText(source.PreviewPosition, source.Length - source.PreviewPosition); 
        source.PreviewPosition = source.Length;
      }
      var token = source.CreateToken(this.OutputTerminal, content); 
      return token;      
    }

  
  }//class
}//namespace
