#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryManChongHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterManChongBoundaryMobileEntries> getCharacterManChongBoundaryMobileRuntime) : IBoundaryManChongHost
{
    public SkillProgramStepOutcome Choose(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterManChongBoundaryMobileRuntime().BeginBoundaryManChongJunxing(frame, effect);
}
