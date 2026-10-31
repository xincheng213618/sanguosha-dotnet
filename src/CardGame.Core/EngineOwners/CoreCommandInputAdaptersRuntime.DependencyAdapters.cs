#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardDeclarationsRuntime))]
internal sealed class CoreCommandInputAdaptersCoreCardDeclarationsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardDeclarationsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCommandInputAdaptersCoreCardDeclarationsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardDeclarationsRuntime> _targetFactory = targetFactory;
    public void AnswerCardDeclaration(global::CardGame.Core.PromptChoice choice) => this._targetFactory().AnswerCardDeclaration(choice);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessNativeUsesRuntime))]
internal sealed class CoreCommandInputAdaptersCoreRoundOnceCardlessNativeUsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessNativeUsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCommandInputAdaptersCoreRoundOnceCardlessNativeUsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessNativeUsesRuntime> _targetFactory = targetFactory;
    public bool TrySubmitRoundOnceCardlessPlay(global::CardGame.Core.PlayCardCommand command, out global::CardGame.Core.CommandResult result) => this._targetFactory().TrySubmitRoundOnceCardlessPlay(command, out result);
    public bool TryResolveRoundOnceCardlessForcedSlash(global::CardGame.Core.PromptChoice selected, bool advance) => this._targetFactory().TryResolveRoundOnceCardlessForcedSlash(selected, advance);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineJudgmentProgramsRuntime))]
internal sealed class CoreCommandInputAdaptersCoreJudgmentProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJudgmentProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCommandInputAdaptersCoreJudgmentProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJudgmentProgramsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CommandResult SubmitProgramJudgmentTriggerAnswer(int actorSeat, global::CardGame.Core.PromptId prompt, global::CardGame.Core.ChoiceId choice) => this._targetFactory().SubmitProgramJudgmentTriggerAnswer(actorSeat, prompt, choice);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiuZhangOlRuntime))]
internal sealed class CoreCommandInputAdaptersCharacterLiuZhangOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuZhangOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCommandInputAdaptersCharacterLiuZhangOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuZhangOlRuntime> _targetFactory = targetFactory;
    public bool IsZonghuAnswer(global::CardGame.Core.ChoiceId id) => this._targetFactory().IsZonghuAnswer(id);
    public global::CardGame.Core.CommandResult SubmitZonghuAnswer(int actor, global::CardGame.Core.PromptId prompt, global::CardGame.Core.ChoiceId id) => this._targetFactory().SubmitZonghuAnswer(actor, prompt, id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime))]
internal sealed class CoreCommandInputAdaptersCoreNativePromptBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCommandInputAdaptersCoreNativePromptBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CommandError? ValidateHumanPrompt(int actorSeat, global::CardGame.Core.DecisionKind expectedKind, global::CardGame.Core.PromptId? promptId, global::CardGame.Core.CommandErrorCode inactiveCode) => this._targetFactory().ValidateHumanPrompt(actorSeat, expectedKind, promptId, inactiveCode);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFactionRequestPaymentsRuntime))]
internal sealed class CoreCommandInputAdaptersCoreFactionRequestPaymentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFactionRequestPaymentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCommandInputAdaptersCoreFactionRequestPaymentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFactionRequestPaymentsRuntime> _targetFactory = targetFactory;
    public void ResolveFactionRequestCostChoice(global::CardGame.Core.PromptChoice selected, bool advanceToHumanBoundary) => this._targetFactory().ResolveFactionRequestCostChoice(selected, advanceToHumanBoundary);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeLegalActionsRuntime))]
internal sealed class CoreCommandInputAdaptersCoreNativeLegalActionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeLegalActionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCommandInputAdaptersCoreNativeLegalActionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeLegalActionsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> BuildLegalActions(global::CardGame.Core.CharacterState actor, bool includeProgramActions = true, int? selectedPhysicalCardId = default) => this._targetFactory().BuildLegalActions(actor, includeProgramActions, selectedPhysicalCardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessNativeResponsesRuntime))]
internal sealed class CoreCommandInputAdaptersCoreRoundOnceCardlessNativeResponsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessNativeResponsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCommandInputAdaptersCoreRoundOnceCardlessNativeResponsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessNativeResponsesRuntime> _targetFactory = targetFactory;
    public bool TryResolveRoundOnceCardlessResponse(global::CardGame.Core.PromptChoice choice, bool advance) => this._targetFactory().TryResolveRoundOnceCardlessResponse(choice, advance);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXiaoQiaoMouOlRuntime))]
