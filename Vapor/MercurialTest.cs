// ********************************************************************************
//     Document    :  MercurialTest.cs 
//     Author      :  Eric Hall
//     License     :  TBD
//     Description :  Exact format of file header comment block is TBD.
// ********************************************************************************

namespace Gadgeteer.Modules.Codeplex.XBee.Vapor
{
    /// <summary>
    /// Source Control Test object.
    /// </summary>
    /// <remarks>
    /// XML comments are required! Details TBD.
    /// </remarks>
    public class MercurialTest
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        static MercurialTest()
        {
            // Lots of inline comments are nice to have.
            Hello = "Hello from Gadgeteer.Modules.Codeplex.XBee.Vapor.MercurialTest";
        }

        /// <summary>
        /// A static string to see if your local Mercurial source control is behaving.
        /// </summary>
        public static string Hello
        {
            get; private set;
        }
    }
}
