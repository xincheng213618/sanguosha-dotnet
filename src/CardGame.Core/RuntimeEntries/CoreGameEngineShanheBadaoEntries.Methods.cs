#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheBadaoEntries
{
    internal bool MatchesShanheBadaoDamageModifier(global::CardGame.Core.SkillProgramDamageModifier modifier, int sourceSeat, bool chain) => _resolve().MatchesShanheBadaoDamageModifier(modifier, sourceSeat, chain);
    internal int ShanhePreviousRoundDamageAtMostThree(int sourceSeat) => _resolve().ShanhePreviousRoundDamageAtMostThree(sourceSeat);
}