internal sealed class CoreCommandInputAdaptersCharacterXiaoQiaoMouOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiaoQiaoMouOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCommandInputAdaptersCharacterXiaoQiaoMouOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiaoQiaoMouOlRuntime> _targetFactory = targetFactory;
    public bool BeginMouXqJueyanResponse(global::CardGame.Core.PromptChoice selected) => this._targetFactory().BeginMouXqJueyanResponse(selected);
    public bool BeginMouXqForcedSlash(global::CardGame.Core.PromptChoice choice) => this._targetFactory().BeginMouXqForcedSlash(choice);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramRepeatedJudgmentRuntime))]
internal sealed class CoreCommandInputAdaptersCoreProgramRepeatedJudgmentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramRepeatedJudgmentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCommandInputAdaptersCoreProgramRepeatedJudgmentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramRepeatedJudgmentRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CommandResult SubmitProgramRepeatJudgmentAnswer(global::CardGame.Core.PromptChoice choice) => this._targetFactory().SubmitProgramRepeatJudgmentAnswer(choice);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineJudgmentReplacementProgramsRuntime))]
internal sealed class CoreCommandInputAdaptersCoreJudgmentReplacementProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJudgmentReplacementProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCommandInputAdaptersCoreJudgmentReplacementProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJudgmentReplacementProgramsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CommandResult SubmitProgramJudgmentReplacementAnswer(int actorSeat, global::CardGame.Core.PromptId prompt, global::CardGame.Core.ChoiceId choice) => this._targetFactory().SubmitProgramJudgmentReplacementAnswer(actorSeat, prompt, choice);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRequestedDeckBasicsRuntime))]
internal sealed class CoreCommandInputAdaptersCoreRequestedDeckBasicsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRequestedDeckBasicsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCommandInputAdaptersCoreRequestedDeckBasicsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRequestedDeckBasicsRuntime> _targetFactory = targetFactory;
    public bool IsRequestedDeckBasicAnswer(global::CardGame.Core.ChoiceId id) => this._targetFactory().IsRequestedDeckBasicAnswer(id);
    public global::CardGame.Core.CommandResult SubmitRequestedDeckBasicAnswer(int actor, global::CardGame.Core.PromptId prompt, global::CardGame.Core.ChoiceId choice) => this._targetFactory().SubmitRequestedDeckBasicAnswer(actor, prompt, choice);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramDodgeResponsesRuntime))]
internal sealed class CoreCommandInputAdaptersCoreProgramDodgeResponsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramDodgeResponsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCommandInputAdaptersCoreProgramDodgeResponsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramDodgeResponsesRuntime> _targetFactory = targetFactory;
    public void BeginProgramDodgeResponse(global::CardGame.Core.PromptChoice choice, bool advanceToHumanBoundary) => this._targetFactory().BeginProgramDodgeResponse(choice, advanceToHumanBoundary);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineBaguaResponsesRuntime))]
internal sealed class CoreCommandInputAdaptersCoreBaguaResponsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineBaguaResponsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCommandInputAdaptersCoreBaguaResponsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineBaguaResponsesRuntime> _targetFactory = targetFactory;
    public void SkipBaguaResponseCore() => this._targetFactory().SkipBaguaResponseCore();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicOffersRuntime))]
internal sealed class CoreCommandInputAdaptersCoreDrawFundedDistinctBasicOffersAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicOffersRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCommandInputAdaptersCoreDrawFundedDistinctBasicOffersOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicOffersRuntime> _targetFactory = targetFactory;
    public bool TrySubmitDrawFundedDistinctBasicPlay(global::CardGame.Core.PlayCardCommand command, out global::CardGame.Core.CommandResult result) => this._targetFactory().TrySubmitDrawFundedDistinctBasicPlay(command, out result);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuoTuOlRuntime))]
