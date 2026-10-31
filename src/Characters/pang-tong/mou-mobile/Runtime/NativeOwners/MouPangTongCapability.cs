#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouPangTongCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterPangTongMouMobileRuntime>> getCharacterPangTongMouMobileRuntime) : IMouPangTongHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterPangTongMouMobileRuntime().Value.ResolveMouPt(f, op);
}
