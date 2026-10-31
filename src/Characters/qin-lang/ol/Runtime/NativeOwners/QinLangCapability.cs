#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class QinLangCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterQinLangOlEntries> getCharacterQinLangOlRuntime) : IQinLangHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffect effect) => getCharacterQinLangOlRuntime().BeginQinLang(f, effect.Op == SkillProgramEffectOp.ResolveXianyingTurnEndUses);
}