internal sealed class CoreCommandInputAdaptersCharacterGuoTuOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoTuOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCommandInputAdaptersCharacterGuoTuOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoTuOlRuntime> _targetFactory = targetFactory;
    public bool IsWeijieAnswer(global::CardGame.Core.ChoiceId id) => this._targetFactory().IsWeijieAnswer(id);
    public global::CardGame.Core.CommandResult SubmitWeijieAnswer(int actor, global::CardGame.Core.PromptId prompt, global::CardGame.Core.ChoiceId id) => this._targetFactory().SubmitWeijieAnswer(actor, prompt, id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFriendDuelResponsesRuntime))]
internal sealed class CoreCommandInputAdaptersCoreFriendDuelResponsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFriendDuelResponsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCommandInputAdaptersCoreFriendDuelResponsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFriendDuelResponsesRuntime> _targetFactory = targetFactory;
    public void ResolveFriendDuelDiscard(global::CardGame.Core.PromptChoice supplied) => this._targetFactory().ResolveFriendDuelDiscard(supplied);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreCommandInputAdaptersCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCommandInputAdaptersCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void AdvanceOneStepCore() => this._targetFactory().AdvanceOneStepCore();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNextActualUseTargetAdjustmentsRuntime))]
internal sealed class CoreCommandInputAdaptersCoreNextActualUseTargetAdjustmentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNextActualUseTargetAdjustmentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCommandInputAdaptersCoreNextActualUseTargetAdjustmentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNextActualUseTargetAdjustmentsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CommandError? ValidateNextActualUseZhangba(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<int> ids, global::System.Collections.Generic.IReadOnlyList<int> targets) => this._targetFactory().ValidateNextActualUseZhangba(owner, ids, targets);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineChainedStateBasicConversionsRuntime))]
internal sealed class CoreCommandInputAdaptersCoreChainedStateBasicConversionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineChainedStateBasicConversionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCommandInputAdaptersCoreChainedStateBasicConversionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineChainedStateBasicConversionsRuntime> _targetFactory = targetFactory;
    public bool TrySubmitChainedStateBasicPlay(global::CardGame.Core.PlayCardCommand command, out global::CardGame.Core.CommandResult result) => this._targetFactory().TrySubmitChainedStateBasicPlay(command, out result);
    public bool IsChainedStateBasicAnswer(global::CardGame.Core.ChoiceId id) => this._targetFactory().IsChainedStateBasicAnswer(id);
    public global::CardGame.Core.CommandResult SubmitChainedStateBasicAnswer(int actorSeat, global::CardGame.Core.PromptId promptId, global::CardGame.Core.ChoiceId choiceId) => this._targetFactory().SubmitChainedStateBasicAnswer(actorSeat, promptId, choiceId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterKongShuOlRuntime))]
internal sealed class CoreCommandInputAdaptersCharacterKongShuOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterKongShuOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCommandInputAdaptersCharacterKongShuOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterKongShuOlRuntime> _targetFactory = targetFactory;
    public bool IsLeiluanSource(global::CardGame.Core.CardConversionSource? source) => this._targetFactory().IsLeiluanSource(source);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeOwnedCardQueriesRuntime))]
internal sealed class CoreCommandInputAdaptersCoreNativeOwnedCardQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeOwnedCardQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCommandInputAdaptersCoreNativeOwnedCardQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeOwnedCardQueriesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card? FindOwnedPlayableCard(global::CardGame.Core.CharacterState actor, int? cardId) => this._targetFactory().FindOwnedPlayableCard(actor, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAssistedFactionSlashRuntime))]
internal sealed class CoreCommandInputAdaptersCoreAssistedFactionSlashAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAssistedFactionSlashRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCommandInputAdaptersCoreAssistedFactionSlashOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAssistedFactionSlashRuntime> _targetFactory = targetFactory;
    public void ResolveAssistedFactionSlashChoice(global::CardGame.Core.PromptChoice selected, bool advanceToHumanBoundary) => this._targetFactory().ResolveAssistedFactionSlashChoice(selected, advanceToHumanBoundary);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuMengMouRuntime))]
