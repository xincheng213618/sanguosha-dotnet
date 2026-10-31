#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramOwnCardDiscardsRuntime
{
    internal SkillProgramStepOutcome ChooseProgramOwnCardDiscard(ProgramSkillFrame frame, ProgramParticipantReference? chooser, IReadOnlyList<CardZoneKind> zones, CardMoveReason reason) => _programPaidCardOperations.Get().ChooseProgramOwnCardDiscard(frame, chooser, zones, reason);
    internal IReadOnlyList<PromptChoice> BuildOwnCardDiscardChoices(long frameId, int chooserSeat, IReadOnlyList<CardZoneKind> zones, CardMoveReason reason) => _programPaidCardOperations.Get().BuildOwnCardDiscardChoices(frameId, chooserSeat, zones, reason);
    internal void ResolveProgramOwnCardDiscardChoice(PromptChoice selected) => _programPaidCardOperations.Get().ResolveProgramOwnCardDiscardChoice(selected);
    [ProgramAiChoice(SkillProgramEffectOp.DrawTwoThenDiscardTwoIfOverMaxHp, SkillProgramEffectOp.ChooseOwnCardDiscard)]
    internal PromptChoice SelectAiProgramOwnCardDiscard(PendingDecision decision, ProgramSkillFrame frame)
    {
        var skill = _contentRegistryOperations._contentRegistry!.GetSkill(frame.SkillId);
        var(selected, thought) = _coreAiBrainsAiBrainsAccess._aiBrains[decision.PlayerSeat].ChooseOwnCardDiscard(_corePlayerViewProjectionOperations.CreateSnapshot(decision.PlayerSeat), decision.Choices, skill.Name, ++_coreAiDiagnosticsThoughtSequenceAccess._thoughtSequence);
        _coreNativeAiDecisionsOperations.AddThought(thought);
        return selected;
    }
}
