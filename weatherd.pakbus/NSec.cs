﻿using System;
using System.Runtime.InteropServices;

namespace weatherd.datasources.pakbus
{
    [StructLayout(LayoutKind.Sequential)]
    public struct NSec
    {
        public const int SecondsBefore1990 = 631152000;
        public static readonly NSec Zero = new(0, 0);

        public int Seconds;
        public int Nanoseconds;

        public NSec(int ss, int ns)
        {
            Seconds = ss;
            Nanoseconds = ns;
        }

        public DateTime ToTime()
        {
            long unixTime = SecondsBefore1990;

            unixTime += Seconds;
            unixTime *= 1000;
            unixTime += Nanoseconds / 1000000;

            return DateTimeOffset.FromUnixTimeMilliseconds(unixTime).DateTime;
        }

        public long ToUnixTimestamp()
            => Seconds + SecondsBefore1990;

        public static NSec FromUnixTimestamp(long unixTime)
            => new((int) unixTime - SecondsBefore1990, 0);

        public static NSec FromTime(DateTime time)
        {
            DateTime universalTime = time.ToUniversalTime();

            double totalSeconds = (universalTime - DateTime.UnixEpoch).TotalSeconds;
            int seconds = (int) Math.Floor(totalSeconds);
            int nanoseconds = (int) ((totalSeconds - seconds) * 1000000000.0);

            seconds -= SecondsBefore1990;

            return new NSec(seconds, nanoseconds);
        }
    }
}
