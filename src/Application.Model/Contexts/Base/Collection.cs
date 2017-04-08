using Newtonsoft.Json;

namespace Farfetch.Application.Model.Contexts.Base
{
    public abstract class Collection<T> : Resource
    {
        public T[] Items { get; set; }
    }
}
