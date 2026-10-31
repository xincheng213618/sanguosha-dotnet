#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryZhangFeiCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterZhangFeiBoundaryMobileRuntime>> getCharacterZhangFeiBoundaryMobileRuntime) : IBoundaryZhangFeiHost
{
    public SkillProgramStepOutcome ResolveBoundaryZhangFei(ProgramSkillFrame frame, SkillProgramEffectOp operation) => getCharacterZhangFeiBoundaryMobileRuntime().Value.ResolveBoundaryZhangFei(frame, operation);
}
