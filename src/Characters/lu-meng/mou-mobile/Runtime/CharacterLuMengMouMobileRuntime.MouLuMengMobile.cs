#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuMengMouMobileRuntime
{
    internal const string MouKeji = MouLuMengComposition.Keji, MouDujiang = MouLuMengComposition.Dujiang, MouDuojing = MouLuMengComposition.Duojing;
    internal bool HasMouLuMengCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveMouLuMengKeji) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMouLuMengDujiang) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMouLuMengDuojing);

    internal static CardConversionSource MouLuMengSource(ProgramSkillFrame f) => global::CardGame.Core.RuntimeSkills.RuntimeRules_lu_meng_mou_mobile_MouLuMengMobile.MouLuMengSource(f);
    internal ActualDiscardRecoveryPhaseKey MouLuMengPhase() => _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { } phase && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(phase) ? phase : throw new InvalidOperationException("Mobile Lu Meng requires the recorded actual phase, including inserted phases.");
    SkillProgramStepOutcome global::CardGame.Core.IMouLuMengHost.Keji(ProgramSkillFrame f) => ResolveMouKeji(f);
    SkillProgramStepOutcome global::CardGame.Core.IMouLuMengHost.Dujiang(ProgramSkillFrame f) => ResolveMouDujiang(f);
    SkillProgramStepOutcome global::CardGame.Core.IMouLuMengHost.Duojing(ProgramSkillFrame f) => BeginMouDuojing(f);
    [ProgramCardLegality(ProgramCardLegalityStage.BeforeTurnUseBan, 60)]
    internal bool MouLuMengCardUseForbidden(ProgramCardUseQuery query) => IsMouLuMengPeachForbidden(query.ActorSeat, query.Kind, query.ActionType);
}
