using System;
using UnityEngine;

namespace DOTE.SharedKernel.Domain
{
    public interface IViewBuilder
    {
        public Type type { get; }
        public object Build(object model, Transform parent);
    }

    public interface IViewBuilder<TModel, TView> : IViewBuilder
    {
        public new TView Build(TModel model, Transform parent);
    }

    public class ViewBuilder<TModel, TView> : IViewBuilder<TModel, TView> where TModel : class where TView : MonoBehaviour
    {
        public static ViewBuilder<TModel, TView> Create(Type actualType, Func<TModel, Transform, TView> buildDelegate)
        {
            return new ViewBuilder<TModel, TView>
            {
                type = actualType,
                buildDelegate = buildDelegate
            };
        }

        public Type type { get; private set; }

        private Func<TModel, Transform, TView> buildDelegate;

        public TView Build(TModel model, Transform parent)
        {
            return Build(model, parent);
        }

        object IViewBuilder.Build(object model, Transform parent)
        {
            return buildDelegate.Invoke(model as TModel, parent);
        }
    }
}