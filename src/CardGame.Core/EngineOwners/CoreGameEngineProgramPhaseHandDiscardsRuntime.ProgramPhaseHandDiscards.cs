using System.Text.Json.Serialization;

namespace CardGame.Core
{
    public sealed partial record ProgramSkillFrame
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public PhaseHandDiscardSelection? PhaseHandDiscard { get; init; }
    }

    public enum PhaseHandDiscardStage { SelectingReturn, ReturnPaid, SelectingClaim, Finishing }

    public sealed record PhaseHandDiscardSelection(int ChooserSeat, ActualDiscardRecoveryPhaseKey Phase,
        IReadOnlyList<DiscardRecoveryEntity> Entries, PhaseHandDiscardStage Stage = PhaseHandDiscardStage.SelectingReturn,
        int? ReturnedCardId = null)
    {
        public IReadOnlyList<DiscardRecoveryEntity> Entries { get; } = Array.AsReadOnly(Entries.ToArray());
    }
}

namespace CardGame.Core.EngineOwners
{
    using Stage = ProgramStageNode<CoreGameEngineProgramPhaseHandDiscardsRuntime, PhaseHandDiscardSelection, PhaseHandDiscardStage>;

    [ProgramRuntimeModule]
    internal sealed class CoreGameEngineProgramPhaseHandDiscardsRuntime(
        IProgramRuntimeFrames frames, IProgramRuntimePrompts prompts, IProgramCompletionFacts facts,
        IProgramSkillCompletion completion, IProgramChildWindows children, IActualDiscardRecoveryPhaseQueries phases,
        IMatchParticipantStateAccess participants, IKernelContentRegistryAccess content, IKernelAdvanceRulesAndPublishStateOperations rules,
        RuntimeEntries.CoreGameEngineProgramCardPaymentsEntries payments,
        RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries cards, INativeLogicalDiscardMovement movement)
    {
        private const string Return = "restore-phase-hand-discard";
        private const string Skip = "restore-phase-hand-decline";
        private const string Claim = "restore-phase-hand-claim";
        private const string Leave = "restore-phase-hand-leave";

        [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 66601, SkillProgramEffectOp.RestorePhaseHandDiscards)]
        internal bool CanRestore(ProgramTriggerLegalityContext context) =>
            context.WindowContext is { Window: SkillProgramTriggerWindow.DiscardPhaseEnded, SourceSeat: { } source } &&
            CurrentDiscardPhase(source) is { } phase &&
            phases.CurrentPhaseDiscards(phase).Any(entry => entry.OriginalFrom == CardLocation.Hand(source));

        internal SkillProgramStepOutcome RestoreProgramPhaseHandDiscards(ProgramSkillFrame frame,
            ProgramParticipantReference chooser, ProgramParticipantReference phaseOwner)
        {
            var chooserSeat = payments.ResolveProgramParticipant(frame, chooser);
            var phaseOwnerSeat = payments.ResolveProgramParticipant(frame, phaseOwner);
            if (CurrentDiscardPhase(phaseOwnerSeat) is not { } phase || !facts.Alive(chooserSeat) || !facts.Alive(phaseOwnerSeat))
                return SkillProgramStepOutcome.Continue;

            var selection = new PhaseHandDiscardSelection(chooserSeat, phase, phases.CurrentPhaseDiscards(phase));
            if (!selection.Entries.Any(e => e.OriginalFrom == CardLocation.Hand(phaseOwnerSeat)))
                return SkillProgramStepOutcome.Continue;
            frames.ReplaceTop(frame = frame with { PhaseHandDiscard = selection });
            Publish(frame, selection);
            return SkillProgramStepOutcome.AwaitChoice;
        }

        private ActualDiscardRecoveryPhaseKey? CurrentDiscardPhase(int seat) =>
            phases.Current() is { Kind: ActualDiscardRecoveryPhaseKind.Discard } phase && phase.ActorSeat == seat ? phase : null;

        private IEnumerable<DiscardRecoveryEntity> Remaining(PhaseHandDiscardSelection selection) =>
            selection.Entries.Where(e => e.CardId != selection.ReturnedCardId && phases.StillAvailable(e));