internal sealed class CoreCommandInputAdaptersCharacterLuMengMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuMengMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCommandInputAdaptersCharacterLuMengMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuMengMouRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CommandResult SubmitYingboPromptAnswer(global::CardGame.Core.PromptChoice selected) => this._targetFactory().SubmitYingboPromptAnswer(selected);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroUsesRuntime))]
internal sealed class CoreCommandInputAdaptersCoreTieredRoundZeroUsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroUsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCommandInputAdaptersCoreTieredRoundZeroUsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroUsesRuntime> _targetFactory = targetFactory;
    public bool TrySubmitTieredRoundZeroPlay(global::CardGame.Core.PlayCardCommand command, out global::CardGame.Core.CommandResult result) => this._targetFactory().TrySubmitTieredRoundZeroPlay(command, out result);
    public bool TryResolveTieredRoundZeroForcedSlash(global::CardGame.Core.PromptChoice selected, bool advance) => this._targetFactory().TryResolveTieredRoundZeroForcedSlash(selected, advance);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime))]
internal sealed class CoreCommandInputAdaptersCorePhysicalOwnedCardQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCommandInputAdaptersCorePhysicalOwnedCardQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player) => this._targetFactory().GetHand(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTargetCardPreselectionRuntime))]
internal sealed class CoreCommandInputAdaptersCoreTargetCardPreselectionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTargetCardPreselectionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCommandInputAdaptersCoreTargetCardPreselectionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTargetCardPreselectionRuntime> _targetFactory = targetFactory;
    public long TargetHandMovementSequence(int seat) => this._targetFactory().TargetHandMovementSequence(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiuFengBoundaryRuntime))]
internal sealed class CoreCommandInputAdaptersCharacterLiuFengBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuFengBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCommandInputAdaptersCharacterLiuFengBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuFengBoundaryRuntime> _targetFactory = targetFactory;
    public bool IsBoundaryLiuFengQinlingAnswer(global::CardGame.Core.ChoiceId id) => this._targetFactory().IsBoundaryLiuFengQinlingAnswer(id);
    public global::CardGame.Core.CommandResult SubmitBoundaryLiuFengQinlingAnswer(int actor, global::CardGame.Core.PromptId prompt, global::CardGame.Core.ChoiceId id) => this._targetFactory().SubmitBoundaryLiuFengQinlingAnswer(actor, prompt, id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuanHaiOlRuntime))]
internal sealed class CoreCommandInputAdaptersCharacterGuanHaiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanHaiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCommandInputAdaptersCharacterGuanHaiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanHaiOlRuntime> _targetFactory = targetFactory;
    public bool IsXieweiSource(global::CardGame.Core.CardConversionSource source) => this._targetFactory().IsXieweiSource(source);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CoreCommandInputAdaptersCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCommandInputAdaptersCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroResponsesRuntime))]
internal sealed class CoreCommandInputAdaptersCoreTieredRoundZeroResponsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroResponsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCommandInputAdaptersCoreTieredRoundZeroResponsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroResponsesRuntime> _targetFactory = targetFactory;
    public void ResolveTieredRoundZeroResponse(global::CardGame.Core.PromptChoice choice, bool advance) => this._targetFactory().ResolveTieredRoundZeroResponse(choice, advance);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime))]
internal sealed class CoreCommandInputAdaptersCoreRecoveryReplacementAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCommandInputAdaptersCoreRecoveryReplacementOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CommandResult SubmitRecoveryReplacementAnswer(global::CardGame.Core.PromptChoice selected) => this._targetFactory().SubmitRecoveryReplacementAnswer(selected);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineExtraEffectForeignSelectionRuntime))]
internal sealed class CoreCommandInputAdaptersCoreExtraEffectForeignSelectionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExtraEffectForeignSelectionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCommandInputAdaptersCoreExtraEffectForeignSelectionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExtraEffectForeignSelectionRuntime> _targetFactory = targetFactory;
    public bool IsExtraForeignPublicAnswer(global::CardGame.Core.TargetCardSelectionFrame pending, global::CardGame.Core.PromptChoice selected) => this._targetFactory().IsExtraForeignPublicAnswer(pending, selected);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramParticipantPhaseOperationsRuntime))]
