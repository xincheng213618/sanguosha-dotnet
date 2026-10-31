#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryChenQunHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterChenQunBoundaryMobileEntries> getCharacterChenQunBoundaryMobileRuntime) : IBoundaryChenQunHost, IBoundaryChenQunSelection
{
    public bool CanDiscard(int owner, Card card) => getCharacterChenQunBoundaryMobileRuntime().BoundaryDingpinCanDiscard(owner, card);
    public bool CanTarget(int owner, int target) => getCharacterChenQunBoundaryMobileRuntime().BoundaryDingpinCanTarget(owner, target);
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => op == SkillProgramEffectOp.PrepareBoundaryChenQunDingpin ? getCharacterChenQunBoundaryMobileRuntime().PrepareBoundaryDingpin(f) : getCharacterChenQunBoundaryMobileRuntime().ResolveBoundaryDingpin(f);
}
