#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoPiMouRuntime
{
    internal bool HasMouXingshang(CharacterState owner) => _coreNationalSkillsOperations.HasRuntimeSkill(owner, "mou:xingshang");
    internal static bool HasPrintedMouXingshang(CharacterState owner) => owner.General.Skills.Concat(owner.SecondaryGeneral?.Skills ?? []).Any(s => s.ContentId == "mou:xingshang");
    internal string[] MouCaoPiPrintedSkills(CharacterState dead) => dead.General.Skills.Concat(dead.SecondaryGeneral?.Skills ?? []).Where(s => s.ContentId is { } id && !_contentRegistryOperations._contentRegistry.GetSkill(id).Tags.HasFlag(SkillTag.Lord)).Select(s => s.ContentId!).Distinct(StringComparer.Ordinal).ToArray();
    [ProgramNativeDrawReturn(SkillProgramEffectOp.MouCaoPiXingshang)]
    internal void ReturnMouCaoPiDraw(ProgramSkillFrame f, NativeDrawOutcome outcome)
    {
        if (f.MouCaoPi is not { Stage: MouCaoPiStage.DrawRequest, Option: "draw" } r || f.NativeDraw is not null || outcome.RecipientSeat != r.TargetSeat || outcome.RequestedCount != 3 || outcome.ActualDrawIds.Count > 3 || outcome.SequenceBefore < r.MovementBefore || outcome.SequenceAfter < outcome.SequenceBefore || outcome.Replacement is not null && outcome.ActualDrawIds.Count != 0)
            throw new InvalidOperationException("Xingshang lost its exact paid draw request or confused replacement gains with drawn cards.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouCaoPi = r with { Stage = MouCaoPiStage.DrawChildren, MovementBefore = outcome.SequenceBefore, MovementAfter = outcome.SequenceAfter, ActualDraw = outcome.ActualDrawIds.Count } });
        ContinueMouCaoPi(f.Id);
    }
}
