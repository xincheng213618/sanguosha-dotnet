namespace CardGame.Core;

internal interface ISlashRedirectTargetQueries : IProgramSelectionQuery
{
    bool IsEligible(int redirectorSeat, int targetSeat, ProgramSkillWindowContext? context = null,
        int? excludedEquipmentId = null);
}

// Native target identity and range are independent of how a program pays its cost.
[ProgramRuntimeModule, ProgramCapability]
internal sealed class SlashRedirectTargetRuntime(IMatchParticipantStateAccess participants,
    IProgramRuntimeFrames frames, IAttackRangeQueries ranges,
    RuntimeEntries.CoreActiveNativeAttackQueriesEntries attacks,
    RuntimeEntries.CoreNativeCardContinuationCursorsEntries cursors,
    RuntimeEntries.CoreDirectedTurnCardPoliciesEntries targetPolicies,
    RuntimeEntries.CoreNativeDamagePoliciesEntries damagePolicies) : ISlashRedirectTargetQueries
{
    public bool IsEligible(int redirectorSeat, int targetSeat, ProgramSkillWindowContext? context = null,
        int? excludedEquipmentId = null)
    {
        if ((uint)redirectorSeat >= (uint)participants.Players.Count ||
            (uint)targetSeat >= (uint)participants.Players.Count ||
            attacks.ActiveCardAttack is not { } attack || attack.TargetSeat != redirectorSeat ||
            attack.SourceSeat == redirectorSeat ||
            targetSeat == redirectorSeat || targetSeat == attack.SourceSeat ||
            !participants.At(redirectorSeat).IsAlive || !participants.At(targetSeat).IsAlive)
            return false;
        if (context is not null && (context.Window != SkillProgramTriggerWindow.SlashTargetRedirecting ||
            context.CardUse?.ParentCardUseFrameId != attack.ResolutionId ||
            context.TargetSeat != redirectorSeat)) return false;
        if (cursors.ActiveFangtianHalberd is not null &&
            frames.OfType<CardUseFrame>().Single(frame => frame.Id == attack.ResolutionId)
                .TargetSeats.Contains(targetSeat)) return false;
        return !targetPolicies.IsDirectedCardTargetProhibited(attack.SourceSeat, targetSeat,
                attack.EffectiveCardKind ?? CardKind.Slash, attack.ResolutionId) &&
            !damagePolicies.IsSlashProhibited(participants.At(targetSeat)) &&
            ranges.IsWithinAttackRange(redirectorSeat, targetSeat, excludedEquipmentId);
    }
}
