#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryGuYongHost(Func<RuntimeEntries.CharacterGuYongBoundaryMobileEntries> getCharacterGuYongBoundaryMobileRuntime) : IBoundaryGuYongHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame frame, SkillProgramEffect effect) => effect.Op == SkillProgramEffectOp.PrepareBoundaryGuYongShenxing ? getCharacterGuYongBoundaryMobileRuntime().FreezeBoundaryGuYongColors(frame, effect) : getCharacterGuYongBoundaryMobileRuntime().PrepareBoundaryGuYongBingyi(frame, effect);
}
