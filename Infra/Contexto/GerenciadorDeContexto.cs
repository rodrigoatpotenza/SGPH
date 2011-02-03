using System;
using System.Data.Objects;
using System.Web;

namespace SgphMvc.Models.Contexto
{
    public static class GerenciadorDeContexto
    {
        public static T ObtemContexto<T>()
            where T : ObjectContext
        {
            var ocKey = "ocm_" + HttpContext.Current.GetHashCode().ToString("x");
            if (HttpContext.Current != null)
            {
                if (!HttpContext.Current.Items.Contains(ocKey))
                {
                    var ctx = typeof(T).GetConstructor(Type.EmptyTypes).Invoke(Type.EmptyTypes) as T;
                    if (ctx != null) HttpContext.Current.Items.Add(ocKey, ctx);
                }

                return HttpContext.Current.Items[ocKey] as T;
            }

            return typeof(T).GetConstructor(Type.EmptyTypes).Invoke(Type.EmptyTypes) as T;
        }
    }
}