#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFinalTargetPublicPilesEntries
{
    internal bool CanActivatePublicPileFlow(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramInstructionFeatures features) => _resolve().CanActivatePublicPileFlow(owner, features);
    internal bool CanCollectFinalTargetPile(int ownerSeat, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanCollectFinalTargetPile(ownerSeat, context);
    internal global::CardGame.Core.SkillProgramStepOutcome ExecuteFinalTargetPublicPile(global::CardGame.Core.SkillProgramEffect e, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ExecuteFinalTargetPublicPile(e, frame);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> FinalTargetPileChoices(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect e) => _resolve().FinalTargetPileChoices(frame, e);
    internal global::CardGame.Core.CardUseFrame FinalTargetPileUse(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().FinalTargetPileUse(frame);
    internal void ResolveFinalTargetPileChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveFinalTargetPileChoice(selected);
}
