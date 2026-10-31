#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramWindowReturnsRuntime
{
    internal void ReturnSurvivedDyingProgramWindow(ProgramSkillFrame frame, bool completed) => _programWindowReturnOperations.Get().ReturnSurvivedDyingProgramWindow(frame, completed);
    internal void ReturnPindianCardsRevealedProgramWindow(ProgramSkillFrame frame, bool completed) => _programWindowReturnOperations.Get().ReturnPindianCardsRevealedProgramWindow(frame, completed);
    internal void ReturnActualSlashTargetPenaltyProgramWindow(ProgramSkillFrame frame, bool completed) => _programWindowReturnOperations.Get().ReturnActualSlashTargetPenaltyProgramWindow(frame, completed);
    internal void ReturnActualSlashTargetBenefitProgramWindow(ProgramSkillFrame frame, bool completed) => _programWindowReturnOperations.Get().ReturnActualSlashTargetBenefitProgramWindow(frame, completed);
    internal void ReturnOtherActualUseTargetedProgramWindow(ProgramSkillFrame frame, bool completed) => _programWindowReturnOperations.Get().ReturnOtherActualUseTargetedProgramWindow(frame, completed);
    internal void ReturnProgramTargetCommittedProgramWindow(ProgramSkillFrame frame, bool completed) => _programWindowReturnOperations.Get().ReturnProgramTargetCommittedProgramWindow(frame, completed);
    internal void ReturnOtherActualTurnStartedProgramWindow(ProgramSkillFrame frame, bool completed) => _programWindowReturnOperations.Get().ReturnOtherActualTurnStartedProgramWindow(frame, completed);
    internal void ReturnTurnBeforeStartProgramWindow(ProgramSkillFrame frame, bool completed) => _programWindowReturnOperations.Get().ReturnTurnBeforeStartProgramWindow(frame, completed);
    internal void ReturnRoundStartingProgramWindow(ProgramSkillFrame frame, bool completed) => _programWindowReturnOperations.Get().ReturnRoundStartingProgramWindow(frame, completed);
    internal void ReturnDrawPhaseEndedProgramWindow(ProgramSkillFrame frame, bool completed) => _programWindowReturnOperations.Get().ReturnDrawPhaseEndedProgramWindow(frame, completed);
    internal void ReturnAfterNormalDrawProgramWindow(ProgramSkillFrame frame, bool completed) => _programWindowReturnOperations.Get().ReturnAfterNormalDrawProgramWindow(frame, completed);
    internal void ReturnDrawPhaseStartingProgramWindow(ProgramSkillFrame frame, bool completed) => _programWindowReturnOperations.Get().ReturnDrawPhaseStartingProgramWindow(frame, completed);
    internal void ReturnPlayEndingProgramWindow(ProgramSkillFrame frame, bool completed) => _programWindowReturnOperations.Get().ReturnPlayEndingProgramWindow(frame, completed);
    internal void ReturnDiscardPhaseStartingProgramWindow(ProgramSkillFrame frame, bool completed) => _programWindowReturnOperations.Get().ReturnDiscardPhaseStartingProgramWindow(frame, completed);
    internal void ReturnDiscardPhaseEndedProgramWindow(ProgramSkillFrame frame, bool completed) => _programWindowReturnOperations.Get().ReturnDiscardPhaseEndedProgramWindow(frame, completed);
    internal void ReturnGameStartingProgramWindow(ProgramSkillFrame frame, bool completed) => _programWindowReturnOperations.Get().ReturnGameStartingProgramWindow(frame, completed);
    internal void ReturnSelfDyingResponseProgramWindow(ProgramSkillFrame frame, bool completed) => _programWindowReturnOperations.Get().ReturnSelfDyingResponseProgramWindow(frame, completed);
    internal void ReturnBeforeDamageAppliedProgramWindow(ProgramSkillFrame frame, bool completed) => _programWindowReturnOperations.Get().ReturnBeforeDamageAppliedProgramWindow(frame, completed);
    internal void ReturnDamageAppliedBeforeDyingProgramWindow(ProgramSkillFrame frame, bool completed) => _programWindowReturnOperations.Get().ReturnDamageAppliedBeforeDyingProgramWindow(frame, completed);
    internal void ReturnAfterHpLostProgramWindow(ProgramSkillFrame frame, bool completed) => _programWindowReturnOperations.Get().ReturnAfterHpLostProgramWindow(frame, completed);
    internal void ReturnCardsGainedProgramWindow(ProgramSkillFrame frame, bool completed) => _programWindowReturnOperations.Get().ReturnCardsGainedProgramWindow(frame, completed);
    internal void ReturnOwnerDiedProgramWindow(ProgramSkillFrame frame, bool completed) => _programWindowReturnOperations.Get().ReturnOwnerDiedProgramWindow(frame, completed);
    internal void ReturnCharacterDiedProgramWindow(ProgramSkillFrame frame, bool completed) => _programWindowReturnOperations.Get().ReturnCharacterDiedProgramWindow(frame, completed);
    internal void ReturnCardEffectBeforeApplyProgramWindow(ProgramSkillFrame frame, bool completed) => _programWindowReturnOperations.Get().ReturnCardEffectBeforeApplyProgramWindow(frame, completed);
    internal void ReturnCardUseCommittedProgramWindow(ProgramSkillFrame frame, bool completed) => _programWindowReturnOperations.Get().ReturnCardUseCommittedProgramWindow(frame, completed);
    internal void ReturnSlashTargetRedirectingProgramWindow(ProgramSkillFrame frame, bool completed) => _programWindowReturnOperations.Get().ReturnSlashTargetRedirectingProgramWindow(frame, completed);
    internal void ReturnJudgmentFinalizedProgramWindow(ProgramSkillFrame frame, bool completed) => _programWindowReturnOperations.Get().ReturnJudgmentFinalizedProgramWindow(frame, completed);
    internal void ReturnJudgmentReplacingProgramWindow(ProgramSkillFrame frame, bool completed) => _programWindowReturnOperations.Get().ReturnJudgmentReplacingProgramWindow(frame, completed);
}
