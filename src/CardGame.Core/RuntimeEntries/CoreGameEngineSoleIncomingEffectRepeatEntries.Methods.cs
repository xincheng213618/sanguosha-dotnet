#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSoleIncomingEffectRepeatEntries
{
    internal bool CanOfferSoleIncomingEffect(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferSoleIncomingEffect(candidate, trigger, context);
    internal void CaptureSoleIncomingAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().CaptureSoleIncomingAttack(attack);
    internal void CompleteSoleIncomingExtraRound(long id) => _resolve().CompleteSoleIncomingExtraRound(id);
    internal bool ContinueSoleIncomingFangtian(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().ContinueSoleIncomingFangtian(attack);
    internal bool HasUnstartedSoleIncomingEffect(long id) => _resolve().HasUnstartedSoleIncomingEffect(id);
    internal bool IsSoleIncomingExtraRound(long id) => _resolve().IsSoleIncomingExtraRound(id);
    internal void IssueSoleIncomingEffect(global::CardGame.Core.ProgramSkillFrame supplied, int cap) => _resolve().IssueSoleIncomingEffect(supplied, cap);
    internal bool SoleIncomingEffectFinalizedBeforeActivityTargetWindow(global::CardGame.Core.ProgramTargetCardWindowQuery query) => _resolve().SoleIncomingEffectFinalizedBeforeActivityTargetWindow(query);
    internal bool TryBeginSoleIncomingExtraRound(long id, bool afterAttack) => _resolve().TryBeginSoleIncomingExtraRound(id, afterAttack);
    internal bool TryGetSoleIncomingEffect(int owner, global::CardGame.Core.ProgramSkillWindowContext? context, scoped out global::CardGame.Core.CardUseFrame use, scoped out global::CardGame.Core.ProgramCardTriggerWindowFrame window) => _resolve().TryGetSoleIncomingEffect(owner, context, out use, out window);
    internal bool HasSoleIncomingEffectCapability { get => _resolve().HasSoleIncomingEffectCapability; }
}
