#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAdjacentRedTargetsRuntime))]
internal sealed class CoreCompletedCategoryUseProofCoreAdjacentRedTargetsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdjacentRedTargetsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCompletedCategoryUseProofCoreAdjacentRedTargetsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdjacentRedTargetsRuntime> _targetFactory = targetFactory;
    public bool IsAdjacentRedTargetFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.AdjacentRedTargetAddedEvent fact, global::System.Collections.Generic.IReadOnlyList<int> original, int? currentProofActor = default) => this._targetFactory().IsAdjacentRedTargetFact(use, fact, original, currentProofActor);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRecipientCategorySlashTargetsRuntime))]
internal sealed class CoreCompletedCategoryUseProofCoreRecipientCategorySlashTargetsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientCategorySlashTargetsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCompletedCategoryUseProofCoreRecipientCategorySlashTargetsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientCategorySlashTargetsRuntime> _targetFactory = targetFactory;
    public bool IsRecipientCategorySlashTargetFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.RecipientCategorySlashTargetsResolvedEvent fact, global::System.Collections.Generic.IReadOnlyList<int> before) => this._targetFactory().IsRecipientCategorySlashTargetFact(use, fact, before);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRoundGainedTrickTargetsRuntime))]
internal sealed class CoreCompletedCategoryUseProofCoreRoundGainedTrickTargetsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundGainedTrickTargetsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCompletedCategoryUseProofCoreRoundGainedTrickTargetsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundGainedTrickTargetsRuntime> _targetFactory = targetFactory;
    public bool IsRoundGainedTrickTargetFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.RoundGainedTrickTargetResolvedEvent fact, global::System.Collections.Generic.IReadOnlyList<int> originalTargets) => this._targetFactory().IsRoundGainedTrickTargetFact(use, fact, originalTargets);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterChengYuMouOlRuntime))]
internal sealed class CoreCompletedCategoryUseProofCharacterChengYuMouOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengYuMouOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCompletedCategoryUseProofCharacterChengYuMouOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengYuMouOlRuntime> _targetFactory = targetFactory;
    public bool IsMouLidanAppliedTargetFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.MouLidanTargetsAppliedEvent fact, global::System.Collections.Generic.IReadOnlyList<int> precedingTargets, out global::System.Collections.Generic.IReadOnlyList<int> resultTargets) => this._targetFactory().IsMouLidanAppliedTargetFact(use, fact, precedingTargets, out resultTargets);
    public bool IsMouLidanTargetSummary(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramCardUseTargetAddedEvent summary) => this._targetFactory().IsMouLidanTargetSummary(use, summary);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShortRangeSlashTargetsRuntime))]
internal sealed class CoreCompletedCategoryUseProofCoreShortRangeSlashTargetsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShortRangeSlashTargetsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCompletedCategoryUseProofCoreShortRangeSlashTargetsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShortRangeSlashTargetsRuntime> _targetFactory = targetFactory;
    public bool IsShortRangeSlashTargetFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ShortRangeSlashTargetResolvedEvent fact, global::System.Collections.Generic.IReadOnlyList<int> originalTargets) => this._targetFactory().IsShortRangeSlashTargetFact(use, fact, originalTargets);
    public bool IsShortRangeSlashRedirectFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramActualSlashTargetRedirectedEvent fact, global::System.Collections.Generic.IReadOnlyList<int> actualTargets) => this._targetFactory().IsShortRangeSlashRedirectFact(use, fact, actualTargets);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSunQuanOlMagicRuntime))]
internal sealed class CoreCompletedCategoryUseProofCharacterSunQuanOlMagicAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunQuanOlMagicRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCompletedCategoryUseProofCharacterSunQuanOlMagicOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunQuanOlMagicRuntime> _targetFactory = targetFactory;
    public bool IsMoQuanyuAddedTargetFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.MoQuanyuTargetAddedEvent fact) => this._targetFactory().IsMoQuanyuAddedTargetFact(use, fact);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuaManOlRuntime))]
internal sealed class CoreCompletedCategoryUseProofCharacterHuaManOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuaManOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCompletedCategoryUseProofCharacterHuaManOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuaManOlRuntime> _targetFactory = targetFactory;
    public bool IsHuaManSouyingMutationFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.SouyingNativeMutationEvent fact, global::System.Collections.Generic.IReadOnlyList<int> original) => this._targetFactory().IsHuaManSouyingMutationFact(use, fact, original);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterYangFuOlRuntime))]
internal sealed class CoreCompletedCategoryUseProofCharacterYangFuOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYangFuOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCompletedCategoryUseProofCharacterYangFuOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYangFuOlRuntime> _targetFactory = targetFactory;
    public bool HasBingzhongTargetTail(global::CardGame.Core.CardUseFrame use) => this._targetFactory().HasBingzhongTargetTail(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterBeiMiHuOlRuntime))]
internal sealed class CoreCompletedCategoryUseProofCharacterBeiMiHuOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterBeiMiHuOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCompletedCategoryUseProofCharacterBeiMiHuOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterBeiMiHuOlRuntime> _targetFactory = targetFactory;
    public bool ValidBeiMiHuTargetMutation(global::CardGame.Core.CardUseFrame use) => this._targetFactory().ValidBeiMiHuTargetMutation(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterYuanTanYuanShangOlRuntime))]
internal sealed class CoreCompletedCategoryUseProofCharacterYuanTanYuanShangOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYuanTanYuanShangOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCompletedCategoryUseProofCharacterYuanTanYuanShangOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYuanTanYuanShangOlRuntime> _targetFactory = targetFactory;
    public bool IsNeifaTargetsResolvedFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.NeifaTargetsResolvedEvent fact, global::System.Collections.Generic.IReadOnlyList<int> original) => this._targetFactory().IsNeifaTargetsResolvedFact(use, fact, original);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime))]
internal sealed class CoreCompletedCategoryUseProofCoreAlternatingBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCompletedCategoryUseProofCoreAlternatingBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory() => this._targetFactory().CompleteProgramEventHistory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineUniqueLeaderTrickTargetsRuntime))]
internal sealed class CoreCompletedCategoryUseProofCoreUniqueLeaderTrickTargetsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUniqueLeaderTrickTargetsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCompletedCategoryUseProofCoreUniqueLeaderTrickTargetsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUniqueLeaderTrickTargetsRuntime> _targetFactory = targetFactory;
    public bool IsUniqueLeaderTrickTargetFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.UniqueLeaderTrickTargetResolvedEvent fact, global::System.Collections.Generic.IReadOnlyList<int> originalTargets) => this._targetFactory().IsUniqueLeaderTrickTargetFact(use, fact, originalTargets);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetQuarantineRuntime))]
internal sealed class CoreCompletedCategoryUseProofCoreSoleTargetQuarantineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetQuarantineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCompletedCategoryUseProofCoreSoleTargetQuarantineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetQuarantineRuntime> _targetFactory = targetFactory;
    public bool IsSoleTargetReplacementFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.SoleTargetReplacedEvent fact, global::System.Collections.Generic.IReadOnlyList<int> before) => this._targetFactory().IsSoleTargetReplacementFact(use, fact, before);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CoreCompletedCategoryUseProofCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCompletedCategoryUseProofCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDesignatedExtraTargetsRuntime))]
internal sealed class CoreCompletedCategoryUseProofCoreDesignatedExtraTargetsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDesignatedExtraTargetsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCompletedCategoryUseProofCoreDesignatedExtraTargetsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDesignatedExtraTargetsRuntime> _targetFactory = targetFactory;
    public bool IsDesignatedExtraTargetFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.DesignatedExtraTargetResolvedEvent fact, global::System.Collections.Generic.IReadOnlyList<int> originalTargets) => this._targetFactory().IsDesignatedExtraTargetFact(use, fact, originalTargets);
    public bool IsDesignatedExtraTargetRedirectFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.DesignatedExtraTargetRedirectedEvent fact, global::System.Collections.Generic.IReadOnlyList<int> actualTargets) => this._targetFactory().IsDesignatedExtraTargetRedirectFact(use, fact, actualTargets);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiuPanOlRuntime))]
internal sealed class CoreCompletedCategoryUseProofCharacterLiuPanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuPanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCompletedCategoryUseProofCharacterLiuPanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuPanOlRuntime> _targetFactory = targetFactory;
    public bool IsPijingAppliedTargetFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.PijingTargetsAppliedEvent fact, global::System.Collections.Generic.IReadOnlyList<int> precedingTargets, out global::System.Collections.Generic.IReadOnlyList<int> resultTargets) => this._targetFactory().IsPijingAppliedTargetFact(use, fact, precedingTargets, out resultTargets);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterQianZhaoOlRuntime))]
internal sealed class CoreCompletedCategoryUseProofCharacterQianZhaoOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterQianZhaoOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCompletedCategoryUseProofCharacterQianZhaoOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterQianZhaoOlRuntime> _targetFactory = targetFactory;
    public bool HasWeifuTargetTail(global::CardGame.Core.CardUseFrame use) => this._targetFactory().HasWeifuTargetTail(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetBudgetRuntime))]
internal sealed class CoreCompletedCategoryUseProofCoreDamageTargetBudgetAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetBudgetRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCompletedCategoryUseProofCoreDamageTargetBudgetOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetBudgetRuntime> _targetFactory = targetFactory;
    public bool IsDamageBudgetResolvedFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.DamageTargetBudgetResolvedEvent fact, global::System.Collections.Generic.IReadOnlyList<int> before, int? currentProofActor = default) => this._targetFactory().IsDamageBudgetResolvedFact(use, fact, before, currentProofActor);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMaXiuMaTieOlRuntime))]
internal sealed class CoreCompletedCategoryUseProofCharacterMaXiuMaTieOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaXiuMaTieOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCompletedCategoryUseProofCharacterMaXiuMaTieOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaXiuMaTieOlRuntime> _targetFactory = targetFactory;
    public bool IsKenshangTargetsReplacedFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.SlashTargetsReplacedEvent fact) => this._targetFactory().IsKenshangTargetsReplacedFact(use, fact);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOverflowTargetCancellationObserversRuntime))]
