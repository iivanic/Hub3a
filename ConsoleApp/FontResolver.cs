using PdfSharpCore.Fonts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
        //This implementation is obviously not very good --> Though it should be enough for everyone to implement their own.
        public class FontResolver : IFontResolver
        {
            public string DefaultFontName => "OpenSans";

            public byte[] GetFont(string faceName)
            {
                using (var ms = new MemoryStream())
                {
                    using (var fs = File.Open("Fonrs/" + faceName, FileMode.Open))
                    {
                        fs.CopyTo(ms);
                        ms.Position = 0;
                        return ms.ToArray();
                    }
                }
            }
            public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
            {
                if (familyName.Equals("OpenSans", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (isBold && isItalic)
                    {
                        return new FontResolverInfo("OpenSans-BoldItalic.ttf");
                    }
                    else if (isBold)
                    {
                        return new FontResolverInfo("OpenSans-Bold.ttf");
                    }
                    else if (isItalic)
                    {
                        return new FontResolverInfo("OpenSans-Italic.ttf");
                    }
                    else
                    {
                        return new FontResolverInfo("OpenSans-Regular.ttf");
                    }
                }
                return null;
            }
        
    }
}
