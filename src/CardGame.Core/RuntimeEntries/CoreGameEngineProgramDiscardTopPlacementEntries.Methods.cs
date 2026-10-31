#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramDiscardTopPlacementEntries
{
    internal void PublishProgramDiscardTopPlacement(ProgramSkillFrame frame) => _resolve().PublishProgramDiscardTopPlacement(frame);
    internal SkillProgramStepOutcome PutProgramDiscardedCardsOnDrawPileTop(ProgramSkillFrame frame) => _resolve().PutProgramDiscardedCardsOnDrawPileTop(frame);
    internal void ResolveProgramDiscardTopPlacementChoice(PromptChoice choice) => _resolve().ResolveProgramDiscardTopPlacementChoice(choice);
}
