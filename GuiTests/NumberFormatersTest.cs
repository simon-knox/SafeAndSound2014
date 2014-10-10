using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SKnoxConsulting.SafeAndSound.Gui.Util;

namespace GuiTests
{
    [TestFixture]
    public class NumberFormatersTest
    {
        [Test]
        public void FormatBytes()
        {
            Assert.That(NumberFormaters.ConvertBytesToString(1023) == "1,023 B");
        }

        [Test]
        public void FormatKiloBytes()
        {
            var result = NumberFormaters.ConvertBytesToString(Convert.ToInt64(NumberFormaters.KILO));
            Assert.That( result == "1 KB");
            var result2 = NumberFormaters.ConvertBytesToString(Convert.ToInt64(NumberFormaters.KILO*1.5));
            Assert.That(result2 == "1.5 KB");
            var result3 = NumberFormaters.ConvertBytesToString(Convert.ToInt64(NumberFormaters.KILO * 1.25));
            Assert.That(result3 == "1.25 KB");
            var result4 = NumberFormaters.ConvertBytesToString(Convert.ToInt64(NumberFormaters.KILO * 1.25 + 1));
            Assert.That(result4 == "1.25 KB");
        }

        [Test]
        public void FormatMegaBytes()
        {          
            var result = NumberFormaters.ConvertBytesToString(Convert.ToInt64(NumberFormaters.MEGA));
            Assert.That(result == "1 MB");
            var result2 = NumberFormaters.ConvertBytesToString(Convert.ToInt64(NumberFormaters.MEGA * 1.25));
            Assert.That(result2 == "1.25 MB");
            var result3 = NumberFormaters.ConvertBytesToString(Convert.ToInt64(NumberFormaters.MEGA * 1.25)*10);
            Assert.That(result3 == "12.5 MB");
            var result4 = NumberFormaters.ConvertBytesToString(Convert.ToInt64(NumberFormaters.MEGA * 1.25)*100);
            Assert.That(result4 == "125 MB");
        }

        [Test]
        public void FormatGigaBytes()
        {
            var result = NumberFormaters.ConvertBytesToString(Convert.ToInt64(NumberFormaters.GIGA));
            Assert.That(result == "1 GB");
            var result2 = NumberFormaters.ConvertBytesToString(Convert.ToInt64(NumberFormaters.GIGA * 1.25));
            Assert.That(result2 == "1.25 GB");
            var result3 = NumberFormaters.ConvertBytesToString(Convert.ToInt64(NumberFormaters.GIGA * 1.25) * 10);
            Assert.That(result3 == "12.5 GB");
            var result4 = NumberFormaters.ConvertBytesToString(Convert.ToInt64(NumberFormaters.GIGA * 1.25) * 100);
            Assert.That(result4 == "125 GB");
        }

        [Test]
        public void FormatTeraBytes()
        {
            var result = NumberFormaters.ConvertBytesToString(Convert.ToInt64(NumberFormaters.TERA));
            Assert.That(result == "1 TB");
            var result2 = NumberFormaters.ConvertBytesToString(Convert.ToInt64(NumberFormaters.TERA * 1.25));
            Assert.That(result2 == "1.25 TB");
            var result3 = NumberFormaters.ConvertBytesToString(Convert.ToInt64(NumberFormaters.TERA * 1.25) * 10);
            Assert.That(result3 == "12.5 TB");
            var result4 = NumberFormaters.ConvertBytesToString(Convert.ToInt64(NumberFormaters.TERA * 1.25) * 100);
            Assert.That(result4 == "125 TB");
        }

        [Test]
        public void FormatNumberWithVariableDecimalPlacesLessThan10()
        {
            Assert.That(NumberFormaters.FormatNumberWithVariableDecimalPlaces(3) == "3");
            Assert.That(NumberFormaters.FormatNumberWithVariableDecimalPlaces(3.1) == "3.1");
            Assert.That(NumberFormaters.FormatNumberWithVariableDecimalPlaces(3.12) == "3.12");
            Assert.That(NumberFormaters.FormatNumberWithVariableDecimalPlaces(3.126) == "3.13");
        }

        [Test]
        public void FormatNumberWithVariableDecimalPlacesLessThan100()
        {
            Assert.That(NumberFormaters.FormatNumberWithVariableDecimalPlaces(30) == "30");
            Assert.That(NumberFormaters.FormatNumberWithVariableDecimalPlaces(30.1) == "30.1");
            Assert.That(NumberFormaters.FormatNumberWithVariableDecimalPlaces(30.12) == "30.1");
            Assert.That(NumberFormaters.FormatNumberWithVariableDecimalPlaces(30.199) == "30.2");
        }

        [Test]
        public void FormatNumberWithVariableDecimalPlacesLessThan1000()
        {
            Assert.That(NumberFormaters.FormatNumberWithVariableDecimalPlaces(300) == "300");
            Assert.That(NumberFormaters.FormatNumberWithVariableDecimalPlaces(300.1) == "300");
            Assert.That(NumberFormaters.FormatNumberWithVariableDecimalPlaces(300.12) == "300");
            Assert.That(NumberFormaters.FormatNumberWithVariableDecimalPlaces(300.999) == "301");
        }

        [Test]
        public void FormatNumberWithVariableDecimalPlacesMoreThan1000()
        {
            Assert.That(NumberFormaters.FormatNumberWithVariableDecimalPlaces(3000) == "3,000");
            Assert.That(NumberFormaters.FormatNumberWithVariableDecimalPlaces(3000.1) == "3,000");
            Assert.That(NumberFormaters.FormatNumberWithVariableDecimalPlaces(3000.12) == "3,000");
            Assert.That(NumberFormaters.FormatNumberWithVariableDecimalPlaces(3000.999) == "3,001");
        }
    }
}
