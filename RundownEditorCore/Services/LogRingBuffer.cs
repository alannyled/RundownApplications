using System.Collections;

namespace RundownEditorCore.Services
{
    public class LogRingBuffer<T> : IEnumerable<T>
    {
        private readonly T[] _buffer;
        private int _index;
        private bool _isFull;

        public LogRingBuffer(int capacity)
        {
            _buffer = new T[capacity];
            _index = 0;
            _isFull = false;
        }

        public void Add(T item)
        {
            _buffer[_index] = item;
            _index = (_index + 1) % _buffer.Length;
            if (_index == 0) _isFull = true;
        }

        public IEnumerator<T> GetEnumerator()
        {
            int start = _isFull ? _index : 0;
            int count = _isFull ? _buffer.Length : _index;

            for (int i = 0; i < count; i++)
            {
                yield return _buffer[(start + i) % _buffer.Length];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

}
