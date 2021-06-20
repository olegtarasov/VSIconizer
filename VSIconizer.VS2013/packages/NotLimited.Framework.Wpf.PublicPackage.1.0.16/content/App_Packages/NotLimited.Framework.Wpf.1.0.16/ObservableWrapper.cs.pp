//////////////////////////////////////////////////////////////////////////
// This file is a part of NotLimited.Framework.Wpf NuGet package.
// You are strongly discouraged from fiddling with it.
// If you do, all hell will break loose and living will envy the dead.
//////////////////////////////////////////////////////////////////////////
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace NotLimited.Framework.Wpf
{
	public class ObservableWrapper<T> : IEnumerable<T>, INotifyCollectionChanged, INotifyPropertyChanged
	{
		private readonly Func<IEnumerable<T>> _collectionAccessor;

		public ObservableWrapper(Func<IEnumerable<T>> collectionAccessor)
		{
			_collectionAccessor = collectionAccessor;
		}

		public IEnumerator<T> GetEnumerator()
		{
			return _collectionAccessor().GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public event NotifyCollectionChangedEventHandler CollectionChanged;

		public virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			var handler = CollectionChanged;
			if (handler != null) handler(this, e);
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName = null)
		{
			var handler = PropertyChanged;
			if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}