using NUnit.Framework;

namespace NETMF.OpenSource.XBee.Util
{
    [TestFixture]
    class ByteUtilsTests
    {
        [Test]
        public void byte_array_to_base16_test()
        {
            var value = new byte[] {0x01, 0xFF, 0x10};
            Assert.AreEqual("01FF10", ByteUtils.ToBase16(value));
        }

        [Test]
        public void empty_byte_array_to_base16_test()
        {
            var value = new byte[0];
            Assert.AreEqual(string.Empty, ByteUtils.ToBase16(value));
        }

        [Test]
        public void byte_to_base16_test()
        {
            const byte value = 0x12;
            Assert.AreEqual("12", ByteUtils.ToBase16(value));
        }

        [Test]
        public void ushort_to_base16_test()
        {
            const ushort value = 0x0123;
            Assert.AreEqual("0123", ByteUtils.ToBase16(value));
        }

        [Test]
        public void one_digit_from_base16_test()
        {
            const string value = "F";
            Assert.AreEqual(15, ByteUtils.FromBase16(value));
        }

        [Test]
        public void two_digit_from_base16_test()
        {
            const string value = "10";
            Assert.AreEqual(16, ByteUtils.FromBase16(value));
        }

        [Test]
        public void get_lsb_test()
        {
            const byte value = 0x01;
            const byte lsbIndex = 1;
            Assert.IsTrue(ByteUtils.GetBit(value, lsbIndex));
        }

        [Test]
        public void get_msb_test()
        {
            const byte value = 0x80;
            const byte msbIndex = 8;
            Assert.IsTrue(ByteUtils.GetBit(value, msbIndex));
        }

        [Test]
        public void format_byte_test()
        {
            const byte value = 0x80;
            Assert.AreEqual("0x80", ByteUtils.FormatByte(value));
        }
    }
}