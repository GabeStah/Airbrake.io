using Utility;

namespace NumericLiterals
{
    class Program
    {
        static void Main(string[] args)
        {
            // Integer representation.
            Logging.Log(24601);
            Logging.Log(24_601);

            // Hexadecimal representation.
            Logging.Log(0x6019);
            Logging.Log(0x60_19);

            // Binary representation.
            Logging.Log(0b110000000011001);
            Logging.Log(0b110_0000_0001_1001);

            Logging.Log("My name is Jean Valjean.");
        }
    }
}
