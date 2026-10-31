#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramPindianProducer(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEnginePindianEntries> getCoreGameEnginePindianRuntime) : IProgramPindianProducer
{
    public void Begin(long programFrameId, SkillPromptPresentation presentation, int sourceSeat, int opponentSeat, string resultBind, SkillProgramCardSetVisibility visibility) => getCoreGameEnginePindianRuntime().BeginSharedPindian(programFrameId, presentation, sourceSeat, opponentSeat, programResultBind: resultBind, programResultVisibility: visibility);
}
