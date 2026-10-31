#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheS5LiJueRuntime
{
    internal int ShanheS5LangxiMinimumDamage(ProgramSkillFrame frame) => ProgramInstructionResolver.Default.Features(_coreProgramLifecycleOperations.GetProgramTrigger(frame)).HasOperation(SkillProgramEffectOp.ChooseHpBoundOtherAndDealPositiveRandomDamage) ? 1 : 0;
    SkillProgramStepOutcome IShanheS5LiJueProgramHost.BeginPositiveRandomDamage(ProgramSkillFrame frame) => BeginShanheS5Langxi(frame);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 5750)]
    internal bool CanOfferShanheS5Langxi(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => !ProgramInstructionResolver.Default.Features(trigger).HasOperation(SkillProgramEffectOp.ChooseHpBoundOtherAndDealPositiveRandomDamage) || context.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow && _characterLiJueOlOperations.LangxiTargets(candidate.OwnerSeat).Length > 0;
    internal SkillProgramStepOutcome BeginShanheS5Langxi(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == frame.Id);
        if (frame.LiJue is not null || frame.InstructionIndex != 1 || frame.TriggerId is null || frame.WindowContext is not { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow } context || index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not ProgramLifecycleTriggerWindowFrame parent || parent.Id != context.ParentFrameId || parent.Window != context.Window || parent.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(frame, parent.Candidates[parent.CandidateIndex]) || context.SourceSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || context.OwnerSeat != frame.OwnerSeat || _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber < 1 || !_coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat].IsAlive || !_characterLiJueOlOperations.LiJueCurrentSource(frame))
            throw new InvalidOperationException("S5 Langxi requires its original unpaid source at a living character's actual preparation window.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { LiJue = new(CharacterLiJueOlRuntime.LiJueSource(frame), frame.GameplayHash, frame.InstructionIndex, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, context.ParentFrameId, LiJueStage.ChoosingTarget) });
        _characterLiJueOlOperations.PublishLangxi(frame);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    [ProgramAiChoice(SkillProgramEffectOp.ChooseHpBoundOtherAndDealPositiveRandomDamage)]
    internal PromptChoice SelectAiShanheS5Langxi(PendingDecision decision, ProgramSkillFrame frame) => _characterLiJueOlOperations.LangxiChoices(frame).First();
}
