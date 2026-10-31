#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineFrozenFactionRecoveryRuntime
{
    void global::CardGame.Core.IFrozenFactionRecoveryProgramHost.FreezeLivingFactionRecovery(ProgramSkillFrame f) => FreezeProgramLivingFactionRecovery(f);
    void global::CardGame.Core.IFrozenFactionRecoveryProgramHost.DrawToFrozenFactionCount(ProgramSkillFrame f) => DrawProgramToFrozenFactionCount(f);
    internal void FreezeProgramLivingFactionRecovery(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.FrozenFactionRecovery is not null || f.InstructionIndex != 1 || f.WindowContext is not { Window: SkillProgramTriggerWindow.SelfDyingResponse } context || _coreDyingCursorRuntimeOperations.ActiveDying is not { } dying || context.ParentFrameId != dying.Id || context.TargetSeat != f.OwnerSeat || dying.VictimSeat != f.OwnerSeat || dying.ResponderSeat != f.OwnerSeat || _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp > 0)
            throw new InvalidOperationException("Frozen faction recovery lost its exact fresh self-dying producer.");
        var x = _coreRuleQueriesOperations.GetLivingFactionCount();
        var points = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DamageAppliedEvent>().Where(e => !e.SourceLess && e.SourceSeat == f.OwnerSeat && e.Amount > 0).Sum(e => e.Amount);
        _coreProgramExpressionsOperations.CaptureProgramExpressionNumber(f.Id, FrozenFactionRecoveryComposition.FactionNumber, x);
        _coreProgramExpressionsOperations.CaptureProgramExpressionNumber(f.Id, FrozenFactionRecoveryComposition.DamageNumber, points);
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { FrozenFactionRecovery = new(f.InstructionIndex, dying.Id) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FrozenFactionRecoveryCapturedEvent(f.Id, new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId), dying.Id, x, points));
        _coreProgramLifecycleOperations.RecoverProgramTargetTo(f.Id, f.OwnerSeat, f.OwnerSeat, SkillProgramNumberExpression.IntegerConstant, x, true);
    }

    internal void DrawProgramToFrozenFactionCount(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        var receipt = f.FrozenFactionRecovery ?? throw new InvalidOperationException("Frozen faction draw lost its completed recovery producer.");
        var factions = _coreProgramExpressionsOperations.ReadProgramExpressionNumber(f, FrozenFactionRecoveryComposition.FactionNumber) ?? throw new InvalidOperationException("The frozen faction hand target lost its issued quantity.");
        if (f.InstructionIndex != receipt.InstructionIndex + 1 || receipt.DrawIssued)
            throw new InvalidOperationException("The frozen faction hand target can be issued only once after recovery children.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { FrozenFactionRecovery = receipt with { DrawIssued = true } });
        var count = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchOutcomeWinnerAccess._winner == Winner.None ? Math.Max(0, factions - _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count) : 0;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FrozenFactionHandDrawIssuedEvent(f.Id, f.OwnerSeat, factions, count));
        if (count > 0)
            _coreProgramLifecycleOperations.DrawProgramCards(f.Id, f.OwnerSeat, count, null, null, SkillProgramCardSetVisibility.Private, new("program.frozen-faction-recovery.draw"));
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.BeforeSelectionState, 250)]
    internal void AssertFrozenFactionRecovery(ProgramSkillFrame f)
    {
        if (f.FrozenFactionRecovery is not { } r)
            return;
        var factions = _coreProgramExpressionsOperations.ReadProgramExpressionNumber(f, FrozenFactionRecoveryComposition.FactionNumber);
        var damage = _coreProgramExpressionsOperations.ReadProgramExpressionNumber(f, FrozenFactionRecoveryComposition.DamageNumber);
        if (r.InstructionIndex != 1 || factions is not { } factionCount || factionCount < 1 || factionCount > _coreMatchParticipantsPlayersAccess._players.Count || damage is not { } damagePoints || damagePoints < 0 || f.WindowContext is not { Window: SkillProgramTriggerWindow.SelfDyingResponse } context || context.ParentFrameId != r.DyingFrameId || f.InstructionIndex is < 1 or > 3 || r.DrawIssued != (f.InstructionIndex >= 2) || !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<FrozenFactionRecoveryCapturedEvent>().Any(e => e.FrameId == f.Id && e.Source == new CardConversionSource(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId) && e.DyingFrameId == r.DyingFrameId && e.FactionCount == factionCount && e.GameDamagePoints == damagePoints))
            throw new InvalidOperationException("A frozen faction recovery lost its exact immutable amount/history receipt.");
    }
}
