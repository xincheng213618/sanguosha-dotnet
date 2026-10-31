#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFinalTargetPublicPilesEntries
{
    internal bool CanActivatePublicPileFlow(CharacterState owner, ProgramInstructionFeatures features) => _resolve().CanActivatePublicPileFlow(owner, features);
    internal bool CanCollectFinalTargetPile(int ownerSeat, ProgramSkillWindowContext context) => _resolve().CanCollectFinalTargetPile(ownerSeat, context);
    internal SkillProgramStepOutcome ExecuteFinalTargetPublicPile(SkillProgramEffect e, ProgramSkillFrame frame) => _resolve().ExecuteFinalTargetPublicPile(e, frame);
    internal IReadOnlyList<PromptChoice> FinalTargetPileChoices(ProgramSkillFrame frame, SkillProgramEffect e) => _resolve().FinalTargetPileChoices(frame, e);
    internal CardUseFrame FinalTargetPileUse(ProgramSkillFrame frame) => _resolve().FinalTargetPileUse(frame);
    internal void ResolveFinalTargetPileChoice(PromptChoice selected) => _resolve().ResolveFinalTargetPileChoice(selected);
}
