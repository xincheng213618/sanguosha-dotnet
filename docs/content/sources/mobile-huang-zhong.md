# 黄忠·移动版

核查日期：2026-10-26。普通移动身份，蜀、男、4体力、神话再临风包。独立 `mobile:liegong` JSON 复用已有指定目标禁闪能力，无新增运行时或opcode。

来源：[移动官网26](https://www.sanguosha.cn/pc/hero-detail-26.html)、[移动BWIKI人物34842](https://wiki.biligame.com/msgs/index.php?title=黄忠&oldid=34842)、[普通烈弓38996](https://wiki.biligame.com/msgs/index.php?title=烈弓/黄忠&oldid=38996)。已通过 MediaWiki revisions API 读取原始wikitext：人物修订2025-12-05、技能2026-06-11；普通经典栏为开服上线、5级获取，烈弓正文与当前官网一致。未混用国战乘势增伤、界版点数射程或谋版记录花色。

条件是在本人出牌阶段使用杀指定目标后判断：目标手牌数不小于本人体力值，或者不大于本人的攻击范围。两个比较均含等号，满足任一条件即可；每个实际指定目标独立可选，没有每阶段一次的技能配额，没有伤害增加。杀包括普通、火、雷、冰四类。

`classic:liegong` 的数值条件相同，但旧定义在 `slashBeforeResponse` 才询问，会晚于指定目标和目标效果相关的其他子结算，不能把它直接引用为当前完整移动版。移动JSON改在既有 `cardUseTargetsFinalized` actor目标窗捕获 `eventTargetHandCount`、`currentHp` 和 `currentAttackRange`，保留本人实际Play来源条件，用现有 `prohibitCurrentResponse` 标记原生同一张杀的对应目标。原生已经支持修改当前和后续prepared目标攻击状态；不会把禁闪泄漏给其他目标、其他用牌或其他杀。既有 `mobile:tieqi` 已使用同一个指定目标禁闪接点，此处没有新pending或新的运行时机制；加载器为现有指定目标禁闪策略补齐eventTargetHandCount的合法上下文，限定实际actor、完整四类杀和prohibitCurrentResponse，不放宽到无目标或无关事件。

代表性行为检查验证实际指定目标时签发禁闪，后续真实获得牌子窗改变目标手牌后仍按原父帧收益结算，保留物理杀和一次冷恢复。构建与定向检查由主代理统一执行，不把文档或JSON解析当作运行通过。

普通经典图：[2600.jpg](https://www.sanguosha.cn/storage/uploads/images/skins/2600.jpg)，来自官网26首图。`portraitKey` 为 `mobile-huang-zhong`，图鉴分组 `myth-wind`，技能85900，人物/将池85999。
