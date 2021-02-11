using Pumkin.AvatarTools2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pumkin.AvatarTools2.Copiers
{
    public static class IComponentCopierExtensions
    {
        public static IComponentDestroyer GetTypeDestroyer(this IComponentCopier copier)
        {
            return IDManager.Items
                .Where(d => d.Value is IComponentDestroyer)
                .Select(x => x.Value as IComponentDestroyer)
                .FirstOrDefault(x => x.ComponentTypesFullNames.Intersect(copier.ComponentTypesFullNames).Any());
        }
    }
}
