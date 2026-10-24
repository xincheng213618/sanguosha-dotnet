namespace CardGame.Core;

public sealed partial class GameEngine
{
    private IWenheBattleRulesFeature? WenheRules => _modeRuntime.Find<IWenheBattleRulesFeature>();
    private IGuanduBattleRulesFeature? GuanduRules => _modeRuntime.Find<IGuanduBattleRulesFeature>();
    private IDragonBoatBattleRulesFeature? DragonBoatRules => _modeRuntime.Find<IDragonBoatBattleRulesFeature>();
    private IModeBattleRulesHost CreateActivityBattleRulesHost() => new ActivityBattleRulesHost(this);
    private IWenheRuleFlowHost CreateWenheRuleFlowHost() => new WenheRuleFlowHost(this);
    private IGuanduEventFlowHost CreateGuanduEventFlowHost() => new GuanduEventFlowHost(this);

    private bool DrainActivityRuleChildren(long frameId) => TryBeginQueuedRecoveryReplacement(frameId, PostEventContinuation.Program) ||
        TryBeginCharacterStateProgramWindow(frameId, CharacterStateContinuation.Program) || TryBeginHpChangedProgramWindow(frameId, PostEventContinuation.Program) ||
        TryBeginCardsMovedProgramWindow(frameId) || TryBeginAdvancedSkillsChanged(frameId);

    private sealed class ActivityBattleRulesHost(GameEngine engine) : IModeBattleRulesHost
    {
        public ModeBattleRulesFacts CaptureFacts() => new(engine._setupComplete, engine._gameStartingProgramsResolved,
            engine._winner != Winner.None, engine._options.HumanSeat, engine._currentSeat, engine._turnNumber,
            engine._roundNumber, engine._phase, engine._resolutionStack.Count, engine._pendingDecision?.Kind,
            engine._pendingCardsMovedBatches.Count, engine._pendingHpChanges.Count, engine._cardZones.Count(CardLocation.DrawPile),
            engine._resolutionStack.OfType<DrawPhaseObligationFrame>().LastOrDefault()?.IsExtra == true,
            engine.CurrentActualDiscardRecoveryPhase(), engine._players.Select(player => new ModeBattlePlayerFacts(player.Seat,
                player.Name, player.IsAlive, player.Hp, player.MaxHp, player.TeamId, engine.GetHand(player).Count,
                engine.GetEquipment(player).Count)).ToArray());
        public IReadOnlyList<TEvent> History<TEvent>() where TEvent : IGameEvent =>
            Array.AsReadOnly(engine.CompleteProgramEventHistory().OfType<TEvent>().ToArray());
        public GuanduBattleConfiguredEvent? InitialGuanduConfiguration => engine._guanduInitialConfiguration;
        private static ModeBattleNativeUse? UseFacts(CardUseFrame? use) => use?.Action is { } action ?
            new(use.Id, new(action.ActionId, action.Type, action.ActorSeat, action.EffectiveKind), use.ExtraCardEffect is not null) : null;
        public ModeBattleNativeUse? FindUse(long frameId) => UseFacts(engine.LifecycleCardUse(frameId));
        public ModeBattleNativeUse? FindUseByAction(long actionId) =>
            UseFacts(engine._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(frame => frame.Action?.ActionId == actionId));
        public ModeBattleDraftFacts Draft(int seat)
        {
            var player = engine._players[seat];
            return new(seat, player.GeneralSelected, player.General.Id, player.GeneralDraftCandidates?.Select(general => general.Id).ToArray() ?? []);
        }
        public bool HasActivitySkill(int seat, string skillId) => engine.HasActivitySkill(engine._players[seat], skillId);
        public bool HasIndependentSkillGrant(int seat, string skillId) =>
            engine.EnabledRuntimeSkillGrants(engine._players[seat]).Any(grant => grant.SkillId == skillId && !grant.IsModeRule);
        public bool IsSlash(CardKind kind) => IsSlashCard(kind);
        public string FactionName(string factionId) => GetFactionName(factionId);
        public IEnumerable<RuleQueryContribution> NativeGeneralContributions(int seat, SkillRuleQuery query) =>
            engine.GuanduGeneralRuleContributions(engine._players[seat], query);
        public int NextRandom(int exclusiveMaximum) => engine._random.Next(exclusiveMaximum);
        public void PublishFact(IGameEvent fact) => engine.AdvanceEventRulesAndQueueFact(fact);
        public void PublishState() => engine.AdvanceRulesAndPublishState();
        public void ClearDecision() => engine.ClearPendingDecision();
        public void SetWinner(Winner winner, string? teamId = null, int? seat = null)
        { engine._winner = winner; engine._winnerTeamId = teamId; engine._winnerSeat = seat; }
        public Winner WinnerForTeam(string teamId) => engine.GetWinnerForTeam(teamId);
        public void CompleteGame() => engine.CompleteGame();
        public void TransferAllHandCards(int fromSeat, int toSeat, string reason) => engine.MoveCards(
            engine.GetHand(engine._players[fromSeat]).ToArray(), CardLocation.Hand(fromSeat), CardLocation.Hand(toSeat), new(reason));
        public void DiscardAllHandAndEquipment(int seat, string reason)
        {
            engine.MoveLogicalDiscardCards(seat, seat, engine.GetHand(engine._players[seat]).ToArray(), CardLocation.Hand(seat), new(reason));
            engine.MoveLogicalDiscardCards(seat, seat, engine.GetEquipment(engine._players[seat]).ToArray(), CardLocation.Equipment(seat), new(reason));
        }
        public void Log(string kind, string message, int sourceSeat, int? targetSeat = null)
        {
            if (targetSeat is { } target) engine.AddLog(kind, message, sourceSeat, target);
            else engine.AddLog(kind, message, sourceSeat);
        }
    }

