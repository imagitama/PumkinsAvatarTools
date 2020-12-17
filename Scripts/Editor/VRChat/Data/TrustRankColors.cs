using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Pumkin.AvatarTools2.VRChat
{
    static class TrustRankColors
    {
        static ReadOnlyDictionary<TrustRanks, Color> Ranks = new ReadOnlyDictionary<TrustRanks, Color>(new Dictionary<TrustRanks, Color>()
        {
            {TrustRanks.Visitor, new Color(0.8f, 0.8f, 0.8f, 1)},
            {TrustRanks.NewUser, new Color(0.09020f, 0.47059f, 1f, 1)},
            {TrustRanks.User, new Color(0.16863f, .81176f, 0.36078f, 1)},
            {TrustRanks.KnownUser, new Color(1f, 0.48235f, 0.25882f, 1)},
            {TrustRanks.Trusted, new Color(0.50588f, 0.26275f, 0.90196f, 1)},
            {TrustRanks.Friend,  new Color(1f, 1f, 0f, 1)}
        });

        public static Color GetColorForRank(TrustRanks rank)
        {
            Ranks.TryGetValue(rank, out Color value);
            return value;
        }
    }
}