internal sealed class CoreCompletedCategoryUseProofCoreOverflowTargetCancellationObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOverflowTargetCancellationObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCompletedCategoryUseProofCoreOverflowTargetCancellationObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOverflowTargetCancellationObserversRuntime> _targetFactory = targetFactory;
    public bool IsOverflowTargetCancellationFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.OverflowUseTargetsCanceledEvent fact, global::System.Collections.Generic.IReadOnlyList<int> beforeTargets) => this._targetFactory().IsOverflowTargetCancellationFact(use, fact, beforeTargets);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMaChengOlRuntime))]
internal sealed class CoreCompletedCategoryUseProofCharacterMaChengOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaChengOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCompletedCategoryUseProofCharacterMaChengOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaChengOlRuntime> _targetFactory = targetFactory;
    public bool IsChenglieAppliedTargetFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ChenglieTargetsAppliedEvent e, global::System.Collections.Generic.IReadOnlyList<int> precedingTargets, out global::System.Collections.Generic.IReadOnlyList<int> resultTargets) => this._targetFactory().IsChenglieAppliedTargetFact(use, e, precedingTargets, out resultTargets);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetRedirectRuntime))]
internal sealed class CoreCompletedCategoryUseProofCoreSoleTargetRedirectAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetRedirectRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCompletedCategoryUseProofCoreSoleTargetRedirectOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetRedirectRuntime> _targetFactory = targetFactory;
    public bool IsSoleTargetAfterProtectionRedirectFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.SoleTargetAfterProtectionRedirectedEvent fact, global::System.Collections.Generic.IReadOnlyList<int> before) => this._targetFactory().IsSoleTargetAfterProtectionRedirectFact(use, fact, before);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSameTypeActualUseAidRuntime))]
internal sealed class CoreCompletedCategoryUseProofCoreSameTypeActualUseAidAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSameTypeActualUseAidRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCompletedCategoryUseProofCoreSameTypeActualUseAidOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSameTypeActualUseAidRuntime> _targetFactory = targetFactory;
    public bool IsSameTypeAidTargetFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.SameTypeAidTargetAddedEvent fact) => this._targetFactory().IsSameTypeAidTargetFact(use, fact);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXunYuMobileGodRuntime))]
internal sealed class CoreCompletedCategoryUseProofCharacterXunYuMobileGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXunYuMobileGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCompletedCategoryUseProofCharacterXunYuMobileGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXunYuMobileGodRuntime> _targetFactory = targetFactory;
    public bool IsDinghanCancellationFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.DinghanTargetCanceledEvent e, global::System.Collections.Generic.IReadOnlyList<int> before) => this._targetFactory().IsDinghanCancellationFact(use, e, before);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShownEntityTurnPoliciesRuntime))]
internal sealed class CoreCompletedCategoryUseProofCoreShownEntityTurnPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShownEntityTurnPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCompletedCategoryUseProofCoreShownEntityTurnPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShownEntityTurnPoliciesRuntime> _targetFactory = targetFactory;
    public bool ShownEntityUseActorMatches(global::CardGame.Core.CardUseFrame use, int originalActor, int provider) => this._targetFactory().ShownEntityUseActorMatches(use, originalActor, provider);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTargetedOrdinaryTrickRuntime))]
internal sealed class CoreCompletedCategoryUseProofCoreTargetedOrdinaryTrickAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTargetedOrdinaryTrickRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCompletedCategoryUseProofCoreTargetedOrdinaryTrickOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTargetedOrdinaryTrickRuntime> _targetFactory = targetFactory;
    public bool IsTargetedOrdinaryTrickResolvedFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.TargetedOrdinaryTrickResolvedEvent fact, global::System.Collections.Generic.IReadOnlyList<int> before) => this._targetFactory().IsTargetedOrdinaryTrickResolvedFact(use, fact, before);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCuiLieAnniversaryRuntime))]
internal sealed class CoreCompletedCategoryUseProofCharacterCuiLieAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCuiLieAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCompletedCategoryUseProofCharacterCuiLieAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCuiLieAnniversaryRuntime> _targetFactory = targetFactory;
    public bool IsAnniversaryCuiLieTargetMutationFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.AnniversaryCuiLieUseChangedEvent e, global::System.Collections.Generic.IReadOnlyList<int> original, int? currentProofActor = default) => this._targetFactory().IsAnniversaryCuiLieTargetMutationFact(use, e, original, currentProofActor);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetProtectionRuntime))]
internal sealed class CoreCompletedCategoryUseProofCoreSoleTargetProtectionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetProtectionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCompletedCategoryUseProofCoreSoleTargetProtectionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetProtectionRuntime> _targetFactory = targetFactory;
    public bool HasSoleTargetProtectionCapability { get => this._targetFactory().HasSoleTargetProtectionCapability; }
}