        private void Publish(ProgramSkillFrame frame, PhaseHandDiscardSelection selection)
        {
            var returning = selection.Stage == PhaseHandDiscardStage.SelectingReturn;
            var actor = participants.At(selection.Phase.ActorSeat);
            var skill = content._contentRegistry.GetSkill(frame.SkillId);
            var prefix = $"restore-phase-hand.frame-{frame.Id}";
            PromptChoice Choice(string suffix, string action, string label, int? id = null) => new(
                new($"{prefix}.{suffix}"), label, id is { } cardId ? [cardId] : [], [],
                new Dictionary<string, string> { ["program-action"] = action });
            PromptChoice[] choices = returning
                ? [.. selection.Entries.Where(e => e.OriginalFrom == CardLocation.Hand(actor.Seat)).OrderBy(e => e.CardId)
                    .Select(e => Choice($"card-{e.CardId}", Return, $"将【{cards.GetAdvancedCard(e.CardId).DisplayName}】交还给 {actor.Name}", e.CardId)),
                    Choice("decline", Skip, "不交还")]
                : [Choice("claim", Claim, "获得本阶段其余仍在弃牌堆的弃牌"), Choice("leave", Leave, "放弃获得")];
            var title = returning ? "选择交还的牌" : "是否获得其余弃牌";
            prompts.Set(new(DecisionKind.ProgramTrigger, selection.ChooserSeat, $"【{skill.Name}】{title}。",
                choices.SelectMany(choice => choice.Cards).ToArray(), [], frame.OwnerSeat)
            {
                PromptId = prompts.NextId(),
                SkillPrompt = new(frame.SkillId, skill.Name, $"{skill.Name} · {title}", skill.Description),
                Choices = Array.AsReadOnly(choices)
            }, participants.At(selection.ChooserSeat).IsHuman);
        }

        [ProgramAnswer(Return, Skip, Claim, Leave)]
        internal void Answer(PromptChoice choice, string action)
        {
            if (frames.Top is not ProgramSkillFrame { PhaseHandDiscard: { } selection } frame ||
                prompts.Current?.PlayerSeat != selection.ChooserSeat)
                throw new InvalidOperationException("The discard phase choice lost its owning frame or chooser.");

            prompts.Clear();
            if (selection.Stage == PhaseHandDiscardStage.SelectingReturn)
            {
                if (!facts.Alive(frame.OwnerSeat) || !facts.Alive(selection.ChooserSeat) ||
                    !facts.Alive(selection.Phase.ActorSeat) || !facts.CanInvokeProgramSkill(frame))
                {
                    completion.Finish(frame, false);
                    return;
                }

                if (action == Skip)
                {
                    AdvanceInstruction(frame);
                    return;
                }

                if (action != Return || choice.Cards is not [var cardId] ||
                    !selection.Entries.Any(e => e.CardId == cardId && e.OriginalFrom == CardLocation.Hand(selection.Phase.ActorSeat) && phases.StillAvailable(e)))
                    throw new InvalidOperationException("The selected hand discard is no longer available to return.");

                var recipient = CardLocation.Hand(selection.Phase.ActorSeat);
                ProgramStageRuntime.Issue(frames, frame with
                {
                    PhaseHandDiscard = selection with { Stage = PhaseHandDiscardStage.Finishing },
                    PendingMovementContinuation = new(frame.OwnerSeat, 0, null)
                }, _ => movement.Move(cards.GetAdvancedCard(cardId), CardLocation.DiscardPile, recipient, Reason(frame), move =>
                {
                    if (move.To != recipient) return;
                    var current = frames.ActiveProgram(frame.Id);
                    frames.ReplaceTop(current with { PhaseHandDiscard = selection with { Stage = PhaseHandDiscardStage.ReturnPaid, ReturnedCardId = cardId } });
                }));
            }
            else if (selection.Stage == PhaseHandDiscardStage.SelectingClaim && action is Claim or Leave)
            {
                if (action == Leave || !CanReceiveRemaining(frame, selection))
                {
                    AdvanceInstruction(frame);
                    return;
                }

                var remaining = Remaining(selection).Select(e => cards.GetAdvancedCard(e.CardId)).ToArray();
                ProgramStageRuntime.Issue(frames, frame with
                {
                    PhaseHandDiscard = selection with { Stage = PhaseHandDiscardStage.Finishing },
                    PendingMovementContinuation = new(frame.OwnerSeat, 0, null)
                }, _ => movement.MoveCards(remaining, CardLocation.DiscardPile, CardLocation.Hand(selection.ChooserSeat), Reason(frame)));
            }
            else
                throw new InvalidOperationException("The discard phase answer does not match the current choice.");

            frames.AdvanceProgram(frame.Id);
        }

