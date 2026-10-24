# 移动版界魏延

核对日期：2026-10-25。独立人物 `boundary:wei-yan-mobile`，男性、蜀势力、4体力；技能 `boundary:kuanggu-mobile`、`boundary:qimou-mobile`。

## 来源与版本边界

- [移动版官网：界·魏延](https://www.sanguosha.cn/index.php/pc/hero-detail-308.html)：身份页完整列出两项技能。狂骨逐点结算，使用受到该次伤害扣减体力以前的距离；奇谋失去体力后，距离减少及额外杀均持续到回合结束，没有摸牌效果。该页另列的国战部分不包含奇谋，不用于本身份人物。
- [移动版 Wiki：界魏延](https://wiki.biligame.com/msgs/%E7%95%8C%E9%AD%8F%E5%BB%B6)：交叉确认4体力、蜀势力、男性及狂骨的逐点/扣体力前距离规则；规则问答说明奇谋不能选择超过当前体力的数量。该页将额外杀写作“本阶段”，与当前官网身份正文的回合结束期限不同，本实现遵循官网当前完整正文，两个权益使用同一实际回合的有效期。
- 本仓库 OL 界版 `boundary:wei-yan` 的奇谋包含额外摸X，本移动版本独立登记，不能直接套用该摸牌步骤。经典狂骨的锁定回复、势版狂骨及其升级不作为本版技能。

## 原生能力复用

狂骨完全使用既有 `afterDamageApplied`、`damageOccurrence:perDamagePoint`、`sourceToTargetDistanceAtDamage`、选项分支及原生回复/摸牌。距离由伤害流水线在扣减体力之前冻结，不在伤害后根据新体力、死亡或装备移除重新计算。每一伤害点独立选择；满体力时回复选项不可用，摸牌仍可选。正常伤害、技能伤害及原生连环/转移继续遵循该同一伤害来源与实际目标协议。

奇谋使用既有的四步 paid-HP-loss 协议：选择本人体力数量、处理摸牌阶段、发行距离权益、发行杀次数权益。第二节点显式 `drawCards:false`，仅完成原付款续接，不产生摸牌。数量为1至当前体力，可以选择全部当前体力并进入真实濒死；由原母帧保留实际支付的X，救援后不会改成回复后的当前体力或已损失体力。

两项权益均由既有回合效果仓库发行，绑定原帧、原技能实例、实际HP付款账单及原回合；使用一次的限定额度与冷恢复都复用该共享机制。没有新增运行时、枚举、独立pending、回放字段或规则版本。

## 验证范围

新增一条 `BoundaryWeiYanMobileChecks.NoDrawPaidLossSurvivesRescueColdAndKeepsActualXForPointBenefits`，针对此前没有实际使用的“不摸牌”付款分支：真实支付全部体力、濒死读档后使用原桃救援、按原X发行两项权益；随后一次两点伤害分别回复和摸牌，原生使用恰好基础一次加X次杀，并在回合结束恢复原距离。

共用付款、输入和既有 OL 摸牌路径继续由 `Paid HP loss quantity and native` 覆盖；逐伤害点回复/摸牌的共享整合可复用 `Shanhe Yaoli: Zaiqi Kuanggu blood blade and recovery draw one payment cold`。不为相同DSL另造定义快照或隐私/非法输入矩阵。
