#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryGuYongHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterGuYongBoundaryMobileEntries> getCharacterGuYongBoundaryMobileRuntime) : IBoundaryGuYongHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame frame, SkillProgramEffect effect) => effect.Op == SkillProgramEffectOp.PrepareBoundaryGuYongShenxing ? getCharacterGuYongBoundaryMobileRuntime().FreezeBoundaryGuYongColors(frame, effect) : getCharacterGuYongBoundaryMobileRuntime().PrepareBoundaryGuYongBingyi(frame, effect);
}
