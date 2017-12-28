using System;

namespace Kengic.Was.Application.Services.Common
{
    public interface IEditApplicationService<in TValue>
    {
        Tuple<bool, string> Create(TValue value);
        Tuple<bool, string> Update(TValue value);
        Tuple<bool, string> Remove(TValue value);
    }
}