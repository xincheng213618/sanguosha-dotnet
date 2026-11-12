namespace CardGame.Core.EngineOwners;

[ProgramRuntimeModule, ProgramCapability]
internal sealed class NativeCardActionInvalidationExecutionRuntime(
    INativeCardActionInvalidations _invalidations, IProgramRuntimeFrames _frames,
    INativeCardRepresentations _representations, INativeMaterialClaims _materials,
    IKernelCardZoneStoreAccess _zones,
    RuntimeEntries.CoreNativeTrickUseEntries _tricks,
    RuntimeEntries.CorePhysicalMovementFacadesEntries _movement,
    RuntimeEntries.CoreNativeCardUseCompletionEntries _completion,
    RuntimeEntries.CoreGameEngineRoundOnceCardlessNativeResponsesEntries _cardless,
    RuntimeEntries.CoreGameEngineTieredRoundZeroResponsesEntries _tiered,
    RuntimeEntries.CoreGameEngineChainedStateBasicReturnsEntries _chained,
    RuntimeEntries.DeferredRuntime<CoreGameEngineDrawFundedDistinctBasicResponsesRuntime> _funded,
    RuntimeEntries.CoreNativeCardUseFramesEntries _uses,
    RuntimeEntries.CoreNativeDamageApplicationEntries _damage,
    RuntimeEntries.CoreNativeAttackCompletionEntries _attacks) : INativeCardActionInvalidationExecution
{
    public bool TryCompleteSimple(long useId, ProgramSimpleCardContinuation continuation,
        NativeSimpleCardCancellationBoundary boundary)
    {
        if (_invalidations.TryReadSimpleCancellation(useId, continuation.Effect, boundary) is not { } policy)
            return false;
        if (_frames.Top is not CardUseFrame use || use.Id != useId)
            throw new InvalidOperationException("An ineffective simple use must drain its original native children before completion.");
        foreach (var card in _tricks.GetCardUsePhysicalCards(useId))
        {
            if (!_materials.PreservesMaterialAtCompletion(useId, card.Id) &&
                _zones._cardZones.GetLocation(card.Id) == CardLocation.Processing)
                _movement.MoveCard(card, CardLocation.Processing, CardLocation.DiscardPile, policy.CleanupReason);
        }
        var original = use.CardId == 0 ? new Card(0, use.CardKind, Suit.None, 0) :
            _representations.Get(useId, use.CardId, requireProcessing: false);
        _completion.FinishCardUse(useId, original, use.CardKind);
        return true;
    }

    public bool HasIneffectiveDodge(NativeOwners.CardAttackHandle attack, CardActionContext action,
        ProgramCardContinuation continuation) => continuation is ProgramCardContinuation.Dodge or ProgramCardContinuation.GroupResponse &&
        _invalidations.IsDodgeIneffective(attack.ResolutionId, action);

    public bool ContinueIneffectiveDodge(NativeOwners.CardAttackHandle attack, CardActionContext action,
        ProgramCardContinuation continuation)
    {
        if (!HasIneffectiveDodge(attack, action, continuation)) return false;
        _cardless.ClearRoundOnceCardlessDodgeResponse(attack);
        _tiered.ClearTieredRoundDodgeResponse(attack);
        _chained.ClearChainedStateBasicDodgeResponse(attack);
        _funded.Value.ClearDrawFundedDistinctBasicDodgeResponse(attack);
        _uses.SetCardUseStep(attack.ResolutionId, ResolutionFrameStep.ResolvingEffect);
        if (!_damage.ApplyAttackDamage(attack)) _attacks.CompleteAttack(attack);
        return true;
    }
}
