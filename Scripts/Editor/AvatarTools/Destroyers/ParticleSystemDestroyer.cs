using Pumkin.Core;
using UnityEngine;

namespace Pumkin.AvatarTools2.Destroyers
{
    [AutoLoad(DefaultIDs.Destroyers.ParticleSystem, ParentModuleID = DefaultIDs.Modules.Destroyer)]
    class ParticleSystemDestroyer : ComponentDestroyerBase
    {
        public override string[] ComponentTypesFullNames { get; } = new string[]
        {
            typeof(ParticleSystem).FullName,
            typeof(ParticleSystemRenderer).FullName
        };
    }
}