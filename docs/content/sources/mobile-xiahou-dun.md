# 夏侯惇·移动版

核查日期：2026-10-26。普通移动身份，魏、男、4体力、标准包。新增人物资料，完整引用 `classic:ganglie`，不新增技能副本、运行时或 opcode。

来源：[移动官网17](https://www.sanguosha.cn/pc/hero-detail-17.html)、[移动BWIKI人物35870](https://wiki.biligame.com/msgs/index.php?title=夏侯惇&oldid=35870)（2026-01-11）、[普通刚烈20131](https://wiki.biligame.com/msgs/index.php?title=刚烈/夏侯惇&oldid=20131)（2022-12-01）。已通过 MediaWiki revisions API 读取原始 wikitext，人物修订确认普通经典标准包、开服上线、13级获取；技能修订的经典栏与当前官网同品普通身份正文一致。

普通刚烈在受到一次伤害后可进行一次判定；非红桃结果才让该伤害的原来源选择弃置两张手牌，或受到夏侯惇造成的一点普通伤害。不是每点伤害判定，不是红色与黑色分支，也不把装备牌计入弃两张手牌的费用。判定红桃不惩罚来源，无来源伤害仍可发动判定但没有虚构的惩罚参与者。

现有 `classic:ganglie` 的实际规则逐项符合：`afterDamageApplied`、`subject:owner`、`damageOccurrence:perDamage`；先执行真实公开判定，结果仅黑桃、梅花、方块且 `eventSourceExists` 时由 `eventSource` 选择。弃牌项要求该选择者至少两张手牌，随后从原来源的 `hand` 选两张原实体并弃置；伤害项将 `sourceRef:owner`、`targetRef:eventSource`、一点普通伤害交给原生操作。判定牌和弃牌均按原绑定移动并沿原帧续接，没有按当前角色或操作者替代受罚者。界版每点伤害的红伤黑弃机制与本版不同，不引用 `boundary:ganglie-mobile`。

已存在的 `SourceLessGanglieChecks.LightningKeepsJudgmentButSkipsMissingSourceAndColdRestores` 和 `SourcedJudgmentStillPunishesOriginalParticipantAndConditionShapeIsValidated` 检查普通刚烈的真实来源、无来源判定和冷恢复。本次纯引用不复制新人物夹具或定义快照；统一加载、注册和相关检查由主代理执行。

普通经典图：[1700.jpg](https://www.sanguosha.cn/storage/uploads/images/skins/1700.jpg)。官网17首图为界版31300，本条选普通经典1700。`portraitKey` 为 `mobile-xiahou-dun`，图鉴分组 `standard`，人物/将池85499。
