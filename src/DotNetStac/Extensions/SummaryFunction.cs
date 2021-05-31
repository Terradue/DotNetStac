namespace Stac.Extensions
{
    public class SummaryFunction
    {
        public IStacExtension Extension { get; }

        public string PropertyName { get; }

        public CreateSummary Summarize { get; }

        public SummaryFunction(IStacExtension extension, string propertyName, CreateSummary summaryFunction)

        {
            Extension = extension;
            PropertyName = propertyName;
            Summarize = summaryFunction;
        }
    }
}
