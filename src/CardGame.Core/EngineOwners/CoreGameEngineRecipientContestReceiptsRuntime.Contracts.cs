#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreRecipientContestReceiptsCharacterYangFuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasBingzhongTargetTail(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreRecipientContestReceiptsCoreActualTurnDamageClaimsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool MatchesDeclaredActualDamageUse(global::CardGame.Core.CardUseFrame use, int actor, int provider);
}

internal interface ICoreRecipientContestReceiptsCoreCurrentSlashFireOffersOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCurrentSlashFireChangedUse(global::CardGame.Core.CardUseFrame use);
    void AssertCurrentSlashFirePolicy(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreRecipientContestReceiptsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreRecipientContestReceiptsCoreRecipientContestsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int[] RecipientContestThirds(global::CardGame.Core.ProgramSkillFrame f);
    bool RecipientContestResultMatches(global::CardGame.Core.ProgramSkillFrame f, out global::CardGame.Core.PindianResult result);
}

internal interface ICoreRecipientContestReceiptsCharacterLiuPanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasPijingTargetTail(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreRecipientContestReceiptsCharacterMaWanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasMaWanHunjiangTargetTail(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreRecipientContestReceiptsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreRecipientContestReceiptsCharacterQianZhaoOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWeifuTargetTail(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreRecipientContestReceiptsCoreEphemeralCardMaterialsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsEphemeralCardDestroyedMaterial(global::CardGame.Core.CardMovementRecord movement);
}

internal interface ICoreRecipientContestReceiptsCharacterMaXiuMaTieOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasKenshangTargetTail(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreRecipientContestReceiptsCharacterMaChengOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasChenglieTargetTail(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreRecipientContestReceiptsCoreOriginalTargetAdditionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasIssuedOriginalTargetAdditionTail(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreRecipientContestReceiptsCoreShortRangeSlashTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasShortRangeSlashTail(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreRecipientContestReceiptsCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreRecipientContestReceiptsCoreSameTypeActualUseAidOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasSameTypeAidTargetTail(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreRecipientContestReceiptsCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreRecipientContestReceiptsCoreUniqueHpPeerProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardConversionSource RecipientContestSource(global::CardGame.Core.ProgramSkillFrame f);
    bool ExactUniqueHpPeerParent(global::CardGame.Core.ProgramSkillFrame f);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> UniqueHpPeerChoices(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreRecipientContestReceiptsCharacterZhangYanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasZhangYanLangdaoTargetTail(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreRecipientContestReceiptsCoreAdvancedLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetAdvancedCard(int id);
}

internal interface ICoreRecipientContestReceiptsCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}
