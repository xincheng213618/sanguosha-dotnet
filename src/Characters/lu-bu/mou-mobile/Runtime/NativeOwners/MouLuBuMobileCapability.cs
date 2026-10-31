#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MouLuBuMobileCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterLuBuMouMobileRuntime>> getCharacterLuBuMouMobileRuntime) : IMouLuBuMobileHost
{
    public SkillProgramStepOutcome Bonus(ProgramSkillFrame f) => getCharacterLuBuMouMobileRuntime().Value.ResolveMouLbBonus(f);
    public SkillProgramStepOutcome Liyu(ProgramSkillFrame f) => getCharacterLuBuMouMobileRuntime().Value.BeginMouLbLiyu(f);
}
