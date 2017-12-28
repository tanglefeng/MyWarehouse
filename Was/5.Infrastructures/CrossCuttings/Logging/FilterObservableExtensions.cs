using System;
using System.Reactive.Linq;

namespace Kengic.Was.CrossCutting.Logging
{
    public static class FilterObservableExtensions
    {
        public static IObservable<T> FilterOnTrigger<T>(this IObservable<T> stream, Func<T, bool> shouldAdd)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }
            if (shouldAdd == null)
            {
                throw new ArgumentNullException(nameof(shouldAdd));
            }

            return Observable.Create<T>(observer =>
            {
                var subscription = stream.Subscribe(
                    newItem =>
                    {
                        bool result;
                        try
                        {
                            result = shouldAdd(newItem);
                        }
                        catch (Exception ex)
                        {
                            observer.OnError(ex);
                            return;
                        }

                        if (result)
                        {
                            observer.OnNext(newItem);
                        }
                    },
                    observer.OnError,
                    observer.OnCompleted);

                return subscription;
            });
        }
    }
}