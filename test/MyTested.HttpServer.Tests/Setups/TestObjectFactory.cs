namespace MyTested.HttpServer.Tests.Setups
{
    using System;

    public class TestObjectFactory
    {
        public static Action<string, string, string> GetFailingValidationAction()
        {
            return (x, y, z) => { throw new NullReferenceException(string.Format("{0} {1} {2}", x, y, z)); };
        }
    }
}