    private sealed class WenheRuleFlowHost(GameEngine engine) : IWenheRuleFlowHost
    {
        private static ModeWenheFlowCursor Cursor(ProgramSkillFrame frame) => new(frame.Id, frame.OwnerSeat,
            frame.WenheRuleFlow ?? throw new InvalidOperationException("The activity flow lost its owning receipt."));
        public ModeWenheFlowCursor? BeginContinuation(long frameId)
        {
            if (engine._resolutionStack.LastOrDefault() is not ProgramSkillFrame { WenheRuleFlow: not null } frame || frame.Id != frameId) return null;
            engine.AssertWenheRuleFlow(frame);
            return Cursor(frame);
        }
        public ModeWenheFlowCursor Current(long frameId) => Cursor(engine.GetActiveProgramFrame(frameId));
        public void AdvanceCursor(long frameId, int expectedIndex, int expectedStage, int nextIndex, int nextStage)
        {
            var frame = engine.GetActiveProgramFrame(frameId);
            var receipt = frame.WenheRuleFlow ?? throw new InvalidOperationException("The activity flow lost its owning receipt.");
            if (receipt.Index != expectedIndex || receipt.Stage != expectedStage ||
                !(nextIndex == expectedIndex && nextStage == expectedStage + 1 && expectedStage is 0 or 1 ||
                  nextIndex == expectedIndex + 1 && nextStage == 0 && expectedStage == 2))
                throw new InvalidOperationException("The activity continuation lost its exact paid cursor.");
            engine.ReplaceRuntimeTop(frame with { WenheRuleFlow = receipt with { Index = nextIndex, Stage = nextStage } });
        }
        public void ClearFlow(long frameId)
        {
            var frame = engine.GetActiveProgramFrame(frameId);
            if (frame.WenheRuleFlow is not { } receipt || receipt.Index != receipt.Seats.Count)
                throw new InvalidOperationException("An unfinished activity flow cannot be cleared.");
            engine.ReplaceRuntimeTop(frame with { WenheRuleFlow = null });
        }
        public bool DrainChildren(long frameId) => engine.DrainActivityRuleChildren(frameId);
        public bool DrainHpChildren(long frameId) => engine.TryBeginHpChangedProgramWindow(frameId, PostEventContinuation.Program);
        public void IncreaseMaximumHp(long frameId, int amount) => engine.ChangeProgramMaximumHp(engine.GetActiveProgramFrame(frameId), amount);
        public bool LoseHp(long frameId, int seat, int amount)
        {
            var frame = engine.GetActiveProgramFrame(frameId);
            return new ProgramSkillHost(engine).LoseHp(frameId, frame.SkillId, seat, amount) == SkillProgramStepOutcome.AwaitChild;
        }
        public void BeginDraw(long frameId, int seat, int amount) =>
            engine.BeginOperationProgramDraw(engine.GetActiveProgramFrame(frameId), seat, amount, new("wenhe.rule.draw"));
        public void EquipRandomAvailable(long frameId, int seat)
        {
            var frame = engine.GetActiveProgramFrame(frameId);
            if (frame.WenheRuleFlow is not { Stage: 2 } receipt || receipt.Index >= receipt.Seats.Count || receipt.Seats[receipt.Index] != seat)
                throw new InvalidOperationException("An activity equipment effect lost its exact recipient cursor.");
            var equipment = engine._cardZones.CardsAt(CardLocation.DrawPile).Where(card => engine.EquipmentRules.IsEquipment(card.Kind) &&
                engine._players[seat].EquipmentSlotCapacity(engine.EquipmentRules.Get(card.Kind).Slot) > 0).ToArray();
            if (equipment.Length > 0) engine.PlaceAdvancedEquipment(equipment[engine._random.Next(equipment.Length)], seat);
        }
    }

