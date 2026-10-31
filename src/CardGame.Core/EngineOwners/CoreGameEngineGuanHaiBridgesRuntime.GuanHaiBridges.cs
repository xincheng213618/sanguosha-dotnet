#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineGuanHaiBridgesRuntime
{
    internal Suit? ProgramMultiCardSuit(CharacterState owner, ProgramMultiCardViewAsSelection selection) => _characterGuanHaiOlOperations.IsXieweiSelection(selection) || _coreWushuangJinzhiOperations.IsWushuangJinzhiSelection(selection) ? Suit.None : _coreAlternatingSuitAndCompletedTopOperations.PhysicalGroupSuit(owner, selection.Cards);
    internal bool? ProgramMultiCardColor(CharacterState owner, ProgramMultiCardViewAsSelection selection) => _characterGuanHaiOlOperations.IsXieweiSelection(selection) || _coreWushuangJinzhiOperations.IsWushuangJinzhiSelection(selection) ? null : _coreTurnRedSlashBenefitsOperations.PhysicalGroupColor(owner, selection.Cards);
}
