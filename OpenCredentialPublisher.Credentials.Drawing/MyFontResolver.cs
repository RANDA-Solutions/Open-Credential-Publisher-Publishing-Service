using PdfSharp.Fonts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace OpenCredentialPublisher.Credentials.Drawing
{
    class MyFontResolver : IFontResolver
    {
        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            // Ignore case of font names.
            var name = familyName.ToLower().TrimEnd('#');

            // Deal with the fonts we know.
            switch (name)
            {
                case "verdana":
                    if (isBold)
                    {
                        if (isItalic)
                            return new FontResolverInfo("Verdana#z");
                        return new FontResolverInfo("Verdana#b");
                    }
                    if (isItalic)
                        return new FontResolverInfo("Verdana#i");
                    return new FontResolverInfo("Verdana#");
            }

            // We pass all other font requests to the default handler.
            // When running on a web server without sufficient permission, you can return a default font at this stage.
            return PlatformFontResolver.ResolveTypeface(familyName, isBold, isItalic);
        }

        public byte[] GetFont(string faceName)
        {
            switch (faceName)
            {
                case "Verdana#":
                    return LoadFontData("OpenCredentialPublisher.Credentials.Drawing.Fonts.Verdana.verdana.ttf"); ;

                case "Verdana#b":
                    return LoadFontData("OpenCredentialPublisher.Credentials.Drawing.Fonts.Verdana.verdanab.ttf"); ;

                case "Verdana#i":
                    return LoadFontData("OpenCredentialPublisher.Credentials.Drawing.Fonts.Verdana.verdanai.ttf");

                case "Verdana#bi":
                    return LoadFontData("OpenCredentialPublisher.Credentials.Drawing.Fonts.Verdana.verdanaz.ttf");
            }

            return null;
        }

        /// <summary>
        /// Returns the specified font from an embedded resource.
        /// </summary>
        private byte[] LoadFontData(string name)
        {
            var assembly = Assembly.GetExecutingAssembly();

            // Test code to find the names of embedded fonts - put a watch on "ourResources"
            //var ourResources = assembly.GetManifestResourceNames();

            using (Stream stream = assembly.GetManifestResourceStream(name))
            {
                if (stream == null)
                    throw new ArgumentException("No resource with name " + name);

                int count = (int)stream.Length;
                byte[] data = new byte[count];
                stream.Read(data, 0, count);
                return data;
            }
        }

        internal static MyFontResolver OurGlobalFontResolver = null;

        /// <summary>
        /// Ensure the font resolver is only applied once (or an exception is thrown)
        /// </summary>
        internal static void Apply()
        {
            if (OurGlobalFontResolver == null || GlobalFontSettings.FontResolver == null)
            {
                if (OurGlobalFontResolver == null)
                    OurGlobalFontResolver = new MyFontResolver();

                GlobalFontSettings.FontResolver = OurGlobalFontResolver;
            }
        }
    }
}
