#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterJiangJiMouRuntime
{
    internal void IssueMouJjRepeat(ProgramSkillFrame f)
    {
        if (!MouJiangJi.Valid(f) || f.MouJiangJi is not { Stage: MouJiangJiStage.YingshiChoice, Target: >= 0 } r ||
            _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == r.WindowId) is not ProgramCardTriggerWindowFrame window ||
            _coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId) is not { Action: { Type: CardActionType.Use } action } use ||
            window.ParentFrameId != use.Id || window.Continuation != ProgramCardContinuation.FinalizedTrick ||
            window.TrickContinuation is null || action.ActionId != r.ActionId || !r.Targets.Contains(r.Target) ||
            _effectEpisodes.Ordinal(use.Id) != 0 ||
            use.EffectEpisodes?.Episodes.Any(e => e.Issue.Source.SkillId == f.SkillId && e.Issue.Source.OwnerSeat == f.OwnerSeat) == true)
            throw new InvalidOperationException("Yingshi requires its exact accepted ordinary-Trick target and a once-issued native extra effect.");
        _effectEpisodes.Issue(use.Id, new(f.Id, f.InstructionIndex - 1, 0, r.Source, r.GameplayHash,
            new(70, NativeCardEffectEpisodeScope.FrozenTarget, TargetSeat: r.Target)), window.TrickContinuation);
    }
}
