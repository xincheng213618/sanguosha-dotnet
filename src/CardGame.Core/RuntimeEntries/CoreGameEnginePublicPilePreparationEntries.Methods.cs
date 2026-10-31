#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePublicPilePreparationEntries
{
    internal SkillProgramStepOutcome BeginPublicPilePreparation(SkillProgramEffect effect, ProgramSkillFrame input) => _resolve().BeginPublicPilePreparation(effect, input);
    internal bool CanRunPublicPilePreparation(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanRunPublicPilePreparation(candidate, trigger, context);
    internal bool DrainPublicPilePreparation(ProgramSkillFrame f, bool movement) => _resolve().DrainPublicPilePreparation(f, movement);
    internal bool ExactPreparationParent(ProgramSkillFrame f, SkillProgramEffectOp op) => _resolve().ExactPreparationParent(f, op);
    internal void FinishPublicPilePreparation(ProgramSkillFrame f, bool completed) => _resolve().FinishPublicPilePreparation(f, completed);
    internal bool PreparationGameEnded() => _resolve().PreparationGameEnded();
    internal bool PreparationPileCurrent(PublicPersistentPileSource p) => _resolve().PreparationPileCurrent(p);
    internal PublicPersistentPileSource? PreparationReferencedPile(ProgramSkillFrame f, SkillProgramEffect effect) => _resolve().PreparationReferencedPile(f, effect);
    internal bool PreparationSourceAlive(ProgramSkillFrame f) => _resolve().PreparationSourceAlive(f);
    internal IEnumerable<Card> PreparationStoreCards(ProgramSkillFrame f) => _resolve().PreparationStoreCards(f);
    internal IReadOnlyList<PromptChoice> PublicPilePreparationChoices(ProgramSkillFrame f) => _resolve().PublicPilePreparationChoices(f);
    internal void PublishPublicPilePreparation(ProgramSkillFrame f) => _resolve().PublishPublicPilePreparation(f);
    internal void ResolvePublicPilePreparationChoice(PromptChoice choice) => _resolve().ResolvePublicPilePreparationChoice(choice);
    internal bool ResumePublicPilePreparation(long id) => _resolve().ResumePublicPilePreparation(id);
    internal bool ReturnPublicPilePreparationMovement(ProgramSkillFrame f) => _resolve().ReturnPublicPilePreparationMovement(f);
    internal PromptChoice SelectAiPublicPilePreparation(PendingDecision p, ProgramSkillFrame f) => _resolve().SelectAiPublicPilePreparation(p, f);
    internal long PreparationSequence { get => _resolve().PreparationSequence; }
}
