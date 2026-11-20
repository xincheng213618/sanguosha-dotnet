#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWangYiMobileRuntime
{
    [ProgramViewContributor(ProgramViewContribution.PrivateCards, 73010)]
    internal IEnumerable<CardSnapshot> MobileWangYiPrivateMijiCards(int viewerSeat)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { SkillId: "mobile:miji", InstructionIndex: 4 } frame || frame.OwnerSeat != viewerSeat || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != viewerSeat || frame.CardSetBindings.SingleOrDefault(binding => binding.Name == "miji-cards")is not { Visibility: SkillProgramCardSetVisibility.Private } cards)
            return[];
        return cards.CardIds.Where(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Processing).Select(id => _coreSnapshotValueProjectionOperations.ToSnapshot(_cardZonesOperations._cardZones.CardsAt(CardLocation.Processing).Single(card => card.Id == id))).ToArray();
    }

    internal bool TryResolveAiDeckTopJudgmentReplacement()
    {
        if (_coreJudgmentCursorRuntimeOperations.Value.ActiveJudgment is not { } judgment || _coreNativeJudgmentOperations.CurrentJudgmentCandidate(judgment)is not { } candidate || !CoreGameEngineJudgmentReplacementProgramsRuntime.IsDeckTopJudgmentReplacement(_coreJudgmentReplacementProgramsOperations.GetProgramJudgmentReplacement(candidate).Trigger))
            return false;
        var current = _coreNativeDamageHandlesOperations.Value.GetJudgmentCard(judgment)!;
        // The choice only uses the current public judgment, never the hidden
        // replacement material. Miji wants black; an already-black result stays.
        var activate = judgment.Reason == "skill.mobile-wang-yi.miji" && _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[judgment.TargetSeat], current)is Suit.Heart or Suit.Diamond;
        _coreJudgmentReplacementProgramsOperations.ResolveProgramJudgmentReplacementChoice(_corePendingDecisionPolicyPipelineOperations._pendingDecision!.Choices[activate ? 0 : 1]);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        return true;
    }
}
