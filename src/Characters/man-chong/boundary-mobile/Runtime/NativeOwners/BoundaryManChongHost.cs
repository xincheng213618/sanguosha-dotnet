#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryManChongHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterManChongBoundaryMobileRuntime>> getCharacterManChongBoundaryMobileRuntime) : IBoundaryManChongHost
{
    public SkillProgramStepOutcome Choose(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterManChongBoundaryMobileRuntime().Value.BeginBoundaryManChongJunxing(frame, effect);
}