        private static CardMoveReason Reason(ProgramSkillFrame frame) =>
            new($"skill-program.{frame.SkillId}.{SkillProgramEffectOp.RestorePhaseHandDiscards}");

        private bool CanReceiveRemaining(ProgramSkillFrame frame, PhaseHandDiscardSelection selection) =>
            facts.Winner == Winner.None && facts.Alive(frame.OwnerSeat) && facts.Alive(selection.ChooserSeat);

        private void AdvanceInstruction(ProgramSkillFrame frame)
        {
            frames.ReplaceTop(frame with { PhaseHandDiscard = null });
            frames.AdvanceProgram(frame.Id);
        }

        private static class Stages
        {
            internal static readonly ProgramStagePlan<CoreGameEngineProgramPhaseHandDiscardsRuntime, PhaseHandDiscardSelection, PhaseHandDiscardStage> Plan = new(
                frame => frame.PhaseHandDiscard, selection => selection.Stage,
                [
                    Stage.AwaitExternalReturn(PhaseHandDiscardStage.SelectingReturn),
                    Stage.ChildrenTo(PhaseHandDiscardStage.ReturnPaid, PhaseHandDiscardStage.SelectingClaim, clearMovement: true,
                        completeWhen: (runtime, frame, selection) => !runtime.CanReceiveRemaining(frame, selection) || !runtime.Remaining(selection).Any()),
                    Stage.Prompt(PhaseHandDiscardStage.SelectingClaim, (runtime, frame, selection) => runtime.PublishClaim(frame, selection)),
                    Stage.ChildrenComplete(PhaseHandDiscardStage.Finishing, clearMovement: true)
                ],
                withStage: (frame, selection, stage) => frame with { PhaseHandDiscard = selection with { Stage = stage } },
                drain: (runtime, frame, _) => runtime.DrainChildren(frame.Id),
                complete: (runtime, frame) => runtime.AdvanceInstruction(frame));
        }

        private void PublishClaim(ProgramSkillFrame frame, PhaseHandDiscardSelection selection)
        {
            Publish(frame, selection);
            rules.AdvanceRulesAndPublishState();
        }

        private bool DrainChildren(long id) => children.Begin(id, PostEventContinuation.AwaitedProgramMovement);

        [ProgramContinuation(ProgramContinuationKind.Resume, 66601)]
        internal bool Resume(long id)
        {
            if (frames.Top is not ProgramSkillFrame { PhaseHandDiscard: not null } frame || frame.Id != id)
                return false;
            ProgramStageRuntime.Run(id, frames, prompts, this, Stages.Plan);
            return true;
        }

        [ProgramContinuation(ProgramContinuationKind.Movement, 66601)]
        internal bool ReturnMovement(ProgramSkillFrame frame) => frame.PhaseHandDiscard is not null && Resume(frame.Id);

        [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 66601)]
        internal bool ContinueAfterLoss(ProgramSkillFrame frame) => frame.PhaseHandDiscard?.Stage is
            PhaseHandDiscardStage.ReturnPaid or PhaseHandDiscardStage.SelectingClaim or PhaseHandDiscardStage.Finishing;

        [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 66601)]
        internal bool DrainAfterDeath(ProgramSkillFrame frame) => frame.PhaseHandDiscard?.Stage is
            PhaseHandDiscardStage.ReturnPaid or PhaseHandDiscardStage.Finishing;

        [ProgramAiChoice(SkillProgramEffectOp.RestorePhaseHandDiscards)]
        internal PromptChoice SelectAi(PendingDecision decision, ProgramSkillFrame frame)
        {
            var selection = frame.PhaseHandDiscard!;
            if (selection.Stage == PhaseHandDiscardStage.SelectingClaim) return decision.Choices[0];
            if (selection.Entries.Count <= 1) return decision.Choices.Last();
            var owner = participants.At(selection.ChooserSeat);
            return decision.Choices.Where(choice => choice.Cards.Count == 1)
                .OrderBy(choice => CoreNativeAiDecisionsRuntime.GetKeepValue(cards.GetAdvancedCard(choice.Cards[0]), owner))
                .ThenBy(choice => choice.Id.Value, StringComparer.Ordinal).First();
        }
    }
}
