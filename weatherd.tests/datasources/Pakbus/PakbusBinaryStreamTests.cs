using System;
using FluentAssertions;
using weatherd.datasources.pakbus;
using weatherd.io;
using Xunit;

namespace weatherd.tests.datasources.Pakbus
{
    public class PakbusBinaryStreamTests
    {
        [Fact]
        public void WriteUSec_WithValidTimestamp_ShouldEmitValidByteSequence()
        {
            // Arrange
            byte[] expectedByteArray = { 0x5D, 0xCE, 0x23, 0xEC, 0xB8, 0xC4 };
            NSec nsec = new NSec(1031399473, 62500000);
            PakbusBinaryStream bs = new PakbusBinaryStream(Endianness.Big);

            // Act
            bs.WriteUSec(nsec);
            byte[] result = bs.ToArray();

            // Assert
            result.Should().NotBeNull();
            result.Should().Equal(expectedByteArray);
        }

        [Fact]
        public void ReadUSec_WithValidByteSequence_ShouldReturnValidNSec()
        {
            // Arrange
            byte[] data = { 0x5D, 0xCE, 0x23, 0xEC, 0xB8, 0xC4 };
            
            NSec expectedNsec = new NSec(1031399473, 62500000);
            PakbusBinaryStream bs = new PakbusBinaryStream(data, Endianness.Big);

            // Act
            NSec result = bs.ReadUSec();

            // Assert
            result.Should().NotBeNull();
            result.Seconds.Should().Be(expectedNsec.Seconds);
            result.Nanoseconds.Should().Be(expectedNsec.Nanoseconds);
        }
        
        [Theory]
        [InlineData(0x00000000, 0.0f)]
        [InlineData(0xBD9C6200, 0.0f)]
        [InlineData(0x3FCBC6A7, 0.4f)]
        [InlineData(0x44CDB31C, 12.8f)]
        [InlineData(0x48F3FF61, 244f)]
        public void ReadFP4_WithValidByteSequence_ShouldReturnExpectedValue(uint data, float expected)
        {
            // Arrange
            byte[] dataBytes = BitConverter.GetBytes(data);

            // Little endian because BitConverter likes little endian.
            PakbusBinaryStream bs = new PakbusBinaryStream(dataBytes, Endianness.Little);
            
            // Act
            float result = bs.ReadFP4();

            // Assert
            result.Should().BeApproximately(expected, 0.1f);
        }
    }
}
