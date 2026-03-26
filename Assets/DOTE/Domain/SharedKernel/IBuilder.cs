using System;

namespace DOTE.SharedKernel.Domain
{
    public interface IBuilder
    {
        public Type Type { get; }
        public object Build();
    }

    public interface IBuilder<TObject> : IBuilder
    {
        public new TObject Build();
    }

    public abstract class ABuilder<TObject> : IBuilder<TObject> where TObject : class
    {
        public Type Type { get; private set; }

        public ABuilder(Type type)
        {
            Type = type;
        }

        public abstract TObject Build();

        object IBuilder.Build()
        {
            return Build();
        }
    }
}