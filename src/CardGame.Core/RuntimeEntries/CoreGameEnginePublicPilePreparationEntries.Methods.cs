#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePublicPilePreparationEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginPublicPilePreparation(global::CardGame.Core.SkillProgramEffect effect, global::CardGame.Core.ProgramSkillFrame input) => _resolve().BeginPublicPilePreparation(effect, input);
    internal bool CanRunPublicPilePreparation(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanRunPublicPilePreparation(candidate, trigger, context);
    internal bool DrainPublicPilePreparation(global::CardGame.Core.ProgramSkillFrame f, bool movement) => _resolve().DrainPublicPilePreparation(f, movement);
    internal bool ExactPreparationParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().ExactPreparationParent(f, op);
    internal void FinishPublicPilePreparation(global::CardGame.Core.ProgramSkillFrame f, bool completed) => _resolve().FinishPublicPilePreparation(f, completed);
    internal bool PreparationGameEnded() => _resolve().PreparationGameEnded();
    internal bool PreparationPileCurrent(global::CardGame.Core.PublicPersistentPileSource p) => _resolve().PreparationPileCurrent(p);
    internal global::CardGame.Core.PublicPersistentPileSource? PreparationReferencedPile(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect effect) => _resolve().PreparationReferencedPile(f, effect);
    internal bool PreparationSourceAlive(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PreparationSourceAlive(f);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.Card> PreparationStoreCards(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PreparationStoreCards(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> PublicPilePreparationChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublicPilePreparationChoices(f);
    internal void PublishPublicPilePreparation(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishPublicPilePreparation(f);
    internal void ResolvePublicPilePreparationChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolvePublicPilePreparationChoice(choice);
    internal bool ResumePublicPilePreparation(long id) => _resolve().ResumePublicPilePreparation(id);
    internal bool ReturnPublicPilePreparationMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnPublicPilePreparationMovement(f);
    internal global::CardGame.Core.PromptChoice SelectAiPublicPilePreparation(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiPublicPilePreparation(p, f);
    internal long PreparationSequence { get => _resolve().PreparationSequence; }
}
