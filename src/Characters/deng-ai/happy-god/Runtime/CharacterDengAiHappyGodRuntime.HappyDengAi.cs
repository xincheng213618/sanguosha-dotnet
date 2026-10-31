#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDengAiHappyGodRuntime
{
    internal const string HappyDaTuoYu = "happy:tuoyu", HappyDaXianjin = "happy:xianjin", HappyDaQijing = "happy:qijing", HappyDaCuixin = "happy:cuixin";
    internal static bool IsShenDaTuoYu(string id) => id is "anniversary-god:tuoyu" or HappyDaTuoYu;
    internal static bool IsShenDaXianjin(string id) => id is "anniversary-god:xianjin" or HappyDaXianjin;
    internal static bool IsShenDaQijing(string id) => id is "anniversary-god:qijing" or HappyDaQijing;
    internal static bool IsShenDaCuixin(string id) => id is "anniversary-god:cuixin" or HappyDaCuixin;
    internal string? ShenDaTuoYuId(CharacterState owner) => _coreNationalSkillsOperations.HasRuntimeSkill(owner, "anniversary-god:tuoyu") ? "anniversary-god:tuoyu" : _coreNationalSkillsOperations.HasRuntimeSkill(owner, HappyDaTuoYu) ? HappyDaTuoYu : null;
    internal int[] ShenDaAllocationCards(ProgramSkillFrame frame) => frame.SkillId != HappyDaTuoYu ? _characterDengAiAnniversaryGodOperations.ShenDaUnassigned(frame.OwnerSeat) : _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]).Select(c => c.Id).Except(_coreProgramEventHistoryOperations.ProgramEventHistory<ShenDengAiAssignedEvent>().Where(e => e.FrameId == frame.Id && e.OwnerSeat == frame.OwnerSeat).Select(e => e.CardId)).ToArray();
}
