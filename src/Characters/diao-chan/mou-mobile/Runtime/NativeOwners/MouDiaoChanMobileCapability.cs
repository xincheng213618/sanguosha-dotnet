#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MouDiaoChanMobileCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterDiaoChanMouMobileRuntime>> getCharacterDiaoChanMouMobileRuntime) : IMouDiaoChanMobileHost
{
    public SkillProgramStepOutcome Start(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterDiaoChanMouMobileRuntime().Value.StartMouDiaoChanMobile(f, op);
}
