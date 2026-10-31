#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouMengHuoCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterMengHuoMouMobileRuntime>> getCharacterMengHuoMouMobileRuntime) : IMouMengHuoHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterMengHuoMouMobileRuntime().Value.ResolveMouMh(f, op);
}
