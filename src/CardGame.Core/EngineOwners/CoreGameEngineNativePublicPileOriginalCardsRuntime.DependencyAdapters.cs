#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiFengOlRuntime))]
internal sealed class CoreNativePublicPileOriginalCardsCharacterLiFengOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiFengOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativePublicPileOriginalCardsCharacterLiFengOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiFengOlRuntime> _targetFactory = targetFactory;
    public bool HasLiFengCapability { get => this._targetFactory().HasLiFengCapability; }

    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.NativePublicPileCardSelection> ShuliangOriginalGrainChoices(global::CardGame.Core.NativePublicPileCardRequest request) => this._targetFactory().ShuliangOriginalGrainChoices(request);
    public bool IsShuliangOriginalGrainChoice(global::CardGame.Core.NativePublicPileCardRequest request, global::CardGame.Core.NativePublicPileCardSelection selection) => this._targetFactory().IsShuliangOriginalGrainChoice(request, selection);
    public void RecordShuliangGrainPayment(global::CardGame.Core.NativePublicPileCardPayment payment) => this._targetFactory().RecordShuliangGrainPayment(payment);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CoreNativePublicPileOriginalCardsCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativePublicPileOriginalCardsCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime))]
internal sealed class CoreNativePublicPileOriginalCardsCoreProgramEventHistoryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativePublicPileOriginalCardsCoreProgramEventHistoryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent => this._targetFactory().ProgramEventHistory<T>();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterPangTongFriendRuntime))]
internal sealed class CoreNativePublicPileOriginalCardsCharacterPangTongFriendAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangTongFriendRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativePublicPileOriginalCardsCharacterPangTongFriendOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangTongFriendRuntime> _targetFactory = targetFactory;
    public bool IsFriendYangmingOriginalUseParent(global::CardGame.Core.CardUseFrame use) => this._targetFactory().IsFriendYangmingOriginalUseParent(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSimaYiMouRuntime))]
internal sealed class CoreNativePublicPileOriginalCardsCharacterSimaYiMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaYiMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativePublicPileOriginalCardsCharacterSimaYiMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaYiMouRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativePublicPileCardRequest? MouSmPublicPileRequest(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind) => this._targetFactory().MouSmPublicPileRequest(actor, kind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeLegalActionsRuntime))]
internal sealed class CoreNativePublicPileOriginalCardsCoreNativeLegalActionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeLegalActionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativePublicPileOriginalCardsCoreNativeLegalActionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeLegalActionsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> BuildLegalActionsCore(global::CardGame.Core.CharacterState actor, bool includeProgramActions, int? selectedPhysicalCardId, global::CardGame.Core.Card? nativeOriginalCard, bool ignoreForcedHandSelection = false) => this._targetFactory().BuildLegalActionsCore(actor, includeProgramActions, selectedPhysicalCardId, nativeOriginalCard, ignoreForcedHandSelection);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeTargetLegalityRuntime))]
internal sealed class CoreNativePublicPileOriginalCardsCoreNativeTargetLegalityAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeTargetLegalityRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativePublicPileOriginalCardsCoreNativeTargetLegalityOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeTargetLegalityRuntime> _targetFactory = targetFactory;
    public bool HasZhuqueFan(global::CardGame.Core.CharacterState actor) => this._targetFactory().HasZhuqueFan(actor);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGongsunZanMouMobileRuntime))]
