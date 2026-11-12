#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuangYueyingOlMouRuntime
{
    internal void IssueMouBingcaiExtraEffect(ProgramSkillFrame frame)
    {
        if (frame.MouBingcai is not { Paid: true, MatchesDamage: true } r || r.Stage != MouBingcaiStage.DrawChildren || !IsPaidMouBingcai(frame) || frame.WindowContext is not { } context || !TryGetMouBingcaiFirstBasic(context, out var use, out var window) || use.Id != r.UseId || window.Id != r.WindowId || use.Action!.ActionId != r.ActionId || r.MaterialId is not { } material || r.PrintedKind is not { } printed || r.EffectiveKind is not { } effective || r.From is not { } from || use.EffectEpisodes?.Episodes.Any(episode => episode.Issue.Source.SkillId == frame.SkillId && episode.Started > 0) == true)
            throw new InvalidOperationException("Bingcai requires its exact paid recast and original first-basic native Use.");
        var old = use.EffectEpisodes?.Episodes.Where(episode => episode.Issue.Source.SkillId == frame.SkillId).ToArray() ?? [];
        if (old.Any(episode => episode.Issue.IssuerFrameId == frame.Id || episode.Issue.Source.OwnerSeat == frame.OwnerSeat))
            throw new InvalidOperationException("One owner may issue only one Bingcai extra effect for this real Use.");
        ProgramSimpleCardContinuation? simple = window.SimpleContinuation;
        if (!CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) && simple is null)
        {
            if (use.CardKind == CardKind.Peach || use.CardKind == CardKind.Alcohol && (use.DyingResponse is not null || _coreActualTurnLegacyCompletionOperations.IsExactLegacyActualUseCompletion(use)))
                simple = new(use.CardId, SimpleCardUseEffect.Recovery);
            else if (use.CardKind == CardKind.Alcohol)
                simple = new(use.CardId, SimpleCardUseEffect.Alcohol);
            else
                throw new InvalidOperationException("Bingcai's basic effect lost its actual native continuation.");
        }

        // The committed window precedes the original recovery's typed return.
        // Declare its accepted producer now; execution waits for that real return.
        var replay = use is { CardId: 0, CardKind: CardKind.Alcohol, PhysicalCardIds.Count: 0 } &&
            simple is { Effect: SimpleCardUseEffect.Recovery } && _nativeCardUseAdmission.IsLegacyProducerUse(use)
            ? NativeCardEffectEpisodeReplay.BoundSelfRescueRecovery : NativeCardEffectEpisodeReplay.Original;
        _nativeCardEffectEpisodes.Issue(use.Id, new(frame.Id, frame.InstructionIndex - 1, 0, r.Source, r.GameplayHash,
            new(40, Replay: replay)), simple: simple);
        var grant = new MouBingcaiExtraEffectGrant(frame.Id, r.Source, r.GameplayHash, r.WindowId, r.BasicKind, r.RecastInvocationId, material, printed, effective, from, r.BatchId, r.Before, r.After, r.MovementSequence);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(MouBingcaiIssuedEvent(use.Id, r.ActionId, grant));
    }

    internal static MouBingcaiExtraEffectIssuedEvent MouBingcaiIssuedEvent(long useId, long actionId, MouBingcaiExtraEffectGrant g) => RuntimeSkills.RuntimeRules_huang_yueying_ol_mou_MouHuangYueyingBingcai_Rounds.MouBingcaiIssuedEvent(useId, actionId, g);
}
