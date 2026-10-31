#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreActiveNativeAttackQueriesRuntime
{
    internal global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack
    {
        get => _resolutionStackOperations._resolutionStack.LastOrDefault(frame => frame switch
        {
            CardUseFrame use => use.CardAttack?.Active == true && !_characterXiaoQiaoMouOlOperations.MouXiaoQiaoOl.PublicRevealSuspends(use.Id) && !_characterGuoSiOlOperations.IsSuspendedGuoSiCompletedParent(use) && !_characterDiaoChanLeOperations.IsDiouSuspendedRescueAttack(use) && !_characterDiaoChanLeOperations.IsDiouSuspendedCompletedAttack(use) && !_characterLiuChengMouMobileOperations.IsMouLiuChengSuspendedCompletedAttack(use),
            ProgramSkillFrame program => program.CardAttack?.Active == true && !_characterXiaoQiaoMouOlOperations.MouXiaoQiaoOl.PublicRevealSuspends(program.Id) && !_characterDiaoChanLeOperations.IsDiouSuspendedRescueAttack(program),
            JudgmentFrame judgment => judgment.CardAttack?.Active == true && !_characterXiaoQiaoMouOlOperations.MouXiaoQiaoOl.PublicRevealSuspends(judgment.Id) && !_characterDiaoChanLeOperations.IsDiouSuspendedRescueAttack(judgment),
            _ => false
        })is { } owner ? _kernelCoreMainOperations.CreateCardAttackHandle(owner.Id) : null;
        set
        {
            global::CardGame.Core.NativeOwners.CardAttackHandle current = ActiveCardAttack;
            if (current is not null && current.ResolutionId != value?.ResolutionId)
                _coreCardAttackStateOperations.UpdateCardAttackState(current.ResolutionId, state => state!with { Active = false });
            if (value is not null)
                _coreCardAttackStateOperations.UpdateCardAttackState(value.ResolutionId, state => state!with { Active = true });
        }
    }
}
