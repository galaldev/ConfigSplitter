using System;
using System.Collections.Generic;
using System.Linq;
namespace ConfigSplitter
{
    public static class Chainning
    {
        public static List<T> Pipeline<T>(this T @this) => new List<T>() { @this };
        public static List<T> Exec<T>(this IEnumerable<T> @this, Action<T> action) => Exec(@this.ToList(), action);
        public static List<T> Exec<T>(this List<T> @this, Action<T> action)
        {
            foreach (T item in @this)
                action(item);
            return @this;
        }
        public static When<T> When<T>(this T @this, Func<T, bool> predicate) => new When<T>(@this, predicate(@this));
        public static When<T> Then<T>(this When<T> @this, Action action)
        {
            if (@this.IsTrue)
                action();
            return new When<T>(@this.Source, @this.IsTrue);
        }
        public static T Else<T>(this When<T> @this, Action action)
        {
            if (!@this.IsTrue)
                action();
            return @this.Source;
        }
    }
}
