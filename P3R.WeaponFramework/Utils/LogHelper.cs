using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3R.WeaponFramework.Utils
{
    internal static class LogHelper
    {
        public static bool FemininePlayer = false;
        private static List<ECharacter> masculine = [ECharacter.Stupei, ECharacter.Akihiko, ECharacter.Ken, ECharacter.Shinjiro];
        private static List<ECharacter> feminine = [ECharacter.Yukari, ECharacter.Mitsuru, ECharacter.Fuuka, ECharacter.Aigis, ECharacter.Metis];
        public static void Init(bool useFEMC)
        {
            var m = new StringBuilder();
            if (useFEMC)
                feminine.Add(ECharacter.Player);
            else
                masculine.Add(ECharacter.Player);
        }
        public static string ToSubjPronoun(this ECharacter character)
            => character switch
            {
                ECharacter.NONE => "they",
                _ => feminine.Contains(character) ? "she" : "he"
            };
        public static string ToObjPronoun(this ECharacter character)
            => character switch
            {
                ECharacter.NONE => "them",
                _ => feminine.Contains(character) ? "her" : "him"
            };
        public static string ToPossAdject(this ECharacter character)
            => character switch
            {
                ECharacter.NONE => "theirs",
                _ => feminine.Contains(character) ? "her" : "his"
            };
        public static string ToPossPronoun(this ECharacter character)
            => character switch
            {
                ECharacter.NONE => "theirs",
                _ => feminine.Contains(character) ? "hers" : "his"
            };
        public static string ToPossPNoun(this ECharacter character)
            => character switch
            {
                ECharacter.NONE => "the unassigned",
                ECharacter.Player => "the Player's",
                _ => $"{character}'s",
            };
    }
}
