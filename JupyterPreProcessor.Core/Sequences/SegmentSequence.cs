using JupyterPreProcessor.Core.Segments;
using System.Collections;

namespace JupyterPreProcessor.Core.Sequences
{
	public class SegmentSequence : ISegmentSequence
	{
		private readonly List<Segment> _segments;


		public SegmentSequence(IEnumerable<Segment> segments)
		{
			_segments = new(segments);
		}


		public Segment this[int index] => _segments[index];


		public int Count => _segments.Count;


		public ISegmentSequenceEnumerator GetEnumerator()
		{
			return new Enumerator(this, -1);
		}

		public ISegmentSequenceEnumerator GetEnumerator(int index)
		{
			return new Enumerator(this, index);
		}

		public ISegmentSequenceEnumerator? GetEnumerator(Predicate<Segment> condition)
		{
			var index = _segments.FindIndex(condition);
			if (index == -1)
				return null;
			else return new Enumerator(this, index);
		}

		IEnumerator<Segment> IEnumerable<Segment>.GetEnumerator() => GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


		private class Enumerator : ISegmentSequenceEnumerator
		{
			private readonly SegmentSequence _owner;
			private int _index;


			public Enumerator(SegmentSequence owner, int index)
			{
				_owner = owner;
				_index = index;
			}


			public Segment Current => _owner[_index];

			object IEnumerator.Current => Current;

			public void Dispose() { }

			public bool MoveNext()
			{
				if (_index + 1 < _owner.Count)
				{
					_index++;
					return true;
				}
				else return false;
			}

			public bool MovePrevious()
			{
				if (_index - 1 >= -1)
				{
					_index--;
					return true;
				}
				else return false;
			}

			public void Reset()
			{
				throw new NotSupportedException();
			}
		}
	}
}
