#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSoleIncomingEffectRepeatEntries
{
    internal bool CanOfferSoleIncomingEffect(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferSoleIncomingEffect(candidate, trigger, context);
    internal void CaptureSoleIncomingAttack(NativeOwners.CardAttackHandle attack) => _resolve().CaptureSoleIncomingAttack(attack);
    internal void CompleteSoleIncomingExtraRound(long id) => _resolve().CompleteSoleIncomingExtraRound(id);
    internal bool ContinueSoleIncomingFangtian(NativeOwners.CardAttackHandle attack) => _resolve().ContinueSoleIncomingFangtian(attack);
    internal bool HasUnstartedSoleIncomingEffect(long id) => _resolve().HasUnstartedSoleIncomingEffect(id);
    internal bool IsSoleIncomingExtraRound(long id) => _resolve().IsSoleIncomingExtraRound(id);
    internal void IssueSoleIncomingEffect(ProgramSkillFrame supplied, int cap) => _resolve().IssueSoleIncomingEffect(supplied, cap);
    internal bool SoleIncomingEffectFinalizedBeforeActivityTargetWindow(ProgramTargetCardWindowQuery query) => _resolve().SoleIncomingEffectFinalizedBeforeActivityTargetWindow(query);
    internal bool TryBeginSoleIncomingExtraRound(long id, bool afterAttack) => _resolve().TryBeginSoleIncomingExtraRound(id, afterAttack);
    internal bool TryGetSoleIncomingEffect(int owner, ProgramSkillWindowContext? context, scoped out CardUseFrame use, scoped out ProgramCardTriggerWindowFrame window) => _resolve().TryGetSoleIncomingEffect(owner, context, out use, out window);
    internal bool HasSoleIncomingEffectCapability { get => _resolve().HasSoleIncomingEffectCapability; }
}
