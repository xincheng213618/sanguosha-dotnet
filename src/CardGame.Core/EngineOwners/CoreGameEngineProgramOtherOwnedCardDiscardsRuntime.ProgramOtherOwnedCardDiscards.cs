#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramOtherOwnedCardDiscardsRuntime
{
    internal SkillProgramStepOutcome ChooseProgramOtherOwnedCardDiscard(ProgramSkillFrame frame, ProgramParticipantReference chooser, IReadOnlyList<CardZoneKind> zones, CardMoveReason reason) => _programPaidCardOperations.Get().ChooseProgramOtherOwnedCardDiscard(frame, chooser, zones, reason);
    internal IReadOnlyList<PromptChoice> BuildOtherOwnedCardDiscardChoices(long frameId, int chooserSeat, IReadOnlyList<CardZoneKind> zones, CardMoveReason reason) => _programPaidCardOperations.Get().BuildOtherOwnedCardDiscardChoices(frameId, chooserSeat, zones, reason);
    internal void ResolveProgramOtherOwnedCardDiscardChoice(PromptChoice selected) => _programPaidCardOperations.Get().ResolveProgramOtherOwnedCardDiscardChoice(selected);
    [ProgramAiChoice(SkillProgramEffectOp.DiscardDamageTargetAndClaimMount, SkillProgramEffectOp.ChooseOtherOwnedCardDiscard)]
    internal PromptChoice SelectAiProgramOtherOwnedCardDiscard(PendingDecision decision, ProgramSkillFrame frame)
    {
        var skill = _contentRegistryOperations._contentRegistry!.GetSkill(frame.SkillId);
        var(selected, thought) = _coreAiBrainsAiBrainsAccess._aiBrains[decision.PlayerSeat].ChooseOtherOwnedCardDiscard(_corePlayerViewProjectionOperations.CreateSnapshot(decision.PlayerSeat), decision.Choices, skill.Name, ++_coreAiDiagnosticsThoughtSequenceAccess._thoughtSequence);
        _coreNativeAiDecisionsOperations.AddThought(thought);
        return selected;
    }
}
