using FluentAssertions;
using UnitsNet;
using weatherd.aprs.weather;
using Xunit;

namespace weatherd.tests.outputs.aprs;

public class WeatherReportMessageTests
{

    [Fact]
    public void WeatherReportMessage_ShouldReportH00_WhenSaturated()
    {
        var wxConditions = new WeatherConditions
        {
            Temperature = Temperature.FromDegreesCelsius(20),
            Humidity = RelativeHumidity.FromPercent(99.5)
        };
        
        var wrm = new WeatherReportMessage("FW2332", wxConditions);

        var result = wrm.Compile();

        result.Should().Contain("h00");
    }
}