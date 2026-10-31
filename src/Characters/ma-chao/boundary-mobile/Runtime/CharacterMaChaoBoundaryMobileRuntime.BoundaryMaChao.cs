#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterMaChaoBoundaryMobileRuntime
{
    internal const string BoundaryMaChaoMobileTieqi = "boundary:tieqi-mobile";
    internal bool IsBoundaryMaChaoUnrespondable(long frameId) => _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is { } attack && attack.ResolutionId == frameId && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(attack.EffectiveCardKind ?? CardKind.Slash) && _coreCardAttackStateOperations.GetCardAttackState(frameId).JudgmentSuitDodgeRestriction is { } restriction && restriction.TargetSeat == attack.TargetSeat && _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramSlashSuitDiscardResolvedEvent>().Any(receipt => receipt.ProgramFrameId == restriction.ProgramFrameId && receipt.SkillId == BoundaryMaChaoMobileTieqi && receipt.OwnerSeat == attack.CardUserSeat && receipt.CardUseFrameId == frameId && receipt.TargetSeat == attack.TargetSeat && !receipt.PaymentCompleted);
}
