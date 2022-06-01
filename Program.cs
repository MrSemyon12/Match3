namespace Match3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (GameRunner game = new GameRunner())
            {
                game.Run();
            }
        }
    }
}