#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterKanZeOlEntries
{
    internal bool AllowsGiveAllHandAndResolveXiashuTrigger(ProgramTriggerLegalityContext gate) => _resolve().AllowsGiveAllHandAndResolveXiashuTrigger(gate);
    internal SkillProgramStepOutcome ArmKuanshi(ProgramSkillFrame supplied, int target) => _resolve().ArmKuanshi(supplied, target);
    internal SkillProgramStepOutcome BeginXiashu(ProgramSkillFrame supplied, int recipient) => _resolve().BeginXiashu(supplied, recipient);
    internal bool CanContinueIssuedKanZe(ProgramSkillFrame f) => _resolve().CanContinueIssuedKanZe(f);
    internal bool CanOfferXiashu(int ownerSeat) => _resolve().CanOfferXiashu(ownerSeat);
    internal bool ConsumeKuanshiDrawSkip(int ownerSeat) => _resolve().ConsumeKuanshiDrawSkip(ownerSeat);
    internal bool DrainXiashu(ProgramSkillFrame f) => _resolve().DrainXiashu(f);
    internal void FinishXiashu(ProgramSkillFrame f) => _resolve().FinishXiashu(f);
    internal ProgramDamageInterception InterceptKuanshiDamage(GameEngine.IDamageAttempt attack, int amount, DamageNature nature) => _resolve().InterceptKuanshiDamage(attack, amount, nature);
    internal bool KanZeFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().KanZeFirstChild(root, child);
    internal ProgramSkillFrame? KanZeObserverRoot() => _resolve().KanZeObserverRoot();
    internal CardConversionSource KanZeSource(ProgramSkillFrame f) => _resolve().KanZeSource(f);
    internal void ObserveKuanshiExpiry(IGameEvent payload) => _resolve().ObserveKuanshiExpiry(payload);
    internal void PublishXiashuDecision(ProgramSkillFrame f) => _resolve().PublishXiashuDecision(f);
    internal void ResolveXiashuChoice(PromptChoice choice) => _resolve().ResolveXiashuChoice(choice);
    internal bool ResumeXiashu(long id) => _resolve().ResumeXiashu(id);
    internal bool ReturnXiashuMovement(ProgramSkillFrame f) => _resolve().ReturnXiashuMovement(f);
    internal bool TryAdvanceKanZeSubtree() => _resolve().TryAdvanceKanZeSubtree();
    internal bool TryPreventKuanshiDamage(int? sourceSeat, int targetSeat, int actualAmount, long damageAttemptResolutionId) => _resolve().TryPreventKuanshiDamage(sourceSeat, targetSeat, actualAmount, damageAttemptResolutionId);
    internal PromptChoice XiashuAiChoice(ProgramSkillFrame f) => _resolve().XiashuAiChoice(f);
    internal PromptChoice XiashuChoice(ProgramSkillFrame f, string branch, string label, IReadOnlyList<int>? cards = default) => _resolve().XiashuChoice(f, branch, label, cards);
    internal IReadOnlyList<PromptChoice> XiashuChoices(ProgramSkillFrame f) => _resolve().XiashuChoices(f);
}
