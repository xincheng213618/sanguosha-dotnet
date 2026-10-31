#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramDiscardTopPlacementEntries
{
    internal void AssertProgramDiscardTopPlacement(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().AssertProgramDiscardTopPlacement(frame);
    internal void PublishProgramDiscardTopPlacement(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishProgramDiscardTopPlacement(frame);
    internal global::CardGame.Core.SkillProgramStepOutcome PutProgramDiscardedCardsOnDrawPileTop(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PutProgramDiscardedCardsOnDrawPileTop(frame);
    internal void ResolveProgramDiscardTopPlacementChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveProgramDiscardTopPlacementChoice(choice);
}
