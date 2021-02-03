using System;

namespace NetBlade.Core
{
    public interface IResponse
    {
        Exception Exception { get; }

        bool Success { get; }

        void SetException(Exception ex);
    }
}
