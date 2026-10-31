#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuMengMouMobileRuntime
{
    internal const string MouKeji = MouLuMengComposition.Keji, MouDujiang = MouLuMengComposition.Dujiang, MouDuojing = MouLuMengComposition.Duojing;
    internal bool HasMouLuMengCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveMouLuMengKeji) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMouLuMengDujiang) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMouLuMengDuojing);

    internal static CardConversionSource MouLuMengSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_lu_meng_mou_mobile_MouLuMengMobile.MouLuMengSource(f);
    internal ActualDiscardRecoveryPhaseKey MouLuMengPhase() => _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { } phase && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(phase) ? phase : throw new InvalidOperationException("Mobile Lu Meng requires the recorded actual phase, including inserted phases.");
    SkillProgramStepOutcome IMouLuMengHost.Keji(ProgramSkillFrame f) => ResolveMouKeji(f);
    SkillProgramStepOutcome IMouLuMengHost.Dujiang(ProgramSkillFrame f) => ResolveMouDujiang(f);
    SkillProgramStepOutcome IMouLuMengHost.Duojing(ProgramSkillFrame f) => BeginMouDuojing(f);
    [ProgramCardLegality(ProgramCardLegalityStage.BeforeTurnUseBan, 60)]
    internal bool MouLuMengCardUseForbidden(ProgramCardUseQuery query) => IsMouLuMengPeachForbidden(query.ActorSeat, query.Kind, query.ActionType);
}
