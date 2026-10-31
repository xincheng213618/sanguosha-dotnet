#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class QinLangCapability(Func<RuntimeEntries.CharacterQinLangOlEntries> getCharacterQinLangOlRuntime) : IQinLangHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffect effect) => getCharacterQinLangOlRuntime().BeginQinLang(f, effect.Op == SkillProgramEffectOp.ResolveXianyingTurnEndUses);
}
