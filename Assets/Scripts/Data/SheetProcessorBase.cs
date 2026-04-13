public abstract class SheetProcessorBase
{
    public abstract string SheetName { get; }
    public abstract void ProcessData(string[] lines);
}
