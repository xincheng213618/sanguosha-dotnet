#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterKanZeOlEntries
{
    internal bool AllowsGiveAllHandAndResolveXiashuTrigger(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().AllowsGiveAllHandAndResolveXiashuTrigger(gate);
    internal global::CardGame.Core.SkillProgramStepOutcome ArmKuanshi(global::CardGame.Core.ProgramSkillFrame supplied, int target) => _resolve().ArmKuanshi(supplied, target);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginXiashu(global::CardGame.Core.ProgramSkillFrame supplied, int recipient) => _resolve().BeginXiashu(supplied, recipient);
    internal bool CanContinueIssuedKanZe(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueIssuedKanZe(f);
    internal bool CanOfferXiashu(int ownerSeat) => _resolve().CanOfferXiashu(ownerSeat);
    internal bool ConsumeKuanshiDrawSkip(int ownerSeat) => _resolve().ConsumeKuanshiDrawSkip(ownerSeat);
    internal bool DrainXiashu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainXiashu(f);
    internal void FinishXiashu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishXiashu(f);
    internal global::CardGame.Core.ProgramDamageInterception InterceptKuanshiDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack, int amount, global::CardGame.Core.DamageNature nature) => _resolve().InterceptKuanshiDamage(attack, amount, nature);
    internal bool KanZeFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().KanZeFirstChild(root, child);
    internal global::CardGame.Core.ProgramSkillFrame? KanZeObserverRoot() => _resolve().KanZeObserverRoot();
    internal global::CardGame.Core.CardConversionSource KanZeSource(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KanZeSource(f);
    internal void ObserveKuanshiExpiry(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveKuanshiExpiry(payload);
    internal void PublishXiashuDecision(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishXiashuDecision(f);
    internal void ResolveXiashuChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveXiashuChoice(choice);
    internal bool ResumeXiashu(long id) => _resolve().ResumeXiashu(id);
    internal bool ReturnXiashuMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnXiashuMovement(f);
    internal bool TryAdvanceKanZeSubtree() => _resolve().TryAdvanceKanZeSubtree();
    internal bool TryPreventKuanshiDamage(int? sourceSeat, int targetSeat, int actualAmount, long damageAttemptResolutionId) => _resolve().TryPreventKuanshiDamage(sourceSeat, targetSeat, actualAmount, damageAttemptResolutionId);
    internal global::CardGame.Core.PromptChoice XiashuAiChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().XiashuAiChoice(f);
    internal global::CardGame.Core.PromptChoice XiashuChoice(global::CardGame.Core.ProgramSkillFrame f, string branch, string label, global::System.Collections.Generic.IReadOnlyList<int>? cards = default) => _resolve().XiashuChoice(f, branch, label, cards);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> XiashuChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().XiashuChoices(f);
}
