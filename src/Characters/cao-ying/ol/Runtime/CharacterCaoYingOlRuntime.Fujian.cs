#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoYingOlRuntime
{
    [ProgramViewContributor(ProgramViewContribution.PrivateCards, 30)]
    internal IEnumerable<CardSnapshot> GetCaoYingPrivateCards(int viewer) => _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Where(f => f.OwnerSeat == viewer && f.CaoYing?.Stage == CaoYingStage.Inspection).SelectMany(f => f.CaoYing!.Cards);
}
