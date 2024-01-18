using System;
using System.Collections.Generic;

namespace Demo.ScriptsFolder.CoreFolder
{
	public class DisposableService
	{
		private readonly List<IDisposable> _disposables = new List<IDisposable>();

		public void Add(IDisposable disposable) =>
			_disposables.Add(disposable);

		public void Dispose()
		{
			foreach(IDisposable disposable in _disposables)
				disposable.Dispose();
		}
	}
}