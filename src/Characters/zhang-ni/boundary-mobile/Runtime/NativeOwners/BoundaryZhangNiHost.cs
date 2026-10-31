#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryZhangNiHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterZhangNiBoundaryMobileRuntime>> getCharacterZhangNiBoundaryMobileRuntime) : IBoundaryZhangNiHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffect e) => e.Op == SkillProgramEffectOp.ConsumeBoundaryZhangNiDrawSkip ? getCharacterZhangNiBoundaryMobileRuntime().Value.ConsumeBoundaryWurongSkip(f) : getCharacterZhangNiBoundaryMobileRuntime().Value.BeginBoundaryWurong(f, e);
}
