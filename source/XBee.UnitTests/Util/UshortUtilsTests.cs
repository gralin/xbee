using NUnit.Framework;

namespace Gadgeteer.Modules.GHIElectronics.Util
{
    [TestFixture]
    class UshortUtilsTests
    {
        [Test]
        public void from_byte_array_to_ushort_test()
        {
            Assert.AreEqual(0x0102, UshortUtils.ToUshort(new byte[] { 0x01, 0x02 }));
        }

        [Test]
        public void from_msb_lsb_to_ushort_test()
        {
            Assert.AreEqual(0x0102, UshortUtils.ToUshort(0x01, 0x02));
        }

        [Test]
        public void from_chars_to_ushort_test()
        {
            Assert.AreEqual(0x4142, UshortUtils.FromAscii("AB"));
        }

        [Test]
        public void get_msb_test()
        {
            Assert.AreEqual(0x01, UshortUtils.Msb(0x0102));
        }

        [Test]
        public void get_lsb_test()
        {
            Assert.AreEqual(0x02, UshortUtils.Lsb(0x0102));
        }

        [Test]
        public void from_ushort_to_chars_test()
        {
            Assert.AreEqual("AB", UshortUtils.ToAscii(0x4142));
        }
    }
}