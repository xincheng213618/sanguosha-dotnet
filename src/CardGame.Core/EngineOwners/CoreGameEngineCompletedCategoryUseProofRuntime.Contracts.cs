#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreCompletedCategoryUseProofCoreAdjacentRedTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsAdjacentRedTargetFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.AdjacentRedTargetAddedEvent fact, global::System.Collections.Generic.IReadOnlyList<int> original, int? currentProofActor = default);
}

internal interface ICoreCompletedCategoryUseProofCoreRecipientCategorySlashTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsRecipientCategorySlashTargetFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.RecipientCategorySlashTargetsResolvedEvent fact, global::System.Collections.Generic.IReadOnlyList<int> before);
}

internal interface ICoreCompletedCategoryUseProofCoreRoundGainedTrickTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsRoundGainedTrickTargetFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.RoundGainedTrickTargetResolvedEvent fact, global::System.Collections.Generic.IReadOnlyList<int> originalTargets);
}

internal interface ICoreCompletedCategoryUseProofCharacterChengYuMouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouLidanAppliedTargetFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.MouLidanTargetsAppliedEvent fact, global::System.Collections.Generic.IReadOnlyList<int> precedingTargets, out global::System.Collections.Generic.IReadOnlyList<int> resultTargets);
    bool IsMouLidanTargetSummary(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramCardUseTargetAddedEvent summary);
}

internal interface ICoreCompletedCategoryUseProofCoreShortRangeSlashTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsShortRangeSlashTargetFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ShortRangeSlashTargetResolvedEvent fact, global::System.Collections.Generic.IReadOnlyList<int> originalTargets);
    bool IsShortRangeSlashRedirectFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramActualSlashTargetRedirectedEvent fact, global::System.Collections.Generic.IReadOnlyList<int> actualTargets);
}

internal interface ICoreCompletedCategoryUseProofCharacterSunQuanOlMagicOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMoQuanyuAddedTargetFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.MoQuanyuTargetAddedEvent fact);
}

internal interface ICoreCompletedCategoryUseProofCharacterHuaManOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsHuaManSouyingMutationFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.SouyingNativeMutationEvent fact, global::System.Collections.Generic.IReadOnlyList<int> original);
}

internal interface ICoreCompletedCategoryUseProofCharacterYangFuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasBingzhongTargetTail(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreCompletedCategoryUseProofCharacterBeiMiHuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ValidBeiMiHuTargetMutation(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreCompletedCategoryUseProofCharacterYuanTanYuanShangOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsNeifaTargetsResolvedFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.NeifaTargetsResolvedEvent fact, global::System.Collections.Generic.IReadOnlyList<int> original);
}

internal interface ICoreCompletedCategoryUseProofCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreCompletedCategoryUseProofCoreUniqueLeaderTrickTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsUniqueLeaderTrickTargetFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.UniqueLeaderTrickTargetResolvedEvent fact, global::System.Collections.Generic.IReadOnlyList<int> originalTargets);
}

internal interface ICoreCompletedCategoryUseProofContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreCompletedCategoryUseProofCoreSoleTargetQuarantineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSoleTargetReplacementFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.SoleTargetReplacedEvent fact, global::System.Collections.Generic.IReadOnlyList<int> before);
}

internal interface ICoreCompletedCategoryUseProofCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreCompletedCategoryUseProofCoreDesignatedExtraTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDesignatedExtraTargetFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.DesignatedExtraTargetResolvedEvent fact, global::System.Collections.Generic.IReadOnlyList<int> originalTargets);
    bool IsDesignatedExtraTargetRedirectFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.DesignatedExtraTargetRedirectedEvent fact, global::System.Collections.Generic.IReadOnlyList<int> actualTargets);
}

internal interface ICoreCompletedCategoryUseProofCharacterLiuPanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsPijingAppliedTargetFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.PijingTargetsAppliedEvent fact, global::System.Collections.Generic.IReadOnlyList<int> precedingTargets, out global::System.Collections.Generic.IReadOnlyList<int> resultTargets);
}

internal interface ICoreCompletedCategoryUseProofCharacterQianZhaoOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWeifuTargetTail(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreCompletedCategoryUseProofCoreDamageTargetBudgetOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDamageBudgetResolvedFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.DamageTargetBudgetResolvedEvent fact, global::System.Collections.Generic.IReadOnlyList<int> before, int? currentProofActor = default);
}

internal interface ICoreCompletedCategoryUseProofCharacterMaXiuMaTieOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsKenshangTargetsReplacedFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.SlashTargetsReplacedEvent fact);
}

internal interface ICoreCompletedCategoryUseProofCoreOverflowTargetCancellationObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsOverflowTargetCancellationFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.OverflowUseTargetsCanceledEvent fact, global::System.Collections.Generic.IReadOnlyList<int> beforeTargets);
}

internal interface ICoreCompletedCategoryUseProofCharacterMaChengOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsChenglieAppliedTargetFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ChenglieTargetsAppliedEvent e, global::System.Collections.Generic.IReadOnlyList<int> precedingTargets, out global::System.Collections.Generic.IReadOnlyList<int> resultTargets);
}

internal interface ICoreCompletedCategoryUseProofCoreSoleTargetRedirectOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSoleTargetAfterProtectionRedirectFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.SoleTargetAfterProtectionRedirectedEvent fact, global::System.Collections.Generic.IReadOnlyList<int> before);
}

internal interface ICoreCompletedCategoryUseProofCoreSameTypeActualUseAidOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSameTypeAidTargetFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.SameTypeAidTargetAddedEvent fact);
}

internal interface ICoreCompletedCategoryUseProofCharacterXunYuMobileGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDinghanCancellationFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.DinghanTargetCanceledEvent e, global::System.Collections.Generic.IReadOnlyList<int> before);
}

internal interface ICoreCompletedCategoryUseProofCoreShownEntityTurnPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ShownEntityUseActorMatches(global::CardGame.Core.CardUseFrame use, int originalActor, int provider);
}

internal interface ICoreCompletedCategoryUseProofCoreTargetedOrdinaryTrickOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsTargetedOrdinaryTrickResolvedFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.TargetedOrdinaryTrickResolvedEvent fact, global::System.Collections.Generic.IReadOnlyList<int> before);
}

internal interface ICoreCompletedCategoryUseProofCharacterCuiLieAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsAnniversaryCuiLieTargetMutationFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.AnniversaryCuiLieUseChangedEvent e, global::System.Collections.Generic.IReadOnlyList<int> original, int? currentProofActor = default);
}

internal interface ICoreCompletedCategoryUseProofCoreSoleTargetProtectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasSoleTargetProtectionCapability { get; }
}
