using System.Text.Json.Serialization;
using CardGame.Core.Campaigns;

namespace CardGame.Core;

public sealed record ShanheWarElephantCounterSlashReceipt(int InstructionIndex, long DamageWindowId,
    int SourceSeat, long? SuspendedAttackId, long? SuspendedDuelId, long ChildUseId);

public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ShanheWarElephantCounterSlashReceipt? ShanheWarElephantCounterSlash { get; init; }
}

internal interface IShanheWarElephantProgramHost
{
    SkillProgramStepOutcome CounterSlash(ProgramSkillFrame frame);
}

internal sealed class UseShanheWarElephantCounterSlashDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.UseShanheWarElephantCounterSlash;
    public override ISkillProgramEffectHandler Handler { get; } = new UseShanheWarElephantCounterSlashHandler();
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.UseSelectedCardsAs, static (_, _) => { });
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        r.AllowOnly("op", "target", "condition");
        if (r.RequiredEnum<SkillProgramEffectTarget>("target") != SkillProgramEffectTarget.Owner)
            throw new InvalidOperationException("Source Xiongshou counter Slash must be owned by the actual damaged participant.");
        var effect = new SkillProgramEffect(Op, SkillProgramEffectTarget.Owner, 0, r.Condition());
        RequireAlways(effect, r.Path);
        return effect;
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.AfterDamageApplied)];
}

internal sealed class UseShanheWarElephantCounterSlashHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.UseShanheWarElephantCounterSlash;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host) =>
        host.GetCapability<IShanheWarElephantProgramHost>().CounterSlash(frame);
}

public sealed partial class GameEngine
{
    private const string ShanheSourceShouyi673 = "shanhe:source-shouyi-673";
    private const string ShanheSourceXiongshou1136 = "shanhe:source-xiongshou-1136";
    private bool IsShanheWarElephantMode => _modeDefinition.Id == ShanheBattleSetup.BattleModeId && _options.ShanheBattle is not null;
    private bool ShanheWarElephantHas(CharacterState owner, string skillId) => IsShanheWarElephantMode &&
        owner.IsAlive && EnabledSkillPrograms(owner).Any(program => program.Id == skillId);

    [ProgramDistanceRule(ProgramDistanceStage.CardUseExemption, 24530)]
    private bool ShanheWarElephantUnlimitedCardDistance(ProgramCardDistanceQuery query) =>
        ShanheWarElephantHas(query.Actor, ShanheSourceShouyi673);

    private bool ShanheWarElephantCannotTurnOver(CharacterState owner) =>
        ShanheWarElephantHas(owner, ShanheSourceXiongshou1136);

    [ProgramCapability]
    private sealed class ShanheWarElephantProgramCapability(GameEngine engine) : IShanheWarElephantProgramHost
    {
        public SkillProgramStepOutcome CounterSlash(ProgramSkillFrame frame) => engine.UseShanheWarElephantCounterSlash(frame);
    }

