namespace Neat_Implemtation
{
    public class GlobalInnovation
    {
        private static int innovationNumber = 0;

        public static int Next { get => innovationNumber++;  }
    }
}
