# 赵云·移动版

核查日期：2026-10-26。普通移动身份，蜀、男、4体力，标准包；完整引用已注册的 `classic:longdan`，没有复制技能或运行时。

来源：[移动官网5](https://www.sanguosha.cn/pc/hero-detail-5.html)、[移动BWIKI人物固定版本34836](https://wiki.biligame.com/msgs/index.php?title=赵云&oldid=34836)（2025-12-05）、[普通龙胆固定版本19859](https://wiki.biligame.com/msgs/index.php?title=龙胆/赵云&oldid=19859)（2022-11-28）。人物普通经典栏列出5级奖励，实际开服普通武将；不采用SP或界版额外技能。

普通龙胆将一张杀作为闪、一张闪作为普通杀使用或打出。现有 `classic-longdan-skills.rules.json` 的两个单实体手牌转换分别开放出牌/响应入口与闪响应入口，没有桃酒互换、额外摸牌或虚构牌。普通、火、雷杀完整牌族经 `ProgramCardKindMatch.Contains` 继续匹配冰杀，因此并不因旧JSON只有三种杀的成员而漏掉当前牌族。

`GameEngine.CardConversions` 使用同一实体材料、有效输入牌种与当前使用/打出上下文生成和重验转换；原生用牌及响应支付保留该实体和龙胆来源。不会把八卦虚拟闪或丈八两张手牌再当作龙胆的单牌材料，也不会因技能无次数限制而绕过普通杀的使用次数与距离。

既有 `BoundaryZhaoYunMobileChecks.ForeignHandResponseGivesMismatchedTopBeforeOwnDiscardAndCold` 已真实使用同一个 `classic:longdan` 的出牌与响应实体，并验证原父帧及cold；`ShanheStartingResourceChecks.NewResourcesUseConversionDodgeAndEquipmentExitCold` 亦覆盖该原生转换支付。纯内容引用沿共享加载/注册与机制检查，不新增重复人物夹具。

普通经典立绘为[官网500.jpg](https://www.sanguosha.cn/storage/uploads/images/skins/500.jpg)，对应当前普通官网5列表的经典图；`portraitKey` 为 `mobile-zhao-yun`，本地目标名 `official-mobile-zhao-yun.jpg`，图鉴分组 `standard`。
