#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoCaoOlGodRuntime
{
    private IProgramSeatAcquisitionPolicy Policy => new OlGuixinPolicy(this);
    SkillProgramStepOutcome IOlGuixinHost.Resolve(ProgramSkillFrame f) => acquisitions.Begin(f, Policy);

    private sealed class OlGuixinPolicy(CharacterCaoCaoOlGodRuntime owner) : IProgramSeatAcquisitionPolicy
    {
        public ProgramSeatAcquisitionMode Mode => ProgramSeatAcquisitionMode.PrioritizedRegion;
        public string SkillId => "ol:guixin";
        public string Reason => "skill-program.ol:guixin.obtain";
        public void PublishPriority(ProgramSkillFrame frame) => owner.PublishPriority(frame);
        public bool ShouldTurnOver(ProgramSkillFrame f, ProgramSeatAcquisitionReceipt receipt, bool before) => true;
        public IGameEvent Started(ProgramSkillFrame f, ProgramSeatAcquisitionReceipt r) =>
            new OlGuixinStartedEvent(f.Id, r.Source, r.GameplayHash, r.DamageWindowId, r.DamageFrameId,
                r.OccurrenceIndex, r.DamageAmount, string.Join(",", r.OtherSeats));
        public IGameEvent Skipped(ProgramSkillFrame f, int cursor, int target) => new OlGuixinSeatSkippedEvent(f.Id, cursor, target);
        public IGameEvent Paid(ProgramSkillFrame f, ProgramSeatAcquisitionPayment p) =>
            new OlGuixinMovementPaidEvent(f.Id, p.Cursor, p.TargetSeat, p.From.Zone, p.BatchId!.Value, p.Before, p.After);
        public IGameEvent Turned(ProgramSkillFrame f, int obtained, bool before, bool after) => new OlGuixinTurnoverIssuedEvent(f.Id, before, after);
        public IGameEvent Completed(ProgramSkillFrame f, ProgramSeatAcquisitionReceipt r, bool cancelled) =>
            new OlGuixinCompletedEvent(f.Id, ProgramRandomSeatAcquisitionRuntime.MovedSeats(r), r.TurnoverIssued, cancelled);
    }

    internal static IReadOnlyList<PromptChoice> OlGuixinPriorityChoices(long id) => Array.AsReadOnly(
        new[] { CardZoneKind.Hand, CardZoneKind.Equipment, CardZoneKind.Judgment }.Select(zone => new PromptChoice(
            new("ol-guixin." + id + "." + zone), zone switch
            {
                CardZoneKind.Hand => "优先手牌区",
                CardZoneKind.Equipment => "优先装备区",
                _ => "优先判定区"
            }, [], [], new Dictionary<string, string>
            {
                ["program-action"] = "ol-guixin-priority", ["frame-id"] = id.ToString(CultureInfo.InvariantCulture),
                ["priority-zone"] = zone.ToString()
            })).ToArray());

    private void PublishPriority(ProgramSkillFrame f)
    {
        frames.ReplaceTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        var skill = registry._contentRegistry.GetSkill(f.SkillId);
        prompts.Set(new(DecisionKind.ProgramTrigger, f.OwnerSeat, "归心：统一选择本次获得牌的优先区域。", [], [], f.OwnerSeat)
        {
            PromptId = prompts.NextId(), Choices = OlGuixinPriorityChoices(f.Id),
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        }, participants._players[f.OwnerSeat].IsHuman);
        progression.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("ol-guixin-priority")]
    internal void AnswerOlGuixinPriority(PromptChoice chosen)
    {
        if (frames.Top is not ProgramSkillFrame { SeatAcquisition: { Mode: ProgramSeatAcquisitionMode.PrioritizedRegion,
                Stage: ProgramSeatAcquisitionStage.PriorityChoice } r } f ||
            prompts.Current?.PlayerSeat != f.OwnerSeat ||
            !OlGuixinPriorityChoices(f.Id).Any(c => c.Id == chosen.Id && c.Cards.SequenceEqual(chosen.Cards) &&
                c.Targets.SequenceEqual(chosen.Targets) && c.Parameters.OrderBy(p => p.Key).SequenceEqual(chosen.Parameters.OrderBy(p => p.Key))))
            throw new InvalidOperationException("OL Guixin needs the original owner's published priority-area choice.");
        var zone = Enum.Parse<CardZoneKind>(chosen.Parameters["priority-zone"]);
        prompts.Clear();
        frames.ReplaceTop(f with { Step = ResolutionFrameStep.ResolvingEffect,
            SeatAcquisition = r with { PreferredZone = zone, Stage = ProgramSeatAcquisitionStage.Visiting } });
        events.AdvanceFact(new OlGuixinPriorityChosenEvent(f.Id, zone));
        ResumeOlGuixin(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 40700)]
    internal bool ResumeOlGuixin(long id) => acquisitions.Resume(id, Policy);
    [ProgramContinuation(ProgramContinuationKind.Movement, 40700)]
    internal bool ReturnOlGuixinMovement(ProgramSkillFrame f) =>
        f.SeatAcquisition?.Mode == ProgramSeatAcquisitionMode.PrioritizedRegion && ResumeOlGuixin(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 40700)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 40700)]
    internal bool RetainOlGuixinPaid(ProgramSkillFrame f) => acquisitions.RetainPaid(f, Policy);
    [ProgramAiChoice(SkillProgramEffectOp.ResolveOlShenCaoCaoGuixin)]
    internal PromptChoice AiOlGuixin(PendingDecision p, ProgramSkillFrame f) => p.Choices.First();
}
