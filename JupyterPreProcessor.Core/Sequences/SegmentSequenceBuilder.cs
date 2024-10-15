using JupyterPreProcessor.Core.Segments;
using System.Collections;

namespace JupyterPreProcessor.Core.Sequences
{
	public class SegmentSequenceBuilder : IList<Segment>
	{
		private readonly List<Segment> _segments;


		public SegmentSequenceBuilder(ISegmentSequence baseSequence)
		{
			_segments = new List<Segment>(baseSequence);
		}

		public SegmentSequenceBuilder()
		{
			_segments = [];
		}


		public int Count => ((ICollection<Segment>)_segments).Count;

		public bool IsReadOnly => false;


		public Segment this[int index]
		{
			get => _segments[index];
			set => _segments[index] = value;
		}


		public void Foreach(Predicate<Segment> filter, Func<Segment, IEnumerable<Segment>> processor)
		{
			var limit = _segments.Count;
			for (int i = 0; i < limit; i++)
			{
				var segment = _segments[i];
				if (filter(segment) == false)
					continue;

				var newSegments = processor(segment).ToArray();

				_segments.RemoveAt(i);
				_segments.InsertRange(i, newSegments);

				limit += newSegments.Length - 1;
				i += newSegments.Length - 1;
			}
		}

		public void Foreach(Func<Segment, IEnumerable<Segment>> processor) => Foreach(s => true, processor);

		public void Foreach(Predicate<Segment> filter, Action<Segment> processor)
		{
			foreach (var item in _segments.Where(new Func<Segment, bool>(filter)))
				processor(item);
		}

		public void Foreach(Action<Segment> processor)
		{
			foreach (var item in _segments)
				processor(item);
		}

		public void Foreach(Predicate<Segment> filter, Func<Segment, Segment?> processor)
		{
			var limit = _segments.Count;
			for (int i = 0; i < limit; i++)
			{
				var segment = _segments[i];
				if (filter(segment) == false)
					continue;

				var newSegment = processor(segment);

				if (newSegment is null)
				{
					_segments.RemoveAt(i);
					limit -= 1;
					i -= 1;
				}
				else
				{
					_segments[i] = newSegment;
				}
			}
		}

		public void Foreach(Func<Segment, Segment?> processor) => Foreach(s => true, processor);

		public ISegmentSequence Build()
		{
			return new SegmentSequence(_segments);
		}

		public int IndexOf(Segment item) => _segments.IndexOf(item);

		public void Insert(int index, Segment item) => _segments.Insert(index, item);

		public void RemoveAt(int index) => _segments.RemoveAt(index);

		public void Add(Segment item) => _segments.Add(item);

		public void Clear() => _segments.Clear();

		public bool Contains(Segment item) => _segments.Contains(item);

		public void CopyTo(Segment[] array, int arrayIndex) => _segments.CopyTo(array, arrayIndex);

		public bool Remove(Segment item) => _segments.Remove(item);

		public IEnumerator<Segment> GetEnumerator() => _segments.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
