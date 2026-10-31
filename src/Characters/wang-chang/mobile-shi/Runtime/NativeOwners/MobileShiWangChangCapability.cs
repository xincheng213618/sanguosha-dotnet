#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MobileShiWangChangCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterWangChangMobileShiRuntime>> getCharacterWangChangMobileShiRuntime) : IMobileShiWangChangHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame, SkillProgramEffectOp op) => getCharacterWangChangMobileShiRuntime().Value.ResolveShiWangChang(frame, op);
}
