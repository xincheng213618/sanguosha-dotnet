#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MouLuZhiCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterLuZhiMouMobileRuntime>> getCharacterLuZhiMouMobileRuntime) : IMouLuZhiHost
{
    public SkillProgramStepOutcome Start(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterLuZhiMouMobileRuntime().Value.StartMouLuZhi(f, op);
}
