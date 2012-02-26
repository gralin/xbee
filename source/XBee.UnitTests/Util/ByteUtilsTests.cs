using System.IO;
using Moq;
using NUnit.Framework;

namespace Gadgeteer.Modules.GHIElectronics.Util
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
        public void parse_10bit_analog_from_bytes_test()
        {
            const byte msb = 0xF1;
            const byte lsb = 0x23;
            Assert.AreEqual(0x0123, ByteUtils.Parse10BitAnalog(msb, lsb));
        }

        [Test]
        public void parse_10bit_analog_from_stream_test()
        {
            var value = new MemoryStream(new byte[] {0xF1, 0x23});
            var streamMock = new Mock<IInputStream>();
            streamMock.Setup(i => i.Read()).Returns(() => (byte)value.ReadByte());
            streamMock.Setup(i => i.Read(It.IsAny<string>())).Returns(() => (byte)value.ReadByte());
            Assert.AreEqual(0x0123, ByteUtils.Parse10BitAnalog(streamMock.Object, 0));
        }

        [Test]
        public void format_byte_test()
        {
            const byte value = 0x80;
            Assert.AreEqual("0x80", ByteUtils.FormatByte(value));
        }
    }
}