    private sealed class GuanduEventFlowHost(GameEngine engine) : IGuanduEventFlowHost
    {
        public long? DamageRewardUse(long frameId)
        {
            var frame = engine.GetActiveProgramFrame(frameId);
            return frame.WindowContext is { } context ? engine.GuanduDamageRewardUse(frame.OwnerSeat, context)?.Id : null;
        }
        public void IssueRepeat(long frameId)
        {
            var frame = engine.GetActiveProgramFrame(frameId);
            var context = frame.WindowContext ?? throw new InvalidOperationException("A repeated card lost its exact program context.");
            var use = engine.GuanduTenthUse(frame.OwnerSeat, context) ?? throw new InvalidOperationException("The tenth-card event lost its actual use.");
            var window = engine._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().Single(item => item.Id == context.ParentFrameId);
            var source = new CardConversionSource(frame.SkillId, GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId);
            engine.ReplaceRuntimeFrame(use.Id, use with { ExtraCardEffect = new() { Source = source, ProgramFrameId = frame.Id,
                ActionId = use.Action!.ActionId, AutomaticGameplayHash = frame.GameplayHash, Simple = window.SimpleContinuation, Trick = window.TrickContinuation } });
            engine.AdvanceEventRulesAndQueueFact(new AutomaticExtraCardEffectIssuedEvent(frame.Id, source, use.Id, use.Action!.ActionId, frame.GameplayHash));
        }
        public void IssueFlow(long frameId, SkillProgramEffectOp operation, long? useFrameId)
        {
            var frame = engine.GetActiveProgramFrame(frameId);
            if (frame.GuanduEventFlow is not null) throw new InvalidOperationException("The event flow was already issued.");
            engine.ReplaceRuntimeTop(frame with { GuanduEventFlow = new(operation, engine._turnNumber, useFrameId) });
            engine.AdvanceEventRulesAndQueueFact(new GuanduEventFlowIssuedEvent(frameId, operation, frame.OwnerSeat, engine._turnNumber, useFrameId));
        }
        public void BeginRewardDraw(long frameId, int ownerSeat)
        {
            var frame = engine.GetActiveProgramFrame(frameId);
            if (frame.OwnerSeat != ownerSeat || frame.GuanduEventFlow is not { Operation: SkillProgramEffectOp.RewardGuanduDamage, UseFrameId: { } useFrameId })
                throw new InvalidOperationException("The event reward lost its issued native use receipt.");
            engine.AdvanceEventRulesAndQueueFact(new GuanduDamageRewardedEvent(useFrameId, ownerSeat));
            engine.BeginOperationProgramDraw(frame, ownerSeat, 1, new("guandu.grain-shortage.draw"));
        }
        public bool LoseOwnerHp(long frameId)
        {
            var frame = engine.GetActiveProgramFrame(frameId);
            return new ProgramSkillHost(engine).LoseHp(frameId, frame.SkillId, frame.OwnerSeat, 1) == SkillProgramStepOutcome.AwaitChild;
        }
        public ModeGuanduFlowCursor? Current(long frameId) => engine._resolutionStack.LastOrDefault() is ProgramSkillFrame { GuanduEventFlow: { } receipt } frame && frame.Id == frameId
            ? new(frameId, frame.OwnerSeat, receipt, frame.NativeDraw is not null) : null;
        public bool DrainChildren(long frameId) => engine.DrainActivityRuleChildren(frameId);
        public void ClearFlow(long frameId)
        {
            var frame = engine.GetActiveProgramFrame(frameId);
            if (frame.GuanduEventFlow is null) throw new InvalidOperationException("The event continuation lost its issued receipt.");
            engine.ReplaceRuntimeTop(frame with { GuanduEventFlow = null });
        }
    }
}
