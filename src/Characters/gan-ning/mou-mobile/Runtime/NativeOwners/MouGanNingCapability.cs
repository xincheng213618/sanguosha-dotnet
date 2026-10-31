#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MouGanNingCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterGanNingMouMobileRuntime>> getCharacterGanNingMouMobileRuntime) : IMouGanNingHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterGanNingMouMobileRuntime().Value.BeginMouGn(f, op);
}
