using JupyterPreProcessor.Core.Segments;

namespace JupyterPreProcessor.Core.Cells.Contracts
{
    public class SourceCell : Cell
    {
        protected override void Validate()
        {
            var enumerator = Segments.GetEnumerator();
            if (enumerator.MoveNext() == false)
                return;


            while (enumerator.Current is ModSegment)
            {
                var hasNext = enumerator.MoveNext();
                if (hasNext == false)
                    return;
            }


            while (enumerator.MoveNext())
            {
                if (enumerator.Current is not TextSegment and not TagSegment)
                    throw new CellValidationException("Unexpected segment type",
                        typeof(SourceCell), GetType(), enumerator.Current);
            }
        }
    }
}
