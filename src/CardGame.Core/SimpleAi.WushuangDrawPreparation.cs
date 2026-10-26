namespace CardGame.Core;

public sealed partial class SimpleAiBrain
{
    private (double Score, string Reason) ScoreWushuangDrawPreparation(GameSnapshot view, PlayerSnapshot self,
        Role role, LegalAction action, IReadOnlyList<LegalAction> legalActions, (double Score, string Reason) baseline,
        Func<LegalAction, int, PublicDamagePrediction>? damagePrediction)
    {
        if (damagePrediction is null || view.CurrentSeat != self.Seat || self.HasAlcoholEffect || self.HpUndefined ||
            self.Hp <= 0 || view.Players.Count(player => player.IsAlive) != 2 ||
            (long)view.DrawPileCount + view.DiscardPileCount < 2 ||
            action.CardId is not > 0 || action.ConversionSource is not null ||
            action.AdditionalConversionSources is { Count: > 0 } ||
            action.TargetSeats.Any(seat => seat != self.Seat) ||
            !self.Hand.Any(card => card.Id == action.CardId && card.Kind == CardKind.DrawTwo) ||
            self.Hp <= 2 && legalActions.Any(candidate => candidate.Kind == LegalActionKind.Peach)) return baseline;

        var target = view.Players.Single(player => player.IsAlive && player.Seat != self.Seat);
        if (GetHostility(view, role, target) <= 15) return baseline;
        var best = double.NegativeInfinity;
        foreach (var candidate in legalActions)
        {
            // Preserve immediate public finishing opportunities, including a
            // currently published skill. This only inspects this decision's
            // actions; it does not construct a future Slash from an unknown draw.
            var direct = candidate.Kind is LegalActionKind.Slash or LegalActionKind.Duel or
                LegalActionKind.FireAttack or LegalActionKind.BarbarianAssault or LegalActionKind.ArrowBarrage;
            var targetsEnemy = candidate.TargetSeats.Contains(target.Seat) ||
                candidate.Kind is LegalActionKind.BarbarianAssault or LegalActionKind.ArrowBarrage;
            var skillLethal = candidate.Kind == LegalActionKind.UseProgramSkill && candidate.ProgramAiHint is { } hint &&
                candidate.SelectableTargetSeats.Contains(target.Seat) &&
                hint.PredictTarget(target.Hp, target.MaxHp, target.Armor, target.IncomingDamageCap) is
                    { IsLethal: true, HpLossAmount: > 0 };
            var prediction = direct && targetsEnemy ? damagePrediction(candidate, target.Seat) : null;
            var payment = prediction?.Wushuang?.TargetPayment ?? prediction;
            if ((payment?.IsLethal == true || skillLethal) &&
                ScoreTacticalAction(view, self, role, candidate, legalActions, damagePrediction).Score > 0) return baseline;

            if (candidate.Kind != LegalActionKind.Slash || candidate.CardId == action.CardId ||
                candidate.ConversionSource is not null || candidate.AdditionalConversionSources is { Count: > 0 } ||
                candidate.TargetSeats is not [var seat] || seat != target.Seat || candidate.CardId is not > 0 ||
                !self.Hand.Any(card => card.Id == candidate.CardId && IsSlashCard(card.Kind)) ||
                prediction?.Wushuang is not { RemainingOrdinarySlashUses: 1 } combat ||
                combat.TargetPayment is not { IsLethal: false, HpLossAmount: > 0 } ||
                combat.RetaliationIfTargetSurvives is { IsLethal: true }) continue;
            var current = ScorePlanningAction(view, self, role, candidate, legalActions,
                ScoreTacticalAction(view, self, role, candidate, legalActions, damagePrediction), damagePrediction).Score;
            if (current > 0) best = Math.Max(best, current);
        }
        var reserve = Math.Max(0, ScoreTacticalAction(view, self, role, action, legalActions, damagePrediction).Score - baseline.Score);
        if (!double.IsFinite(best) || reserve >= 6 || best + 6 - reserve <= baseline.Score) return baseline;
        // A small ordering preference stays below existing wine (+8) and
        // certain equipment-lethal preparation. Drawn identities are unknown.
        return (best + 6 - reserve, baseline.Reason +
            " 单骑无双：当前仅余一次普通出杀，已有独立、非终结的合法杀，先用无中扩展本回合选择，再按实际摸牌重新决定，不预知牌堆。");
    }
}
