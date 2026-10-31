#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MouLuXunCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterLuXunMouMobileRuntime>> getCharacterLuXunMouMobileRuntime) : IMouLuXunHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp operation) => getCharacterLuXunMouMobileRuntime().Value.BeginMouLuXun(f, operation);
}
