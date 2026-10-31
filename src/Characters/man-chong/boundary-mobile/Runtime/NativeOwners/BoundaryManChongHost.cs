#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryManChongHost(Func<RuntimeEntries.CharacterManChongBoundaryMobileEntries> getCharacterManChongBoundaryMobileRuntime) : IBoundaryManChongHost
{
    public SkillProgramStepOutcome Choose(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterManChongBoundaryMobileRuntime().BeginBoundaryManChongJunxing(frame, effect);
}
