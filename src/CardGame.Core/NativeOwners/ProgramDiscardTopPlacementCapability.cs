#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramDiscardTopPlacementCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramDiscardTopPlacementEntries> getCoreGameEngineProgramDiscardTopPlacementRuntime) : IProgramDiscardTopPlacementHost
{
    public SkillProgramStepOutcome PutDiscardedCardsOnDrawPileTop(ProgramSkillFrame frame) => getCoreGameEngineProgramDiscardTopPlacementRuntime().PutProgramDiscardedCardsOnDrawPileTop(frame);
}
