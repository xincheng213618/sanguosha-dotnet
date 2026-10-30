using System.Globalization;
namespace CardGame.Core;

public sealed partial class GameEngine
{
    [ProgramCapability] private sealed class OlGuixinCapability(GameEngine e) : IOlGuixinHost
    {
        public IProgramSeatAcquisitionPolicy Policy { get; } = new OlGuixinPolicy(e);
        public SkillProgramStepOutcome Resolve(ProgramSkillFrame f) => e.SeatAcquisitions.Begin(f, Policy);
    }
    private sealed class OlGuixinPolicy(GameEngine e) : IProgramSeatAcquisitionPolicy
    {
        public ProgramSeatAcquisitionMode Mode => ProgramSeatAcquisitionMode.PrioritizedRegion;
        public string SkillId => "ol:guixin";
        public SkillProgramEffectOp Operation => SkillProgramEffectOp.ResolveOlShenCaoCaoGuixin;
        public string Reason => "skill-program.ol:guixin.obtain";
        public string InvalidReceiptMessage => "OL Guixin lost its original damage point, immutable priority, private RNG choice or actual paid movement.";
        public void PublishPriority(ProgramSkillFrame f) => e.PublishOlGuixinPriority(f);
        public bool ShouldTurnOver(ProgramSkillFrame f, ProgramSeatAcquisitionReceipt r, bool before) => true;
        public IGameEvent Started(ProgramSkillFrame f, ProgramSeatAcquisitionReceipt r) => new OlGuixinStartedEvent(f.Id, r.Source, r.GameplayHash, r.DamageWindowId, r.DamageFrameId, r.OccurrenceIndex, r.DamageAmount, string.Join(",", r.OtherSeats));
        public IGameEvent Skipped(ProgramSkillFrame f, int cursor, int target) => new OlGuixinSeatSkippedEvent(f.Id, cursor, target);
        public IGameEvent Paid(ProgramSkillFrame f, ProgramSeatAcquisitionPayment p) => new OlGuixinMovementPaidEvent(f.Id, p.Cursor, p.TargetSeat, p.From.Zone, p.BatchId, p.Before, p.After);
        public IGameEvent Turned(ProgramSkillFrame f, int obtained, bool before, bool after) => new OlGuixinTurnoverIssuedEvent(f.Id, before, after);
        public IGameEvent Completed(ProgramSkillFrame f, ProgramSeatAcquisitionReceipt r, bool cancelled) => new OlGuixinCompletedEvent(f.Id, r.Payments.Count, r.TurnoverIssued, cancelled);
        public bool ValidStarted(ProgramSkillFrame f, ProgramSeatAcquisitionReceipt r) => e.ProgramEventHistory<OlGuixinStartedEvent>().Where(x => x.FrameId == f.Id).ToArray() is [var fact] && Equals(fact, Started(f, r));
        public bool ValidPriority(ProgramSkillFrame f, ProgramSeatAcquisitionReceipt r)
        {
            var choices = e.ProgramEventHistory<OlGuixinPriorityChosenEvent>().Where(x => x.FrameId == f.Id).ToArray();
            return r.PreferredZone is { } preferred ? choices is [var choice] && choice.PreferredZone == preferred : choices.Length == 0;
        }
        public bool ValidSkipped(ProgramSkillFrame f, int cursor, int target) => e.ProgramEventHistory<OlGuixinSeatSkippedEvent>().Count(x => Equals(x, Skipped(f, cursor, target))) == 1;
        public bool HasSkipped(ProgramSkillFrame f, int cursor) => e.ProgramEventHistory<OlGuixinSeatSkippedEvent>().Any(x => x.FrameId == f.Id && x.Cursor == cursor);
        public bool ValidPaid(ProgramSkillFrame f, ProgramSeatAcquisitionPayment p) => e.ProgramEventHistory<OlGuixinMovementPaidEvent>().Where(x => x.FrameId == f.Id && x.Cursor == p.Cursor).ToArray() is [var fact] && Equals(fact, Paid(f, p));
        public bool ValidTurned(ProgramSkillFrame f, ProgramSeatAcquisitionReceipt r) => e.ProgramEventHistory<OlGuixinTurnoverIssuedEvent>().Where(x => x.FrameId == f.Id).ToArray() is [var fact] && Equals(fact, Turned(f, 0, r.FaceDownBefore!.Value, r.FaceDownAfter!.Value));
        public bool NoTurnoverEvent(ProgramSkillFrame f) => true;
    }
    private static IReadOnlyList<PromptChoice> OlGuixinPriorityChoices(long id) => Array.AsReadOnly(MobileGuixinZones.Select(zone =>
        new PromptChoice(new("ol-guixin." + id + "." + zone), zone switch
        { CardZoneKind.Hand => "优先手牌区", CardZoneKind.Equipment => "优先装备区", _ => "优先判定区" }, [], [],
            new Dictionary<string, string> { ["program-action"] = "ol-guixin-priority", ["frame-id"] = id.ToString(CultureInfo.InvariantCulture), ["priority-zone"] = zone.ToString() })).ToArray());
    private void PublishOlGuixinPriority(ProgramSkillFrame f)
    {
        ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        var skill = _contentRegistry.GetSkill(f.SkillId);
        _pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "归心：统一选择本次获得牌的优先区域。", [], [], f.OwnerSeat)
        { PromptId = CreatePromptId(), Choices = OlGuixinPriorityChoices(f.Id), SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description) };
        _status = _players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        AdvanceRulesAndPublishState();
    }
    [ProgramAnswer("ol-guixin-priority")] private void AnswerOlGuixinPriority(PromptChoice chosen)
    {
        var f = _resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("OL Guixin lost its original frame.");
        RequireOlGuixin(f); var r = f.SeatAcquisition!;
        if (r.Stage != ProgramSeatAcquisitionStage.PriorityChoice || !OlGuixinPriorityChoices(f.Id).Any(c => c.Id == chosen.Id && c.Cards.SequenceEqual(chosen.Cards) &&
            c.Targets.SequenceEqual(chosen.Targets) && c.Parameters.OrderBy(p => p.Key).SequenceEqual(chosen.Parameters.OrderBy(p => p.Key))))
            throw new InvalidOperationException("OL Guixin needs one published priority-area choice for this actual damage point.");
        var zone = Enum.Parse<CardZoneKind>(chosen.Parameters["priority-zone"]);
        ClearPendingDecision(); ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.ResolvingEffect, SeatAcquisition = r with { PreferredZone = zone, Stage = ProgramSeatAcquisitionStage.Visiting } });
        AdvanceEventRulesAndQueueFact(new OlGuixinPriorityChosenEvent(f.Id, zone)); ResumeOlGuixin(f.Id);
    }
    [ProgramContinuation(ProgramContinuationKind.Resume, 40700)] private bool ResumeOlGuixin(long id) => SeatAcquisitions.Resume(id, ProgramCapabilities.Get<IOlGuixinHost>().Policy);
    [ProgramContinuation(ProgramContinuationKind.Movement, 40700)] private bool ReturnOlGuixinMovement(ProgramSkillFrame f) =>
        f.SeatAcquisition?.Mode == ProgramSeatAcquisitionMode.PrioritizedRegion && ResumeOlGuixin(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 40700)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 40700)] private bool RetainOlGuixinPaid(ProgramSkillFrame f) => SeatAcquisitions.RetainPaid(f, ProgramCapabilities.Get<IOlGuixinHost>().Policy);
    [ProgramAiChoice(SkillProgramEffectOp.ResolveOlShenCaoCaoGuixin)] private PromptChoice AiOlGuixin(PendingDecision p, ProgramSkillFrame f) => p.Choices.First();
}
