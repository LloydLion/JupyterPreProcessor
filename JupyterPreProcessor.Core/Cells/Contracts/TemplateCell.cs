using JupyterPreProcessor.Core.Segments;

namespace JupyterPreProcessor.Core.Cells.Contracts
{
    public class TemplateCell : Cell
    {
        protected override void Validate()
        {
            var enumerator = Segments.GetEnumerator();
            if (enumerator.MoveNext() == false)
                throw new CellValidationException("Expected at least one segment",
                    typeof(TemplateCell), GetType());

            if (enumerator.Current is not TemplateSegment)
                throw new CellValidationException("First segment must be TemplateSegment",
                    typeof(TemplateCell), GetType(), enumerator.Current);


            while (enumerator.MoveNext())
            {
                if (enumerator.Current is not TextSegment and not TemplateParameterSegment)
                    throw new CellValidationException("Unexpected segment type",
                        typeof(TemplateCell), GetType(), enumerator.Current);
            }
        }
    }
}