internal sealed class CoreNativePublicPileOriginalCardsCharacterGongsunZanMouMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGongsunZanMouMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativePublicPileOriginalCardsCharacterGongsunZanMouMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGongsunZanMouMobileRuntime> _targetFactory = targetFactory;
    public bool HasMouGszMobile { get => this._targetFactory().HasMouGszMobile; }

    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.NativePublicPileCardSelection> MouGszHuOriginalChoices(global::CardGame.Core.NativePublicPileCardRequest request) => this._targetFactory().MouGszHuOriginalChoices(request);
    public bool IsMouGszHuOriginalChoice(global::CardGame.Core.NativePublicPileCardRequest request, global::CardGame.Core.NativePublicPileCardSelection choice) => this._targetFactory().IsMouGszHuOriginalChoice(request, choice);
    public bool PrepareMouGszHuResponse(global::CardGame.Core.CharacterState provider, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardKind kind, global::CardGame.Core.CardConversionSource? conversion, int? actor = default) => this._targetFactory().PrepareMouGszHuResponse(provider, cards, kind, conversion, actor);
    public void PrepareMouGszHuUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().PrepareMouGszHuUse(use);
    public bool IsMouGszHuOriginalPayment(global::CardGame.Core.NativePublicPileCardPayment p) => this._targetFactory().IsMouGszHuOriginalPayment(p);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileSourcesRuntime))]
internal sealed class CoreNativePublicPileOriginalCardsCorePublicPileSourcesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileSourcesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativePublicPileOriginalCardsCorePublicPileSourcesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileSourcesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> PublicPileCards(global::CardGame.Core.PublicPersistentPileSource source) => this._targetFactory().PublicPileCards(source);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime))]
internal sealed class CoreNativePublicPileOriginalCardsCoreDyingCursorRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativePublicPileOriginalCardsCoreDyingCursorRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DyingFrame? ActiveDying { get => this._targetFactory().ActiveDying; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime))]
internal sealed class CoreNativePublicPileOriginalCardsCoreActiveNativeAttackQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativePublicPileOriginalCardsCoreActiveNativeAttackQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get => this._targetFactory().ActiveCardAttack; set => this._targetFactory().ActiveCardAttack = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CoreNativePublicPileOriginalCardsCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativePublicPileOriginalCardsCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime))]
internal sealed class CoreNativePublicPileOriginalCardsCoreNativeCardContinuationCursorsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativePublicPileOriginalCardsCoreNativeCardContinuationCursorsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NullificationWindowFrame? ActiveNullificationWindow { get => this._targetFactory().ActiveNullificationWindow; }
    public global::CardGame.Core.NativeOwners.FactionDefenseHandle? ActiveFactionDefense { get => this._targetFactory().ActiveFactionDefense; set => this._targetFactory().ActiveFactionDefense = value; }
    public global::CardGame.Core.NativeOwners.FactionCardRequestHandle? ActiveFactionCardRequest { get => this._targetFactory().ActiveFactionCardRequest; set => this._targetFactory().ActiveFactionCardRequest = value; }
    public global::CardGame.Core.NativeOwners.BorrowedSwordHandle? ActiveBorrowedSword { get => this._targetFactory().ActiveBorrowedSword; set => this._targetFactory().ActiveBorrowedSword = value; }
    public global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle? ActiveQinglongCrescentBlade { get => this._targetFactory().ActiveQinglongCrescentBlade; set => this._targetFactory().ActiveQinglongCrescentBlade = value; }
    public global::CardGame.Core.NativeOwners.DuelHandle? ActiveDuel { get => this._targetFactory().ActiveDuel; set => this._targetFactory().ActiveDuel = value; }
    public global::CardGame.Core.NativeOwners.GroupCardHandle? ActiveGroupCard { get => this._targetFactory().ActiveGroupCard; set => this._targetFactory().ActiveGroupCard = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreNativePublicPileOriginalCardsCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativePublicPileOriginalCardsCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeFrame(expectedFrameId, next);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuangZuMobileShiRuntime))]
internal sealed class CoreNativePublicPileOriginalCardsCharacterHuangZuMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangZuMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativePublicPileOriginalCardsCharacterHuangZuMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangZuMobileShiRuntime> _targetFactory = targetFactory;
    public bool IsShiHzOriginalUseChoice(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind) => this._targetFactory().IsShiHzOriginalUseChoice(owner, card, kind);
    public bool IsShiHzOriginalUseParent(global::CardGame.Core.CardUseFrame use) => this._targetFactory().IsShiHzOriginalUseParent(use);
}
