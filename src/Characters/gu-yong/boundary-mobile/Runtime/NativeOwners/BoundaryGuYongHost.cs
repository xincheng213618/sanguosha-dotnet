#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryGuYongHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterGuYongBoundaryMobileRuntime>> getCharacterGuYongBoundaryMobileRuntime) : IBoundaryGuYongHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame frame, SkillProgramEffect effect) => effect.Op == SkillProgramEffectOp.PrepareBoundaryGuYongShenxing ? getCharacterGuYongBoundaryMobileRuntime().Value.FreezeBoundaryGuYongColors(frame, effect) : getCharacterGuYongBoundaryMobileRuntime().Value.PrepareBoundaryGuYongBingyi(frame, effect);
}
