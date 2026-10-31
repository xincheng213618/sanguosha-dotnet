#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryZhuHuanHost(Func<RuntimeEntries.CharacterZhuHuanBoundaryMobileEntries> getCharacterZhuHuanBoundaryMobileRuntime) : IBoundaryZhuHuanHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame) => getCharacterZhuHuanBoundaryMobileRuntime().BeginBoundaryPingkou(frame);
    public void SkipDraw(ProgramSkillFrame frame) => getCharacterZhuHuanBoundaryMobileRuntime().SkipBoundaryZhuHuanDraw(frame);
}
