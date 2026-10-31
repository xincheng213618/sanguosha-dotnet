#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhugeGuoLeRuntime
{
    [ProgramTargetWindow(ProgramTargetWindowStage.FinalizedBeforeActivity, 20)]
    internal bool LeZhugeGuoFinalizedBeforeActivityTargetWindow(ProgramTargetCardWindowQuery query) => HasLeZhugeGuoCapability && (CoreNativeCardClassificationsRuntime.IsSlashCard(query.Kind) || CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(query.Kind));
    internal const string LeXidi = "le:xidi", LeChengyan = "le:chengyan", LeChengyanDrawReason = "skill-program.le:chengyan.draw";
    internal bool HasLeZhugeGuoCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveLeZhugeGuoChengyan);

    internal static CardConversionSource LeZhugeGuoSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_zhuge_guo_le_LeZhugeGuoXidi.LeZhugeGuoSource(f);
    internal bool HasLeXidiEntityMark(int cardId) => _cardEntityMarks.Has(cardId, "xidi");
    internal bool IsLeXidiHandLimitExempt(CharacterState owner, int cardId) => _coreNationalSkillsOperations.HasRuntimeSkill(owner, LeXidi) && _cardZonesOperations._cardZones.GetLocation(cardId) == CardLocation.Hand(owner.Seat) && HasLeXidiEntityMark(cardId);
    internal SkillProgramStepOutcome ReorderLeXidi(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.SkillId != LeXidi || f.ActivationId != f.TriggerId || f.WindowContext?.Window is not (SkillProgramTriggerWindow.TurnStartBeforeNormalFlow or SkillProgramTriggerWindow.TurnEnding))
            throw new InvalidOperationException("Xidi reorder requires its real preparation or ending window.");
        var count = _cardEntityMarks.CountMarked(f.OwnerSeat, "xidi", CardZoneKind.Hand);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LeXidiReorderStartedEvent(f.Id, f.OwnerSeat, count, Math.Max(1, count)));
        return _coreProgramTopReorderOperations.BeginProgramTopReorder(f, Math.Max(1, count), null);
    }

    SkillProgramStepOutcome ILeZhugeGuoHost.Reorder(ProgramSkillFrame f) => ReorderLeXidi(f);
    SkillProgramStepOutcome ILeZhugeGuoHost.Chengyan(ProgramSkillFrame f) => StartLeChengyan(f);
}
