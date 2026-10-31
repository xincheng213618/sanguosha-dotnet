namespace CardGame.Core;

// Native policies are shared card/health primitives. Guolun, Songsang and
// Zhanji retain their qualification, selections, invoices and proofs in the module.
internal interface ISpPangTongNativeFacts : IProgramSelectionQuery
{
    Winner Winner { get; }
    bool UsesFormalWoodenOx { get; }
    bool UsesTreatedWounded { get; }
    bool IsRuleWounded(CharacterState player);
    int EffectiveOwnedCardRank(CharacterState owner, Card card);
    bool IsSourceEquipment(int owner, string skillId, string instanceId, Card card);
    CardLocation NormalizeEphemeralDestination(Card card, CardLocation from, CardLocation to);
    bool RecoveryRidesOn(ResolutionFrame child, ResolutionFrame parent);
    bool SameNameObserverSuffix(long rootFrameId);
}

internal interface ISpPangTongNativeEntries : IProgramRuntimeOperations
{
    IReadOnlyList<PromptChoice> OwnedCardPaymentChoices(long frameId, int chooser, int owner,
        IReadOnlyList<CardZoneKind> zones, OwnedCardMoveIntent intent);
    bool BeginPublicHandReveal(long frameId, string returnKey);
    void Recover(long frameId, int owner, int target, int amount);
    void ChangeMaximumHp(ProgramSkillFrame frame, int amount);
    void PublishState();
}
