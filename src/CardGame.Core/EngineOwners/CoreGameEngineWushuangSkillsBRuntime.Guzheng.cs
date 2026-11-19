#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;

internal sealed partial class CoreGameEngineWushuangSkillsBRuntime
{
    private const string BGuzheng = "wushuang:skill-3139";
    private const string GuzhengReturn = "guzheng-return";
    private const string GuzhengReturned = "guzheng-returned";
    private const string GuzhengClaim = "guzheng-claim";
    private const string GuzhengDrain = "guzheng-drain";

    private IReadOnlyList<DiscardRecoveryEntity> WushuangGuzhengEntries(int phaseOwner) =>
        _discardPhases.Current() is { Kind: ActualDiscardRecoveryPhaseKind.Discard } phase && phase.ActorSeat == phaseOwner
            ? _discardPhases.CurrentPhaseDiscards(phase)
            : [];

    private SkillProgramStepOutcome BeginWushuangGuzheng(ProgramSkillFrame frame)
    {
        if (frame.WindowContext?.SourceSeat is not { } phaseOwner)
            return SkillProgramStepOutcome.Continue;
        var entries = WushuangGuzhengEntries(phaseOwner);
        if (entries.Count == 0)
            return SkillProgramStepOutcome.Continue;
        _guzhengFrames.ReplaceTop(frame = frame with
        {
            WushuangB = new(frame.InstructionIndex, SkillProgramEffectOp.ResolveWushuangGuzheng, GuzhengReturn, phaseOwner)
            {
                DiscardEntries = entries.OrderBy(entry => entry.CardId).ToArray()
            }
        });
        PublishWushuangBChoice(frame);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    private IReadOnlyList<PromptChoice> WushuangGuzhengChoices(ProgramSkillFrame frame, WushuangBReceipt receipt)
    {
        if (receipt.Stage == GuzhengClaim)
            return [BChoice(frame, receipt.Stage, "claim", "获得剩余弃置牌"), BChoice(frame, receipt.Stage, "pass", "不获得剩余弃置牌")];
        if (receipt.Stage != GuzhengReturn)
            return [];

        var recipient = _coreMatchParticipantsPlayersAccess._players[receipt.TargetSeat];
        return Array.AsReadOnly(receipt.DiscardEntries!.Where(_discardPhases.StillAvailable).Select(entry =>
            BChoice(frame, receipt.Stage, entry.CardId.ToString(CultureInfo.InvariantCulture),
                $"将【{_coreAdvancedLifecycleOperations.GetAdvancedCard(entry.CardId).DisplayName}】交还给{recipient.Name}", [entry.CardId])).ToArray());
    }

    private void AnswerWushuangGuzheng(ProgramSkillFrame frame, WushuangBReceipt receipt, PromptChoice selected)
    {
        if (receipt.Stage == GuzhengReturn)
        {
            if (!BSourceCurrent(frame) || !_coreMatchParticipantsPlayersAccess._players[receipt.TargetSeat].IsAlive)
            {
                FinishWushuangB(frame);
                return;
            }

            // Wushuang returns any genuine discard of the phase, regardless of the discarded card's source zone.
            var cardId = selected.Cards.Single();
            var recipient = CardLocation.Hand(receipt.TargetSeat);
            ProgramStageRuntime.Issue(_guzhengFrames, frame with
            {
                WushuangB = receipt with
                {
                    Stage = GuzhengDrain,
                    DiscardEntries = receipt.DiscardEntries!.Where(entry => entry.CardId != cardId).ToArray()
                },
                PendingMovementContinuation = new(frame.OwnerSeat, 0, null)
            }, _ => _guzhengMovement.Move(_coreAdvancedLifecycleOperations.GetAdvancedCard(cardId),
                CardLocation.DiscardPile, recipient, new("skill-program.wushuang-guzheng.return"), move =>
                {
                    if (move.To != recipient)
                        return;
                    var current = _guzhengFrames.ActiveProgram(frame.Id);
                    _guzhengFrames.ReplaceTop(current with { WushuangB = current.WushuangB! with { Stage = GuzhengReturned } });
                }));
        }
        else if (selected.Parameters["value"] == "claim" && CanClaimWushuangGuzheng(frame))
        {
            ProgramStageRuntime.Issue(_guzhengFrames, frame with
            {
                WushuangB = receipt with { Stage = GuzhengDrain },
                PendingMovementContinuation = new(frame.OwnerSeat, 0, null)
            }, _ =>
            {
                foreach (var entry in receipt.DiscardEntries!.Where(_discardPhases.StillAvailable))
                    _guzhengMovement.Move(_coreAdvancedLifecycleOperations.GetAdvancedCard(entry.CardId),
                        CardLocation.DiscardPile, CardLocation.Hand(frame.OwnerSeat), new("skill-program.wushuang-guzheng.claim"));
            });
        }
        else
        {
            FinishWushuangB(frame);
            return;
        }

        _guzhengFrames.AdvanceProgram(frame.Id);
    }

    private bool CanClaimWushuangGuzheng(ProgramSkillFrame frame) =>
        _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive;

    private void ContinueWushuangGuzheng(ProgramSkillFrame frame, WushuangBReceipt receipt)
    {
        if (receipt.Stage is GuzhengReturn or GuzhengClaim)
        {
            PublishWushuangBChoice(frame);
            return;
        }

        if (_guzhengChildren.Begin(frame.Id, PostEventContinuation.AwaitedProgramMovement))
            return;
        frame = _guzhengFrames.ActiveProgram(frame.Id);
        receipt = frame.WushuangB!;
        if (receipt.Stage == GuzhengReturned && CanClaimWushuangGuzheng(frame) && receipt.DiscardEntries!.Any(_discardPhases.StillAvailable))
        {
            _guzhengFrames.ReplaceTop(frame = frame with
            {
                WushuangB = receipt with { Stage = GuzhengClaim },
                PendingMovementContinuation = null
            });
            PublishWushuangBChoice(frame);
            return;
        }

        FinishWushuangB(frame);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 61202)]
    private bool ContinueWushuangGuzhengAfterLoss(ProgramSkillFrame frame) =>
        frame.WushuangB is { Operation: SkillProgramEffectOp.ResolveWushuangGuzheng, Stage: GuzhengReturned or GuzhengClaim or GuzhengDrain };

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 61202)]
    private bool DrainWushuangGuzhengAfterDeath(ProgramSkillFrame frame) =>
        frame.WushuangB is { Operation: SkillProgramEffectOp.ResolveWushuangGuzheng, Stage: GuzhengReturned or GuzhengDrain };
}
