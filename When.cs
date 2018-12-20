namespace ConfigSplitter
{
    public class When<T>
    {
        public When(T source, bool isTrue)
        {
            Source = source;
            IsTrue = isTrue;
        }
        public T Source { get; set; }
        public bool IsTrue { get; set; }
    }
}