internal sealed class CoreCommandInputAdaptersCoreProgramParticipantPhaseOperationsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramParticipantPhaseOperationsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCommandInputAdaptersCoreProgramParticipantPhaseOperationsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramParticipantPhaseOperationsRuntime> _targetFactory = targetFactory;
    public bool IsTurnPhysicalUseForbidden(int seat, global::System.Collections.Generic.IReadOnlyList<int> ids) => this._targetFactory().IsTurnPhysicalUseForbidden(seat, ids);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreHumanNativeCommandsRuntime))]
internal sealed class CoreCommandInputAdaptersCoreHumanNativeCommandsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreHumanNativeCommandsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCommandInputAdaptersCoreHumanNativeCommandsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreHumanNativeCommandsRuntime> _targetFactory = targetFactory;
    public void HumanSelectGeneralCore(string generalId, bool advanceToHumanBoundary) => this._targetFactory().HumanSelectGeneralCore(generalId, advanceToHumanBoundary);
    public void HumanPlayCore(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action, bool advanceToHumanBoundary) => this._targetFactory().HumanPlayCore(actor, action, advanceToHumanBoundary);
    public void HumanUseEquipmentEffectCore(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind equipmentKind, global::System.Collections.Generic.IReadOnlyList<int> cardIds, global::System.Collections.Generic.IReadOnlyList<int> targetSeats, bool advanceToHumanBoundary) => this._targetFactory().HumanUseEquipmentEffectCore(actor, equipmentKind, cardIds, targetSeats, advanceToHumanBoundary);
    public void HumanEndPlayCore(bool advanceToHumanBoundary) => this._targetFactory().HumanEndPlayCore(advanceToHumanBoundary);
    public void HumanSelectGodFactionCore(string factionId, bool advanceToHumanBoundary) => this._targetFactory().HumanSelectGodFactionCore(factionId, advanceToHumanBoundary);
    public void HumanRequestFactionDefenseCore(bool advanceToHumanBoundary) => this._targetFactory().HumanRequestFactionDefenseCore(advanceToHumanBoundary);
    public void HumanRespondCore(bool useDodge, bool useBagua, int? requestedDodgeCardId, global::CardGame.Core.CardKind? requestedResponseCardKind, bool advanceToHumanBoundary) => this._targetFactory().HumanRespondCore(useDodge, useBagua, requestedDodgeCardId, requestedResponseCardKind, advanceToHumanBoundary);
    public void HumanFactionDefenseResponseCore(bool useDodge, bool useBagua, int? requestedCardId, bool advanceToHumanBoundary) => this._targetFactory().HumanFactionDefenseResponseCore(useDodge, useBagua, requestedCardId, advanceToHumanBoundary);
    public void HumanNullificationCore(bool useNullification, int? requestedNullificationCardId, bool advanceToHumanBoundary, global::CardGame.Core.CardConversionSource? selectedConversionSource = default) => this._targetFactory().HumanNullificationCore(useNullification, requestedNullificationCardId, advanceToHumanBoundary, selectedConversionSource);
    public void HumanHarvestCardCore(int cardId, bool advanceToHumanBoundary) => this._targetFactory().HumanHarvestCardCore(cardId, advanceToHumanBoundary);
    public void HumanTargetCardSelectionCore(int slot, bool advanceToHumanBoundary) => this._targetFactory().HumanTargetCardSelectionCore(slot, advanceToHumanBoundary);
    public void HumanFireAttackCardCore(int cardId, bool advanceToHumanBoundary) => this._targetFactory().HumanFireAttackCardCore(cardId, advanceToHumanBoundary);
    public void HumanFireAttackSkipCore(bool advanceToHumanBoundary) => this._targetFactory().HumanFireAttackSkipCore(advanceToHumanBoundary);
    public void HumanStoneAxeCore(global::System.Collections.Generic.IReadOnlyList<int> discardedCardIds, bool advanceToHumanBoundary) => this._targetFactory().HumanStoneAxeCore(discardedCardIds, advanceToHumanBoundary);
    public void HumanCixiongDoubleSwordsCore(global::CardGame.Core.PromptChoice selected, bool advanceToHumanBoundary) => this._targetFactory().HumanCixiongDoubleSwordsCore(selected, advanceToHumanBoundary);
    public void HumanQinglongCrescentBladeCore(global::CardGame.Core.PromptChoice selected, bool advanceToHumanBoundary) => this._targetFactory().HumanQinglongCrescentBladeCore(selected, advanceToHumanBoundary);
    public void HumanZhuqueFanCore(bool convertToFireSlash, bool advanceToHumanBoundary) => this._targetFactory().HumanZhuqueFanCore(convertToFireSlash, advanceToHumanBoundary);
    public void HumanIceSwordCore(global::CardGame.Core.PromptChoice selected, bool advanceToHumanBoundary) => this._targetFactory().HumanIceSwordCore(selected, advanceToHumanBoundary);
    public void HumanQilinBowCore(global::CardGame.Core.PromptChoice selected, bool advanceToHumanBoundary) => this._targetFactory().HumanQilinBowCore(selected, advanceToHumanBoundary);
    public void HumanSkipDiscardPolicyCore(bool useSkill, bool advanceToHumanBoundary) => this._targetFactory().HumanSkipDiscardPolicyCore(useSkill, advanceToHumanBoundary);
    public void HumanBorrowedSwordFactionSlashCore(bool advanceToHumanBoundary) => this._targetFactory().HumanBorrowedSwordFactionSlashCore(advanceToHumanBoundary);
    public void HumanBorrowedSwordResponseCore(bool useSlash, int? requestedSlashCardId, global::CardGame.Core.CardKind? requestedEffectiveKind, bool advanceToHumanBoundary) => this._targetFactory().HumanBorrowedSwordResponseCore(useSlash, requestedSlashCardId, requestedEffectiveKind, advanceToHumanBoundary);
    public void HumanZhangbaSlashResponseCore(global::System.Collections.Generic.IReadOnlyList<int> requestedCardIds, bool advanceToHumanBoundary) => this._targetFactory().HumanZhangbaSlashResponseCore(requestedCardIds, advanceToHumanBoundary);
    public void HumanProgramViewAsSlashResponseCore(global::System.Collections.Generic.IReadOnlyList<int> requestedCardIds, global::CardGame.Core.CardConversionSource source, bool advanceToHumanBoundary) => this._targetFactory().HumanProgramViewAsSlashResponseCore(requestedCardIds, source, advanceToHumanBoundary);
    public void HumanRequestFactionSlashCore(bool advanceToHumanBoundary) => this._targetFactory().HumanRequestFactionSlashCore(advanceToHumanBoundary);
    public void HumanSlashResponseCore(bool useSlash, int? requestedSlashCardId, global::CardGame.Core.CardKind? requestedResponseCardKind, bool advanceToHumanBoundary) => this._targetFactory().HumanSlashResponseCore(useSlash, requestedSlashCardId, requestedResponseCardKind, advanceToHumanBoundary);
    public void HumanFactionSlashResponseCore(bool useSlash, int? requestedCardId, global::CardGame.Core.CardKind? requestedEffectiveKind, bool advanceToHumanBoundary) => this._targetFactory().HumanFactionSlashResponseCore(useSlash, requestedCardId, requestedEffectiveKind, advanceToHumanBoundary);
    public void HumanDyingResponseCore(bool usePeach, int? requestedPeachCardId, bool useAlcohol, int? requestedAlcoholCardId, bool advanceToHumanBoundary, global::CardGame.Core.CardConversionSource? peachConversionSource = default, global::CardGame.Core.CardConversionSource? alcoholConversionSource = default) => this._targetFactory().HumanDyingResponseCore(usePeach, requestedPeachCardId, useAlcohol, requestedAlcoholCardId, advanceToHumanBoundary, peachConversionSource, alcoholConversionSource);
    public void HumanDyingProgramTriggerCore(global::CardGame.Core.PromptChoice selected, bool advanceToHumanBoundary) => this._targetFactory().HumanDyingProgramTriggerCore(selected, advanceToHumanBoundary);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuanShuJunOlRuntime))]