    private SkillProgramStepOutcome UseShanheWarElephantCounterSlash(ProgramSkillFrame supplied)
    {
        var frame = GetActiveProgramFrame(supplied.Id);
        if (!IsShanheWarElephantMode || frame.SkillId != ShanheSourceXiongshou1136 || frame.ShanheWarElephantCounterSlash is not null ||
            frame.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied, Amount: > 1 } context ||
            context.TargetSeat != frame.OwnerSeat || _resolutionStack.Count < 2 ||
            _resolutionStack[^2] is not DamageTriggerWindowFrame window || window.Id != context.ParentFrameId ||
            window.TriggerWindow != context.Window || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count ||
            !MountObserverCandidateMatches(frame, window.Candidates[window.CandidateIndex].ToProgramCandidate()))
            throw new InvalidOperationException("Source Xiongshou lost its actual first large-damage owner and original after-damage window.");
        var original = GetDamageTriggerAttack(window);
        if (original.TargetSeat != frame.OwnerSeat || original.DamageAmount != context.Amount ||
            context.SourceSeat != (original.IsSourceLess ? null : original.SourceSeat))
            throw new InvalidOperationException("Source Xiongshou cannot borrow another damage's source or amount.");
        // The first qualifying damage consumes this actual turn's trigger even if
        // it has no living legal source. Smaller hits do not consume the trigger.
        if (context.SourceSeat is not { } source || !IsValidPlayerSeat(source) || source == frame.OwnerSeat ||
            !_players[source].IsAlive || !_players[frame.OwnerSeat].IsAlive ||
            !CanUseSlashTarget(_players[frame.OwnerSeat], _players[source], new Card(0, CardKind.Slash, Suit.None, 0),
                new(frame.SkillId, GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId),
                CardKind.Slash, noEffectiveRank: true, physicalCardIds: []))
            return SkillProgramStepOutcome.Continue;
        var attack = ActiveCardAttack?.ResolutionId;
        var duel = ActiveDuel?.OwnerFrameId;
        ReplaceRuntimeTop(frame = frame with { ShanheWarElephantCounterSlash = new(frame.InstructionIndex,
            window.Id, source, attack, duel, _resolutionSequence + 1) });
        if (attack is { } attackId) UpdateCardAttackState(attackId, state => state! with { Active = false });
        if (duel is { } duelId) UpdateCardContinuations(duelId, state => state with { Duel = state.Duel! with { Active = false } });
        return UseTriggeredProgramVirtualSlash(frame, source);
    }

    private IReadOnlyList<Card> SeparateShanheWarElephantCounterParentProcessing(CardAttackHandle attack, IReadOnlyList<Card> processing)
    {
        if (!IsShanheWarElephantMode || processing.Count == 0) return processing;
        var parent = _resolutionStack.OfType<ProgramSkillFrame>()
            .SingleOrDefault(frame => frame.ShanheWarElephantCounterSlash?.ChildUseId == attack.ResolutionId);
        if (parent?.ShanheWarElephantCounterSlash is not { } receipt) return processing;
        var index = _resolutionStack.FindIndex(frame => frame.Id == parent.Id);
        if (parent.SkillId != ShanheSourceXiongshou1136 || receipt.InstructionIndex != parent.InstructionIndex ||
            parent.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied, Amount: > 1 } context ||
            context.TargetSeat != parent.OwnerSeat || context.SourceSeat != receipt.SourceSeat || index < 1 ||
            _resolutionStack[index - 1] is not DamageTriggerWindowFrame window || window.Id != receipt.DamageWindowId ||
            window.Id != context.ParentFrameId || window.TriggerWindow != context.Window ||
            !DamageProgramMatchesWindow(parent, window) || index + 1 >= _resolutionStack.Count ||
            _resolutionStack[index + 1] is not CardUseFrame { CardId: 0, CardKind: CardKind.Slash, Action: { } action } child ||
            child.Id != receipt.ChildUseId || child.SourceSeat != parent.OwnerSeat ||
            attack.ProgramSkillCardUseFrameId != parent.Id || attack.SourceSeat != parent.OwnerSeat ||
            attack.TargetSeat != receipt.SourceSeat || attack.PhysicalCards.Count != 0 ||
            action.Type != CardActionType.Use || action.ActorSeat != parent.OwnerSeat || action.ProviderSeat != parent.OwnerSeat ||
            action.PhysicalCards.Count != 0 || action.EffectiveKind != CardKind.Slash ||
            !action.TargetSeats.SequenceEqual([receipt.SourceSeat]) ||
            !action.ConversionChain.SequenceEqual([new CardConversionSource(parent.SkillId, GetProgramBindingId(parent), parent.OwnerSeat, parent.SkillInstanceId)]) ||
            action.ParentActionId != _resolutionStack.Take(index).OfType<CardUseFrame>().LastOrDefault()?.Action?.ActionId)
            throw new InvalidOperationException("Source Xiongshou counter lost its exact damaged-owner window or issued native child chain.");
        if (index < 2 || _resolutionStack[index - 2] is not DamageFrame damage || damage.Id != window.ParentFrameId ||
            context.DamageFrameId != damage.Id || damage.SourceSeat != receipt.SourceSeat ||
            damage.TargetSeat != parent.OwnerSeat || damage.Amount != context.Amount)
            throw new InvalidOperationException("Source Xiongshou counter lost its exact already-applied parent damage entity.");
        // This original is deliberately inactive while its counter owns the
        // response window. Read the exact stored owner, never the active cursor.
        IDamageAttempt original = _resolutionStack.FirstOrDefault(frame => frame.Id == damage.ParentFrameId) switch
        {
            ProgramSkillFrame { AttackAttempt: not null } program => new ProgramAttackHandle(this, program.Id),
            CardUseFrame { CardAttack: not null } use => new CardAttackHandle(this, use.Id),
            ProgramSkillFrame { CardAttack: not null } program => new CardAttackHandle(this, program.Id),
            JudgmentFrame { CardAttack: not null } judgment => new CardAttackHandle(this, judgment.Id),
            _ => throw new InvalidOperationException("Source Xiongshou counter lost its original stored damage-attempt owner.")
        };
        if (!original.DamageWasApplied || original.TargetSeat != parent.OwnerSeat || original.IsSourceLess ||
            original.SourceSeat != receipt.SourceSeat || original.DamageAmount != context.Amount)
            throw new InvalidOperationException("Source Xiongshou counter cannot claim another damage window's retained materials.");

        var held = new HashSet<int>();
        if (receipt.SuspendedAttackId is { } attackId)
        {
            var ownerIndex = _resolutionStack.FindIndex(frame => frame.Id == attackId);
            if (ownerIndex < 0 || ownerIndex >= index - 1 || GetCardAttackState(attackId) is not { Active: false } state)
                throw new InvalidOperationException("Source Xiongshou retained material requires its actual inactive ancestor attack.");
            foreach (var id in state.PhysicalCardIds) Hold(id);
        }
        if (receipt.SuspendedDuelId is { } duelId)
        {
            var ownerIndex = _resolutionStack.FindIndex(frame => frame.Id == duelId);
            if (ownerIndex < 0 || ownerIndex >= index - 1 || GetCardContinuations(duelId).Duel is not { Active: false } ||
                _resolutionStack[ownerIndex] is not CardUseFrame duel)
                throw new InvalidOperationException("Source Xiongshou retained material requires its actual inactive ancestor Duel.");
            foreach (var id in duel.Action?.PhysicalCards.Select(card => card.CardId) ?? duel.PhysicalCardIds ?? []) Hold(id);
        }
        return held.Count == 0 ? processing : processing.Where(card => !held.Contains(card.Id)).ToArray();

        void Hold(int cardId)
        {
            if (_cardZones.GetLocation(cardId) != CardLocation.Processing) return;
            if (_cardMovements.LastOrDefault(move => move.CardId == cardId) is not { To: var destination } ||
                destination != CardLocation.Processing || attack.PhysicalCards.Any(card => card.Id == cardId))
                throw new InvalidOperationException("Source Xiongshou retained material lost its real paid ancestor ownership.");
            held.Add(cardId);
        }
    }

    [NativeAttackReturn(NativeAttackReturnPhase.BeforeParentResume, 24530)]
    private bool RestoreShanheWarElephantCounterSlashParent(AttackCompletionReceipt completion)
    {
        if (completion.ProgramFrameId is not { } parentId ||
            _resolutionStack.LastOrDefault() is not ProgramSkillFrame { ShanheWarElephantCounterSlash: { } receipt } parent ||
            parent.Id != parentId) return false;
        if (receipt.ChildUseId != completion.ResolutionId || receipt.InstructionIndex != parent.InstructionIndex ||
            _resolutionStack.Any(frame => frame.Id == receipt.ChildUseId))
            throw new InvalidOperationException("Source Xiongshou lost its exact retired counter Slash child.");
        if (receipt.SuspendedAttackId is { } attackId) UpdateCardAttackState(attackId, state => state! with { Active = true });
        if (receipt.SuspendedDuelId is { } duelId) UpdateCardContinuations(duelId, state => state with { Duel = state.Duel! with { Active = true } });
        ReplaceRuntimeTop(parent with { ShanheWarElephantCounterSlash = null });
        AdvanceRuntimeProgram(parent.Id);
        return true;
    }
}
