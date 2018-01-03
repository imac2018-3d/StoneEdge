using System;

namespace Utils {
	/// <summary>
	/// Convenient extension methods for all object types.
	/// </summary>
	public static class ConvenienceExtensions {
		/// <summary>
		/// Performs an operation on this object if it is not null.
		/// </summary>
		public static void MapNotNull<T>(this T o, Action<T> a) {
			if (o != null)
				a (o);
		}
		/// <summary>
		/// Performs an operation on this object if it is not null, and returns the result.
		/// </summary>
		public static TResult MapNotNull<T,TResult>(this T o, Func<T, TResult> a) {
			return o != null ? a (o) : default(TResult);
		}
	}
}