internal sealed class CoreCommandInputAdaptersCharacterHuanShuJunOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuanShuJunOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCommandInputAdaptersCharacterHuanShuJunOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuanShuJunOlRuntime> _targetFactory = targetFactory;
    public bool IsHuanpeiAnswer(global::CardGame.Core.ChoiceId id) => this._targetFactory().IsHuanpeiAnswer(id);
    public global::CardGame.Core.CommandResult SubmitHuanpeiAnswer(int actor, global::CardGame.Core.PromptId prompt, global::CardGame.Core.ChoiceId id) => this._targetFactory().SubmitHuanpeiAnswer(actor, prompt, id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineExtendedViewAsRuntime))]
internal sealed class CoreCommandInputAdaptersCoreExtendedViewAsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExtendedViewAsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCommandInputAdaptersCoreExtendedViewAsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExtendedViewAsRuntime> _targetFactory = targetFactory;
    public void ResolveExtendedViewAsResponse(global::CardGame.Core.PromptChoice choice) => this._targetFactory().ResolveExtendedViewAsResponse(choice);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreCommandInputAdaptersCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCommandInputAdaptersCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CommandResult SubmitProgramTriggerAnswer(int actorSeat, global::CardGame.Core.PromptId promptId, global::CardGame.Core.ChoiceId choiceId) => this._targetFactory().SubmitProgramTriggerAnswer(actorSeat, promptId, choiceId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMaXiuMaTieOlRuntime))]
internal sealed class CoreCommandInputAdaptersCharacterMaXiuMaTieOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaXiuMaTieOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCommandInputAdaptersCharacterMaXiuMaTieOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaXiuMaTieOlRuntime> _targetFactory = targetFactory;
    public bool TryBeginKenshangQinglongSlash(global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle pending, global::CardGame.Core.PromptChoice selected) => this._targetFactory().TryBeginKenshangQinglongSlash(pending, selected);
    public bool TryBeginKenshangBorrowedSlash(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending, global::CardGame.Core.PromptChoice selected) => this._targetFactory().TryBeginKenshangBorrowedSlash(pending, selected);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterChengJiChengCuiOlRuntime))]
internal sealed class CoreCommandInputAdaptersCharacterChengJiChengCuiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengJiChengCuiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCommandInputAdaptersCharacterChengJiChengCuiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengJiChengCuiOlRuntime> _targetFactory = targetFactory;
    public bool TryBeginTousuiQinglongSlash(global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle pending, global::CardGame.Core.PromptChoice selected) => this._targetFactory().TryBeginTousuiQinglongSlash(pending, selected);
    public bool TryBeginTousuiBorrowedSlash(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending, global::CardGame.Core.PromptChoice selected) => this._targetFactory().TryBeginTousuiBorrowedSlash(pending, selected);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicPaymentRuntime))]
internal sealed class CoreCommandInputAdaptersCoreDrawFundedDistinctBasicPaymentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicPaymentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCommandInputAdaptersCoreDrawFundedDistinctBasicPaymentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicPaymentRuntime> _targetFactory = targetFactory;
    public bool IsDrawFundedDistinctBasicAnswer(global::CardGame.Core.ChoiceId choice) => this._targetFactory().IsDrawFundedDistinctBasicAnswer(choice);
    public global::CardGame.Core.CommandResult SubmitDrawFundedDistinctBasicAnswer(int actorSeat, global::CardGame.Core.PromptId promptId, global::CardGame.Core.ChoiceId choiceId) => this._targetFactory().SubmitDrawFundedDistinctBasicAnswer(actorSeat, promptId, choiceId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSkillPromptsRuntime))]
internal sealed class CoreCommandInputAdaptersCoreSkillPromptsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillPromptsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCommandInputAdaptersCoreSkillPromptsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillPromptsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CommandResult SubmitPindianPromptAnswer(global::CardGame.Core.PromptChoice selected) => this._targetFactory().SubmitPindianPromptAnswer(selected);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreMatchSetupRuntime))]
internal sealed class CoreCommandInputAdaptersCoreMatchSetupAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchSetupRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCommandInputAdaptersCoreMatchSetupOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchSetupRuntime> _targetFactory = targetFactory;
    public void StartCore() => this._targetFactory().StartCore();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramParticipantPromptsRuntime))]
internal sealed class CoreCommandInputAdaptersCoreProgramParticipantPromptsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramParticipantPromptsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCommandInputAdaptersCoreProgramParticipantPromptsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramParticipantPromptsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? GetPendingDecisionForActor(int actorSeat) => this._targetFactory().GetPendingDecisionForActor(actorSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramTopReorderRuntime))]
internal sealed class CoreCommandInputAdaptersCoreProgramTopReorderAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramTopReorderRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCommandInputAdaptersCoreProgramTopReorderOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramTopReorderRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CommandResult SubmitProgramTopReorderAnswer(global::CardGame.Core.PromptChoice choice) => this._targetFactory().SubmitProgramTopReorderAnswer(choice);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CoreCommandInputAdaptersCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCommandInputAdaptersCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime))]
internal sealed class CoreCommandInputAdaptersCoreNativeCardContinuationCursorsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCommandInputAdaptersCoreNativeCardContinuationCursorsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.FactionDefenseHandle? ActiveFactionDefense { get => this._targetFactory().ActiveFactionDefense; set => this._targetFactory().ActiveFactionDefense = value; }
    public global::CardGame.Core.NullificationWindowFrame? ActiveNullificationWindow { get => this._targetFactory().ActiveNullificationWindow; }
    public global::CardGame.Core.TargetCardSelectionFrame? ActiveTargetCardSelection { get => this._targetFactory().ActiveTargetCardSelection; }
    public global::CardGame.Core.CardUseFrame? ActiveFireAttack { get => this._targetFactory().ActiveFireAttack; }
    public global::CardGame.Core.NativeOwners.StoneAxeHandle? ActiveStoneAxe { get => this._targetFactory().ActiveStoneAxe; set => this._targetFactory().ActiveStoneAxe = value; }
    public global::CardGame.Core.NativeOwners.CixiongDoubleSwordsHandle? ActiveCixiongDoubleSwords { get => this._targetFactory().ActiveCixiongDoubleSwords; set => this._targetFactory().ActiveCixiongDoubleSwords = value; }
    public global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle? ActiveQinglongCrescentBlade { get => this._targetFactory().ActiveQinglongCrescentBlade; set => this._targetFactory().ActiveQinglongCrescentBlade = value; }
    public global::CardGame.Core.NativeOwners.FactionCardRequestHandle? ActiveFactionCardRequest { get => this._targetFactory().ActiveFactionCardRequest; set => this._targetFactory().ActiveFactionCardRequest = value; }
    public global::CardGame.Core.NativeOwners.IceSwordHandle? ActiveIceSword { get => this._targetFactory().ActiveIceSword; set => this._targetFactory().ActiveIceSword = value; }
    public global::CardGame.Core.NativeOwners.QilinBowHandle? ActiveQilinBow { get => this._targetFactory().ActiveQilinBow; set => this._targetFactory().ActiveQilinBow = value; }
    public global::CardGame.Core.NativeOwners.BorrowedSwordHandle? ActiveBorrowedSword { get => this._targetFactory().ActiveBorrowedSword; set => this._targetFactory().ActiveBorrowedSword = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime))]
internal sealed class CoreCommandInputAdaptersCoreCardConversionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCommandInputAdaptersCoreCardConversionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime> _targetFactory = targetFactory;
    public void CaptureSelectedResponseConversion(global::CardGame.Core.PromptChoice choice) => this._targetFactory().CaptureSelectedResponseConversion(choice);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOpeningCustomizationRuntime))]
internal sealed class CoreCommandInputAdaptersCoreOpeningCustomizationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOpeningCustomizationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCommandInputAdaptersCoreOpeningCustomizationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOpeningCustomizationRuntime> _targetFactory = targetFactory;
    public void ResolveOpeningHandChoice(global::CardGame.Core.PromptChoice selected) => this._targetFactory().ResolveOpeningHandChoice(selected);
}
