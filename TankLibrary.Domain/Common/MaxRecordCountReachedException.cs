using System;
namespace TankLibrary.Domain.Common
{
    public class MaxRecordCountReachedException : Exception
    {
        public MaxRecordCountReachedException()
            : base("Max record count reached!")
        {
        }

        public MaxRecordCountReachedException(string message)
            : base(message)
        {

        }
    }
}
