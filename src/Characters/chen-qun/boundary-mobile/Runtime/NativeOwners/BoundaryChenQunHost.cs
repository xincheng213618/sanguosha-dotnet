#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryChenQunHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterChenQunBoundaryMobileRuntime>> getCharacterChenQunBoundaryMobileRuntime) : IBoundaryChenQunHost, IBoundaryChenQunSelection
{
    public bool CanDiscard(int owner, Card card) => getCharacterChenQunBoundaryMobileRuntime().Value.BoundaryDingpinCanDiscard(owner, card);
    public bool CanTarget(int owner, int target) => getCharacterChenQunBoundaryMobileRuntime().Value.BoundaryDingpinCanTarget(owner, target);
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => op == SkillProgramEffectOp.PrepareBoundaryChenQunDingpin ? getCharacterChenQunBoundaryMobileRuntime().Value.PrepareBoundaryDingpin(f) : getCharacterChenQunBoundaryMobileRuntime().Value.ResolveBoundaryDingpin(f);
}
