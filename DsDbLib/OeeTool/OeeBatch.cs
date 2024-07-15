namespace DsDbLib.OeeTool
{
    public class OeeBatch
    {
        public TimeSpan CycleTime { get; set; }
        public int BatchId { get; set; }
        public string? BatchName { get; set; }
        public int State { get; set; }
        public int RecipeId { get; set; }
        public string? RecipeName { get; set; }
    }

}
