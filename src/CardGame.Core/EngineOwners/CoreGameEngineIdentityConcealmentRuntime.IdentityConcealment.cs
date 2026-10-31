#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Text.Json.Serialization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineIdentityConcealmentRuntime
{
    internal const string IdentityConcealedGeneralId = global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_IdentityConcealment.IdentityConcealedGeneralId;
    internal static GeneralDefinition IdentityConcealedGeneral() => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_IdentityConcealment.IdentityConcealedGeneral();
    internal bool GeneralHasIdentityConcealment(GeneralDefinition general) => _coreModeAllegianceOperations.ModeAllegiancePolicy.AllowsIdentityConcealment && general.Skills.Select(s => s.ContentId).OfType<string>().Any(id => _contentRegistryOperations._contentRegistry!.Skills.GetValueOrDefault(id)?.Program?.Triggers.Any(t => t.Window == SkillProgramTriggerWindow.IdentityGeneralAppeared && t.Effects.Any(e => e.Op == SkillProgramEffectOp.AfterActualIdentityAppearanceRecover)) == true);
    internal void InitializeSelectedIdentityConcealment(CharacterState player)
    {
        if (!GeneralHasIdentityConcealment(player.General) || player.IdentityConcealed)
            return;
        var initial = player.General.InitialHp ?? player.General.BaseHp;
        if (player.Role == Role.Lord)
            initial++;
        player.IdentityConcealment = new(player.General.Id, initial);
        player.GeneralRevealed = false;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new IdentityConcealmentStartedEvent(player.Seat));
    }

    internal bool TryBeginIdentityAppearanceForTurn(CharacterState player)
    {
        if (!player.IdentityConcealed)
            return false;
        if (_resolutionStackOperations._resolutionStack.Count != 0)
            throw new InvalidOperationException("Own-turn identity appearance needs its exact clean native boundary.");
        BeginIdentityAppearance(player, IdentityAppearanceEntry.ActualTurnStart, null, 0, null, null);
        return true;
    }

    internal bool TryBeginIdentityAppearanceForDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack, int amount)
    {
        if (amount <= 0 || !_coreMatchParticipantsPlayersAccess._players[attack.TargetSeat].IdentityConcealed)
            return false;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != attack.ResolutionId || _coreProgramAttackAttemptOperations.CurrentDamageAttempt?.ResolutionId != attack.ResolutionId)
            throw new InvalidOperationException("Identity appearance lost its actual native damage owner.");
        BeginIdentityAppearance(_coreMatchParticipantsPlayersAccess._players[attack.TargetSeat], IdentityAppearanceEntry.DamageDeduction, attack.ResolutionId, amount, attack.IsSourceLess ? null : attack.SourceSeat, attack.EffectiveCardKind, attack);
        return true;
    }

    internal bool TryBeginIdentityAppearanceForProgramHpLoss(long frameId, string skillId, int targetSeat, int amount)
    {
        if (amount <= 0 || !_coreMatchParticipantsPlayersAccess._players[targetSeat].IdentityConcealed)
            return false;
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frameId);
        if (frame.SkillId != skillId)
            throw new InvalidOperationException("Identity HP-loss appearance lost its owning program source.");
        BeginIdentityAppearance(_coreMatchParticipantsPlayersAccess._players[targetSeat], IdentityAppearanceEntry.ProgramHpLoss, frameId, amount, null, null);
        return true;
    }

    internal void BeginIdentityAppearance(CharacterState player, IdentityAppearanceEntry entry, long? parent, int deduction, int? damageSource, CardKind? cardKind, global::CardGame.Core.GameEngine.IDamageAttempt? attack = null)
    {
        var state = player.IdentityConcealment ?? throw new InvalidOperationException("Identity appearance cannot repeat.");
        if (state.GeneralId != player.General.Id)
            throw new InvalidOperationException("Concealment belongs to a different selected real template.");
        var receipt = new IdentityAppearanceReceipt(entry, parent, player.Seat, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, player.General.Id, Math.Clamp(state.InitialHp, 1, player.MaxHp), player.MaxHp, deduction, damageSource, cardKind, attack?.DamageAmount, attack?.SourceSeat, attack?.IsSourceLess ?? false, attack is null ? null : _coreNativeCardClassificationsOperations.GetDamageNature(attack), attack is null ? null : _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.Action?.ActionId, parent is { } parentId && _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == parentId)is ProgramSkillFrame p ? p.InstructionIndex : null);
        var id = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        _coreRuntimeOperations.PushRuntimeFrame(new IdentityAppearanceFrame(id, receipt));
        player.IdentityConcealment = null;
        player.Hp = receipt.RestoredHp;
        player.GeneralRevealed = true;
        if (deduction > 0)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new IdentityHpDeductionPreventedEvent(id, player.Seat, deduction, entry, damageSource));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new IdentityGeneralAppearedEvent(id, receipt));
        AdvanceIdentityAppearance((IdentityAppearanceFrame)_resolutionStackOperations._resolutionStack.Last());
    }

    [NativeFrameAutomaticStep(NativeFrameAutomaticPhase.AfterOpening, 40, NativeFrameAutomaticPending.RequireNoneBeforeFrame, NativeFrameAutomaticPublish.AfterAdvance)]
    [NativeFrameRoute(typeof(IdentityAppearanceFrame), 90)]
    internal void AdvanceIdentityAppearance(IdentityAppearanceFrame frame)
    {
        RequireIdentityAppearance(frame);
        if (frame.ActiveWindowId is not null)
            return;
        var player = _coreMatchParticipantsPlayersAccess._players[frame.Receipt.OwnerSeat];
        var facts = _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(player);
        var candidates = _coreProgramLifecycleOperations.CollectEligibleProgramTriggerCandidates(player, SkillProgramTriggerWindow.IdentityGeneralAppeared, facts).Where(c => frame.Receipt.ActualTurnNumber > 0 && frame.Receipt.ActualTurnOwnerSeat != c.OwnerSeat).ToArray();
        if (candidates.Length == 0)
        {
            CompleteIdentityAppearance(frame);
            return;
        }

        var window = new ProgramLifecycleTriggerWindowFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, player.Seat, SkillProgramTriggerWindow.IdentityGeneralAppeared, candidates, ProgramLifecycleContinuation.ResumeIdentityAppearance, facts)
        {
            IdentityAppearanceFrameId = frame.Id
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { ActiveWindowId = window.Id });
        _coreRuntimeOperations.PushRuntimeFrame(window);
        _coreRuntimeOperations.AdvanceRuntimeTop<ProgramLifecycleTriggerWindowFrame>();
    }

    internal ProgramSkillWindowContext CreateIdentityAppearanceContext(ProgramLifecycleTriggerWindowFrame window, ProgramTriggerCandidate candidate)
    {
        var appearance = _resolutionStackOperations._resolutionStack.OfType<IdentityAppearanceFrame>().Single(f => f.Id == window.IdentityAppearanceFrameId && f.ActiveWindowId == window.Id);
        return new(SkillProgramTriggerWindow.IdentityGeneralAppeared, window.Id, candidate.OwnerSeat, SourceSeat: appearance.Receipt.ActualTurnOwnerSeat, TargetSeat: appearance.Receipt.OwnerSeat, Facts: window.Facts)
        {
            IdentityAppearance = appearance.Receipt
        };
    }

    [ProgramTriggerProof(ProgramTriggerProofPhase.NativeWindow, 30, SkillProgramTriggerWindow.IdentityGeneralAppeared)]
    internal bool CanRunIdentityAppearanceCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => context.IdentityAppearance is { } r && r.OwnerSeat == c.OwnerSeat && r.ActualTurnNumber > 0 && r.ActualTurnOwnerSeat != c.OwnerSeat && r.ActualTurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && _resolutionStackOperations._resolutionStack.OfType<IdentityAppearanceFrame>().Any(f => f.Receipt == r && f.ActiveWindowId == context.ParentFrameId);
    internal void CompleteIdentityAppearance(IdentityAppearanceFrame frame)
    {
        RequireIdentityAppearance(frame);
        var r = frame.Receipt;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new IdentityAppearanceCompletedEvent(frame.Id, r.OwnerSeat));
        _coreRuntimeOperations.PopResolutionFrame(frame.Id, ResolutionFrameKind.IdentityAppearance);
        if (r.Entry == IdentityAppearanceEntry.ActualTurnStart)
        {
            ContinueTurnAfterIdentityAppearance(_coreMatchParticipantsPlayersAccess._players[r.OwnerSeat]);
            return;
        }

        if (r.ParentFrameId is not { } parent)
            throw new InvalidOperationException("Identity appearance lost its exact parent return.");
        if (r.Entry == IdentityAppearanceEntry.ProgramHpLoss)
        {
            _coreRuntimeOperations.AdvanceRuntimeProgram(parent);
            return;
        }

        var attack = IdentityAppearanceAttack(parent) ?? throw new InvalidOperationException("Identity appearance lost its actual suspended damage attempt.");
        if (!IdentityAppearanceAttackMatches(r, attack))
            throw new InvalidOperationException("Identity appearance cannot close another native attack.");
        switch (attack)
        {
            case global::CardGame.Core.NativeOwners.CardAttackHandle card:
                _coreNativeAttackCompletionOperations.CompleteAttack(card);
                break;
            case global::CardGame.Core.NativeOwners.ProgramAttackHandle program:
                _coreProgramAttackAttemptOperations.CompleteProgramAttack(program);
                break;
            default:
                throw new InvalidOperationException("Appearance cannot complete an unsupported native attack owner.");
        }
    }

    internal void ContinueTurnAfterIdentityAppearance(CharacterState player)
    {
        _coreEndRefillNextTurnOperations.ConsumeEndRefillNextTurn(player);
        if (_coreRecoveryReplacementOperations.TryQueueHengyeRecovery(player))
            return;
        _characterLuMengMouOperations.ResolveHengyeTurnStart(player);
        _coreActualTurnsOperations.ContinueTurnAfterHengye(player);
    }

    internal global::CardGame.Core.GameEngine.IDamageAttempt? IdentityAppearanceAttack(long id) => _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == id) switch
    {
        ProgramSkillFrame { AttackAttempt: not null } => _kernelCoreGameEngineProgramAttackAttemptOperations.CreateProgramAttackHandle(id),
        ProgramSkillFrame { CardAttack.Active: true } => _kernelCoreMainOperations.CreateCardAttackHandle(id),
        CardUseFrame { CardAttack.Active: true } => _kernelCoreMainOperations.CreateCardAttackHandle(id),
        JudgmentFrame { CardAttack.Active: true } => _kernelCoreMainOperations.CreateCardAttackHandle(id),
        _ => null
    };
    internal void ResumeIdentityAppearanceWindow(ProgramLifecycleTriggerWindowFrame window)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as IdentityAppearanceFrame ?? throw new InvalidOperationException("Appearance window returned to the wrong parent.");
        if (frame.Id != window.IdentityAppearanceFrameId || frame.ActiveWindowId != window.Id)
            throw new InvalidOperationException("Appearance window lost its typed return identity.");
        CompleteIdentityAppearance(frame);
    }

    internal void RequireIdentityAppearance(IdentityAppearanceFrame frame)
    {
        var r = frame.Receipt;
        if (!Enum.IsDefined(r.Entry) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.OwnerSeat) || _coreMatchParticipantsPlayersAccess._players[r.OwnerSeat].IdentityConcealed || _coreMatchParticipantsPlayersAccess._players[r.OwnerSeat].General.Id != r.GeneralId || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<IdentityGeneralAppearedEvent>().Count(e => e.AppearanceFrameId == frame.Id && e.Receipt == r) != 1 || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<IdentityAppearanceCompletedEvent>().Any(e => e.AppearanceFrameId == frame.Id))
            throw new InvalidOperationException("Identity appearance lost its exact once-revealed template and owning invoice.");
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == frame.Id);
        if (index < 0 || r.Entry == IdentityAppearanceEntry.ActualTurnStart && (index != 0 || r.ParentFrameId is not null || r.OwnerSeat != r.ActualTurnOwnerSeat) || r.Entry != IdentityAppearanceEntry.ActualTurnStart && (index < 1 || _resolutionStackOperations._resolutionStack[index - 1].Id != r.ParentFrameId) || r.Entry == IdentityAppearanceEntry.DamageDeduction && (IdentityAppearanceAttack(r.ParentFrameId!.Value)is not { } attack || !IdentityAppearanceAttackMatches(r, attack)) || r.Entry == IdentityAppearanceEntry.ProgramHpLoss && (_resolutionStackOperations._resolutionStack[index - 1] is not ProgramSkillFrame p || p.InstructionIndex != r.OriginalInstructionIndex))
            throw new InvalidOperationException("Identity appearance lost its frozen native source, amount, nature, action or typed parent cursor.");
    }

    internal bool IdentityAppearanceAttackMatches(IdentityAppearanceReceipt r, global::CardGame.Core.GameEngine.IDamageAttempt a) => a.ResolutionId == r.ParentFrameId && a.TargetSeat == r.OwnerSeat && a.SourceSeat == r.NativeSourceSeat && a.IsSourceLess == r.SourceLess && a.DamageAmount == r.ObservedAttackAmount && a.EffectiveCardKind == r.CardKind && _coreNativeCardClassificationsOperations.GetDamageNature(a) == r.Nature && (a.IsSourceLess ? null : (int? )a.SourceSeat) == r.DamageSourceSeat && _coreCardUseLifecycleOperations.LifecycleCardUse(a.ResolutionId)?.Action?.ActionId == r.NativeActionId;
    internal SkillProgramStepOutcome ArmGaolingRecovery(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        var appearance = _resolutionStackOperations._resolutionStack.OfType<IdentityAppearanceFrame>().Single(a => a.ActiveWindowId == f.WindowContext?.ParentFrameId);
        if (f.SelectedTargetSeats is not [var target] || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || !_coreMatchParticipantsPlayersAccess._players[target].HasHpValue || f.WindowContext?.IdentityAppearance != appearance.Receipt || appearance.Receipt.OwnerSeat != f.OwnerSeat || appearance.Receipt.ActualTurnOwnerSeat == f.OwnerSeat || f.GaolingRecovery is not null)
            throw new InvalidOperationException("Gaoling needs one actual wounded recipient after an off-turn native identity appearance.");
        var receipt = new GaolingRecoveryReceipt(appearance.Id, _coreQimeiProgramsOperations.QimeiSource(f), f.GameplayHash, target);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { GaolingRecovery = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GaolingRecoverySelectedEvent(f.Id, appearance.Id, receipt.Source, f.GameplayHash, target));
        return SkillProgramStepOutcome.Continue;
    }

    internal bool ValidGaolingRecovery(ProgramSkillFrame f) => f.GaolingRecovery is { } r && r.Source.OwnerSeat == f.OwnerSeat && r.Source.SkillId == f.SkillId && r.Source.SkillInstanceId == f.SkillInstanceId && r.Source.BindingId == f.TriggerId && r.GameplayHash == f.GameplayHash && f.SelectedTargetSeats is [var target] && target == r.TargetSeat && f.WindowContext?.IdentityAppearance is { } a && a.OwnerSeat == f.OwnerSeat && a.ActualTurnOwnerSeat != f.OwnerSeat && _resolutionStackOperations._resolutionStack.OfType<IdentityAppearanceFrame>().Any(p => p.Id == r.AppearanceFrameId && p.Receipt == a && p.ActiveWindowId == f.WindowContext.ParentFrameId) && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<GaolingRecoverySelectedEvent>().Count(e => e.ProgramFrameId == f.Id && e.AppearanceFrameId == r.AppearanceFrameId && e.Source == r.Source && e.GameplayHash == r.GameplayHash && e.TargetSeat == r.TargetSeat) == 1;
}
