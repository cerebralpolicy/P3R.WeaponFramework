using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3R.WeaponFramework.Hooks
{
    internal unsafe class EpisodeHook
    {
        private delegate bool IsAstreaSave();
        private IsAstreaSave? isAstreaSave;

        public EpisodeHook()
        {
            ScanHooks.Add(
                nameof(IsAstreaSave),
                "48 83 EC 28 E8 ?? ?? ?? ?? 48 85 C0 74 ?? E8 ?? ?? ?? ?? 48 8B C8 E8 ?? ?? ?? ?? 3C 01 0F 94 C0 48 83 C4 28 C3 48 83 C4 28 C3",
                (hooks, result) => this.isAstreaSave = hooks.CreateWrapper<IsAstreaSave>(result, out _));
        }

        public bool IsAstrea() => this.isAstreaSave!();



    }
}
