#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDaQiaoLeRuntime
{
    internal void IssueZixiExtraEffect(ProgramSkillFrame f, CardUseFrame use, ProgramCardTriggerWindowFrame window)
    {
        var r = f.ZixiEffects!;
        if (r.JudgmentCount != 1 || window.Continuation == ProgramCardContinuation.Slash && !CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || window.Continuation == ProgramCardContinuation.FinalizedTrick && window.TrickContinuation is null || window.Continuation == ProgramCardContinuation.FinalizedSimpleCard && window.SimpleContinuation is null || use.EffectEpisodes?.Episodes.Any(episode => episode.Issue.Source.SkillId == f.SkillId && episode.Issue.Source.OwnerSeat == f.OwnerSeat) == true)
            throw new InvalidOperationException("Zixi cannot issue another effect without the original accepted native continuation.");
        // This accepted window has one designated character; Borrowed Sword's
        // following seat is the independent Slash request paired with slot zero.
        var scope = use.CardKind == CardKind.BorrowedSword ? NativeCardEffectEpisodeScope.ExecutionPair : NativeCardEffectEpisodeScope.ExecutionOccurrence;
        _nativeCardEffectEpisodes.Issue(use.Id, new(f.Id, f.InstructionIndex - 1, 0, r.Source, r.GameplayHash,
            new(20, scope, TargetSeat: r.TargetSeat, TargetIndex: 0, ContinueAfterActorDeath: true)),
            window.TrickContinuation, window.SimpleContinuation);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZixiExtraEffectIssuedEvent(f.Id, r.Source, r.GameplayHash, r.UseId, r.ActionId, r.TargetSeat, 1));
    }
}
